using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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
    public float recipeCount;//食谱中食物的数量
    public float foodCount;//单选食物的数量
    public string mealPeriod; //用餐时间段
    public string heat;//热量
    public string imageUrl; //图片url
    public bool isDel = false;
    public GameObject foodItemObj;
    public FoodKindItemData Clone()
    {
        var clone = CreateInstance<FoodKindItemData>();
        clone.id = this.id;
        clone.subCategoryName = this.subCategoryName;
        clone.foodCode = this.foodCode;
        clone.foodName = this.foodName;
        clone.isOnClick = this.isOnClick; // 独立状态
        clone.edible = this.edible;//食部
        clone.water = this.water;// 计量单位
        clone.energyKcal = this.energyKcal;//热量
        clone.energyKj = this.energyKj;
        clone.protein = this.protein;//蛋白质
        clone.fat = this.fat;//脂肪
        clone.cho = this.cho;//碳水化合物
        clone.categoryName = this.categoryName; //大类
        clone.categoryId = this.categoryId;
        clone.count = this.count;
        clone.recipeCount = this.recipeCount;//食谱中食物的数量
        clone.foodCount = this.foodCount;//单选食物的数量
        clone.mealPeriod = this.mealPeriod; //用餐时间段
        clone.heat = this.heat;//热量
        clone.imageUrl = this.imageUrl; //图片url
        clone.isDel = this.isDel;
        clone.foodItemObj = this.foodItemObj;
        // 复制其他字段...
        return clone;
    }
}
/// <summary>
/// 食物和食谱选择后的
/// </summary>
[System.Serializable]
public class ChooseFoodData
{
    public List<FoodKindItemData> foods = new List<FoodKindItemData>();
    public List<RecipeItem> recipes = new List<RecipeItem>();
    public List<FoodKindItemData> allFoods = new List<FoodKindItemData>();
}

/// <summary>
/// 接收所有食物   /food/pageQuery
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
/// 选完后发送给服务器数据   /analyse/intake
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
/// 发送到服务器时的食物编号数量
/// </summary>
[System.Serializable]
public class FoodEveryMealItem
{
    public string foodCode; //编号
    public float quantity;  //数量
}
/// <summary>
/// 个人信息
/// </summary>
[System.Serializable]
public class UserInfo
{
    public string birthday;
    public string height;
    public string weight;
    public string sex;
    public string level;        //活动强度
    public string physique;     //身体状况
    public string proportion; //三餐营养比例
    public bool isBaby;       //判断是否为婴儿
}
/// <summary>
/// 选择完食物接收到服务器返回的总数据   
/// </summary>
[System.Serializable]
public class FoodRecriveConverDay
{
    public string msg;
    public int code;
    public FoodEveryMealEnergy data;
}
/// <summary>
/// 每个对应的分类  
/// </summary>
[System.Serializable]
public class FoodEveryMealEnergy
{
    public string score;                    //得分
    public string foodNum;                  //食材种类数
    public PromptInfo promptInfo;           //提示信息
    public EveryMealEnergy breakfastEnergy; //早餐所有量
    public EveryMealEnergy lunchEnergy;     //午餐所有量
    public EveryMealEnergy dinnerEnergy;    //晚餐所有量
    public EveryMealEnergy totalEnergy;     //一天所有量
    public EveryMealEnergy recEnergyIntake; //一天正常值区间
    public CompareResult compareResult;     //对应区间相差
    public FiberAndFineProtein fiberAndFineProtein;//膳食纤维及优质蛋白摄入
    public List<ElementResult> elementResult; //其他微量元素信息
}
/// <summary>
/// 全部选完后提示信息
/// </summary>
[System.Serializable]
public class PromptInfo
{
    public List<string> caloricIntakeMessage;   //能量摄入提示
    public List<string> energySupplyMessage;    //营养素供能提示
    public List<string> mealRatioMessage;       //三餐占比提示
    public List<string> fineProteinMessage;     //优质蛋白提示
    public List<string> perfectMessage;         //满分提示
}
/// <summary>
/// 每餐的热量蛋白质等
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
/// 今天所有食物的总的热量蛋白质等
/// </summary>
[System.Serializable]
public class CompareResult
{
    public float choDiff;       //碳水化合物
    public float fatDiff;       //脂肪
    public float energyDiff;    //总热量
    public float proteinDiff;   //蛋白质
}
/// <summary>
/// 膳食纤维和蛋白质摄入
/// </summary>
[System.Serializable]
public class FiberAndFineProtein
{
    public IntakePlan plan;
    public IntakePlan actual;
}
/// <summary>
/// 摄入数据类型
/// </summary>
[System.Serializable]
public class IntakePlan
{
    public string fiberIntake;          //纤维摄入量
    public string fineProteinIntake;    //优质蛋白质摄入量
    public string totalProteinIntake;   //总蛋白质摄入量
}
/// <summary>
/// 其他微量元素信息
/// </summary>
[System.Serializable]
public class ElementResult
{
    public string zhName;       //名字
    public string enName;       //简称
    public string unit;         //单位
    public string totalContent; //摄取值
}

