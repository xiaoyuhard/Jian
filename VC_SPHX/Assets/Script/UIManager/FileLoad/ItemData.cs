using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ģ��չʾ��������
/// </summary>
[CreateAssetMenu(fileName = "EquipmentItem", menuName = "Data/EquipmentItem")]
[System.Serializable]

public class EquipmentItemData : ScriptableObject
{
    public string id;      // ����
    public string parent;   //ģ������
    public string introduce;    //�豸����
    public string iconName;    // ͼƬ����
}





