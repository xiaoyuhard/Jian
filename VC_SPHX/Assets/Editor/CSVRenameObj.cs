using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class CSVRenameObj : EditorWindow
{
    private TextAsset csvFile;
    private char separator = ',';
    private List<string> unmatchedObjects = new List<string>();
    private List<string> unmatchedCSV = new List<string>();
    private GameObject objModel;
    private string objOldName;
    private string objNewName;

    [MenuItem("Tools/�߼�CSV��������������")]
    static void Init() => GetWindow<CSVRenameObj>("�߼�CSV�������������ƹ���");

    void OnGUI()
    {
        csvFile = (TextAsset)EditorGUILayout.ObjectField("CSV�ļ�", csvFile, typeof(TextAsset), false);
        separator = EditorGUILayout.TextField("�ָ���", separator.ToString())[0];
        objOldName = EditorGUILayout.TextField("����ԭ�е�ǰ׺", objOldName);
        objNewName = EditorGUILayout.TextField("�����µ�ǰ׺", objNewName);

        if (GUILayout.Button("ִ��CSV������"))
        {
            ExecuteCSVRename();
        }
    }

    private void ExecuteCSVRename()
    {
        unmatchedObjects.Clear();
        unmatchedCSV.Clear();
        if (csvFile == null)
        {
            Debug.LogError("��ѡ��CSV�ļ���");
            return;
        }

        // ����CSV����
        List<RenameRule> rules = ParseCSVRules(csvFile.text);
        foreach (var item in rules)
        {
            unmatchedCSV.Add(item.NewName);
        }
        GameObject[] selectedObjects = Selection.gameObjects;
        //GameObject[] selectedObjects = objModel.transform.GetComponentsInChildren<GameObject>();
        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("δѡ���κ����壡");
            return;
        }

        Undo.RecordObjects(selectedObjects, "CSV Rename");
        int renamedCount = 0;

        foreach (GameObject obj in selectedObjects)
        {
            bool isMatched = false;
            foreach (RenameRule rule in rules)
            {
                Match match = rule.Regex.Match(GetMiddleChars(obj.name));
                //Match match = rule.Regex.Match(obj.name);
                if (match.Success)
                {
                    // ��̬�滻�߼�������ԭʼ���Ƶ�β��
                    string newName = rule.NewName;
                    if (match.Groups.Count > 1) // ����в�����
                    {
                        newName += match.Groups[1].Value;
                    }
                    obj.name = objNewName + newName;
                    isMatched = true;
                    renamedCount++;
                    unmatchedCSV.Remove(newName);
                    break; // ƥ�䵽��һ��������˳�
                }
            }

            if (!isMatched)
            {
                unmatchedObjects.Add(obj.name);
            }
        }


        // ������
        Debug.Log($"�ɹ������� {renamedCount} ������");
        if (unmatchedObjects.Count > 0)
        {
            //Debug.LogWarning($"δƥ������� ({unmatchedObjects.Count} ��):\n" + string.Join("\n", unmatchedObjects));
            Debug.LogWarning("csv��û���ҵ�����" + string.Join("\n", unmatchedCSV));
        }
    }
    // ʾ������ȡ��3������ַ���������0��ʼ��
    string GetMiddleChars(string objName)
    {
        if (string.IsNullOrEmpty(objOldName))
        {
            // ���û�о�ǰ׺������ԭ���ƣ�ȥ�����пո�
            return objName.Replace(" ", "");
        }

        // ����������ʽ��ƥ���ǰ׺�������ַ���������ո�
        StringBuilder pattern = new StringBuilder();
        pattern.Append(@"^\s*"); // ����ǰ׺ǰ�Ŀո�
        foreach (char c in objOldName)
        {
            pattern.Append(Regex.Escape(c.ToString()));
            pattern.Append(@"\s*"); // ����ÿ���ַ���Ŀո�
        }
        pattern.Append(@"(.+?)\s*$"); // ����ʣ�ಿ�ֲ�ȥ��ĩβ�ո�

        Match match = Regex.Match(objName, pattern.ToString(), RegexOptions.IgnoreCase);
        if (match.Success)
        {
            // ��ȡ������м䲿�ֲ�ȥ�����пո�
            string middlePart = match.Groups[1].Value.Replace(" ", "");
            return middlePart;
        }

        // �޷�ƥ���ǰ׺ʱ�������������Ʋ�ȥ���ո�
        return objName.Replace(" ", "");
    }

    private List<RenameRule> ParseCSVRules(string csvText)
    {
        List<RenameRule> rules = new List<RenameRule>();
        StringReader reader = new StringReader(csvText);
        reader.ReadLine(); // ����������

        string line;
        while ((line = reader.ReadLine()) != null)
        {
            string[] parts = line.Split(separator);
            if (parts.Length >= 2)
            {
                //string oldNamePattern = "^" + Regex.Escape(parts[0]).Replace(@"\*", ".*") + "(.*)"; // ֧��ͨ���*
                string oldNamePattern = parts[0];
                string regexPattern = "^" + Regex.Escape(oldNamePattern)
                                          .Replace(@"\*", ".*") // ����ͨ���
                                          .Replace(@"\?", ".")  // �������ַ�ͨ��
                                      + "$";


                rules.Add(new RenameRule
                {
                    Regex = new Regex(regexPattern),
                    NewName = parts[1]
                });
            }
        }
        return rules;
    }

    private class RenameRule
    {
        public Regex Regex { get; set; }
        public string NewName { get; set; }
    }
}
