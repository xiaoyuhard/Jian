using DG.Tweening;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using RTS;
using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.UI;


//using static UnityEditor.Progress;
//using static UnityEngine.Rendering.DebugUI;


/// <summary>
/// 群体选择食谱界面
/// </summary>
public class FoodGroupChooseUI : UICaoZuoBase
{
    public static FoodGroupChooseUI Instance { get; private set; }

    [System.Serializable]
    public class WeekDayMealInf
    {
        public string mealName;
        public Toggle mealTgl;
        public Transform content;
        public GameObject mealHeatObj;
    }
    // 新增加的数据结构
    [System.Serializable]
    public class DayMealGroup
    {
        public string day;            // 星期几（如"周一"）
        public List<WeekDayMealInf> meals = new List<WeekDayMealInf>(); // 对应三餐
    }

    //public GameObject dayObj;           //切换成天选择
    //public GameObject weekObj;          //切换成周选择
    public GameObject dayObj;           //选择天的界面
    public GameObject weekObj;          //选择周的界面
    public Dropdown dayOrWeekDrop;  //天周切换Dropdown

    public Button btnChooseEnd;     //选择结束提交按钮
    public GameObject totalObj;     //显示总量的 obj

    public Toggle brackTgl;
    public Toggle lunchTgl;
    public Toggle dinnerTgl;
    public Toggle monTgl;
    public Toggle tuesTgl;
    public Toggle webTgl;
    public Toggle thurTgl;
    public Toggle firTgl;
    public Toggle satTgl;
    public Toggle sunTgl;

    public List<Toggle> weekDayTglList;             //每天的tgl
    public List<Toggle> weekMealTglList;            //每餐的tgl 点击切换并选择
    public List<Transform> weekMealContentList;     //每餐的content
    public List<GameObject> weekMealHeatObj;        //周里每餐显示总的数据

    private string[] mealList = { "早餐", "午餐", "晚餐" };
    private string[] weekMealList = { "周一早餐", "周一午餐", "周一晚餐", "周二早餐", "周二午餐", "周二晚餐", "周三早餐", "周三午餐", "周三晚餐", "周四早餐", "周四午餐", "周四晚餐", "周五早餐", "周五午餐", "周五晚餐", "周六早餐", "周六午餐", "周六晚餐", "周日早餐", "周日午餐", "周日晚餐" };

    List<WeekDayMealInf> weekMealInfs = new List<WeekDayMealInf>();

    private Dictionary<string, DayMealGroup> dayMealDict = new Dictionary<string, DayMealGroup>();

    public TMP_InputField recipeInput;          //搜索查询
    public Button recipeBtn;


    public GameObject contentRecipe;
    public GameObject recipeItem;           //食谱选择列表的item

    public GameObject alterRecipeGroupUI;   //食谱选择UI界面
    public GameObject modifyRecipeGroupUI;   //食谱修改数量UI界面
    public GameObject swopRecipeGroupUI;   //食谱交换UI界面

    public Transform mealContent;
    public GameObject recipeItemPrefab;     //选完列表的item
    public GameObject foodRecipeItemPrefab;

    public GameObject tipText;       //没有选择哪一餐 提示

    private Dictionary<string, RecipeGroup> recipeListDic = new Dictionary<string, RecipeGroup>();      //保存每餐所对应的选择食谱列表和 已经选择了的食谱

    Transform contentMeal;      //本次选择的是哪一餐 所对应的实例化食谱content缓存;

    private Queue<GameObject> toggleFoodRecipePool = new Queue<GameObject>();  //食谱对象池

