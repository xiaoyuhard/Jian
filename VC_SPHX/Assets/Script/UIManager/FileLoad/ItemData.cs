using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 模型展示的数据类
/// </summary>
[CreateAssetMenu(fileName = "EquipmentItem", menuName = "Data/EquipmentItem")]
[System.Serializable]

public class EquipmentItemData : ScriptableObject
{
    public string id;      // 名字
    public string parent;   //模型名字
    public string introduce;    //设备介绍
    public string iconName;    // 图片名字
}





