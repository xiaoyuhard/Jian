//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(DataManager))]
//public class DataEditorTools : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//        DataManager manager = (DataManager)target;

//        GUILayout.Space(10);
//        if (GUILayout.Button("保存选中数据到 JSON"))
//        {
//            string folderPath = Path.Combine(Application.streamingAssetsPath, "Data/Items");
//            if (!Directory.Exists(folderPath))
//            {
//                Directory.CreateDirectory(folderPath);
//            }

//            foreach (EquipmentItemData data in manager.itemList)
//            {
//                string filePath = Path.Combine(folderPath, $"{data.id}.json");
//                manager.SaveDataToJson(data, filePath);
//            }
//        }
//    }
//}