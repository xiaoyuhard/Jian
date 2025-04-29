using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(ExpStepCtrl))]
public class ExpStepCtrlEditor:Editor
{
    private SerializedProperty _steps;

    private void OnEnable()
    {
        _steps = serializedObject.FindProperty("steps");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // 自定义绘制 ExpAction 主字段
        EditorGUILayout.LabelField("核心操作", EditorStyles.boldLabel);
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.ObjectField(_steps);
        if (EditorGUI.EndChangeCheck())
        {
            // 当TargetObject字段发生变化时，可以执行一些操作，比如更新引用等。
        }
        //DrawExpActionProperty(_steps);

        //// 自定义绘制 ExpAction 列表
        //EditorGUILayout.Space();
        //EditorGUILayout.LabelField("子操作列表", EditorStyles.boldLabel);
        //DrawExpActionList(_steps);

        serializedObject.ApplyModifiedProperties();
    }

    // 绘制单个 ExpAction 属性
    private void DrawExpActionProperty(SerializedProperty property)
    {
        // 创建临时 Editor 实例
        Editor expActionEditor = CreateEditor(property.objectReferenceValue, typeof(ExpStep));

        EditorGUILayout.BeginVertical(GUI.skin.box);
        {
            // 绘制对象字段
            EditorGUILayout.PropertyField(property, new GUIContent("操作配置"));

            // 如果对象已赋值，绘制其自定义内容
            if (property.objectReferenceValue != null)
            {
                expActionEditor.OnInspectorGUI();
            }
        }
        EditorGUILayout.EndVertical();

        DestroyImmediate(expActionEditor); // 及时销毁 Editor 实例
    }

    // 绘制 ExpAction 列表
    private void DrawExpActionList(SerializedProperty listProperty)
    {
        for (int i = 0; i < listProperty.arraySize; i++)
        {
            SerializedProperty element = listProperty.GetArrayElementAtIndex(i);
            EditorGUILayout.BeginVertical(GUI.skin.window);
            {
                DrawExpActionProperty(element);
                if (GUILayout.Button("移除"))
                {
                    listProperty.DeleteArrayElementAtIndex(i);
                    break;
                }
            }
            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("添加新操作"))
        {
            listProperty.arraySize++;
        }
    }

}
