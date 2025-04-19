using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(DataManager))]

public class DataManagerEditor : Editor
{
    private Vector2 scrollPos;
    private string searchFilter = "";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DataManager dm = (DataManager)target;

        GUILayout.Space(10);
        EditorGUILayout.BeginVertical("Box");

        // ���ư�ť����
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("������������"))
        {
            dm.LoadAllExcelData();
            Debug.Log("������ǿ��ˢ��");
        }

        if (GUILayout.Button("����������ʵ��"))
        {
            CreateManagerInstance();
        }
        EditorGUILayout.EndHorizontal();

        // ������������
        searchFilter = EditorGUILayout.TextField("��������", searchFilter);

        // ����չʾ����
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(300));
        foreach (var kvp in dm.itemDictionary)
        {
            EditorGUILayout.LabelField($"����: {kvp.Key}", EditorStyles.boldLabel);
            int count = 0;

            foreach (var item in kvp.Value)
            {
                if (!string.IsNullOrEmpty(searchFilter) &&
                    !item.parent.Contains(searchFilter) &&
                    !item.id.Contains(searchFilter))
                {
                    continue;
                }

                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.LabelField($"���: {item.parent}");
                EditorGUILayout.LabelField($"����: {item.iconName}");
                EditorGUILayout.LabelField($"��λ: {item.id}");
                EditorGUILayout.EndVertical();
                count++;

                if (count > 50) // ��ֹ���ݹ��࿨��
                {
                    EditorGUILayout.HelpBox("����ʾǰ50�����ݣ���ʹ����������", MessageType.Info);
                    break;
                }
            }
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();
    }

    private void CreateManagerInstance()
    {
        if (DataManager.Instance == null)
        {
            GameObject go = new GameObject("DataManager");
            go.AddComponent<DataManager>();
            Debug.Log("�Ѵ���DataManagerʵ��");
        }
        else
        {
            Debug.LogWarning("ʵ���Ѵ��ڣ�");
        }
    }
}

// �༭�����߲˵�
public class DataEditorTools
{
    [MenuItem("Tools/Data/���ٷ��ʴ���")]
    static void ShowDataWindow()
    {
        DataBrowserWindow.ShowWindow();
    }

    [MenuItem("Tools/Data/��ExcelĿ¼")]
    static void OpenExcelFolder()
    {
        string path = Path.Combine(Application.streamingAssetsPath);
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        EditorUtility.RevealInFinder(path);
    }
}

// ���������������
public class DataBrowserWindow : EditorWindow
{
    [MenuItem("Tools/Data/�����������")]
    public static void ShowWindow()
    {
        GetWindow<DataBrowserWindow>("���������");
    }

    void OnGUI()
    {
        if (DataManager.Instance == null)
        {
            EditorGUILayout.HelpBox("DataManagerʵ�������ڣ�", MessageType.Error);
            if (GUILayout.Button("����ʵ��"))
            {
                GameObject go = new GameObject("DataManager");
                go.AddComponent<DataManager>();
            }
            return;
        }

        EditorGUILayout.LabelField("��ǰ����ͳ��", EditorStyles.boldLabel);
        EditorGUILayout.LabelField($"�Ѽ��ر�����: {DataManager.Instance.itemDictionary.Count}");

        foreach (var kvp in DataManager.Instance.itemDictionary)
        {
            EditorGUILayout.LabelField($"{kvp.Key}: {kvp.Value.Count}������");
        }
    }
}