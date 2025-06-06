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
/// ���������
/// </summary>
public class MealGroupReportUI : UICaoZuoBase
{
    public static MealGroupReportUI Instance { get; private set; }


    public Transform dayBrackContent;
    public Transform dayLunchContent;
    public Transform dayDinnerContent;
    public GameObject recipePrefab;         //ʳ�׵�Ԥ����
    public GameObject recipeItemPrefab;     //ʳ����ʳ���Ԥ����

    public Transform everyMealParent;        //Ⱥ����ÿ��������ʾ
    public GameObject everyMealObj;

    //public Transform everyMealParentWeek;        //Ⱥ����ÿ��������ʾ
    public GameObject mealObjPerfab;            //ÿ�͵�

    public Transform allFoodPartent;
    public GameObject allFoodItemprefab;

    //public Transform allFoodPartentWeek;            //Ⱥ���ܵ���
    public GameObject allFoodItemprefabWeek;


    public GameObject modifyRecipeGroupUI;   //ʳ���޸�����UI����
    public GameObject swopRecipeGroupUI;   //ʳ�׽���UI����

    private Dictionary<string, RecipeGroup> recipeListDic = new Dictionary<string, RecipeGroup>();   //����ÿ������Ӧ��ѡ��ʳ���б�� �Ѿ�ѡ���˵�ʳ��

    public Button anewChooseBtn;            //����ѡ��ť
    public Button nextGroupBtn;             //��һ����Ͱ�ť

    public Button reportChartBtn;
    public GameObject reportChartUI;

    public UserInfo userInfo = new UserInfo();

    private string[] weekMealList = { "��һ���", "��һ���", "��һ���", "�ܶ����", "�ܶ����", "�ܶ����", "�������", "�������", "�������", "�������", "�������", "�������", "�������", "�������", "�������", "�������", "�������", "�������", "�������", "�������", "�������" };
    public List<Transform> weekMealContentList;     //ÿ�͵�content
    public List<Transform> weekGroupAllFoodParent;  //��ÿ��������ʾ


    public GameObject dayPan;
    public GameObject weekPan;

