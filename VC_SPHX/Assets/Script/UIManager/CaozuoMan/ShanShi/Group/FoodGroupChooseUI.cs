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
/// Ⱥ��ѡ��ʳ�׽���
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
    // �����ӵ����ݽṹ
    [System.Serializable]
    public class DayMealGroup
    {
        public string day;            // ���ڼ�����"��һ"��
        public List<WeekDayMealInf> meals = new List<WeekDayMealInf>(); // ��Ӧ����
    }

    //public GameObject dayObj;           //�л�����ѡ��
    //public GameObject weekObj;          //�л�����ѡ��
    public GameObject dayObj;           //ѡ����Ľ���
    public GameObject weekObj;          //ѡ���ܵĽ���
    public Dropdown dayOrWeekDrop;  //�����л�Dropdown

    public Button btnChooseEnd;     //ѡ������ύ��ť
    public GameObject totalObj;     //��ʾ������ obj

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

    public List<Toggle> weekDayTglList;             //ÿ���tgl
    public List<Toggle> weekMealTglList;            //ÿ�͵�tgl ����л���ѡ��
    public List<Transform> weekMealContentList;     //ÿ�͵�content
    public List<GameObject> weekMealHeatObj;        //����ÿ����ʾ�ܵ�����

    private string[] mealList = { "���", "���", "���" };
    private string[] weekMealList = { "��һ���", "��һ���", "��һ���", "�ܶ����", "�ܶ����", "�ܶ����", "�������", "�������", "�������", "�������", "�������", "�������", "�������", "�������", "�������", "�������", "�������", "�������", "�������", "�������", "�������" };

    List<WeekDayMealInf> weekMealInfs = new List<WeekDayMealInf>();

    private Dictionary<string, DayMealGroup> dayMealDict = new Dictionary<string, DayMealGroup>();

    public TMP_InputField recipeInput;          //������ѯ
    public Button recipeBtn;


    public GameObject contentRecipe;
    public GameObject recipeItem;           //ʳ��ѡ���б��item

    public GameObject alterRecipeGroupUI;   //ʳ��ѡ��UI����
    public GameObject modifyRecipeGroupUI;   //ʳ���޸�����UI����
    public GameObject swopRecipeGroupUI;   //ʳ�׽���UI����

    public Transform mealContent;
    public GameObject recipeItemPrefab;     //ѡ���б��item
    public GameObject foodRecipeItemPrefab;

    public GameObject tipText;       //û��ѡ����һ�� ��ʾ

    private Dictionary<string, RecipeGroup> recipeListDic = new Dictionary<string, RecipeGroup>();      //����ÿ������Ӧ��ѡ��ʳ���б�� �Ѿ�ѡ���˵�ʳ��

    Transform contentMeal;      //����ѡ�������һ�� ����Ӧ��ʵ����ʳ��content����;

    private Queue<GameObject> toggleFoodRecipePool = new Queue<GameObject>();  //ʳ�׶����

    List<RecipeItem> recipeItems = new List<RecipeItem>();
    RecipeItem recipeItemCache;//ѡ���е�ʳƷ�Ļ���
    GameObject meatHeatObj;     //ÿ���ܵ�������ʾ����
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
        // ��ʼ��ÿ�������ֵ�
        foreach (var mealName in weekMealList)
        {
            // �������ڼ���ǰ2���ַ���
            string day = mealName.Substring(0, 2);

            if (!dayMealDict.ContainsKey(day))
            {
                dayMealDict.Add(day, new DayMealGroup { day = day });
            }

            // �ҵ���Ӧ��WeekMealInf�������ѳ�ʼ����
            var mealInf = weekMealInfs.Find(m => m.mealName == mealName);
            if (mealInf != null)
            {
                dayMealDict[day].meals.Add(mealInf);
            }
        }
    }

    // ��ʽ��ʼ�����������༭�����ã�
    public void InitializeManager()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    /// <summary>
    /// �������ֵ䴫������
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
        OnTglResFood("���", true, mealContent, totalObj);

        //mealName = "���";
        brackTgl.isOn = true;
        recipeInput.text = "";
        //contentMeal = mealContent;
        // ��ʼ��������Ĭ��ѡ����ʾ
        UpdateVisibility(dayOrWeekDrop.value);

        // ���������˵���ѡ��仯
        dayOrWeekDrop.onValueChanged.AddListener(OnDayOrWeekChanged);

    }
    // ��ѡ��仯ʱ����
    private void OnDayOrWeekChanged(int index)
    {
        UpdateVisibility(index);
    }

    // ����ѡ����ʾ/��������
    private void UpdateVisibility(int index)
    {
        // �����"��"ѡ���������0��Ӧ"��"��
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
            // ����Ĭ����ʾ��һ
            //UpdateDayDisplay("��һ");
            //OnTglResFood("��һ���", true, mealContent);
            var currentGroup = recipeListDic["��һ���"];
            RefreshFoodRecipeList(currentGroup.recipeShowList);
            ResRecipeCon("��һ���");
        }
        //dayObj.SetActive(isDay);
        //weekObj.SetActive(!isDay);
    }

    void Start()
    {
        recipeBtn.onClick.AddListener(OnSearchRecipe);

        btnChooseEnd.onClick.AddListener(ClickChooseEnd);

        //// ����ѡ��Toggle�¼�
        //for (int i = 0; i < weekDayTglList.Count; i++)
        //{
        //    string targetDay = GetDayByIndex(i);
        //    weekDayTglList[i].onValueChanged.AddListener(isOn =>
        //    {
        //        if (isOn) UpdateDayDisplay(targetDay);
        //    });
        //}

        brackTgl.onValueChanged.AddListener((isOn) =>
                OnTglResFood("���", isOn, mealContent, totalObj));
        lunchTgl.onValueChanged.AddListener((isOn) =>
               OnTglResFood("���", isOn, mealContent, totalObj));
        dinnerTgl.onValueChanged.AddListener((isOn) =>
               OnTglResFood("���", isOn, mealContent, totalObj));

        foreach (var item in weekMealInfs)
        {
            item.mealTgl.onValueChanged.AddListener((isOn) =>
                OnTglResFood(item.mealName, isOn, item.content, item.mealHeatObj));
        }


    }
    // ������ʾ�߼�
    private void UpdateDayDisplay(string targetDay)
    {
        contentMeal = null;

        foreach (var pair in dayMealDict)
        {
            bool isTargetDay = pair.Key == targetDay;

            foreach (var meal in pair.Value.meals)
            {
                // ��������������ʵ������ѡ�����·�ʽ֮һ��

                // ��ʽ1��ֱ�ӿ���GameObject����
                //meal.content.gameObject.SetActive(isTargetDay);

                // ��ʽ2��ͨ��Toggle״̬���ƣ�����ToggleGroup��
                // meal.mealTgl.gameObject.SetActive(isTargetDay);
                // if(isTargetDay) meal.mealTgl.isOn = true;
            }
        }

        // ��ѡ������������
        ScrollRect scroll = GetComponent<ScrollRect>();
        if (scroll) scroll.normalizedPosition = new Vector2(0, 1);
    }

    // ���߷���
    private string GetDayByIndex(int index)
    {
        string[] days = { "��һ", "�ܶ�", "����", "����", "����", "����", "����" };
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
    /// ѡ������������ݷ���������
    /// </summary>
    public void SendServerCon()
    {
        List<FoodRecipeGroupItem> breakMealList = MergeAllFoods(recipeListDic["���"].recipeSelectedList);
        List<FoodRecipeGroupItem> lunchMealList = MergeAllFoods(recipeListDic["���"].recipeSelectedList);
        List<FoodRecipeGroupItem> dinnerMealList = MergeAllFoods(recipeListDic["���"].recipeSelectedList);

        FoodSendConverDay foodSendConverDay = new FoodSendConverDay();
        foodSendConverDay.breakfast = AddFoodSendConverList(breakMealList);
        foodSendConverDay.lunch = AddFoodSendConverList(lunchMealList);
        foodSendConverDay.dinner = AddFoodSendConverList(dinnerMealList);
        //foodSendConverDay.userInfo = userInfo;
        //ServerCon.Instance.ConverToJsonPost(SerializeData(foodSendConverDay), "/analyse/intake");
    }
    private string SerializeData(FoodSendConverDay data)
    {
        // ����һ��ʹ�� Unity ���� JsonUtility����Ҫ��װ�ࣩ
        Wrapper<FoodSendConverDay> wrapper = new Wrapper<FoodSendConverDay> { items = data };
        //return JsonUtility.ToJson(wrapper);

        // ��������ʹ�� Newtonsoft.Json��ֱ�����л��б�
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
        // 1. �����ֵ����ںϲ����ݣ�Key: ʳ��Ψһ��ʶ id��
        Dictionary<string, FoodRecipeGroupItem> foodDict = new Dictionary<string, FoodRecipeGroupItem>();

        // 2. �ϲ� foods �б�
        foreach (var item in chooseFoodData)
        {
            MergeFoodList(item.foodRecipeGroupItems, foodDict);

        }

        //// 3. �ϲ� recipes �е�ʳ��
        //foreach (var recipe in chooseFoodData.recipes)
        //{
        //    MergeFoodList(recipe.foodKindItems, foodDict);
        //}

        // 4. ��������� allFoods
        //chooseFoodData.allFoods = new List<FoodRecipeGroupItem>(foodDict.Values);
        return new List<FoodRecipeGroupItem>(foodDict.Values);
    }


    private void MergeFoodList(List<FoodRecipeGroupItem> sourceList, Dictionary<string, FoodRecipeGroupItem> targetDict)
    {
        foreach (var sourceFood in sourceList)
        {
            if (targetDict.TryGetValue(sourceFood.foodCode, out var existingFood))
            {
                // �ϲ� count
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
    /// �����ж�һ���������Ƿ�������һ��
    /// </summary>
    private bool IsMealListNull()
    {
        bool isMealNull = false;
        if (recipeListDic["���"].recipeSelectedList.Count > 0 && recipeListDic["���"].recipeSelectedList.Count > 0 && recipeListDic["���"].recipeSelectedList.Count > 0)
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
            // �ؼ��޸ģ�ʹ�������ʼ��
            List<RecipeItem> recipeItems = FoodManager.Instance.response.rows
                .Select(r => r.Clone()) // �����������
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
                .Select(r => r.Clone()) // �����������
                .ToList();

            recipeListDic.Add(item, new RecipeGroup
            {
                recipeShowList = recipeItems,
                recipeSelectedList = new List<RecipeItem>()
            });
        }
    }

    /// <summary>
    /// �����ť�л�ʱ�� ͬʱ�л�
    /// </summary>
    private void OnTglResFood(string foodType, bool isOn, Transform content, GameObject mealObj)
    {
        // ֻ������״̬
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
    /// ʳ�׵�ˢ�¼���
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

        // ����/����Toggle
        foreach (var food in recipeItems)
        {
            // �ӳ��л�ȡ��ʵ������Toggle
            GameObject toggleObj = GetPooledRecipeToggle();
            food.itemObj = toggleObj;
            Toggle toggle = toggleObj.GetComponent<Toggle>();
            Text text1 = toggleObj.transform.Find("Name").GetComponent<Text>();
            Text code = toggleObj.transform.Find("IncludingFood").GetComponent<Text>();

            // ����Toggle״̬
            toggle.onValueChanged.RemoveAllListeners();
            toggle.interactable = !food.isOnClick;
            toggle.isOn = food.isOnClick;
            text1.text = food.recipeName;
            code.text = food.includingFood;

            // �����¼�
            toggle.onValueChanged.AddListener(isOn =>
            {
                if (isOn) OnFoodRecipeSelected(food, toggle, isOn);
            });
        }
    }

    GameObject GetPooledRecipeToggle()
    {
        //// ���Դӳ��л�ȡ���ö���
        if (toggleFoodRecipePool.Count > 0)
        {
            var obj = toggleFoodRecipePool.Dequeue();
            if (obj != null)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // û�п��ö���ʱ������ʵ��
        GameObject newToggle = Instantiate(recipeItem, contentRecipe.transform);
        newToggle.SetActive(true);
        return newToggle;
    }

    // ʳ�ױ�ѡ��ʱ
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

    // ��ʾ/���ر༭���
    void ShowRecipeGroupEditPanel(bool show, List<FoodRecipeGroupItem> food)
    {
        alterRecipeGroupUI.SetActive(show);
        AlterRecipeGroupUI.Instance.UpFoodItem(food, recipeItemCache.recipeName);
        recipeItemCache.itenTgl.interactable = false;
        recipeItemCache.itenTgl.isOn = true;
        recipeItemCache.isOnClick = true;
    }


    /// <summary>
    /// ���յ��ӷ�������������ʳ������
    /// </summary>
    /// <param name="food"></param>
    public void ReceiveRecipeItem(List<FoodRecipeGroupItem> food)
    {
        ShowRecipeGroupEditPanel(true, food);
    }

    /// <summary>
    /// ѡ��ʳ�����ӵ�ѡ����ʳ����б�ˢ��
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
    /// ˢ������ӵ�ʳ���б�
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
    /// ʵ��������ʳ������
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
        // �����µ�Toggle
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
    /// �޸�ʳ�� ������ˢ��
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
    /// ɾ��ʳ�� ����ˢ��
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
    /// ʳ�ײ�ѯ������
    /// </summary>
    /// <param name="oldRecipe">�ɵ�ʳ��</param>
    /// <param name="newRecipe">������µ�ʳ��</param>
    public void SwopRecipeGroup(RecipeItem oldRecipe, RecipeItem newRecipe)
    {

        // ��ȡ��Ҫ�������б�
        var targetList = recipeListDic[oldRecipe.mealPeriod].recipeSelectedList;

        // ʹ�� for ѭ��ͨ�������޸�
        for (int i = 0; i < targetList.Count; i++)
        {
            if (targetList[i].id == oldRecipe.id)
            {
                targetList[i] = newRecipe; // ͨ������ֱ���滻Ԫ��
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

                break; // ���IDΨһ����ǰ�˳�
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
    /// ɾ��ѡ����������� ��Ӧ�Ĳ͵Ķ�Ӧʳ��  ������ʳ�״����� Ȼ����в��ұ�ʳ�������������һ�� 
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
        // 2. ��ȫ��ȡʳ����
        if (!recipeListDic.TryGetValue(mealName, out RecipeGroup recipeGroup))
        {
            Debug.LogWarning($"�Ҳ��� {mealName} ��Ӧ��ʳ����");
            return null;
        }
        // 3. ���ʳ���б���Ч��
        if (recipeGroup?.recipeShowList == null || recipeGroup.recipeShowList.Count == 0)
        {
            Debug.LogWarning($"{mealName} ��ʳ���б�Ϊ�ջ�δ��ʼ��");
            return null;
        }
        return recipeGroup.recipeShowList.Where(f => f.recipeName.Contains(recipeName)).ToList();

    }

    /// <summary>
    /// ʳ��������ѯ
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
        var currentRecipeList = recipeListDic[mealName].recipeShowList; // ��ȡ��ǰ�Ͷ�����

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
    /// ˢ���µ�����
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
