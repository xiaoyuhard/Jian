using Newtonsoft.Json;
using RTS;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

/// <summary>
/// 食物选择控制 并且显示食物最后选择完的报告
/// </summary>
public class ChooseFoodAllInformCon : MonoSingletonBase<ChooseFoodAllInformCon>
{
    //private Dictionary<string, List<FoodKindItemData>> foodBreakfastList = new Dictionary<string, List<FoodKindItemData>>();
    //private Dictionary<string, List<FoodKindItemData>> foodLunchList = new Dictionary<string, List<FoodKindItemData>>();
    //private Dictionary<string, List<FoodKindItemData>> foodDinnerList = new Dictionary<string, List<FoodKindItemData>>();
    //private Dictionary<string, List<FoodKindItemData>> foodChooseBlDic = new Dictionary<string, List<FoodKindItemData>>();

    //private List<FoodKindItemData> foodBreakfastList = new List<FoodKindItemData>();
    //private List<FoodKindItemData> foodLunchList = new List<FoodKindItemData>();
    //private List<FoodKindItemData> foodDinnerList = new List<FoodKindItemData>();
    private ChooseFoodData foodBreakfastList = new ChooseFoodData();
    private ChooseFoodData foodLunchList = new ChooseFoodData();
    private ChooseFoodData foodDinnerList = new ChooseFoodData();

    public Transform foodBreakfastParent;
    public Transform foodLunchParent;
    public Transform foodDinnerParent;
    public GameObject foodPrefab;

    public ToggleGroup group;

    public GameObject foodRecipePrefab;

    public Dropdown dropdown;
    public Button choossFoodAllBtn;
    public GameObject textTip;
    //private List<TotalFoodAll> totalFoodAllLists = new List<TotalFoodAll>();
    //private TotalFoodAll totalFoodAll = new TotalFoodAll();

    public GameObject everyMealUIPrefab;
    public Transform everyMealParent;
    public GameObject everyMealObj;

    public Button selectEndBtn;
    public GameObject selectEndTipText;
    public GameObject goYingYiangShiText;
    public GameObject delectTipText;
    public GameObject amendNullTipText;
    public GameObject dontDelRecipeFoodTipText;

    public Toggle amendTgl;
    public Toggle deltgl;
    public GameObject editUI;

    private FoodKindItemData editFoodItem;
    private RecipeItem editFoodRecipeItem;
    public bool againBl = false;

    public UserInfo userInfo;

    public Transform allFoodPartent;
    public GameObject allFoodItemprefab;

    public List<GameObject> tipEnergyList;

    public Button reportChartBtn;
    public GameObject reportChartUI;

    public Button exportBtn;

    private void OnEnable()
    {
        //foodBreakfastList..Clear();
        //foodLunchList.Clear();
        //foodDinnerList.Clear();
        //totalFoodAllLists.Clear();
        dropdown.value = 0;
        editUI.SetActive(false);
        //totalFoodAll = null;
        againBl = true;
        foreach (var item in tipEnergyList)
        {
            item.SetActive(false);
        }
        reportChartUI.SetActive(false);
        SetChooseMealActive(false);
    }

    public void SetChooseMealActive(bool isActive)
    {
        dropdown.gameObject.SetActive(isActive);
        selectEndBtn.gameObject.SetActive(isActive);
    }

    // Start is called before the first frame update
    void Start()
    {
        //MessageCenter.Instance.Register()
        //choossFoodAllBtn.onClick.AddListener(SelectEnd);

        lastValue = dropdown.value;
        selectEndBtn.onClick.AddListener(SelectEndUI);


        // 绑定事件
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        amendTgl.onValueChanged.AddListener(AmendTglClick);//修改按钮
        deltgl.onValueChanged.AddListener(DelTglClick);//删除按钮
        GameObject toggleObj = Instantiate(foodPrefab, foodBreakfastParent);
        toggleObj.SetActive(true);
        GameObject toggleObj2 = Instantiate(foodPrefab, foodBreakfastParent);
        toggleObj2.SetActive(true);
        reportChartBtn.onClick.AddListener(ClickReportChartBtn);

        exportBtn.onClick.AddListener(ExportClick);

    }
    //导出报告按钮
    private void ExportClick()
    {
        MessageCenter.Instance.Send("SendHomeReset", ""); //氨基酸工作台
        ShanShiCon.Instance.CloseObj();
        UIManager.Instance.CloseAllUICaoZuo();
        ExcelExporter.Instance.SaveToExcel();
    }

    private void AmendTglClick(bool arg0)
    {
        if (editFoodItem != null)
        {
            editUI.SetActive(true);
            AmendUI.Instance.SetIconInf(editFoodItem);
        }
        else
        {

        }
    }