    List<RecipeItem> recipeItems = new List<RecipeItem>();
    RecipeItem recipeItemCache;//选择中的食品的缓存
    GameObject meatHeatObj;     //每餐总的能力显示缓存
    string mealName = "";
    void Awake()
    {
        InitializeManager();

        for (int i = 0; i < weekMealList.Length; i++)
        {
            WeekDayMealInf mealInf = new WeekDayMealInf();
            mealInf.mealName = weekMealList[i];
            mealInf.mealTgl = weekMealTglList[i];
            mealInf.content = weekMealContentList[i];
            mealInf.mealHeatObj = weekMealHeatObj[i];

            weekMealInfs.Add(mealInf);
        }
        // 初始化每日三餐字典
        foreach (var mealName in weekMealList)
        {
            // 解析星期几（前2个字符）
            string day = mealName.Substring(0, 2);

            if (!dayMealDict.ContainsKey(day))
            {
                dayMealDict.Add(day, new DayMealGroup { day = day });
            }

            // 找到对应的WeekMealInf（假设已初始化）
            var mealInf = weekMealInfs.Find(m => m.mealName == mealName);
            if (mealInf != null)
            {
                dayMealDict[day].meals.Add(mealInf);
            }
        }
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
    /// 把数据字典传到报告
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, RecipeGroup> BackRecipeDic()
    {
        return recipeListDic;
    }


    private void OnEnable()
    {
        recipeItems = FoodManager.Instance.response.rows;
        ResRecipeDic();

        //RefreshFoodRecipeList(recipeItems);
        OnTglResFood("早餐", true, mealContent, totalObj);

        //mealName = "早餐";
        brackTgl.isOn = true;
        recipeInput.text = "";
        //contentMeal = mealContent;
        // 初始化：根据默认选项显示
        UpdateVisibility(dayOrWeekDrop.value);

        // 监听下拉菜单的选项变化
        dayOrWeekDrop.onValueChanged.AddListener(OnDayOrWeekChanged);

    }
    // 当选项变化时触发
    private void OnDayOrWeekChanged(int index)
    {
        UpdateVisibility(index);
    }

    // 根据选项显示/隐藏物体
    private void UpdateVisibility(int index)
    {
        // 如果是"天"选项（假设索引0对应"天"）
        bool isDay = (index == 0);
        if (index == 0)
        {
            dayOrWeek = false;
            contentMeal = mealContent;
            dayObj.SetActive(true);
            weekObj.SetActive(false);
        }
        else
        {
            dayObj.SetActive(false);
            weekObj.SetActive(true);
            dayOrWeek = true;
            monTgl.isOn = true;
            contentMeal = null;
            // 设置默认显示周一
            //UpdateDayDisplay("周一");
            //OnTglResFood("周一早餐", true, mealContent);
            var currentGroup = recipeListDic["周一早餐"];
            RefreshFoodRecipeList(currentGroup.recipeShowList);
            ResRecipeCon("周一早餐");
        }
        //dayObj.SetActive(isDay);
        //weekObj.SetActive(!isDay);
    }

    void Start()
    {
        recipeBtn.onClick.AddListener(OnSearchRecipe);

        btnChooseEnd.onClick.AddListener(ClickChooseEnd);

        //// 绑定周选择Toggle事件
        //for (int i = 0; i < weekDayTglList.Count; i++)
        //{
        //    string targetDay = GetDayByIndex(i);
        //    weekDayTglList[i].onValueChanged.AddListener(isOn =>
        //    {
        //        if (isOn) UpdateDayDisplay(targetDay);
        //    });
        //}

        brackTgl.onValueChanged.AddListener((isOn) =>
                OnTglResFood("早餐", isOn, mealContent, totalObj));
        lunchTgl.onValueChanged.AddListener((isOn) =>
               OnTglResFood("午餐", isOn, mealContent, totalObj));
        dinnerTgl.onValueChanged.AddListener((isOn) =>
               OnTglResFood("晚餐", isOn, mealContent, totalObj));

        foreach (var item in weekMealInfs)
        {
            item.mealTgl.onValueChanged.AddListener((isOn) =>
                OnTglResFood(item.mealName, isOn, item.content, item.mealHeatObj));
        }


    }
    // 更新显示逻辑
    private void UpdateDayDisplay(string targetDay)
    {
        contentMeal = null;

        foreach (var pair in dayMealDict)
        {
            bool isTargetDay = pair.Key == targetDay;

            foreach (var meal in pair.Value.meals)
            {
                // 控制显隐（根据实际需求选择以下方式之一）

                // 方式1：直接控制GameObject显隐
                //meal.content.gameObject.SetActive(isTargetDay);

                // 方式2：通过Toggle状态控制（需解除ToggleGroup）
                // meal.mealTgl.gameObject.SetActive(isTargetDay);
                // if(isTargetDay) meal.mealTgl.isOn = true;
            }
        }

        // 可选：滚动到顶部
        ScrollRect scroll = GetComponent<ScrollRect>();
        if (scroll) scroll.normalizedPosition = new Vector2(0, 1);
    }

    // 工具方法
    private string GetDayByIndex(int index)
    {
        string[] days = { "周一", "周二", "周三", "周四", "周五", "周六", "周日" };
        return days[Mathf.Clamp(index, 0, 6)];
    }




    private void ClickChooseEnd()
    {
        if (!dayOrWeek)
        {
            if (IsMealListNull())
            {
                SendServerCon();
                UIManager.Instance.OpenUICaoZuo("MealGroupReportUI");

            }
        }
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
        //foodSendConverDay.userInfo = userInfo;
        //ServerCon.Instance.ConverToJsonPost(SerializeData(foodSendConverDay), "/analyse/intake");
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

                targetDict.Add(sourceFood.foodCode, sourceFood);
            }
        }
    }


    /// <summary>
    /// 进行判断一日里三餐是否都至少有一个
    /// </summary>
    private bool IsMealListNull()
    {
        bool isMealNull = false;
        if (recipeListDic["早餐"].recipeSelectedList.Count > 0 && recipeListDic["午餐"].recipeSelectedList.Count > 0 && recipeListDic["晚餐"].recipeSelectedList.Count > 0)
        {
            isMealNull = true;
        }

        return isMealNull;
    }
    bool dayOrWeek = false;


    private void Update()
    {


    }

    public void ResRecipeDic()
    {
        recipeListDic.Clear();
        foreach (var item in mealList)
        {
            //RecipeGroup recipeGroup = new RecipeGroup();
            //List<RecipeItem> recipeItems = new List<RecipeItem>(FoodManager.Instance.response.rows);
            // 关键修改：使用深拷贝初始化
            List<RecipeItem> recipeItems = FoodManager.Instance.response.rows
                .Select(r => r.Clone()) // 调用深拷贝方法
                .ToList();
            //recipeGroup.recipeShowList = recipeItems;
            //recipeGroup.recipeSelectedList = new List<RecipeItem>();
            //recipeListDic.Add(item, recipeGroup);
            recipeListDic.Add(item, new RecipeGroup
            {
                recipeShowList = recipeItems,
                recipeSelectedList = new List<RecipeItem>()
            });
        }

        foreach (var item in weekMealList)
        {

            List<RecipeItem> recipeItems = FoodManager.Instance.response.rows
                .Select(r => r.Clone()) // 调用深拷贝方法
                .ToList();

            recipeListDic.Add(item, new RecipeGroup
            {
                recipeShowList = recipeItems,
                recipeSelectedList = new List<RecipeItem>()
            });
        }
    }

    /// <summary>
    /// 点击按钮切换时间 同时切换
    /// </summary>
    private void OnTglResFood(string foodType, bool isOn, Transform content, GameObject mealObj)
    {
        // 只处理开启状态
        if (!isOn) return;
        mealName = foodType;
        //recipeListDic[foodType].recipeShowList = recipeItems;
        //List<RecipeItem> foodRecipeList = recipeListDic[foodType].recipeShowList;
        var currentGroup = recipeListDic[foodType];
        contentMeal = content;
        meatHeatObj = mealObj;
        ResRecipeCon(foodType);
        RefreshFoodRecipeList(currentGroup.recipeShowList);

    }


    /// <summary>
    /// 食谱的刷新加载
    /// </summary>
    /// <param name="foods"></param>
    /// <param name="filter"></param>
    void RefreshFoodRecipeList(List<RecipeItem> recipeItems, string filter = "")
    {
        for (int i = 0; i < contentRecipe.transform.childCount; i++)
        {
            Transform child = contentRecipe.transform.GetChild(i);
            child.gameObject.SetActive(false);
            toggleFoodRecipePool.Enqueue(child.gameObject);
        }

        // 创建/复用Toggle
        foreach (var food in recipeItems)
        {
            // 从池中获取或实例化新Toggle
            GameObject toggleObj = GetPooledRecipeToggle();
            food.itemObj = toggleObj;
            Toggle toggle = toggleObj.GetComponent<Toggle>();
            Text text1 = toggleObj.transform.Find("Name").GetComponent<Text>();
            Text code = toggleObj.transform.Find("IncludingFood").GetComponent<Text>();

            // 重置Toggle状态
            toggle.onValueChanged.RemoveAllListeners();
            toggle.interactable = !food.isOnClick;
            toggle.isOn = food.isOnClick;
            text1.text = food.recipeName;
            code.text = food.includingFood;

            // 绑定新事件
            toggle.onValueChanged.AddListener(isOn =>
            {
                if (isOn) OnFoodRecipeSelected(food, toggle, isOn);
            });
        }
    }

    GameObject GetPooledRecipeToggle()
    {
        //// 尝试从池中获取可用对象
        if (toggleFoodRecipePool.Count > 0)
        {
            var obj = toggleFoodRecipePool.Dequeue();
            if (obj != null)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // 没有可用对象时创建新实例
        GameObject newToggle = Instantiate(recipeItem, contentRecipe.transform);
        newToggle.SetActive(true);
        return newToggle;
    }

    // 食谱被选中时
    void OnFoodRecipeSelected(RecipeItem food, Toggle tglItem, bool isOn)
    {
        if (isOn)
        {
            if (contentMeal == null)
            {
                tipText.gameObject.SetActive(true);
                StartCoroutine(WaitCloseTip());
                return;
            }
            if (food.isOnClick) return;
            //tglItem.interactable = false;

            //tglItem.isOn = true;
            //food.isOnClick = true;
            ServerCon.Instance.LoadRecipe("/cookbook/getGroupRecipeFood", $"?recipeId={food.id}", "Choose");
            recipeItemCache = food;
            recipeItemCache.itenTgl = tglItem;
        }
    }

    // 显示/隐藏编辑面板
    void ShowRecipeGroupEditPanel(bool show, List<FoodRecipeGroupItem> food)
    {
        alterRecipeGroupUI.SetActive(show);
        AlterRecipeGroupUI.Instance.UpFoodItem(food, recipeItemCache.recipeName);
        recipeItemCache.itenTgl.interactable = false;
        recipeItemCache.itenTgl.isOn = true;
        recipeItemCache.isOnClick = true;
    }


    /// <summary>
    /// 接收到从服务器发回来的食材数据
    /// </summary>
    /// <param name="food"></param>
    public void ReceiveRecipeItem(List<FoodRecipeGroupItem> food)
    {
        ShowRecipeGroupEditPanel(true, food);
    }

    /// <summary>
    /// 选择食物后添加到选择完食物的列表并刷新
    /// </summary>
    public void ReciveAlterRecipeGroupUI(List<FoodRecipeGroupItem> food)
    {
        RecipeItem recipeItem = new RecipeItem();
        recipeItem.mealPeriod = mealName;
        recipeItem.id = recipeItemCache.id;
        recipeItem.recipeName = recipeItemCache.recipeName;
        recipeItem.foodRecipeGroupItems = food;
        recipeItemCache = null;

        recipeListDic[mealName].recipeSelectedList.Add(recipeItem);
        RefreshAddFoodRecipeList(food, recipeItem);

    }

    /// <summary>
    /// 刷新已添加的食谱列表
    /// </summary>
    private void ResRecipeCon(string foodType)
    {
        if (contentMeal != null)
        {
            foreach (Transform child in contentMeal)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (var item in recipeListDic[foodType].recipeSelectedList)
        {
            RefreshAddFoodRecipeList(item.foodRecipeGroupItems, item);
        }

        CalculateHeat(recipeListDic[foodType].recipeSelectedList, meatHeatObj);
    }

    private void CalculateHeat(List<RecipeItem> foods, GameObject totalMealObj)
    {
        float totalTodayProtein = 0;
        float totalTodayFat = 0;
        float totalTodayCho = 0;
        float totalTodayHeat = 0;
        foreach (var food in foods)
        {
            foreach (var item in food.foodRecipeGroupItems)
            {
                totalTodayProtein += float.Parse(item.protein) * float.Parse(item.part);
                totalTodayFat += float.Parse(item.fat) * float.Parse(item.part);
                totalTodayCho += float.Parse(item.cho) * float.Parse(item.part);
                totalTodayHeat += float.Parse(item.heat) * float.Parse(item.heat);
            }
        }


        totalMealObj.transform.GetChild(2).GetComponent<Text>().text = totalTodayHeat.ToString("F2");
        totalMealObj.transform.GetChild(5).GetComponent<Text>().text = totalTodayProtein.ToString("F2");
        totalMealObj.transform.GetChild(8).GetComponent<Text>().text = totalTodayFat.ToString("F2");
        totalMealObj.transform.GetChild(11).GetComponent<Text>().text = totalTodayCho.ToString("F2");
    }

    /// <summary>
    /// 实例化单个食谱数据
    /// </summary>
    void RefreshAddFoodRecipeList(List<FoodRecipeGroupItem> foods, RecipeItem recipe)
    {
        GameObject obj = Instantiate(recipeItemPrefab, contentMeal);
        obj.SetActive(true);

        obj.transform.Find("RecipeName").GetComponent<Text>().text = recipe.recipeName;
        obj.GetComponent<FoodGroupRecipeItem>().recipeItem = new RecipeItem();
        obj.GetComponent<FoodGroupRecipeItem>().recipeItem = recipe;
        obj.GetComponent<FoodGroupRecipeItem>().recipeItem.foodRecipeGroupItems = new List<FoodRecipeGroupItem>(recipe.foodRecipeGroupItems);
        float allCount = 0;
        float allHeat = 0;
        float allWeight = 0;
        float allProtein = 0;
        float allFat = 0;
        float allCho = 0;
        // 创建新的Toggle
        foreach (var food in foods)
        {
            GameObject toggleObj = Instantiate(foodRecipeItemPrefab, obj.transform.GetChild(0).GetChild(0));
            toggleObj.SetActive(true);
            allCount += float.Parse(food.part);
            allHeat += float.Parse(BackMultiplyuantity(float.Parse(food.part), float.Parse(food.weight)));
            allWeight += float.Parse(BackMultiplyuantity(float.Parse(food.part), float.Parse(food.heat)));
            allProtein += float.Parse(BackMultiplyuantity(float.Parse(food.part), float.Parse(food.protein)));
            allFat += float.Parse(BackMultiplyuantity(float.Parse(food.part), float.Parse(food.fat)));
            allCho += float.Parse(BackMultiplyuantity(float.Parse(food.part), float.Parse(food.cho)));
            toggleObj.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = food.foodName;
            toggleObj.transform.Find("Count").GetComponent<Text>().text = food.part.ToString();
            toggleObj.transform.Find("Heat").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(food.part), float.Parse(food.weight));
            toggleObj.transform.Find("Weight").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(food.part), float.Parse(food.heat));
            toggleObj.transform.Find("Protein").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(food.part), float.Parse(food.protein));
            toggleObj.transform.Find("Fat").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(food.part), float.Parse(food.fat));
            toggleObj.transform.Find("carbohydrate").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(food.part), float.Parse(food.cho));

        }
        Transform allOjb = obj.transform.Find("AllFoodHeat").GetComponent<Transform>();
        allOjb.transform.Find("Count").GetComponent<Text>().text = allCount.ToString();
        allOjb.transform.Find("Heat").GetComponent<Text>().text = allHeat.ToString();
        allOjb.transform.Find("Weight").GetComponent<Text>().text = allWeight.ToString();
        allOjb.transform.Find("Protein").GetComponent<Text>().text = allProtein.ToString();
        allOjb.transform.Find("Fat").GetComponent<Text>().text = allFat.ToString();
        allOjb.transform.Find("carbohydrate").GetComponent<Text>().text = allCho.ToString();


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
        ResRecipeCon(recipeItemModify.mealPeriod);
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
                foreach (var meal in recipeListDic[recipeItemModify.mealPeriod].recipeShowList)
                {
                    if (meal.id == item.id)
                    {
                        meal.isOnClick = false;

                        meal.itemObj.GetComponent<Toggle>().interactable = !meal.isOnClick;
                        meal.itemObj.GetComponent<Toggle>().isOn = meal.isOnClick;
                        break;
                    }
                }
                break;
            }
        }
        ResRecipeCon(recipeItemModify.mealPeriod);

    }
    /// <summary>
    /// 食谱查询并交换
    /// </summary>
    /// <param name="oldRecipe">旧的食谱</param>
    /// <param name="newRecipe">点击后新的食谱</param>
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
        ResRecipeCon(newRecipe.mealPeriod);
        swopRecipeGroupUI.SetActive(false);

    }


    public string BackMultiplyuantity(float quantity, float parameters)
    {
        return ((quantity * parameters) /*/ 100*/).ToString("F2");
    }

    /// <summary>
    /// 删除选择完的数据里 对应的餐的对应食谱  把整个食谱传过来 然后进行查找本食谱中是哪天的哪一餐 
    /// </summary>
    public void DelFoodRecipeItme()
    {


    }


    public List<RecipeItem> BackRecipeGroupItem(string recipeName)
    {
        if (string.IsNullOrEmpty(recipeName))
        {
            return null;
        }
        // 2. 安全获取食谱组
        if (!recipeListDic.TryGetValue(mealName, out RecipeGroup recipeGroup))
        {
            Debug.LogWarning($"找不到 {mealName} 对应的食谱组");
            return null;
        }
        // 3. 检查食谱列表有效性
        if (recipeGroup?.recipeShowList == null || recipeGroup.recipeShowList.Count == 0)
        {
            Debug.LogWarning($"{mealName} 的食谱列表为空或未初始化");
            return null;
        }
        return recipeGroup.recipeShowList.Where(f => f.recipeName.Contains(recipeName)).ToList();

    }

    /// <summary>
    /// 食谱搜索查询
    /// </summary>
    private void OnSearchRecipe()
    {
        string keyword = recipeInput.text;
        //if (string.IsNullOrEmpty(keyword))
        //{
        //    RefreshFoodRecipeList(recipeItems);

        //}
        //else
        //{
        //    var filtered = recipeItems.Where(f => f.recipeName.Contains(keyword)).ToList();
        //    RefreshFoodRecipeList(filtered);

        //}
        var currentRecipeList = recipeListDic[mealName].recipeShowList; // 获取当前餐段数据

        if (string.IsNullOrEmpty(keyword))
        {
            RefreshFoodRecipeList(currentRecipeList);
        }
        else
        {
            var filtered = currentRecipeList
                .Where(f => f.recipeName.Contains(keyword))
                .ToList();
            RefreshFoodRecipeList(filtered);
        }
    }
    /// <summary>
    /// 刷新新的数据
    /// </summary>
    public void ForceRefreshAllMeals()
    {
        foreach (var meal in mealList)
        {
            recipeListDic[meal].recipeShowList = FoodManager.Instance.response.rows
                .Select(r => r.Clone())
                .ToList();
        }
        RefreshFoodRecipeList(recipeListDic[mealName].recipeShowList);
    }


    IEnumerator WaitCloseTip()
    {
        yield return new WaitForSeconds(2f);
        tipText.gameObject.SetActive(false);

    }
}
