using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "FoodKindItemData", menuName = "Data/FoodKindItemData")]
[System.Serializable]
public class FoodKindItemData : ScriptableObject
{
    public string id;      // 名字
    public string code; //编号
    public string iconName; // 物体名字
    public string unit;// 计量单位
    public string heat;//热量
    public string protein;//蛋白质
    public string fat;//脂肪
    public string carbohydrate;//碳水化合物
    public string Edible;//食部
    public bool isOnClick = false;
    public float count;
    public DataCategory type; // 类型字段（对应枚举）
    public string mealPeriod; //用餐时间段
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