    //修改了食物的数量
    public void EditBackFood(FoodKindItemData foodKind)
    {
        switch (foodKind.mealPeriod)
        {
            case "0":
                EditFoodItem(foodBreakfastList.allFoods, foodKind);
                break;
            case "1":
                EditFoodItem(foodLunchList.allFoods, foodKind);
                break;
            case "2":
                EditFoodItem(foodDinnerList.allFoods, foodKind);
                break;
            default:
                break;
        }

    }
    //private void EditFoodItem(Dictionary<string, List<FoodKindItemData>> foodDic, FoodKindItemData foodKind)
    private void EditFoodItem(List<FoodKindItemData> foodDic, FoodKindItemData foodKind)
    {
        //if (!foodDic.ContainsKey(foodKind.id))
        //{
        //    Debug.LogError($"找不到分类：{foodKind.id}");
        //    return;
        //}
        // 2. 获取目标分类列表
        //List<FoodKindItemData> targetList = foodDic[foodKind.id];
        List<FoodKindItemData> targetList = foodDic;

        // 3. 遍历列表查找匹配项
        foreach (FoodKindItemData food in targetList)
        {
            if (food.foodCode == foodKind.foodCode)
            {
                food.count = foodKind.count;
                food.recipeCount = foodKind.count;
                food.foodCount = foodKind.count;

                // 如果只需修改第一个匹配项，可在此处break
                break;
            }
        }
        //RefreshMealUI();
        SendServerCon();

    }

    //删除按钮 删除对应的食物
    private void DelTglClick(bool arg0)
    {

        //else
        //{
        if (editFoodItem != null)
        {
            if (!editFoodItem.isDel)
            {
                dontDelRecipeFoodTipText.SetActive(true);
                StartCoroutine(WaitCloseText(dontDelRecipeFoodTipText));
                return;
            }
            switch (editFoodItem.mealPeriod)
            {
                case "0":
                    DelFoodItem(foodBreakfastList.foods, editFoodItem);
                    break;
                case "1":
                    DelFoodItem(foodLunchList.foods, editFoodItem);
                    break;
                case "2":
                    DelFoodItem(foodDinnerList.foods, editFoodItem);
                    break;
                default:
                    break;
            }
        }
        //}
        if (editFoodRecipeItem != null)
        {
            switch (editFoodRecipeItem.mealPeriod)
            {
                case "0":
                    DelFoodRecipeItem(foodBreakfastList.recipes, editFoodRecipeItem);
                    break;
                case "1":
                    DelFoodRecipeItem(foodLunchList.recipes, editFoodRecipeItem);
                    break;
                case "2":
                    DelFoodRecipeItem(foodDinnerList.recipes, editFoodRecipeItem);
                    break;
                default:
                    break;
            }
        }
    }

    //删除对应的食物 然后告知服务器
    //private void DelFoodItem(Dictionary<string, List<FoodKindItemData>> foodDic, FoodKindItemData foodKind)
    private void DelFoodItem(List<FoodKindItemData> foodDic, FoodKindItemData foodKind)
    {
        //if (!foodDic.ContainsKey(foodKind.id))
        //{
        //    Debug.LogError($"找不到分类：{foodKind.id}");
        //    return;
        //}
        // 2. 获取目标分类列表
        //List<FoodKindItemData> targetList = foodDic[foodKind.id];
        //if (allFoodDic.ContainsKey(foodKind.categoryName))
        bool isMeal = false;
        switch (foodKind.mealPeriod)
        {
            case "0":
                isMeal = BackMealCountBl(foodKind.mealPeriod);
                break;
            case "1":
                isMeal = BackMealCountBl(foodKind.mealPeriod);
                break;
            case "2":
                isMeal = BackMealCountBl(foodKind.mealPeriod);
                break;
        }
        if (allFoodDic[foodKind.categoryName].Count > 1 && !isMeal)
        {

            foodDic.Remove(foodKind);
            //allFoodDic[foodKind.categoryName].Remove(foodKind);
            //List<FoodKindItemData> targetList = foodDic;
            //targetList.Remove(foodKind);
            SendServerCon();
        }

        //if (foodDic.Count > 1)
        //{

        //}
        else
        {
            delectTipText.SetActive(true);
            StartCoroutine(WaitCloseText(delectTipText));

        }

        //RefreshMealUI();
    }
    //删除对应的食谱 然后告知服务器
    //private void DelFoodItem(Dictionary<string, List<FoodKindItemData>> foodDic, FoodKindItemData foodKind)
    private void DelFoodRecipeItem(List<RecipeItem> foodDic, RecipeItem foodKind)
    {
        //if (!foodDic.ContainsKey(foodKind.id))
        //{
        //    Debug.LogError($"找不到分类：{foodKind.id}");
        //    return;
        //}
        // 2. 获取目标分类列表
        //List<FoodKindItemData> targetList = foodDic[foodKind.id];
        //if (allFoodDic.ContainsKey(foodKind.categoryName))

        //if (allFoodDic[foodKind.categoryName].Count > 1)
        //{
        //    allFoodDic[foodKind.categoryName].Remove(foodKind);
        //    List<FoodKindItemData> targetList = foodDic;
        //    targetList.Remove(foodKind);
        //}
        bool isDel = true;
        foreach (var item in foodKind.foodKindItems)
        {
            if (allFoodDic[item.categoryName].Count == 1)
            {
                isDel = false;
            }
        }
        if (!BackMealCountBl(foodKind.mealPeriod) && isDel)
        {
            foodDic.Remove(foodKind);
            SendServerCon();
        }
        else
        {
            delectTipText.SetActive(true);
            StartCoroutine(WaitCloseText(delectTipText));

        }

        //RefreshMealUI();
    }