    void Awake()
    {
        InitializeManager();
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
    /// ���շ��������ص����� 
    /// </summary>
    public void ReceiveSerRecipe()
    {
        SumTotalEveryMeal();
        //sUIManager.Instance.OpenUICaoZuo("MealGroupReportUI");
        UIManager.Instance.CloseUICaoZuo("ChooseGroupShanShiUI");
    }

    /// <summary>
    /// ���մ�ѡ��ʳ���ﴫ����������
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
        AddFoodChooseCount(recipeListDic["���"].recipeSelectedList);
        AddFoodChooseCount(recipeListDic["���"].recipeSelectedList);
        AddFoodChooseCount(recipeListDic["���"].recipeSelectedList);
    }
    public void AddFoodChooseCount(List<RecipeItem> chooseFoodData)
    {

        // 2. �ϲ� foods �б�
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
        foodSendConverDay.userInfo = userInfo;
        ServerCon.Instance.ConverToJsonPost(SerializeData(foodSendConverDay), "/group/analyse/oneDay");
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
                sourceFood.count = (float.Parse(sourceFood.weight) * float.Parse(sourceFood.part));

                targetDict.Add(sourceFood.foodCode, sourceFood);
            }
        }
    }

    //�����ջ��������� 
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


    //һ������
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

        SumTotalRecipeFood(recipeListDic["���"].recipeSelectedList, dayBrackContent, "0");

        SumTotalRecipeFood(recipeListDic["���"].recipeSelectedList, dayLunchContent, "1");
        SumTotalRecipeFood(recipeListDic["���"].recipeSelectedList, dayDinnerContent, "2");
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
        totalTgl1.transform.GetChild(0).GetComponent<Text>().text = "����";
        totalTgl1.transform.GetChild(1).GetComponent<Text>().text = totalEnergy.totalEnergyKcal;
        GameObject totalTgl2 = Instantiate(allFoodItemprefab, allFoodPartent);
        totalTgl2.SetActive(true);
        totalTgl2.transform.GetChild(0).GetComponent<Text>().text = "������";
        totalTgl2.transform.GetChild(1).GetComponent<Text>().text = totalEnergy.protein;
        GameObject totalTgl3 = Instantiate(allFoodItemprefab, allFoodPartent);
        totalTgl3.SetActive(true);
        totalTgl3.transform.GetChild(0).GetComponent<Text>().text = "֬��";
        totalTgl3.transform.GetChild(1).GetComponent<Text>().text = totalEnergy.fat;
        GameObject totalTgl4 = Instantiate(allFoodItemprefab, allFoodPartent);
        totalTgl4.SetActive(true);
        totalTgl4.transform.GetChild(0).GetComponent<Text>().text = "̼ˮ������";
        totalTgl4.transform.GetChild(1).GetComponent<Text>().text = totalEnergy.cho;


    }

    public void SumTotalRecipeFood(List<RecipeItem> chooseFood, Transform foodParent, string mealName)
    {
        //totalFoodAllLists.Clear();
        // �������Toggle
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
        SumTotalEveryMeal();
        modifyRecipeGroupUI.SetActive(false);
        SendServerCon();
    }

    /// <summary>
    /// ɾ��ʳ�� ����ˢ��
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
    /// ʳ�ײ�ѯ������
    /// </summary>
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
        SumTotalEveryMeal();
        swopRecipeGroupUI.SetActive(false);
        SendServerCon();

    }


    /// <summary>
    /// ��ʾ��������ܵ�����
    /// </summary>
     //�����ջ��������� 
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

    //һ������
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
        totalTgl1.transform.GetChild(0).GetComponent<Text>().text = "����";
        totalTgl1.transform.GetChild(1).GetComponent<Text>().text = total.totalEnergy.totalEnergyKcal;
        GameObject totalTgl2 = Instantiate(allFoodItemprefab, allFoodPartent);
        totalTgl2.SetActive(true);
        totalTgl2.transform.GetChild(0).GetComponent<Text>().text = "������";
        totalTgl2.transform.GetChild(1).GetComponent<Text>().text = total.totalEnergy.protein;
        GameObject totalTgl3 = Instantiate(allFoodItemprefab, allFoodPartent);
        totalTgl3.SetActive(true);
        totalTgl3.transform.GetChild(0).GetComponent<Text>().text = "֬��";
        totalTgl3.transform.GetChild(1).GetComponent<Text>().text = total.totalEnergy.fat;
        GameObject totalTgl4 = Instantiate(allFoodItemprefab, allFoodPartent);
        totalTgl4.SetActive(true);
        totalTgl4.transform.GetChild(0).GetComponent<Text>().text = "̼ˮ������";
        totalTgl4.transform.GetChild(1).GetComponent<Text>().text = total.totalEnergy.cho;
    }

    private void ShowMealIns(GroupWeekInDayEnergy day, Transform content)
    {

        GameObject totalTgl1 = Instantiate(allFoodItemprefab, content);
        totalTgl1.SetActive(true);
        totalTgl1.transform.GetChild(0).GetComponent<Text>().text = "����";
        totalTgl1.transform.GetChild(1).GetComponent<Text>().text = day.totalEnergy.totalEnergyKcal;
        GameObject totalTgl2 = Instantiate(allFoodItemprefab, content);
        totalTgl2.SetActive(true);
        totalTgl2.transform.GetChild(0).GetComponent<Text>().text = "������";
        totalTgl2.transform.GetChild(1).GetComponent<Text>().text = day.totalEnergy.protein;
        GameObject totalTgl3 = Instantiate(allFoodItemprefab, content);
        totalTgl3.SetActive(true);
        totalTgl3.transform.GetChild(0).GetComponent<Text>().text = "֬��";
        totalTgl3.transform.GetChild(1).GetComponent<Text>().text = day.totalEnergy.fat;
        GameObject totalTgl4 = Instantiate(allFoodItemprefab, content);
        totalTgl4.SetActive(true);
        totalTgl4.transform.GetChild(0).GetComponent<Text>().text = "̼ˮ������";
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
