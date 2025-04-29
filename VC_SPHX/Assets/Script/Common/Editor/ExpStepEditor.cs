using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

//[CustomEditor(typeof(ExpStep))]
public class ExpStepEditor:Editor
{
    private SerializedProperty _name;
    private SerializedProperty _type;
    private SerializedProperty _obj;
    private SerializedProperty _director;

    private void OnEnable()
    {
        _name = serializedObject.FindProperty("name");
        _type = serializedObject.FindProperty("type");
        _obj = serializedObject.FindProperty("obj");
        _director = serializedObject.FindProperty("director");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // 基础字段区域
        EditorGUILayout.BeginVertical(GUI.skin.box);
        {
            EditorGUILayout.PropertyField(_name, new GUIContent("Action Name"));
            EditorGUILayout.PropertyField(_type, new GUIContent("Action Type"));
        }
        EditorGUILayout.EndVertical();

        // 根据类型显示对应字段
        EditorGUILayout.Space(5);
        EditorGUILayout.BeginVertical(GUI.skin.window);
        {
            var actionType = (ExpActionType)_type.enumValueIndex;

            switch (actionType)
            {
                case ExpActionType.TriggerObject:
                    DrawObjectField("Target Object", _obj);
                    break;

                case ExpActionType.ClickPlayAnim:
                    DrawDirectorField("Timeline Controller", _director);
                    break;

                case ExpActionType.None:
                    EditorGUILayout.HelpBox("Select an action type to configure", MessageType.Info);
                    break;
            }
        }
        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawObjectField(string label, SerializedProperty property)
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.PropertyField(property, new GUIContent(label));
            if (GUILayout.Button("Find", GUILayout.Width(60)))
            {
                // 添加自动查找逻辑（例如按标签查找）
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawDirectorField(string label, SerializedProperty property)
    {
        EditorGUI.BeginChangeCheck();
        var newDirector = EditorGUILayout.ObjectField(label, property.objectReferenceValue,
            typeof(PlayableDirector), true) as PlayableDirector;

        if (EditorGUI.EndChangeCheck())
        {
            property.objectReferenceValue = newDirector;
        }

        if (property.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("Assign a PlayableDirector component", MessageType.Warning);
        }
    }

}