    public bool BackMealCountBl(string mealPeriod)
    {
        bool isMeal = false;

        switch (mealPeriod)
        {
            case "0":
                if (foodBreakfastList.foods.Count + foodBreakfastList.recipes.Count <= 1)
                {
                    isMeal = true;
                }
                break;
            case "1":
                if (foodLunchList.foods.Count + foodLunchList.recipes.Count <= 1)
                {
                    isMeal = true;
                }
                break;
            case "2":
                if (foodDinnerList.foods.Count + foodDinnerList.recipes.Count <= 1)
                {
                    isMeal = true;
                }
                break;
        }

        return isMeal;
    }

    public void RefreshMealUI()
    {
        //SumTotalBreakfastFood(foodBreakfastList, foodBreakfastParent, "Zao");
        //SumTotalBreakfastFood(foodLunchList, foodLunchParent, "Zhong");
        //SumTotalBreakfastFood(foodDinnerList, foodDinnerParent, "Wan");
        SumTotalEveryMeal();

    }

    public void MergeAllFoods(ChooseFoodData chooseFoodData)
    {
        // 1. 创建字典用于合并数据（Key: 食物唯一标识 id）
        Dictionary<string, FoodKindItemData> foodDict = new Dictionary<string, FoodKindItemData>();

        // 2. 合并 foods 列表
        MergeFoodList(chooseFoodData.foods, foodDict);

        // 3. 合并 recipes 中的食材
        foreach (var recipe in chooseFoodData.recipes)
        {
            MergeFoodList(recipe.foodKindItems, foodDict);
        }

        // 4. 将结果存入 allFoods
        chooseFoodData.allFoods = new List<FoodKindItemData>(foodDict.Values);
    }

    private void MergeFoodList(List<FoodKindItemData> sourceList, Dictionary<string, FoodKindItemData> targetDict)
    {
        foreach (var sourceFood in sourceList)
        {
            if (targetDict.TryGetValue(sourceFood.foodCode, out var existingFood))
            {
                // 合并 count
                existingFood.count += sourceFood.count;
            }
            else
            {

                targetDict.Add(sourceFood.foodCode, sourceFood);
            }
        }
    }

    // 复制属性（确保所有字段被正确复制）
    private void CopyFoodProperties(FoodKindItemData source, FoodKindItemData target)
    {
        target = source;
    }
    public bool endChooseValue = false;

    //点击选完所有餐的按钮 然后发送服务器所有数据
    private void SelectEndUI()
    {
        SetChooseCategoryNameCount();
        Debug.Log(lastValue);
        if (YiTiJiUI.Instance.BackUserInfo().isBaby && FoodChooseKind())
        {
            ShanShiCon.Instance.posList[4].SetActive(false);  //关闭选择食物的提示位置
            endChooseValue = false;
            UIManager.Instance.CloseUICaoZuo("ChooseShanShiUI");
            //ShanShiCon.Instance.conditionMet = true;
            ShanShiCon.Instance.posList[5].SetActive(true);  //打开营养师的提示位置

            GameManager.Instance.SetStepDetection(true);  //打开营养师的提示位置
            SendServerCon();
            goYingYiangShiText.SetActive(true);
            StartCoroutine(WaitCloseText(goYingYiangShiText));
            return;
        }
        else if ((allFoodDic.Count == 6) && /*(lastValue == 2 || endChooseValue)*/FoodChooseKind())
        {
            ShanShiCon.Instance.posList[4].SetActive(false);  //关闭选择食物的提示位置
            endChooseValue = false;
            UIManager.Instance.CloseUICaoZuo("ChooseShanShiUI");
            //ShanShiCon.Instance.conditionMet = true;
            ShanShiCon.Instance.posList[5].SetActive(true);  //打开营养师的提示位置

            GameManager.Instance.SetStepDetection(true);  //打开营养师的提示位置
            SendServerCon();
            goYingYiangShiText.SetActive(true);
            StartCoroutine(WaitCloseText(goYingYiangShiText));

            return;
        }
        selectEndTipText.SetActive(true);
        StartCoroutine(WaitCloseText(selectEndTipText));
    }

