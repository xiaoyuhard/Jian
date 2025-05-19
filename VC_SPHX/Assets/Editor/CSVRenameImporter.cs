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
    [MenuItem("Tools/高级CSV重命名")]
    static void Init() => GetWindow<CSVRenameImporter>("高级CSV重命名工具");

    void OnGUI()
    {
        csvFile = (TextAsset)EditorGUILayout.ObjectField("CSV文件", csvFile, typeof(TextAsset), false);
        separator = EditorGUILayout.TextField("分隔符", separator.ToString())[0];
        objOldName = EditorGUILayout.TextField("物体原有的前缀", objOldName.ToString())[0];

        if (GUILayout.Button("执行CSV男重命名"))
        {
            ExecuteCSVRename("男性_");
        }
        if (GUILayout.Button("执行CSV女重命名"))
        {
            ExecuteCSVRename("女性_");
        }
    }

    private void ExecuteCSVRename(string sex)
    {
        unmatchedObjects.Clear();
        unmatchedCSV.Clear();
        if (csvFile == null)
        {
            Debug.LogError("请选择CSV文件！");
            return;
        }

        // 解析CSV规则
        List<RenameRule> rules = ParseCSVRules(csvFile.text);
        foreach (var item in rules)
        {
            unmatchedCSV.Add(item.NewName);
        }
        GameObject[] selectedObjects = Selection.gameObjects;
        //GameObject[] selectedObjects = objModel.transform.GetComponentsInChildren<GameObject>();
        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("未选中任何物体！");
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
                    // 动态替换逻辑：保留原始名称的尾部
                    string newName = rule.NewName;
                    if (match.Groups.Count > 1) // 如果有捕获组
                    {
                        newName += match.Groups[1].Value;
                    }
                    obj.name = sex + newName;
                    isMatched = true;
                    renamedCount++;
                    unmatchedCSV.Remove(newName);
                    break; // 匹配到第一个规则后退出
                }
            }

            if (!isMatched)
            {
                unmatchedObjects.Add(obj.name);
            }
        }


        // 输出结果
        Debug.Log($"成功重命名 {renamedCount} 个物体");
        if (unmatchedObjects.Count > 0)
        {
            //Debug.LogWarning($"未匹配的物体 ({unmatchedObjects.Count} 个):\n" + string.Join("\n", unmatchedObjects));
            Debug.LogWarning("csv里没有找到的有" + string.Join("\n", unmatchedCSV));
        }
    }
    // 示例：提取第3到最后字符（索引从0开始）
    string GetMiddleChars(string objName)
    {
        if (objName.Contains("男性_") || objName.Contains("女性_"))
        {
            string pattern = @"^[^_]+_[^_]+_(.*)";
            Match match = Regex.Match(objName.Replace(" ", ""), pattern);

            //if (match.Success)
            {
                return match.Groups[1].Value; // 返回捕获的剩余部分
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
        reader.ReadLine(); // 跳过标题行

        string line;
        while ((line = reader.ReadLine()) != null)
        {
            string[] parts = line.Split(separator);
            if (parts.Length >= 2)
            {
                //string oldNamePattern = "^" + Regex.Escape(parts[0]).Replace(@"\*", ".*") + "(.*)"; // 支持通配符*
                string oldNamePattern = Regex.Escape(parts[0]); // 支持通配符*
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
