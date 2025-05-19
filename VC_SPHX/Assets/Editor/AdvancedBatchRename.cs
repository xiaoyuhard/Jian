using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AdvancedBatchRename : EditorWindow
{
    private string baseName = "Object";
    private int startNumber = 1;
    private int padding = 3; // 编号位数（如3→001）
    private bool useSelectionOrder = true;
    private string customRule = "";

    [MenuItem("Tools/高级批量重命名")]
    static void Init() => GetWindow<AdvancedBatchRename>("高级重命名工具");

    void OnGUI()
    {
        GUILayout.Label("命名规则设置", EditorStyles.boldLabel);
        baseName = EditorGUILayout.TextField("基础名称", baseName);
        startNumber = EditorGUILayout.IntField("起始编号", startNumber);
        padding = EditorGUILayout.IntSlider("编号位数", padding, 1, 5);
        useSelectionOrder = EditorGUILayout.Toggle("按选中顺序编号", useSelectionOrder);
        customRule = EditorGUILayout.TextField("自定义规则（可选）", customRule);

        if (GUILayout.Button("应用重命名"))
        {
            RenameWithAdvancedRules();
        }
    }

    private void RenameWithAdvancedRules()
    {
        GameObject[] selected = Selection.gameObjects;
        if (selected.Length == 0)
        {
            Debug.LogWarning("请先选中物体！");
            return;
        }

        // 按选中顺序排序（可选）
        List<GameObject> orderedList = new List<GameObject>(selected);
        if (useSelectionOrder)
        {
            orderedList.Sort((a, b) =>
                EditorUtility.NaturalCompare(a.name, b.name));
        }

        Undo.RecordObjects(orderedList.ToArray(), "Advanced Rename");

        for (int i = 0; i < orderedList.Count; i++)
        {
            GameObject obj = orderedList[i];
            int currentNumber = startNumber + i;

            // 生成编号部分（如 001）
            string numberPart = currentNumber.ToString().PadLeft(padding, '0');

            // 应用自定义规则（示例：根据标签添加前缀）
            string prefix = "";
            if (obj.CompareTag("Enemy")) prefix = "Enemy_";
            if (obj.CompareTag("NPC")) prefix = "NPC_";

            // 组合最终名称
            obj.name = $"{prefix}{baseName}_{numberPart}";

            // 应用自定义规则（如动态变量）
            if (!string.IsNullOrEmpty(customRule))
            {
                obj.name = obj.name.Replace("{index}", $"{i + 1}")
                                    .Replace("{x}", obj.transform.position.x.ToString("F1"));
            }
        }

        Debug.Log($"已重命名 {orderedList.Count} 个物体");
    }
}
