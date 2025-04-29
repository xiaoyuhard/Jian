using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 食物数据类 发送及接收数据
/// </summary>
[CreateAssetMenu(fileName = "FoodKindItemData", menuName = "Data/FoodKindItemData")]
[System.Serializable]
public class FoodKindItemData : ScriptableObject
{
    public int id;      // 名字
    public string subCategoryName; //亚类
    public string foodCode; //编号
    public string foodName; // 物体名字
    public string edible;//食部
    public string water;// 计量单位
    public string energyKcal;//热量
    public string energyKj;
    public string protein;//蛋白质
    public string fat;//脂肪
    public string cho;//碳水化合物
    public string categoryName; //大类
    public int categoryId;
    public bool isOnClick = false;
    public float count;
    public string mealPeriod; //用餐时间段
    public string heat;//热量

}

[System.Serializable]
public class FoodResponse
{
    public int total;
    public List<FoodKindItemData> rows;
    public int code;
    public string msg;
}

[System.Serializable]
public class FoodSendConverDay
{
    public List<FoodEveryMealItem> breakfast;
    public List<FoodEveryMealItem> lunch;
    public List<FoodEveryMealItem> dinner;
    public UserInfo userInfo;
}

[System.Serializable]
public class FoodEveryMealItem
{
    public string foodCode;
    public float quantity;
}

[System.Serializable]
public class UserInfo
{
    public string birthday;
    public string height;
    public string weight;
    public string sex;
    public string level;
    public string physique;
}

[System.Serializable]
public class FoodRecriveConverDay
{
    public string msg;
    public int code;
    public FoodEveryMealEnergy data;
}
[System.Serializable]

public class FoodEveryMealEnergy
{
    public EveryMealEnergy breakfastEnergy;
    public EveryMealEnergy lunchEnergy;
    public EveryMealEnergy dinnerEnergy;
    public EveryMealEnergy totalEnergy;
    public EveryMealEnergy recEnergyIntake;
    public CompareResult compareResult;
    public List<ElementResult> elementResult;
}
[System.Serializable]

public class EveryMealEnergy
{
    public string totalEnergyKcal;
    public string protein;
    public string fat;
    public string cho;
}
[System.Serializable]

public class CompareResult
{
    public float choDiff;
    public float fatDiff;
    public float energyDiff;
    public float proteinDiff;
}

[System.Serializable]
public class ElementResult
{
    public string zhName;
    public string enName;
    public string unit;
    public string totalContent;
}


