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
    public float recipeCount;//ʳ����ʳ�������
    public float foodCount;//��ѡʳ�������
    public string mealPeriod; //�ò�ʱ���
    public string heat;//����
    public string imageUrl; //ͼƬurl
    public bool isDel = false;
    public GameObject foodItemObj;
}
/// <summary>
/// ʳ���ʳ��ѡ����
/// </summary>
[System.Serializable]
public class ChooseFoodData
{
    public List<FoodKindItemData> foods = new List<FoodKindItemData>();
    public List<RecipeItem> recipes = new List<RecipeItem>();
    public List<FoodKindItemData> allFoods = new List<FoodKindItemData>();
}

/// <summary>
/// ��������ʳ��   /food/pageQuery
/// </summary>
[System.Serializable]
public class FoodResponse
{
    public int total;
    public List<FoodKindItemData> rows;
    public int code;
    public string msg;
}
/// <summary>
/// ѡ����͸�����������   /analyse/intake
/// </summary>
[System.Serializable]
public class FoodSendConverDay
{
    public List<FoodEveryMealItem> breakfast;
    public List<FoodEveryMealItem> lunch;
    public List<FoodEveryMealItem> dinner;
    public UserInfo userInfo;
}
/// <summary>
/// ���͵�������ʱ��ʳ��������
/// </summary>
[System.Serializable]
public class FoodEveryMealItem
{
    public string foodCode; //���
    public float quantity;  //����
}
/// <summary>
/// ������Ϣ
/// </summary>
[System.Serializable]
public class UserInfo
{
    public string birthday;
    public string height;
    public string weight;
    public string sex;
    public string level;        //�ǿ��
    public string physique;     //����״��
    public string proportion; //����Ӫ������
    public bool isBaby;       //�ж��Ƿ�ΪӤ��
}
/// <summary>
/// ѡ����ʳ����յ����������ص�������   
/// </summary>
[System.Serializable]
public class FoodRecriveConverDay
{
    public string msg;
    public int code;
    public FoodEveryMealEnergy data;
}
/// <summary>
/// ÿ����Ӧ�ķ���  
/// </summary>
[System.Serializable]
public class FoodEveryMealEnergy
{
    public PromptInfo promptInfo;           //��ʾ��Ϣ
    public EveryMealEnergy breakfastEnergy; //���������
    public EveryMealEnergy lunchEnergy;     //���������
    public EveryMealEnergy dinnerEnergy;    //���������
    public EveryMealEnergy totalEnergy;     //һ��������
    public EveryMealEnergy recEnergyIntake; //һ������ֵ����
    public CompareResult compareResult;     //��Ӧ�������
    public FiberAndFineProtein fiberAndFineProtein;//��ʳ��ά�����ʵ�������
    public List<ElementResult> elementResult; //����΢��Ԫ����Ϣ
}
/// <summary>
/// ȫ��ѡ�����ʾ��Ϣ
/// </summary>
[System.Serializable]
public class PromptInfo
{
    public List<string> caloricIntakeMessage;   //����������ʾ
    public List<string> energySupplyMessage;    //Ӫ���ع�����ʾ
    public List<string> mealRatioMessage;       //����ռ����ʾ
    public List<string> fineProteinMessage;     //���ʵ�����ʾ
    public List<string> perfectMessage;         //������ʾ
}
/// <summary>
/// ÿ�͵����������ʵ�
/// </summary>
[System.Serializable]
public class EveryMealEnergy
{
    public string totalEnergyKcal;
    public string protein;
    public string fat;
    public string cho;
}
/// <summary>
/// ��������ʳ����ܵ����������ʵ�
/// </summary>
[System.Serializable]
public class CompareResult
{
    public float choDiff;       //̼ˮ������
    public float fatDiff;       //֬��
    public float energyDiff;    //������
    public float proteinDiff;   //������
}
/// <summary>
/// ��ʳ��ά�͵���������
/// </summary>
[System.Serializable]
public class FiberAndFineProtein
{
    public IntakePlan plan;
    public IntakePlan actual;
}
/// <summary>
/// ������������
/// </summary>
[System.Serializable]
public class IntakePlan
{
    public string fiberIntake;          //��ά������
    public string fineProteinIntake;    //���ʵ�����������
    public string totalProteinIntake;   //�ܵ�����������
}
/// <summary>
/// ����΢��Ԫ����Ϣ
/// </summary>
[System.Serializable]
public class ElementResult
{
    public string zhName;       //����
    public string enName;       //���
    public string unit;         //��λ
    public string totalContent; //��ȡֵ
}

/// <summary>
/// ����Ӫ���ƻ�  /analyse/nutrition/plan
/// </summary>
[System.Serializable]
public class ThreeMeals
{
    public string msg;
    public int code;
    public TreePlan data;

}
[System.Serializable]
public class TreePlan
{
    public Plan breakfastPlan;
    public Plan lunchPlan;
    public Plan dinnerPlan;
}

/// <summary>
/// �ƻ�����
/// </summary>
[System.Serializable]
public class Plan
{
    public string totalEnergyKcal;
    public string protein;
    public string fat;
    public string cho;
}
/// <summary>
/// ����ʳ����Ϣ
/// </summary>
[System.Serializable]
public class SendRecipe
{
    public string pageNum;      //ҳ��
    public string pageSize;     //ÿҳ��С
    public string recipeName;   //ʳ������
}
/// <summary>
/// ����ʳ����Ϣ  /cookbook/list
/// </summary>
[System.Serializable]
public class ReceiveRecipe
{
    public int total;       //����
    public List<RecipeItem> rows;
    public int code;
    public string msg;
}
/// <summary>
/// ʳ����Ϣ��
/// </summary>
[System.Serializable]
public class RecipeItem
{
    public string id;       //ʳ��id
    public string recipeName;   //ʳ������
    public string includingFood;//����ʳ��
    public bool isOnClick = false;
    public List<FoodKindItemData> foodKindItems;
    public string mealPeriod; //�ò�ʱ���

}
/// <summary>
/// ʳ��������ʳ��
/// </summary>
[System.Serializable]
public class RecipeAllFood
{
    public List<FoodKindItemData> data;
    public int code;
    public string msg;
}

