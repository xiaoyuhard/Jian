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

        // �Զ������ ExpAction ���ֶ�
        EditorGUILayout.LabelField("���Ĳ���", EditorStyles.boldLabel);
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.ObjectField(_steps);
        if (EditorGUI.EndChangeCheck())
        {
            // ��TargetObject�ֶη����仯ʱ������ִ��һЩ����������������õȡ�
        }
        //DrawExpActionProperty(_steps);

        //// �Զ������ ExpAction �б�
        //EditorGUILayout.Space();
        //EditorGUILayout.LabelField("�Ӳ����б�", EditorStyles.boldLabel);
        //DrawExpActionList(_steps);

        serializedObject.ApplyModifiedProperties();
    }

    // ���Ƶ��� ExpAction ����
    private void DrawExpActionProperty(SerializedProperty property)
    {
        // ������ʱ Editor ʵ��
        Editor expActionEditor = CreateEditor(property.objectReferenceValue, typeof(ExpStep));

        EditorGUILayout.BeginVertical(GUI.skin.box);
        {
            // ���ƶ����ֶ�
            EditorGUILayout.PropertyField(property, new GUIContent("��������"));

            // ��������Ѹ�ֵ���������Զ�������
            if (property.objectReferenceValue != null)
            {
                expActionEditor.OnInspectorGUI();
            }
        }
        EditorGUILayout.EndVertical();

        DestroyImmediate(expActionEditor); // ��ʱ���� Editor ʵ��
    }

    // ���� ExpAction �б�
    private void DrawExpActionList(SerializedProperty listProperty)
    {
        for (int i = 0; i < listProperty.arraySize; i++)
        {
            SerializedProperty element = listProperty.GetArrayElementAtIndex(i);
            EditorGUILayout.BeginVertical(GUI.skin.window);
            {
                DrawExpActionProperty(element);
                if (GUILayout.Button("�Ƴ�"))
                {
                    listProperty.DeleteArrayElementAtIndex(i);
                    break;
                }
            }
            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("����²���"))
        {
            listProperty.arraySize++;
        }
    }

}
