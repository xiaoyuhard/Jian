using OfficeOpenXml.FormulaParsing.ExpressionGraph;
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


    public GameObject modifyRecipeGroupUI;   //ʳ���޸�����UI����
    public GameObject swopRecipeGroupUI;   //ʳ�׽���UI����

    private Dictionary<string, RecipeGroup> recipeListDic = new Dictionary<string, RecipeGroup>();   //����ÿ������Ӧ��ѡ��ʳ���б�� �Ѿ�ѡ���˵�ʳ��


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

        recipeListDic = FoodGroupChooseUI.Instance.BackRecipeDic();
        SumTotalEveryMeal();

    }


    // Start is called before the first frame update
    void Start()
    {

    }



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
        // �������Toggle
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
            }
        }
        SumTotalEveryMeal();

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
