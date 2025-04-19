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

        // 控制按钮区域
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("立即加载数据"))
        {
            dm.LoadAllExcelData();
            Debug.Log("数据已强制刷新");
        }

        if (GUILayout.Button("创建管理器实例"))
        {
            CreateManagerInstance();
        }
        EditorGUILayout.EndHorizontal();

        // 数据搜索过滤
        searchFilter = EditorGUILayout.TextField("快速搜索", searchFilter);

        // 数据展示区域
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(300));
        foreach (var kvp in dm.itemDictionary)
        {
            EditorGUILayout.LabelField($"表名: {kvp.Key}", EditorStyles.boldLabel);
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
                EditorGUILayout.LabelField($"编号: {item.parent}");
                EditorGUILayout.LabelField($"名称: {item.iconName}");
                EditorGUILayout.LabelField($"单位: {item.id}");
                EditorGUILayout.EndVertical();
                count++;

                if (count > 50) // 防止数据过多卡死
                {
                    EditorGUILayout.HelpBox("已显示前50条数据，请使用搜索过滤", MessageType.Info);
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
            Debug.Log("已创建DataManager实例");
        }
        else
        {
            Debug.LogWarning("实例已存在！");
        }
    }
}

// 编辑器工具菜单
public class DataEditorTools
{
    [MenuItem("Tools/Data/快速访问窗口")]
    static void ShowDataWindow()
    {
        DataBrowserWindow.ShowWindow();
    }

    [MenuItem("Tools/Data/打开Excel目录")]
    static void OpenExcelFolder()
    {
        string path = Path.Combine(Application.streamingAssetsPath);
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        EditorUtility.RevealInFinder(path);
    }
}

// 独立数据浏览窗口
public class DataBrowserWindow : EditorWindow
{
    [MenuItem("Tools/Data/数据浏览窗口")]
    public static void ShowWindow()
    {
        GetWindow<DataBrowserWindow>("数据浏览器");
    }

    void OnGUI()
    {
        if (DataManager.Instance == null)
        {
            EditorGUILayout.HelpBox("DataManager实例不存在！", MessageType.Error);
            if (GUILayout.Button("创建实例"))
            {
                GameObject go = new GameObject("DataManager");
                go.AddComponent<DataManager>();
            }
            return;
        }

        EditorGUILayout.LabelField("当前数据统计", EditorStyles.boldLabel);
        EditorGUILayout.LabelField($"已加载表数量: {DataManager.Instance.itemDictionary.Count}");

        foreach (var kvp in DataManager.Instance.itemDictionary)
        {
            EditorGUILayout.LabelField($"{kvp.Key}: {kvp.Value.Count}条数据");
        }
    }
}