    public void SetChooseCategoryNameCount()
    {
        allFoodDic.Clear();
        AddFoodChooseCount(foodBreakfastList.allFoods);
        AddFoodChooseCount(foodLunchList.allFoods);
        AddFoodChooseCount(foodDinnerList.allFoods);
    }

    Dictionary<string, List<FoodKindItemData>> allFoodDic = new Dictionary<string, List<FoodKindItemData>>();
    public void AddFoodChooseCount(List<FoodKindItemData> foodList)
    {
        foreach (var item in foodList)
        {
            if (allFoodDic.ContainsKey(item.categoryName))
            {
                allFoodDic[item.categoryName].Add(item);
            }
            else
            {
                allFoodDic.Add(item.categoryName, new List<FoodKindItemData> { item });
            }
        }
    }

    public void SendServerCon()
    {
        MergeAllFoods(foodBreakfastList);
        MergeAllFoods(foodLunchList);
        MergeAllFoods(foodDinnerList);
        SetChooseCategoryNameCount();

        FoodSendConverDay foodSendConverDay = new FoodSendConverDay();
        foodSendConverDay.breakfast = AddFoodSendConverList(foodBreakfastList.allFoods);
        foodSendConverDay.lunch = AddFoodSendConverList(foodLunchList.allFoods);
        foodSendConverDay.dinner = AddFoodSendConverList(foodDinnerList.allFoods);
        foodSendConverDay.userInfo = userInfo;
        ServerCon.Instance.ConverToJsonPost(SerializeData(foodSendConverDay), "/analyse/intake");
    }
    private string SerializeData(FoodSendConverDay data)
    {
        // 方法一：使用 Unity 内置 JsonUtility（需要包装类）
        Wrapper<FoodSendConverDay> wrapper = new Wrapper<FoodSendConverDay> { items = data };
        //return JsonUtility.ToJson(wrapper);

        // 方法二：使用 Newtonsoft.Json（直接序列化列表）
        return JsonConvert.SerializeObject(data);
    }



    public List<FoodEveryMealItem> AddFoodSendConverList(List<FoodKindItemData> foodKinds)
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


    private int lastValue; // 记录上一次的值

    // 当 Dropdown 值变化时调用
    private void OnDropdownValueChanged(int newValue)
    {
        //if (FoodChooseKind() && newValue != lastValue)
        {
            //FoodManager.Instance.LoadAllFromJson(); // 启动时加载数据
            isSame = true;
            lastValue = newValue; // 更新记录值
            Debug.Log("333");
            //foodChooseBlDic.Clear();
            foodChooseCount = 0;
            FoodManager.Instance.LoadFoodData();

            return;
        }

        Debug.Log(foodBreakfastList.foods.Count + "  zao" + foodLunchList.foods.Count + "  zhong" + foodDinnerList.foods.Count + "  wan");
        dropdown.value = lastValue;
        textTip.SetActive(true);
        StartCoroutine(WaitCloseText(textTip));
    }

    public int BackDropValue()
    {
        return dropdown.value;
    }
    IEnumerator WaitCloseText(GameObject obj)
    {
        yield return new WaitForSeconds(1);
        obj.SetActive(false);
    }

    public bool isSame = false;
    public bool chooseValue = false;
    int foodChooseCount = 0;

    public void AddFoodDic(ChooseFoodData chooseFoodData, string kindName, int count)
    {
        foodChooseCount = count;
        // 创建新列表并复制数据（浅拷贝）
        List<FoodKindItemData> foods = new List<FoodKindItemData>(chooseFoodData.foods);
        List<RecipeItem> recipes = new List<RecipeItem>(chooseFoodData.recipes);
        List<FoodKindItemData> allFoods = new List<FoodKindItemData>(chooseFoodData.allFoods);
        switch (dropdown.value)
        {
            case 0:
                //foodBreakfastList.Remove(kindName);
                //foodBreakfastList.Add(kindName, foodLsit);
                foodBreakfastList.foods = foods;
                foodBreakfastList.recipes = recipes;
                foodBreakfastList.allFoods = allFoods;
                //Debug.Log(foodBreakfastList.Count());
                //totalFoodAllLists.RemoveAll(item => item.foodName == "Zao");


                break;
            case 1:
                //foodLunchList.Remove(kindName);
                //foodLunchList.Add(kindName, foodLsit);
                foodLunchList.foods = foods;
                foodLunchList.recipes = recipes;
                foodLunchList.allFoods = allFoods;
                //Debug.Log(foodLunchList.Count());
                //totalFoodAllLists.RemoveAll(item => item.foodName == "Zhong");


                break;
            case 2:
                //foodDinnerList.Remove(kindName);
                //foodDinnerList.Add(kindName, foodLsit);
                foodDinnerList.foods = foods;
                foodDinnerList.recipes = recipes;
                foodDinnerList.allFoods = allFoods;
                //Debug.Log(foodDinnerList.Count());
                //totalFoodAllLists.RemoveAll(item => item.foodName == "Wan");

                break;
        }

    }

