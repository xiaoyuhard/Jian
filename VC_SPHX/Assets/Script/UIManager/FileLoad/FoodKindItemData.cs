using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "FoodKindItemData", menuName = "Data/FoodKindItemData")]
[System.Serializable]
public class FoodKindItemData : ScriptableObject
{
    public string id;      // ����
    public string code; //���
    public string iconName; // ��������
    public string unit;// ������λ
    public string heat;//����
    public string protein;//������
    public string fat;//֬��
    public string carbohydrate;//̼ˮ������
    public string Edible;//ʳ��
    public bool isOnClick = false;
    public float count;
    public DataCategory type; // �����ֶΣ���Ӧö�٣�
    public string mealPeriod; //�ò�ʱ���
}
public enum DataCategory
{
    grain,
    TypeB,
    TypeC,
    TypeD,
    TypeE,
    TypeF
}