/// <summary>
/// 三餐营养计划  /analyse/nutrition/plan
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
/// 计划数据
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
/// 请求食谱信息
/// </summary>
[System.Serializable]
public class SendRecipe
{
    public string pageNum;      //页码
    public string pageSize;     //每页大小
    public string recipeName;   //食谱名称
}
/// <summary>
/// 接收食谱信息  /cookbook/list
/// </summary>
[System.Serializable]
public class ReceiveRecipe
{
    public int total;       //总数
    public List<RecipeItem> rows;
    public int code;
    public string msg;
}
/// <summary>
/// 食谱信息处
/// </summary>
[System.Serializable]
public class RecipeItem
{
    public string id;       //食谱id
    public string recipeName;   //食谱名称
    public string includingFood;//包含食材
    public bool isOnClick = false;
    public List<FoodKindItemData> foodKindItems = new List<FoodKindItemData>(); //单人选择时食谱中的食物
    public List<FoodRecipeGroupItem> foodRecipeGroupItems = new List<FoodRecipeGroupItem>();//群体选择时食谱中的食物
    public string mealPeriod; //用餐时间段
    public GameObject itemObj;
    public Toggle itenTgl;
    public RecipeItem Clone()
    {
        return new RecipeItem
        {
            id = this.id,
            recipeName = this.recipeName,
            includingFood = this.includingFood,
            isOnClick = this.isOnClick, // 独立状态
            foodKindItems = this.foodKindItems?.Select(f => f.Clone()).ToList(),
            foodRecipeGroupItems = this.foodRecipeGroupItems?.Select(f => f.Clone()).ToList(),
            mealPeriod = this.mealPeriod,
            itemObj = this.itemObj // 注意：UI 对象通常不需要深拷贝
        };
    }
}
/// <summary>
/// 食谱中所有食物
/// </summary>
[System.Serializable]
public class RecipeAllFood
{
    public List<FoodKindItemData> data;
    public int code;
    public string msg;
}

/// <summary>
/// 群体选择食谱 单个食谱的数据
/// </summary>
[System.Serializable]
public class FoodRecipeGroupItem
{
    public int id;              //食物id
    public string imageUrl;     //图片地址
    public string foodCode;     //食物编码
    public string foodName;     //食物名称
    public string heat;      //热量
    public string protein;      //蛋白质
    public string fat;          //脂肪
    public string cho;          //碳水化合物
    public string weight;       //重量
    public string part;         //一份
    public float count = 0;

    public bool isInput = false;//是否输入
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
/// 接收群体所选的食谱中食物数据
/// </summary>
[System.Serializable]
public class FoodRecipeGroupResponse
{
    public int code;
    public string msg;
    public List<FoodRecipeGroupItem> data;
}

/// <summary>
/// 群体选择食谱的数据 保存本餐的食谱 和已经选择好的食谱
/// </summary>
[System.Serializable]
public class RecipeGroup
{
    public string mealName;                     //选择的哪一餐
    public List<RecipeItem> recipeShowList;     //选择食谱列表
    public List<RecipeItem> recipeSelectedList; //选择完食谱的保存列表
}
/// <summary>
/// 获取群体配餐人员列表
/// </summary>
[System.Serializable]
public class GetAllPhysique
{
    public string mag;
    public int code;
    public List<ReceiveAllPhysique> data;
}
/// <summary>
/// 接收群体配餐人员列表
/// </summary>
[System.Serializable]
public class ReceiveAllPhysique
{
    public string physique;     //人群
}
/// <summary>
/// 每人每日摄入热量(群体配餐)发送
/// </summary>
[System.Serializable]
public class SendHeatIntake
{
    public string physique;
    public string sex;
    public string level;
}
/// <summary>
/// 每人每日摄入热量(群体配餐)接收
/// </summary>
[System.Serializable]
public class ReceiveHeatIntake
{
    public string mag;
    public int code;
    public HeatIntakeItem data;
}
/// <summary>
/// 推荐摄入量接收的内容
/// </summary>
[System.Serializable]
public class HeatIntakeItem
{
    public string recIntake;        //推荐摄入量

}