    public void DeleteFoodKind(string foodKindNmae)
    {
        selectEndBtn.gameObject.SetActive(true);
        ShanShiCon.Instance.posList[5].SetActive(false);  //关闭营养师的提示位置
        ShanShiCon.Instance.posList[4].SetActive(true);  //打开选择食物的提示位置

        switch (foodKindNmae)
        {
            case "Zao":
                dropdown.value = 0;
                foodBreakfastList.foods.Clear();
                foodBreakfastList.recipes.Clear();
                foodBreakfastList.allFoods.Clear();
                //totalFoodAllLists.RemoveAll(item => item.foodName == foodKindNmae);
                chooseValue = true;
                break;
            case "Zhong":
                dropdown.value = 1;
                chooseValue = true;

                foodLunchList.foods.Clear();
                foodLunchList.recipes.Clear();
                foodLunchList.allFoods.Clear();
                //totalFoodAllLists.RemoveAll(item => item.foodName == foodKindNmae);
                break;
            case "Wan":
                dropdown.value = 2;
                chooseValue = true;

                foodDinnerList.foods.Clear();
                foodDinnerList.recipes.Clear();
                foodDinnerList.allFoods.Clear();
                //totalFoodAllLists.RemoveAll(item => item.foodName == foodKindNmae);
                break;
        }
    }

    //判断是否3餐都选择完了
    //public bool TotalFoodShow()
    //{
    //    //Debug.Log(totalFoodAllLists.Count+" totalfoodall");
    //    if (totalFoodAllLists.Count < 3) return false;

    //    return true;
    //}

    //进行食物选择的判断 必须每餐都得有
    public bool FoodChooseKind()
    {
        switch (lastValue)
        {
            case 0:
                if (foodBreakfastList.allFoods.Count < 1) return false;
                return true;
            case 1:
                if (foodLunchList.allFoods.Count < 1) return false;
                return true;

            case 2:
                if (foodDinnerList.allFoods.Count < 1) return false;
                return true;

            default:
                return false;
        }


        Debug.Log(foodChooseCount);
        if (foodChooseCount < 6) return false;
        return true;

    }


    #region 报告出单食物
    //出报告显示
    //public void SumTotalBreakfastFood(Dictionary<string, List<FoodKindItemData>> foodDic, Transform foodParent, string mealName)
    //public void SumTotalBreakfastFood(List<FoodKindItemData> foodDic, Transform foodParent, string mealName)
    //{
    //    //totalFoodAllLists.Clear();
    //    // 清空现有Toggle
    //    foreach (Transform child in foodParent)
    //    {
    //        Destroy(child.gameObject);
    //    }
    //    TotalFoodAll totalFood = new TotalFoodAll();
    //    //foreach (KeyValuePair<string, List<FoodKindItemData>> entry in foodDic)
    //    //{
    //    //string category = entry.Key; // 获取当前分类（如 "Fruits"）
    //    //List<FoodKindItemData> foodList = entry.Value; // 获取当前分类下的食物列表
    //    List<FoodKindItemData> foodList = foodDic; // 获取当前分类下的食物列表
    //    TotalFoodAll totalFoodAll = new TotalFoodAll();
    //    // 遍历当前分类下的所有食物
    //    foreach (FoodKindItemData foodItem in foodList)
    //    {
    //        GameObject toggleObj = Instantiate(foodPrefab, foodParent);
    //        toggleObj.SetActive(true);
    //        SetFoodItemText(toggleObj, "Name", foodItem.foodName);
    //        SetFoodItemText(toggleObj, "Amount", foodItem.count.ToString());
    //        //SetFoodItemText(toggleObj, "Unit", BackMultiplyuantity(foodItem.count, float.Parse(foodItem.water)));
    //        SetFoodItemText(toggleObj, "EnergyKcal", BackMultiplyuantity(foodItem.count, float.Parse(foodItem.heat)));
    //        //SetFoodItemText(toggleObj, "energyKj", BackMultiplyuantity(foodItem.count, float.Parse(foodItem.energyKj)));
    //        SetFoodItemText(toggleObj, "Protein", BackMultiplyuantity(foodItem.count, float.Parse(foodItem.protein)));
    //        SetFoodItemText(toggleObj, "Fat", BackMultiplyuantity(foodItem.count, float.Parse(foodItem.fat)));
    //        SetFoodItemText(toggleObj, "Cho", BackMultiplyuantity(foodItem.count, float.Parse(foodItem.cho)));
    //        //toggleObj.transform.Find("Name").GetComponent<Text>().text = foodItem.foodName;
    //        //toggleObj.transform.Find("Amount").GetComponent<Text>().text = foodItem.count.ToString();
    //        //toggleObj.transform.Find("Unit").GetComponent<Text>().text = BackMultiplyuantity(foodItem.count, float.Parse(foodItem.water));
    //        //toggleObj.transform.Find("EnergyKcal").GetComponent<Text>().text = foodItem.energyKcal;
    //        //toggleObj.transform.Find("Protein").GetComponent<Text>().text = foodItem.protein;
    //        //toggleObj.transform.Find("Fat").GetComponent<Text>().text = foodItem.fat;
    //        //toggleObj.transform.Find("Cho").GetComponent<Text>().text = foodItem.cho;

