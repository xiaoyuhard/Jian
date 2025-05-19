using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BatchRenameTool : EditorWindow
{
    private string replaceFrom = "";
    private string replaceTo = "";
    private string prefix = "";
    private string suffix = "";

    [MenuItem("Tools/批量重命名")]
    static void Init()
    {
        GetWindow<BatchRenameTool>("批量重命名工具").Show();
    }

    void OnGUI()
    {
        GUILayout.Label("批量重命名设置", EditorStyles.boldLabel);
        replaceFrom = EditorGUILayout.TextField("替换文本（从）", replaceFrom);
        replaceTo = EditorGUILayout.TextField("替换为（到）", replaceTo);
        prefix = EditorGUILayout.TextField("添加前缀", prefix);
        suffix = EditorGUILayout.TextField("添加后缀", suffix);

        if (GUILayout.Button("应用修改"))
        {
            RenameSelectedObjects();
        }
    }

    private void RenameSelectedObjects()
    {
        GameObject[] selectedObjects = Selection.gameObjects;
        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("请先选中需要重命名的物体！");
            return;
        }

        Undo.RecordObjects(selectedObjects, "Batch Rename"); // 支持撤销操作

        foreach (GameObject obj in selectedObjects)
        {
            string newName = obj.name;

            // 文本替换
            if (!string.IsNullOrEmpty(replaceFrom))
            {
                newName = newName.Replace(replaceFrom, replaceTo);
            }

            // 添加前缀/后缀
            newName = prefix + newName + suffix;

            obj.name = newName;
        }

        Debug.Log($"成功修改 {selectedObjects.Length} 个物体的名称");
    }
}