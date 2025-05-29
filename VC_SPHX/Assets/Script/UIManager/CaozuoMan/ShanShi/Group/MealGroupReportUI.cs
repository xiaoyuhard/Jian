using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 出报告界面
/// </summary>
public class MealGroupReportUI : UICaoZuoBase
{
    public static MealGroupReportUI Instance { get; private set; }


    public Transform dayBrackContent;
    public Transform dayLunchContent;
    public Transform dayDinnerContent;
    public GameObject recipePrefab;         //食谱的预制体
    public GameObject recipeItemPrefab;     //食谱里食物的预制体

    public Transform everyMealParent;        //群体天每餐总量显示
    public GameObject everyMealObj;


    public GameObject modifyRecipeGroupUI;   //食谱修改数量UI界面
    public GameObject swopRecipeGroupUI;   //食谱交换UI界面

    private Dictionary<string, RecipeGroup> recipeListDic = new Dictionary<string, RecipeGroup>();   //保存每餐所对应的选择食谱列表和 已经选择了的食谱


    void Awake()
    {
        InitializeManager();
    }

    // 显式初始化方法（供编辑器调用）
    public void InitializeManager()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    /// <summary>
    /// 接收服务器返回的数据 
    /// </summary>
    public void ReceiveSerRecipe()
    {
        SumTotalEveryMeal();
        //sUIManager.Instance.OpenUICaoZuo("MealGroupReportUI");
        UIManager.Instance.CloseUICaoZuo("ChooseGroupShanShiUI");
    }

    /// <summary>
    /// 接收从选择食谱里传过来的数据
    /// </summary>
    private void OnEnable()
    {

        recipeListDic = FoodGroupChooseUI.Instance.BackRecipeDic();
        SumTotalEveryMeal();

    }


    // Start is called before the first frame update
    void Start()
    {

    }



    //一天总量
    public void SumTotalEveryMeal()
    {
        foreach (Transform child in everyMealParent)
        {
            Destroy(child.gameObject);
        }
        //foreach (Transform child in allFoodPartent)
        //{
        //    Destroy(child.gameObject);
        //}

        SumTotalRecipeFood(recipeListDic["早餐"].recipeSelectedList, dayBrackContent, "0");

        SumTotalRecipeFood(recipeListDic["午餐"].recipeSelectedList, dayLunchContent, "1");
        SumTotalRecipeFood(recipeListDic["晚餐"].recipeSelectedList, dayDinnerContent, "2");
        //foreach (EveryMealEnergy foodItem in everyMealEnergies)
        //{
        //    GameObject toggleObj = Instantiate(everyMealObj, everyMealParent);
        //    toggleObj.SetActive(true);
        //    toggleObj.transform.GetChild(2).GetComponent<Text>().text = foodItem.totalEnergyKcal.ToString();
        //    //toggleObj.transform.GetChild(3).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(/*data[i].id */"Icons" + "/" + foodItem.totalEnergyKcaStandard);
        //    toggleObj.transform.GetChild(5).GetComponent<Text>().text = foodItem.protein.ToString();
        //    //toggleObj.transform.GetChild(4).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(/*data[i].id */"Icons" + "/" + foodItem.proteinStandard);
        //    toggleObj.transform.GetChild(8).GetComponent<Text>().text = foodItem.fat.ToString();
        //    //toggleObj.transform.GetChild(5).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(/*data[i].id */"Icons" + "/" + foodItem.fatStandard);
        //    toggleObj.transform.GetChild(11).GetComponent<Text>().text = foodItem.cho.ToString();

        //}


    }