    //        //totalFood.totalEnergyKca += float.Parse(foodItem.energyKcal);
    //        //totalFood.protein += float.Parse(foodItem.protein);
    //        //totalFood.fat += float.Parse(foodItem.fat);
    //        //totalFood.cho += float.Parse(foodItem.cho);
    //        Toggle toggle = toggleObj.GetComponent<Toggle>();

    //        toggle.onValueChanged.AddListener((isOn) =>
    //        {
    //            if (isOn) OnAlterFoodSelected(foodItem, toggle, isOn);
    //        });
    //        //}
    //    }
    //    //totalFood.foodName = mealName;
    //    //totalFood.totalEnergyKcaStandard = Compare(totalFood.totalEnergyKca, 5);
    //    //totalFoodAllLists.Add(totalFood);
    //    //Debug.Log(totalFoodAllLists.Count + "zongshu ");
    //}
    #endregion

    public void SumTotalRecipeFood(ChooseFoodData chooseFood, Transform foodParent, string mealName)
    {
        //totalFoodAllLists.Clear();
        // 清空现有Toggle
        foreach (Transform child in foodParent)
        {
            Destroy(child.gameObject);
        }
        List<RecipeItem> recipes = chooseFood.recipes;
        foreach (var itemRec in recipes)
        {
            itemRec.mealPeriod = mealName;
            GameObject recipeObj = Instantiate(foodRecipePrefab, foodParent);
            recipeObj.SetActive(true);
            Toggle toggleRec = recipeObj.GetComponent<Toggle>();
            toggleRec.group = group;
            toggleRec.onValueChanged.AddListener((isOn) =>
            {
                if (isOn) OnAlterFoodRecipeSelected(itemRec, toggleRec, isOn);
            });
            foreach (var itemFood in itemRec.foodKindItems)
            {
                GameObject toggleObj = Instantiate(foodPrefab, recipeObj.transform.GetChild(0).GetChild(0));
                toggleObj.SetActive(true);
                //SetFoodItemText(toggleObj, "Name", itemFood.foodName);
                toggleObj.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = itemFood.foodName;

                SetFoodItemText(toggleObj, "Amount", itemFood.recipeCount.ToString());
                SetFoodItemText(toggleObj, "EnergyKcal", BackMultiplyuantity(itemFood.recipeCount, float.Parse(itemFood.heat)));
                SetFoodItemText(toggleObj, "Protein", BackMultiplyuantity(itemFood.recipeCount, float.Parse(itemFood.protein)));
                SetFoodItemText(toggleObj, "Fat", BackMultiplyuantity(itemFood.recipeCount, float.Parse(itemFood.fat)));
                SetFoodItemText(toggleObj, "Cho", BackMultiplyuantity(itemFood.recipeCount, float.Parse(itemFood.cho)));
                Toggle toggle = toggleObj.GetComponent<Toggle>();
                toggle.group = group;
                itemFood.isDel = false;

                toggle.onValueChanged.AddListener((isOn) =>
                {
                    if (isOn) OnAlterFoodSelected(itemFood, toggle, isOn);
                });
            }
        }


        List<FoodKindItemData> foodList = chooseFood.foods; // 获取当前分类下的食物列表
        TotalFoodAll totalFoodAll = new TotalFoodAll();
        // 遍历当前分类下的所有食物
        foreach (FoodKindItemData foodItem in foodList)
        {
            GameObject toggleObj = Instantiate(foodPrefab, foodParent);
            toggleObj.SetActive(true);
            //SetFoodItemText(toggleObj, "Name", foodItem.foodName);
            toggleObj.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = foodItem.foodName;
            SetFoodItemText(toggleObj, "Amount", foodItem.count.ToString());
            SetFoodItemText(toggleObj, "EnergyKcal", BackMultiplyuantity(foodItem.count, float.Parse(foodItem.heat)));
            SetFoodItemText(toggleObj, "Protein", BackMultiplyuantity(foodItem.count, float.Parse(foodItem.protein)));
            SetFoodItemText(toggleObj, "Fat", BackMultiplyuantity(foodItem.count, float.Parse(foodItem.fat)));
            SetFoodItemText(toggleObj, "Cho", BackMultiplyuantity(foodItem.count, float.Parse(foodItem.cho)));

            Toggle toggle = toggleObj.GetComponent<Toggle>();
            toggle.group = group;
            foodItem.isDel = true;
            toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn) OnAlterFoodSelected(foodItem, toggle, isOn);
            });
            //}
        }

    }
    //传入text名字 设置text文本
    public void SetFoodItemText(GameObject tgl, string textName, string foodInfor)
    {
        //if (tgl.transform.Find(textName) == null) return;
        tgl.transform.Find(textName).GetComponent<Text>().text = foodInfor;

    }

    public string BackMultiplyuantity(float quantity, float parameters)
    {
        return ((quantity * parameters) /*/ 100*/).ToString("F2");
    }

    void OnAlterFoodSelected(FoodKindItemData food, Toggle tglItem, bool isOn)
    {
        if (isOn)
        {
            editFoodItem = food;
            //AmendUI.Instance.SetIconInf(food);
            editFoodRecipeItem = null;

        }
        else
        {
            editFoodItem = null;
        }
    }
    void OnAlterFoodRecipeSelected(RecipeItem food, Toggle tglItem, bool isOn)
    {
        if (isOn)
        {
            editFoodRecipeItem = food;
            //AmendUI.Instance.SetIconInf(food);
            editFoodItem = null;

        }
        else
        {
            editFoodRecipeItem = null;

        }
    }
    public static int Compare(float a, float b)
    {
        if (a > b) return 1;
        if (a == b) return 0;
        return 2;
    }

    //储存收回来的数据 
    public void ReceiveServerInform(FoodRecriveConverDay response)
    {
        SetChooseMealActive(false);
        editFoodItem = null;
        promptInfo = response.data.promptInfo;
        everyMealEnergies.Clear();
        everyMealEnergies.Add(response.data.breakfastEnergy);
        everyMealEnergies.Add(response.data.lunchEnergy);
        everyMealEnergies.Add(response.data.dinnerEnergy);
        elementResult = response.data.elementResult;
        compareResult = response.data.compareResult;
        totalEnergy = response.data.totalEnergy;
        recEnergyIntake = response.data.recEnergyIntake;
        fiberAndFineProtein = response.data.fiberAndFineProtein;
        ExcelExporter.Instance.response = response;
        RefreshMealUI();

    }
    PromptInfo promptInfo = new PromptInfo();
    EveryMealEnergy totalEnergy = new EveryMealEnergy();
    EveryMealEnergy recEnergyIntake = new EveryMealEnergy();
    CompareResult compareResult = new CompareResult();
    FiberAndFineProtein fiberAndFineProtein = new FiberAndFineProtein();
    private List<EveryMealEnergy> everyMealEnergies = new List<EveryMealEnergy>();
    private List<ElementResult> elementResult = new List<ElementResult>();

    private void ClickReportChartBtn()
    {
        reportChartUI.SetActive(true);
        SetXchaetUI.Instance.SetPieChat(allFoodDic, promptInfo, totalEnergy, recEnergyIntake, compareResult, everyMealEnergies, fiberAndFineProtein, ServerCon.Instance.BackThreeMeals(), userInfo);
    }


    //一天总量
    public void SumTotalEveryMeal()
    {
        foreach (Transform child in everyMealParent)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in allFoodPartent)
        {
            Destroy(child.gameObject);
        }
        SumTotalRecipeFood(foodBreakfastList, foodBreakfastParent, "0");

        SumTotalRecipeFood(foodLunchList, foodLunchParent, "1");
        SumTotalRecipeFood(foodDinnerList, foodDinnerParent, "2");
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
            //toggleObj.transform.GetChild(6).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(/*data[i].id */"Icons" + "/" + foodItem.choStandard);
            // 显示食物名称
            //totalFoodAll.heat += foodItem.heat;
            //totalFoodAll.heat += foodItem.protein;
            //totalFoodAll.heat += foodItem.fat;
            //totalFoodAll.heat += foodItem.carbohydrate;
        }
        //GameObject totalTgl = Instantiate(everyMealObj, allFoodPartent);
        //totalTgl.SetActive(true);
        //totalTgl.transform.GetChild(2).GetComponent<Text>().text = totalEnergy.totalEnergyKcal;
        //totalTgl.transform.GetChild(5).GetComponent<Text>().text = totalEnergy.protein;
        //totalTgl.transform.GetChild(8).GetComponent<Text>().text = totalEnergy.fat;
        //totalTgl.transform.GetChild(11).GetComponent<Text>().text = totalEnergy.cho;

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

        foreach (var item in elementResult)
        {
            GameObject elementItem = Instantiate(allFoodItemprefab, allFoodPartent);
            //GameObject elementItem = Instantiate(everyMealObj, allFoodPartent);
            elementItem.SetActive(true);
            elementItem.transform.GetChild(0).GetComponent<Text>().text = item.zhName;
            elementItem.transform.GetChild(1).GetComponent<Text>().text = item.totalContent;
        }
        bool fl = true;
        //if (compareResult.choDiff > RecEnergyIntakeSplit(recEnergyIntake.totalEnergyKcal, 1))
        if (compareResult.choDiff > RecEnergyIntakeSplit(recEnergyIntake.totalEnergyKcal, 0))
        {
            tipEnergyList[0].SetActive(true);
            tipEnergyList[1].SetActive(false);

            fl = false;
        }
        else if (compareResult.choDiff < RecEnergyIntakeSplit(recEnergyIntake.totalEnergyKcal, 0))
        {
            tipEnergyList[0].SetActive(false);
            tipEnergyList[1].SetActive(true);
            fl = false;

        }
        if (compareResult.fatDiff > RecEnergyIntakeSplit(recEnergyIntake.protein, 1))
        {
            tipEnergyList[2].SetActive(true);
            tipEnergyList[3].SetActive(false);
            fl = false;

        }
        else if (compareResult.fatDiff < RecEnergyIntakeSplit(recEnergyIntake.protein, 0))
        {
            tipEnergyList[2].SetActive(false);
            tipEnergyList[3].SetActive(true);
            fl = false;

        }
        if (compareResult.energyDiff > RecEnergyIntakeSplit(recEnergyIntake.fat, 1))
        {
            tipEnergyList[4].SetActive(true);
            tipEnergyList[5].SetActive(false);
            fl = false;

        }
        else if (compareResult.energyDiff < RecEnergyIntakeSplit(recEnergyIntake.fat, 0))
        {
            tipEnergyList[4].SetActive(false);
            tipEnergyList[5].SetActive(true);
            fl = false;

        }
        if (compareResult.proteinDiff > RecEnergyIntakeSplit(recEnergyIntake.cho, 1))
        {
            tipEnergyList[6].SetActive(true);
            tipEnergyList[7].SetActive(false);
            fl = false;
        }
        else if (compareResult.proteinDiff < RecEnergyIntakeSplit(recEnergyIntake.cho, 0))
        {
            tipEnergyList[6].SetActive(false);
            tipEnergyList[7].SetActive(true);
            fl = false;
        }
        if (fl)
        {
            tipEnergyList[8].SetActive(true);

        }
        else
        {
            tipEnergyList[8].SetActive(false);

        }

    }
    public float RecEnergyIntakeSplit(string recEnergyIntake, int index)
    {
        char separator = '~';
        string[] result = recEnergyIntake.Split(separator);
        if (result.Length == 1)
        {
            return float.Parse(result[0]);
        }
        return float.Parse(result[index]);
    }



    public void EnableInform()
    {
        //SetChooseMealActive(false);

        SelectEnd();
    }

    private void SelectEnd()
    {
        //if (TotalFoodShow())
        {
            //totalFoodAllLists.Clear();

            //UIManager.Instance.OpenUICaoZuo("MealReportUI");
            GameObjMan.Instance.CLoseFirst();
            SumTotalEveryMeal();
            //ShanShiCon.Instance.conditionMet = true;

            //return;
        }

    }

    /// <summary>
    /// 点击3d物体然后显示选择页面
    /// </summary>
    /// <param name="foodCode"></param>
    public void ClickObjShowAlter(FoodKindItemData foodObj)
    {
        foreach (var item in FoodManager.Instance.itemDictionary)
        {
            if (foodObj.categoryName == item.Key)
            {
                foreach (var foodItem in item.Value)
                {
                    if (foodObj.foodCode == foodItem.foodCode)
                    {
                        //currentSelectedFood = item;
                        //ShowEditPanel(true, item);
                        //AlterUI.Instance.UpFoodItem(item, foodSort);
                        break;
                    }
                }
            }
        }

    }


    // Update is called once per frame
    void Update()
    {

    }
}

[System.Serializable]
public class TotalFoodAll
{
    public string foodName;
    public float totalEnergyKca;//热量
    public float totalEnergyKcaStandard = 0;//热量标准 0为达到 1为超过 2为低于
    public float protein;//蛋白质
    public float proteinStandard = 0;//蛋白质标准 0为达到 1为超过 2为低于

    public float fat;//脂肪
    public float fatStandard = 0;//脂肪标准 0为达到 1为超过 2为低于

    public float cho;//碳水化合物
    public float choStandard = 0;//碳水化合物标准 0为达到 1为超过 2为低于


}
