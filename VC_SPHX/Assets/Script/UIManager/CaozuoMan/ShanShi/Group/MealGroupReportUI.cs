using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System;
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

    //public Transform everyMealParentWeek;        //群体周每餐总量显示
    public GameObject mealObjPerfab;            //每餐的

    public Transform allFoodPartent;
    public GameObject allFoodItemprefab;

    //public Transform allFoodPartentWeek;            //群体周的总
    public GameObject allFoodItemprefabWeek;


    public GameObject modifyRecipeGroupUI;   //食谱修改数量UI界面
    public GameObject swopRecipeGroupUI;   //食谱交换UI界面

    private Dictionary<string, RecipeGroup> recipeListDic = new Dictionary<string, RecipeGroup>();   //保存每餐所对应的选择食谱列表和 已经选择了的食谱

    public Button anewChooseBtn;            //重新选择按钮
    public Button nextGroupBtn;             //下一组配餐按钮

    public Button reportChartBtn;
    public GameObject reportChartUI;

    public UserInfo userInfo = new UserInfo();

    private string[] weekMealList = { "周一早餐", "周一午餐", "周一晚餐", "周二早餐", "周二午餐", "周二晚餐", "周三早餐", "周三午餐", "周三晚餐", "周四早餐", "周四午餐", "周四晚餐", "周五早餐", "周五午餐", "周五晚餐", "周六早餐", "周六午餐", "周六晚餐", "周日早餐", "周日午餐", "周日晚餐" };
    public List<Transform> weekMealContentList;     //每餐的content
    public List<Transform> weekGroupAllFoodParent;  //周每天总量显示


    public GameObject dayPan;
    public GameObject weekPan;

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
        userInfo = FoodGroupChooseUI.Instance.userInfo;
        recipeListDic = FoodGroupChooseUI.Instance.BackRecipeDic();
        SumTotalEveryMeal();
        dayPan.SetActive(false);
        weekPan.SetActive(false);

        weekPan.SetActive(true);
        for (int i = 0; i < 7; i++)
        {
            SumTotalEveryMealWeek();

        }
    }


    // Start is called before the first frame update
    void Start()
    {
        anewChooseBtn.onClick.AddListener(AnweChooseClick);
        nextGroupBtn.onClick.AddListener(NextGroupClick);

        reportChartBtn.onClick.AddListener(ClickReportChartBtn);

    }
    Dictionary<string, List<FoodRecipeGroupItem>> allFoodDic = new Dictionary<string, List<FoodRecipeGroupItem>>();
    public void SetChooseCategoryNameCount()
    {
        allFoodDic.Clear();
        AddFoodChooseCount(recipeListDic["早餐"].recipeSelectedList);
        AddFoodChooseCount(recipeListDic["午餐"].recipeSelectedList);
        AddFoodChooseCount(recipeListDic["晚餐"].recipeSelectedList);
    }
    public void AddFoodChooseCount(List<RecipeItem> chooseFoodData)
    {

        // 2. 合并 foods 列表
        foreach (var item in chooseFoodData)
        {
            MergeFoodList(item.foodRecipeGroupItems);

        }

    }


    private void MergeFoodList(List<FoodRecipeGroupItem> sourceList)
    {
        foreach (var item in sourceList)
        {
            if (allFoodDic.ContainsKey(item.categoryName))
            {
                allFoodDic[item.categoryName].Add(item);
            }
            else
            {
                allFoodDic.Add(item.categoryName, new List<FoodRecipeGroupItem> { item });
            }
        }
    }

    private void ClickReportChartBtn()
    {
        reportChartUI.SetActive(true);
        SetXchaetUIGroupDay.Instance.SetPieChatGroupDay(allFoodDic, score, foodNum, promptInfo, totalEnergy, recEnergyIntake, compareResult, everyMealEnergies, fiberAndFineProtein, user, userInfo);
    }
    private void NextGroupClick()
    {
        UIManager.Instance.OpenUICaoZuo("StudInforUI");
        UIManager.Instance.CloseUICaoZuo("MealGroupReportUI");

    }

    private void AnweChooseClick()
    {
        UIManager.Instance.CloseUICaoZuo("MealGroupReportUI");
        UIManager.Instance.OpenUICaoZuo("ChooseGroupShanShiUI");


    }


    /// <summary>
    /// 选择完的所有数据发到服务器
    /// </summary>
    public void SendServerCon()
    {
        List<FoodRecipeGroupItem> breakMealList = MergeAllFoods(recipeListDic["早餐"].recipeSelectedList);
        List<FoodRecipeGroupItem> lunchMealList = MergeAllFoods(recipeListDic["午餐"].recipeSelectedList);
        List<FoodRecipeGroupItem> dinnerMealList = MergeAllFoods(recipeListDic["晚餐"].recipeSelectedList);

        FoodSendConverDay foodSendConverDay = new FoodSendConverDay();
        foodSendConverDay.breakfast = AddFoodSendConverList(breakMealList);
        foodSendConverDay.lunch = AddFoodSendConverList(lunchMealList);
        foodSendConverDay.dinner = AddFoodSendConverList(dinnerMealList);
        foodSendConverDay.userInfo = userInfo;
        ServerCon.Instance.ConverToJsonPost(SerializeData(foodSendConverDay), "/group/analyse/oneDay");
    }
    private string SerializeData(FoodSendConverDay data)
    {
        // 方法一：使用 Unity 内置 JsonUtility（需要包装类）
        Wrapper<FoodSendConverDay> wrapper = new Wrapper<FoodSendConverDay> { items = data };
        //return JsonUtility.ToJson(wrapper);

        // 方法二：使用 Newtonsoft.Json（直接序列化列表）
        return JsonConvert.SerializeObject(data);
    }

    public List<FoodEveryMealItem> AddFoodSendConverList(List<FoodRecipeGroupItem> foodKinds)
    {
        List<FoodEveryMealItem> foodSendConvers = new List<FoodEveryMealItem>();
        foreach (var item in foodKinds)
        {
            FoodEveryMealItem converItem = new FoodEveryMealItem();
            converItem.foodCode = item.foodCode;
            converItem.quantity = Mathf.Round(item.count * 100) / 100;

            foodSendConvers.Add(converItem);
        }
        return foodSendConvers;
    }

    public List<FoodRecipeGroupItem> MergeAllFoods(List<RecipeItem> chooseFoodData)
    {
        // 1. 创建字典用于合并数据（Key: 食物唯一标识 id）
        Dictionary<string, FoodRecipeGroupItem> foodDict = new Dictionary<string, FoodRecipeGroupItem>();

        // 2. 合并 foods 列表
        foreach (var item in chooseFoodData)
        {
            MergeFoodList(item.foodRecipeGroupItems, foodDict);

        }

        //// 3. 合并 recipes 中的食材
        //foreach (var recipe in chooseFoodData.recipes)
        //{
        //    MergeFoodList(recipe.foodKindItems, foodDict);
        //}

        // 4. 将结果存入 allFoods
        //chooseFoodData.allFoods = new List<FoodRecipeGroupItem>(foodDict.Values);
        return new List<FoodRecipeGroupItem>(foodDict.Values);
    }


    private void MergeFoodList(List<FoodRecipeGroupItem> sourceList, Dictionary<string, FoodRecipeGroupItem> targetDict)
    {
        foreach (var sourceFood in sourceList)
        {
            if (targetDict.TryGetValue(sourceFood.foodCode, out var existingFood))
            {
                // 合并 count
                //float.Parse(existingFood.weight) += (float.Parse(sourceFood.weight) * float.Parse(sourceFood.part));
                existingFood.count += (float.Parse(sourceFood.weight) * float.Parse(sourceFood.part));
            }
            else
            {
                sourceFood.count = (float.Parse(sourceFood.weight) * float.Parse(sourceFood.part));

                targetDict.Add(sourceFood.foodCode, sourceFood);
            }
        }
    }

    //储存收回来的数据 
    public void ReceiveServerInform(FoodEveryMealEnergy response)
    {
        dayPan.SetActive(true);
        score = response.score;
        foodNum = response.foodNum;
        promptInfo = response.promptInfo;
        everyMealEnergies.Clear();
        everyMealEnergies.Add(response.breakfastEnergy);
        everyMealEnergies.Add(response.lunchEnergy);
        everyMealEnergies.Add(response.dinnerEnergy);
        elementResult = response.elementResult;
        compareResult = response.compareResult;
        totalEnergy = response.totalEnergy;
        recEnergyIntake = response.recEnergyIntake;
        fiberAndFineProtein = response.fiberAndFineProtein;
        ExcelExporter.Instance.response = response;
        ReceiveSerRecipe();
        SumTotalEveryMeal();
    }
    string score;
    string foodNum;
    PromptInfo promptInfo = new PromptInfo();
    EveryMealEnergy totalEnergy = new EveryMealEnergy();
    EveryMealEnergy recEnergyIntake = new EveryMealEnergy();
    CompareResult compareResult = new CompareResult();
    FiberAndFineProtein fiberAndFineProtein = new FiberAndFineProtein();
    private List<EveryMealEnergy> everyMealEnergies = new List<EveryMealEnergy>();
    private List<ElementResult> elementResult = new List<ElementResult>();
    ThreeMeals user = new ThreeMeals();


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
        foreach (EveryMealEnergy foodItem in everyMealEnergies)
        {
            GameObject toggleObj = Instantiate(everyMealObj, everyMealParent);
            toggleObj.SetActive(true);
            toggleObj.transform.GetChild(2).GetComponent<Text>().text = foodItem.totalEnergyKcal.ToString();
            //toggleObj.transform.GetChild(3).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(/*data[i].id */"Icons" + "/" + foodItem.totalEnergyKcaStandard);
            toggleObj.transform.GetChild(5).GetComponent<Text>().text = foodItem.protein.ToString();
            //toggleObj.transform.GetChild(4).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(/*data[i].id */"Icons" + "/" + foodItem.proteinStandard);
            toggleObj.transform.GetChild(8).GetComponent<Text>().text = foodItem.fat.ToString();
            //toggleObj.transform.GetChild(5).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(/*data[i].id */"Icons" + "/" + foodItem.fatStandard);
            toggleObj.transform.GetChild(11).GetComponent<Text>().text = foodItem.cho.ToString();

        }
        GameObject totalTgl1 = Instantiate(allFoodItemprefab, allFoodPartent);
        totalTgl1.SetActive(true);
        totalTgl1.transform.GetChild(0).GetComponent<Text>().text = "热量";
        totalTgl1.transform.GetChild(1).GetComponent<Text>().text = totalEnergy.totalEnergyKcal;
        GameObject totalTgl2 = Instantiate(allFoodItemprefab, allFoodPartent);
        totalTgl2.SetActive(true);
        totalTgl2.transform.GetChild(0).GetComponent<Text>().text = "蛋白质";
        totalTgl2.transform.GetChild(1).GetComponent<Text>().text = totalEnergy.protein;
        GameObject totalTgl3 = Instantiate(allFoodItemprefab, allFoodPartent);
        totalTgl3.SetActive(true);
        totalTgl3.transform.GetChild(0).GetComponent<Text>().text = "脂肪";
        totalTgl3.transform.GetChild(1).GetComponent<Text>().text = totalEnergy.fat;
        GameObject totalTgl4 = Instantiate(allFoodItemprefab, allFoodPartent);
        totalTgl4.SetActive(true);
        totalTgl4.transform.GetChild(0).GetComponent<Text>().text = "碳水化合物";
        totalTgl4.transform.GetChild(1).GetComponent<Text>().text = totalEnergy.cho;


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
            //itemRec.mealPeriod = mealName;
            GameObject recipeObj = Instantiate(recipePrefab, foodParent);
            recipeObj.transform.Find("RecipeName").GetComponent<Text>().text = itemRec.recipeName;
            recipeObj.GetComponent<FoodGroupRecipeItemReport>().recipeItem = new RecipeItem();
            recipeObj.GetComponent<FoodGroupRecipeItemReport>().recipeItem = itemRec;
            recipeObj.GetComponent<FoodGroupRecipeItemReport>().recipeItem.foodRecipeGroupItems = new List<FoodRecipeGroupItem>(itemRec.foodRecipeGroupItems);

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
                allHeat += float.Parse(BackMultiplyuantity(float.Parse(food.part), float.Parse(food.heat)));
                allWeight += float.Parse(BackMultiplyuantity(float.Parse(food.part), float.Parse(food.weight)));
                allProtein += float.Parse(BackMultiplyuantity(float.Parse(food.part), float.Parse(food.protein)));
                allFat += float.Parse(BackMultiplyuantity(float.Parse(food.part), float.Parse(food.fat)));
                allCho += float.Parse(BackMultiplyuantity(float.Parse(food.part), float.Parse(food.cho)));
                //SetFoodItemText(toggleObj, "Name", itemFood.foodName);
                toggleObj.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = food.foodName;
                toggleObj.transform.Find("Count").GetComponent<Text>().text = food.part.ToString();
                toggleObj.transform.Find("Heat").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(food.part), float.Parse(food.heat));

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
        SendServerCon();
    }

    /// <summary>
    /// 删除食谱 进行刷新
    /// </summary>
    public void DelRecipeGroup(RecipeItem recipeItemModify)
    {
        if (recipeListDic[recipeItemModify.mealPeriod].recipeSelectedList.Count == 1)
        {
            return;
        }
        foreach (var item in recipeListDic[recipeItemModify.mealPeriod].recipeSelectedList)
        {
            if (item.id == recipeItemModify.id)
            {
                recipeListDic[recipeItemModify.mealPeriod].recipeSelectedList.Remove(item);
            }
        }
        SumTotalEveryMeal();
        SendServerCon();

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
        SendServerCon();

    }


    /// <summary>
    /// 显示计算接收周的数据
    /// </summary>
     //储存收回来的数据 
    public void ReceiveServerInformWeek(FoodEveryMealEnergyGroupWeek response)
    {
        weekPan.SetActive(true);
        for (int i = 0; i < 7; i++)
        {
            SumTotalEveryMealWeek();

        }
        score = response.score;
        foodNum = response.foodNum;
        promptInfo = response.promptInfo;
        monday = response.monday;
        tuesday = response.tuesday;
        wednesday = response.wednesday;
        thursday = response.thursday;
        friday = response.friday;
        saturday = response.saturday;
        sunday = response.sunday;
        total = response.total;

        fiberAndFineProtein = response.fiberAndFineProtein;
        //ExcelExporter.Instance.response = response;
 
        UIManager.Instance.CloseUICaoZuo("ChooseGroupShanShiUI");
        SumTotalEveryMeal();
    }
    GroupWeekInDayEnergy monday = new GroupWeekInDayEnergy();
    GroupWeekInDayEnergy tuesday = new GroupWeekInDayEnergy();
    GroupWeekInDayEnergy wednesday = new GroupWeekInDayEnergy();
    GroupWeekInDayEnergy thursday = new GroupWeekInDayEnergy();
    GroupWeekInDayEnergy friday = new GroupWeekInDayEnergy();
    GroupWeekInDayEnergy saturday = new GroupWeekInDayEnergy();
    GroupWeekInDayEnergy sunday = new GroupWeekInDayEnergy();
    GroupWeekInDayEnergy total = new GroupWeekInDayEnergy();

    //一天总量
    public void SumTotalEveryMealWeek()
    {
        //foreach (Transform child in everyMealParentWeek)
        //{
        //    Destroy(child.gameObject);
        //}
        foreach (Transform child in allFoodPartent)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < weekMealList.Length; i++)
        {
            //GameObject toggleObj = Instantiate(mealObjPerfab, everyMealParentWeek);

            SumTotalRecipeFood(recipeListDic[weekMealList[i]].recipeSelectedList, weekMealContentList[i], "0");
            switch (i)
            {
                case 2:
                    ShowMealIns(monday, weekGroupAllFoodParent[0]);

                    break;
                case 5:
                    ShowMealIns(tuesday, weekGroupAllFoodParent[1]);

                    break;
                case 8:
                    ShowMealIns(wednesday, weekGroupAllFoodParent[2]);
                    break;
                case 11:
                    ShowMealIns(thursday, weekGroupAllFoodParent[3]);
                    break;
                case 14:
                    ShowMealIns(friday, weekGroupAllFoodParent[4]);
                    break;
                case 17:
                    ShowMealIns(saturday, weekGroupAllFoodParent[5]);
                    break;
                case 20:
                    ShowMealIns(sunday, weekGroupAllFoodParent[6]);
                    break;

                default:
                    break;
            }
        }
        //allFoodPartentWeek;
        //allFoodItemprefabWeek;
        GameObject totalTgl1 = Instantiate(allFoodItemprefab, allFoodPartent);
        totalTgl1.SetActive(true);
        totalTgl1.transform.GetChild(0).GetComponent<Text>().text = "热量";
        totalTgl1.transform.GetChild(1).GetComponent<Text>().text = total.totalEnergy.totalEnergyKcal;
        GameObject totalTgl2 = Instantiate(allFoodItemprefab, allFoodPartent);
        totalTgl2.SetActive(true);
        totalTgl2.transform.GetChild(0).GetComponent<Text>().text = "蛋白质";
        totalTgl2.transform.GetChild(1).GetComponent<Text>().text = total.totalEnergy.protein;
        GameObject totalTgl3 = Instantiate(allFoodItemprefab, allFoodPartent);
        totalTgl3.SetActive(true);
        totalTgl3.transform.GetChild(0).GetComponent<Text>().text = "脂肪";
        totalTgl3.transform.GetChild(1).GetComponent<Text>().text = total.totalEnergy.fat;
        GameObject totalTgl4 = Instantiate(allFoodItemprefab, allFoodPartent);
        totalTgl4.SetActive(true);
        totalTgl4.transform.GetChild(0).GetComponent<Text>().text = "碳水化合物";
        totalTgl4.transform.GetChild(1).GetComponent<Text>().text = total.totalEnergy.cho;
    }

    private void ShowMealIns(GroupWeekInDayEnergy day, Transform content)
    {

        GameObject totalTgl1 = Instantiate(allFoodItemprefab, content);
        totalTgl1.SetActive(true);
        totalTgl1.transform.GetChild(0).GetComponent<Text>().text = "热量";
        totalTgl1.transform.GetChild(1).GetComponent<Text>().text = day.totalEnergy.totalEnergyKcal;
        GameObject totalTgl2 = Instantiate(allFoodItemprefab, content);
        totalTgl2.SetActive(true);
        totalTgl2.transform.GetChild(0).GetComponent<Text>().text = "蛋白质";
        totalTgl2.transform.GetChild(1).GetComponent<Text>().text = day.totalEnergy.protein;
        GameObject totalTgl3 = Instantiate(allFoodItemprefab, content);
        totalTgl3.SetActive(true);
        totalTgl3.transform.GetChild(0).GetComponent<Text>().text = "脂肪";
        totalTgl3.transform.GetChild(1).GetComponent<Text>().text = day.totalEnergy.fat;
        GameObject totalTgl4 = Instantiate(allFoodItemprefab, content);
        totalTgl4.SetActive(true);
        totalTgl4.transform.GetChild(0).GetComponent<Text>().text = "碳水化合物";
        totalTgl4.transform.GetChild(1).GetComponent<Text>().text = day.totalEnergy.cho;


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
