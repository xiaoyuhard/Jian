using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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
    public FoodKindItemData Clone()
    {
        var clone = CreateInstance<FoodKindItemData>();
        clone.id = this.id;
        clone.subCategoryName = this.subCategoryName;
        clone.foodCode = this.foodCode;
        clone.foodName = this.foodName;
        clone.isOnClick = this.isOnClick; // ����״̬
        clone.edible = this.edible;//ʳ��
        clone.water = this.water;// ������λ
        clone.energyKcal = this.energyKcal;//����
        clone.energyKj = this.energyKj;
        clone.protein = this.protein;//������
        clone.fat = this.fat;//֬��
        clone.cho = this.cho;//̼ˮ������
        clone.categoryName = this.categoryName; //����
        clone.categoryId = this.categoryId;
        clone.count = this.count;
        clone.recipeCount = this.recipeCount;//ʳ����ʳ�������
        clone.foodCount = this.foodCount;//��ѡʳ�������
        clone.mealPeriod = this.mealPeriod; //�ò�ʱ���
        clone.heat = this.heat;//����
        clone.imageUrl = this.imageUrl; //ͼƬurl
        clone.isDel = this.isDel;
        clone.foodItemObj = this.foodItemObj;
        // ���������ֶ�...
        return clone;
    }
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
    public string score;                    //�÷�
    public string foodNum;                  //ʳ��������
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
    public List<FoodKindItemData> foodKindItems = new List<FoodKindItemData>(); //����ѡ��ʱʳ���е�ʳ��
    public List<FoodRecipeGroupItem> foodRecipeGroupItems = new List<FoodRecipeGroupItem>();//Ⱥ��ѡ��ʱʳ���е�ʳ��
    public string mealPeriod; //�ò�ʱ���
    public GameObject itemObj;
    public Toggle itenTgl;
    public RecipeItem Clone()
    {
        return new RecipeItem
        {
            id = this.id,
            recipeName = this.recipeName,
            includingFood = this.includingFood,
            isOnClick = this.isOnClick, // ����״̬
            foodKindItems = this.foodKindItems?.Select(f => f.Clone()).ToList(),
            foodRecipeGroupItems = this.foodRecipeGroupItems?.Select(f => f.Clone()).ToList(),
            mealPeriod = this.mealPeriod,
            itemObj = this.itemObj // ע�⣺UI ����ͨ������Ҫ���
        };
    }
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

/// <summary>
/// Ⱥ��ѡ��ʳ�� ����ʳ�׵�����
/// </summary>
[System.Serializable]
public class FoodRecipeGroupItem
{
    public int id;              //ʳ��id
    public string imageUrl;     //ͼƬ��ַ
    public string foodCode;     //ʳ�����
    public string foodName;     //ʳ������
    public string heat;      //����
    public string protein;      //������
    public string fat;          //֬��
    public string cho;          //̼ˮ������
    public string weight;       //����
    public string part;         //һ��
    public float count = 0;

    public bool isInput = false;//�Ƿ�����
    public FoodRecipeGroupItem Clone()
    {
        return new FoodRecipeGroupItem
        {
            id = this.id,
            imageUrl = this.imageUrl,
            foodCode = this.foodCode,
            foodName = this.foodName,
            heat = this.heat,
            protein = this.protein,
            fat = this.fat,
            cho = this.cho,
            weight = this.weight,
            part = this.part,
            isInput = this.isInput
        };
    }
}

/// <summary>
/// ����Ⱥ����ѡ��ʳ����ʳ������
/// </summary>
[System.Serializable]
public class FoodRecipeGroupResponse
{
    public int code;
    public string msg;
    public List<FoodRecipeGroupItem> data;
}

/// <summary>
/// Ⱥ��ѡ��ʳ�׵����� ���汾�͵�ʳ�� ���Ѿ�ѡ��õ�ʳ��
/// </summary>
[System.Serializable]
public class RecipeGroup
{
    public string mealName;                     //ѡ�����һ��
    public List<RecipeItem> recipeShowList;     //ѡ��ʳ���б�
    public List<RecipeItem> recipeSelectedList; //ѡ����ʳ�׵ı����б�
}
/// <summary>
/// ��ȡȺ�������Ա�б�
/// </summary>
[System.Serializable]
public class GetAllPhysique
{
    public string mag;
    public int code;
    public List<ReceiveAllPhysique> data;
}
/// <summary>
/// ����Ⱥ�������Ա�б�
/// </summary>
[System.Serializable]
public class ReceiveAllPhysique
{
    public string physique;     //��Ⱥ
}
/// <summary>
/// ÿ��ÿ����������(Ⱥ�����)����
/// </summary>
[System.Serializable]
public class SendHeatIntake
{
    public string physique;
    public string sex;
    public string level;
}
/// <summary>
/// ÿ��ÿ����������(Ⱥ�����)����
/// </summary>
[System.Serializable]
public class ReceiveHeatIntake
{
    public string mag;
    public int code;
    public HeatIntakeItem data;
}
/// <summary>
/// �Ƽ����������յ�����
/// </summary>
[System.Serializable]
public class HeatIntakeItem
{
    public string recIntake;        //�Ƽ�������

}