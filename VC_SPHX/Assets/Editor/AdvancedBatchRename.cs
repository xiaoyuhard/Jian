using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AdvancedBatchRename : EditorWindow
{
    private string baseName = "Object";
    private int startNumber = 1;
    private int padding = 3; // ���λ������3��001��
    private bool useSelectionOrder = true;
    private string customRule = "";

    [MenuItem("Tools/�߼�����������")]
    static void Init() => GetWindow<AdvancedBatchRename>("�߼�����������");

    void OnGUI()
    {
        GUILayout.Label("������������", EditorStyles.boldLabel);
        baseName = EditorGUILayout.TextField("��������", baseName);
        startNumber = EditorGUILayout.IntField("��ʼ���", startNumber);
        padding = EditorGUILayout.IntSlider("���λ��", padding, 1, 5);
        useSelectionOrder = EditorGUILayout.Toggle("��ѡ��˳����", useSelectionOrder);
        customRule = EditorGUILayout.TextField("�Զ�����򣨿�ѡ��", customRule);

        if (GUILayout.Button("Ӧ��������"))
        {
            RenameWithAdvancedRules();
        }
    }

    private void RenameWithAdvancedRules()
    {
        GameObject[] selected = Selection.gameObjects;
        if (selected.Length == 0)
        {
            Debug.LogWarning("����ѡ�����壡");
            return;
        }

        // ��ѡ��˳�����򣨿�ѡ��
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

            // ���ɱ�Ų��֣��� 001��
            string numberPart = currentNumber.ToString().PadLeft(padding, '0');

            // Ӧ���Զ������ʾ�������ݱ�ǩ���ǰ׺��
            string prefix = "";
            if (obj.CompareTag("Enemy")) prefix = "Enemy_";
            if (obj.CompareTag("NPC")) prefix = "NPC_";

            // �����������
            obj.name = $"{prefix}{baseName}_{numberPart}";

            // Ӧ���Զ�������綯̬������
            if (!string.IsNullOrEmpty(customRule))
            {
                obj.name = obj.name.Replace("{index}", $"{i + 1}")
                                    .Replace("{x}", obj.transform.position.x.ToString("F1"));
            }
        }

        Debug.Log($"�������� {orderedList.Count} ������");
    }
}
