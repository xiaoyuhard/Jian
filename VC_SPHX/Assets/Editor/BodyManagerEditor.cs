using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(BodyManager))]

public class BodyManagerEditor : Editor
{
    private Vector2 scrollPos;
    private string searchFilter = "";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BodyManager dm = (BodyManager)target;

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
        foreach (var kvp in dm.itemBoneDic)
        {
            EditorGUILayout.LabelField($"����: {kvp.Key}", EditorStyles.boldLabel);
            int count = 0;

            foreach (var item in kvp.Value)
            {
                if (!string.IsNullOrEmpty(searchFilter) &&
                    !item.Level1.Contains(searchFilter) &&
                    !item.Level2.Contains(searchFilter))
                {
                    continue;
                }

                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.LabelField($"���: {item.Level1}");
                EditorGUILayout.LabelField($"����: {item.Level2}");
                EditorGUILayout.LabelField($"��λ: {item.Level3}");
                EditorGUILayout.LabelField($"��λ: {item.Level4}");
                EditorGUILayout.EndVertical();
                count++;

                if (count > 50) // ��ֹ���ݹ��࿨��
                {
                    EditorGUILayout.HelpBox("����ʾǰ50�����ݣ���ʹ����������", MessageType.Info);
                    break;
                }
            }
        }
        foreach (var kvp in dm.itemEndocrDic)
        {
            EditorGUILayout.LabelField($"����: {kvp.Key}", EditorStyles.boldLabel);
            int count = 0;

            foreach (var item in kvp.Value)
            {
                if (!string.IsNullOrEmpty(searchFilter) &&
                    !item.Female.Contains(searchFilter) &&
                    !item.Male.Contains(searchFilter))
                {
                    continue;
                }

                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.LabelField($"���: {item.Female}");
                EditorGUILayout.LabelField($"����: {item.Male}");
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
        if (BodyManager.Instance == null)
        {
            GameObject go = new GameObject("BodyManager");
            go.AddComponent<BodyManager>();
            Debug.Log("�Ѵ���BodyManagerʵ��");
        }
        else
        {
            Debug.LogWarning("ʵ���Ѵ��ڣ�");
        }
    }
}
// �༭�����߲˵�
public class BodyEditorTools
{
    [MenuItem("Tools/Body/���ٷ��ʴ���")]
    static void ShowDataWindow()
    {
        BodyBrowserWindow.ShowWindow();
    }

    [MenuItem("Tools/Body/��ExcelĿ¼")]
    static void OpenExcelFolder()
    {
        string path = Path.Combine(Application.streamingAssetsPath);
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        EditorUtility.RevealInFinder(path);
    }
}


// ���������������
public class BodyBrowserWindow : EditorWindow
{
    [MenuItem("Tools/Body/�����������")]
    public static void ShowWindow()
    {
        GetWindow<BodyBrowserWindow>("���������");
    }

    void OnGUI()
    {
        if (BodyManager.Instance == null)
        {
            EditorGUILayout.HelpBox("BodyManagerʵ�������ڣ�", MessageType.Error);
            if (GUILayout.Button("����ʵ��"))
            {
                GameObject go = new GameObject("BodyManager");
                go.AddComponent<BodyManager>();
            }
            return;
        }

        EditorGUILayout.LabelField("��ǰ����ͳ��", EditorStyles.boldLabel);
        //EditorGUILayout.LabelField($"�Ѽ��ر�����: {BodyManager.Instance.itemDictionary.Count}");

        //foreach (var kvp in BodyManager.Instance.itemDictionary)
        //{
        //    EditorGUILayout.LabelField($"{kvp.Key}: {kvp.Value.Count}������");
        //}
    }
}