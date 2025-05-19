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

    [MenuItem("Tools/����������")]
    static void Init()
    {
        GetWindow<BatchRenameTool>("��������������").Show();
    }

    void OnGUI()
    {
        GUILayout.Label("��������������", EditorStyles.boldLabel);
        replaceFrom = EditorGUILayout.TextField("�滻�ı����ӣ�", replaceFrom);
        replaceTo = EditorGUILayout.TextField("�滻Ϊ������", replaceTo);
        prefix = EditorGUILayout.TextField("���ǰ׺", prefix);
        suffix = EditorGUILayout.TextField("��Ӻ�׺", suffix);

        if (GUILayout.Button("Ӧ���޸�"))
        {
            RenameSelectedObjects();
        }
    }

    private void RenameSelectedObjects()
    {
        GameObject[] selectedObjects = Selection.gameObjects;
        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("����ѡ����Ҫ�����������壡");
            return;
        }

        Undo.RecordObjects(selectedObjects, "Batch Rename"); // ֧�ֳ�������

        foreach (GameObject obj in selectedObjects)
        {
            string newName = obj.name;

            // �ı��滻
            if (!string.IsNullOrEmpty(replaceFrom))
            {
                newName = newName.Replace(replaceFrom, replaceTo);
            }

            // ���ǰ׺/��׺
            newName = prefix + newName + suffix;

            obj.name = newName;
        }

        Debug.Log($"�ɹ��޸� {selectedObjects.Length} �����������");
    }
}