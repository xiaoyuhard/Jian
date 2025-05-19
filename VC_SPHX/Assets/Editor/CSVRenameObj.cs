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

    [MenuItem("Tools/高级CSV重命名物体名字")]
    static void Init() => GetWindow<CSVRenameObj>("高级CSV重命名物体名称工具");

    void OnGUI()
    {
        csvFile = (TextAsset)EditorGUILayout.ObjectField("CSV文件", csvFile, typeof(TextAsset), false);
        separator = EditorGUILayout.TextField("分隔符", separator.ToString())[0];
        objOldName = EditorGUILayout.TextField("物体原有的前缀", objOldName);
        objNewName = EditorGUILayout.TextField("物体新的前缀", objNewName);

        if (GUILayout.Button("执行CSV重命名"))
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
                //Match match = rule.Regex.Match(obj.name);
                if (match.Success)
                {
                    // 动态替换逻辑：保留原始名称的尾部
                    string newName = rule.NewName;
                    if (match.Groups.Count > 1) // 如果有捕获组
                    {
                        newName += match.Groups[1].Value;
                    }
                    obj.name = objNewName + newName;
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
        if (string.IsNullOrEmpty(objOldName))
        {
            // 如果没有旧前缀，返回原名称（去除所有空格）
            return objName.Replace(" ", "");
        }

        // 构建正则表达式以匹配旧前缀，允许字符间有任意空格
        StringBuilder pattern = new StringBuilder();
        pattern.Append(@"^\s*"); // 允许前缀前的空格
        foreach (char c in objOldName)
        {
            pattern.Append(Regex.Escape(c.ToString()));
            pattern.Append(@"\s*"); // 允许每个字符后的空格
        }
        pattern.Append(@"(.+?)\s*$"); // 捕获剩余部分并去除末尾空格

        Match match = Regex.Match(objName, pattern.ToString(), RegexOptions.IgnoreCase);
        if (match.Success)
        {
            // 提取捕获的中间部分并去除所有空格
            string middlePart = match.Groups[1].Value.Replace(" ", "");
            return middlePart;
        }

        // 无法匹配旧前缀时，返回整个名称并去除空格
        return objName.Replace(" ", "");
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
                string oldNamePattern = parts[0];
                string regexPattern = "^" + Regex.Escape(oldNamePattern)
                                          .Replace(@"\*", ".*") // 处理通配符
                                          .Replace(@"\?", ".")  // 处理单个字符通配
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
