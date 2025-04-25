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

        // 控制按钮区域
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("立即加载数据"))
        {
            //dm.LoadAllExcelData();
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
                    !item.foodCode.Contains(searchFilter) &&
                    !item.foodName.Contains(searchFilter))
                {
                    continue;
                }

                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.LabelField($"编号: {item.foodCode}");
                EditorGUILayout.LabelField($"名称: {item.foodName}");
                EditorGUILayout.LabelField($"单位: {item.water}");
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
        if (FoodManager.Instance == null)
        {
            GameObject go = new GameObject("FoodManager");
            go.AddComponent<FoodManager>();
            Debug.Log("已创建FoodManager实例");
        }
        else
        {
            Debug.LogWarning("实例已存在！");
        }
    }
}


// 编辑器工具菜单
public class FoodEditorTools
{
    [MenuItem("Tools/Food/快速访问窗口")]
    static void ShowDataWindow()
    {
        FoodBrowserWindow.ShowWindow();
    }

    [MenuItem("Tools/Food/打开Excel目录")]
    static void OpenExcelFolder()
    {
        string path = Path.Combine( Application.streamingAssetsPath);
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        EditorUtility.RevealInFinder(path);
    }
}

// 独立数据浏览窗口
public class FoodBrowserWindow : EditorWindow
{
    [MenuItem("Tools/Food/数据浏览窗口")]
    public static void ShowWindow()
    {
        GetWindow<FoodBrowserWindow>("数据浏览器");
    }

    void OnGUI()
    {
        if (FoodManager.Instance == null)
        {
            EditorGUILayout.HelpBox("FoodManager实例不存在！", MessageType.Error);
            if (GUILayout.Button("创建实例"))
            {
                GameObject go = new GameObject("FoodManager");
                go.AddComponent<FoodManager>();
            }
            return;
        }

        EditorGUILayout.LabelField("当前数据统计", EditorStyles.boldLabel);
        EditorGUILayout.LabelField($"已加载表数量: {FoodManager.Instance.itemDictionary.Count}");

        foreach (var kvp in FoodManager.Instance.itemDictionary)
        {
            EditorGUILayout.LabelField($"{kvp.Key}: {kvp.Value.Count}条数据");
        }
    }
}