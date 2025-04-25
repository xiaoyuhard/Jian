using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(FoodManager))]

public class FoodManagerEditor : Editor
{
    private Vector2 scrollPos;
    private string searchFilter = "";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        FoodManager dm = (FoodManager)target;

        GUILayout.Space(10);
        EditorGUILayout.BeginVertical("Box");

        // ���ư�ť����
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("������������"))
        {
            //dm.LoadAllExcelData();
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
                    !item.foodCode.Contains(searchFilter) &&
                    !item.foodName.Contains(searchFilter))
                {
                    continue;
                }

                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.LabelField($"���: {item.foodCode}");
                EditorGUILayout.LabelField($"����: {item.foodName}");
                EditorGUILayout.LabelField($"��λ: {item.water}");
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
        if (FoodManager.Instance == null)
        {
            GameObject go = new GameObject("FoodManager");
            go.AddComponent<FoodManager>();
            Debug.Log("�Ѵ���FoodManagerʵ��");
        }
        else
        {
            Debug.LogWarning("ʵ���Ѵ��ڣ�");
        }
    }
}


// �༭�����߲˵�
public class FoodEditorTools
{
    [MenuItem("Tools/Food/���ٷ��ʴ���")]
    static void ShowDataWindow()
    {
        FoodBrowserWindow.ShowWindow();
    }

    [MenuItem("Tools/Food/��ExcelĿ¼")]
    static void OpenExcelFolder()
    {
        string path = Path.Combine( Application.streamingAssetsPath);
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        EditorUtility.RevealInFinder(path);
    }
}

// ���������������
public class FoodBrowserWindow : EditorWindow
{
    [MenuItem("Tools/Food/�����������")]
    public static void ShowWindow()
    {
        GetWindow<FoodBrowserWindow>("���������");
    }

    void OnGUI()
    {
        if (FoodManager.Instance == null)
        {
            EditorGUILayout.HelpBox("FoodManagerʵ�������ڣ�", MessageType.Error);
            if (GUILayout.Button("����ʵ��"))
            {
                GameObject go = new GameObject("FoodManager");
                go.AddComponent<FoodManager>();
            }
            return;
        }

        EditorGUILayout.LabelField("��ǰ����ͳ��", EditorStyles.boldLabel);
        EditorGUILayout.LabelField($"�Ѽ��ر�����: {FoodManager.Instance.itemDictionary.Count}");

        foreach (var kvp in FoodManager.Instance.itemDictionary)
        {
            EditorGUILayout.LabelField($"{kvp.Key}: {kvp.Value.Count}������");
        }
    }
}