    public void SumTotalRecipeFood(List<RecipeItem> chooseFood, Transform foodParent, string mealName)
    {
        //totalFoodAllLists.Clear();
        // 清空现有Toggle
        foreach (Transform child in foodParent)
        {
            Destroy(child.gameObject);
        }
        foreach (var itemRec in chooseFood)
        {
            itemRec.mealPeriod = mealName;
            GameObject recipeObj = Instantiate(recipePrefab, foodParent);
            recipeObj.transform.Find("RecipeName").GetComponent<Text>().text = itemRec.recipeName;
            recipeObj.GetComponent<FoodGroupRecipeItem>().recipeItem = new RecipeItem();
            recipeObj.GetComponent<FoodGroupRecipeItem>().recipeItem = itemRec;
            recipeObj.GetComponent<FoodGroupRecipeItem>().recipeItem.foodRecipeGroupItems = new List<FoodRecipeGroupItem>(itemRec.foodRecipeGroupItems);

            recipeObj.SetActive(true);
            float allCount = 0;
            float allHeat = 0;
            float allWeight = 0;
            float allProtein = 0;
            float allFat = 0;
            float allCho = 0;
            foreach (var food in itemRec.foodRecipeGroupItems)
            {
                GameObject toggleObj = Instantiate(recipeItemPrefab, recipeObj.transform.GetChild(0).GetChild(0));
                toggleObj.SetActive(true);
                allCount += float.Parse(food.part);
                allHeat += float.Parse(BackMultiplyuantity(float.Parse(food.part), float.Parse(food.weight)));
                allWeight += float.Parse(BackMultiplyuantity(float.Parse(food.part), float.Parse(food.heat)));
                allProtein += float.Parse(BackMultiplyuantity(float.Parse(food.part), float.Parse(food.protein)));
                allFat += float.Parse(BackMultiplyuantity(float.Parse(food.part), float.Parse(food.fat)));
                allCho += float.Parse(BackMultiplyuantity(float.Parse(food.part), float.Parse(food.cho)));
                //SetFoodItemText(toggleObj, "Name", itemFood.foodName);
                toggleObj.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = food.foodName;
                toggleObj.transform.Find("Count").GetComponent<Text>().text = food.part.ToString();
                toggleObj.transform.Find("Heat").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(food.part), float.Parse(food.weight));

                toggleObj.transform.Find("Weight").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(food.part), float.Parse(food.weight));
                toggleObj.transform.Find("Protein").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(food.part), float.Parse(food.protein));
                toggleObj.transform.Find("Fat").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(food.part), float.Parse(food.fat));
                toggleObj.transform.Find("carbohydrate").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(food.part), float.Parse(food.cho));
            }


            Transform allOjb = recipeObj.transform.Find("AllFoodHeat").GetComponent<Transform>();
            allOjb.transform.Find("Count").GetComponent<Text>().text = allCount.ToString();
            allOjb.transform.Find("Heat").GetComponent<Text>().text = allHeat.ToString();
            allOjb.transform.Find("Weight").GetComponent<Text>().text = allWeight.ToString();
            allOjb.transform.Find("Protein").GetComponent<Text>().text = allProtein.ToString();
            allOjb.transform.Find("Fat").GetComponent<Text>().text = allFat.ToString();
            allOjb.transform.Find("carbohydrate").GetComponent<Text>().text = allCho.ToString();

        }
    }


    /// <summary>
    /// 修改食铺 并进行刷新
    /// </summary>
    public void ReciveModifyRecipeGroup(RecipeItem recipeItemModify)
    {
        foreach (var item in recipeListDic[recipeItemModify.mealPeriod].recipeSelectedList)
        {
            if (item.id == recipeItemModify.id)
            {
                item.foodRecipeGroupItems = recipeItemModify.foodRecipeGroupItems;
            }
        }
        SumTotalEveryMeal();
        modifyRecipeGroupUI.SetActive(false);

    }

    /// <summary>
    /// 删除食谱 进行刷新
    /// </summary>
    public void DelRecipeGroup(RecipeItem recipeItemModify)
    {
        foreach (var item in recipeListDic[recipeItemModify.mealPeriod].recipeSelectedList)
        {
            if (item.id == recipeItemModify.id)
            {
                recipeListDic[recipeItemModify.mealPeriod].recipeSelectedList.Remove(item);
            }
        }
        SumTotalEveryMeal();

    }
    /// <summary>
    /// 食谱查询并交换
    /// </summary>
    public void SwopRecipeGroup(RecipeItem oldRecipe, RecipeItem newRecipe)
    {

        // 获取需要操作的列表
        var targetList = recipeListDic[oldRecipe.mealPeriod].recipeSelectedList;

        // 使用 for 循环通过索引修改
        for (int i = 0; i < targetList.Count; i++)
        {
            if (targetList[i].id == oldRecipe.id)
            {
                targetList[i] = newRecipe; // 通过索引直接替换元素
                foreach (var meal in recipeListDic[oldRecipe.mealPeriod].recipeShowList)
                {
                    if (meal.id == oldRecipe.id)
                    {
                        meal.isOnClick = false;

                        meal.itemObj.GetComponent<Toggle>().interactable = !meal.isOnClick;
                        meal.itemObj.GetComponent<Toggle>().isOn = meal.isOnClick;
                    }
                    if (meal.id == targetList[i].id)
                    {
                        meal.isOnClick = true;

                        meal.itemObj.GetComponent<Toggle>().interactable = !meal.isOnClick;
                        meal.itemObj.GetComponent<Toggle>().isOn = meal.isOnClick;
                    }

                }

                break; // 如果ID唯一可提前退出
            }
        }
        SumTotalEveryMeal();
        swopRecipeGroupUI.SetActive(false);

    }


    public string BackMultiplyuantity(float quantity, float parameters)
    {
        return ((quantity * parameters) /*/ 100*/).ToString("F2");
    }
    // Update is called once per frame
    void Update()
    {

    }
}
