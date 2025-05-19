using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
public class CSVRenameImporter : EditorWindow
{
    private TextAsset csvFile;
    private char separator = ',';
    private List<string> unmatchedObjects = new List<string>();
    private List<string> unmatchedCSV = new List<string>();
    private GameObject objModel;
    private char objOldName;
    [MenuItem("Tools/�߼�CSV������")]
    static void Init() => GetWindow<CSVRenameImporter>("�߼�CSV����������");

    void OnGUI()
    {
        csvFile = (TextAsset)EditorGUILayout.ObjectField("CSV�ļ�", csvFile, typeof(TextAsset), false);
        separator = EditorGUILayout.TextField("�ָ���", separator.ToString())[0];
        objOldName = EditorGUILayout.TextField("����ԭ�е�ǰ׺", objOldName.ToString())[0];

        if (GUILayout.Button("ִ��CSV��������"))
        {
            ExecuteCSVRename("����_");
        }
        if (GUILayout.Button("ִ��CSVŮ������"))
        {
            ExecuteCSVRename("Ů��_");
        }
    }

    private void ExecuteCSVRename(string sex)
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
                if (match.Success)
                {
                    // ��̬�滻�߼�������ԭʼ���Ƶ�β��
                    string newName = rule.NewName;
                    if (match.Groups.Count > 1) // ����в�����
                    {
                        newName += match.Groups[1].Value;
                    }
                    obj.name = sex + newName;
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
        if (objName.Contains("����_") || objName.Contains("Ů��_"))
        {
            string pattern = @"^[^_]+_[^_]+_(.*)";
            Match match = Regex.Match(objName.Replace(" ", ""), pattern);

            //if (match.Success)
            {
                return match.Groups[1].Value; // ���ز����ʣ�ಿ��
            }
            //Debug.Log(objName + "safs    " + objName.Substring(8));
            //return objName.Substring(8);
        }
        return objName;
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
                string oldNamePattern = Regex.Escape(parts[0]); // ֧��ͨ���*
                string cleanedLine = Regex.Replace(oldNamePattern.Replace(" ", ""), @"\\", "");

                rules.Add(new RenameRule
                {
                    Regex = new Regex(cleanedLine),
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
