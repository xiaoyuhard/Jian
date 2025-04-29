using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// ʳ�������� ���ͼ���������
/// </summary>
[CreateAssetMenu(fileName = "FoodKindItemData", menuName = "Data/FoodKindItemData")]
[System.Serializable]
public class FoodKindItemData : ScriptableObject
{
    public int id;      // ����
    public string subCategoryName; //����
    public string foodCode; //���
    public string foodName; // ��������
    public string edible;//ʳ��
    public string water;// ������λ
    public string energyKcal;//����
    public string energyKj;
    public string protein;//������
    public string fat;//֬��
    public string cho;//̼ˮ������
    public string categoryName; //����
    public int categoryId;
    public bool isOnClick = false;
    public float count;
    public string mealPeriod; //�ò�ʱ���
    public string heat;//����

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


