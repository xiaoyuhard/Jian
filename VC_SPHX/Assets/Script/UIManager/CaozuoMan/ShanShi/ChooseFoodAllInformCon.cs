using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using RTS;
using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
//using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

/// <summary>
/// 食物选择控制 并且显示食物最后选择完的报告
/// </summary>
public class ChooseFoodAllInformCon : MonoSingletonBase<ChooseFoodAllInformCon>
{
    //private Dictionary<string, List<FoodKindItemData>> foodBreakfastList = new Dictionary<string, List<FoodKindItemData>>();
    //private Dictionary<string, List<FoodKindItemData>> foodLunchList = new Dictionary<string, List<FoodKindItemData>>();
    //private Dictionary<string, List<FoodKindItemData>> foodDinnerList = new Dictionary<string, List<FoodKindItemData>>();
    //private Dictionary<string, List<FoodKindItemData>> foodChooseBlDic = new Dictionary<string, List<FoodKindItemData>>();

    private List<FoodKindItemData> foodBreakfastList = new List<FoodKindItemData>();
    private List<FoodKindItemData> foodLunchList = new List<FoodKindItemData>();
    private List<FoodKindItemData> foodDinnerList = new List<FoodKindItemData>();


    public Transform foodBreakfastParent;
    public Transform foodLunchParent;
    public Transform foodDinnerParent;
    public GameObject foodPrefab;

    public Dropdown dropdown;
    public Button choossFoodAllBtn;
    public GameObject textTip;
    private List<TotalFoodAll> totalFoodAllLists = new List<TotalFoodAll>();
    private TotalFoodAll totalFoodAll = new TotalFoodAll();

    public GameObject everyMealUIPrefab;
    public Transform everyMealParent;
    public GameObject everyMealObj;

    public Button selectEndBtn;
    public GameObject selectEndTipText;
    public GameObject goYingYiangShiText;
    public GameObject delectTipText;

    public Toggle amendTgl;
    public Toggle deltgl;
    public GameObject editUI;

    private FoodKindItemData editFoodItem;
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
        foodBreakfastList.Clear();
        foodLunchList.Clear();
        foodDinnerList.Clear();
        totalFoodAllLists.Clear();
        dropdown.value = 0;
        editUI.SetActive(false);
        totalFoodAll = null;
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


    private void ClickReportChartBtn()
    {
        reportChartUI.SetActive(true);
        SetXchaetUI.Instance.SetPieChat(totalEnergy, recEnergyIntake, compareResult);
    }



    private void AmendTglClick(bool arg0)
    {
        if (editFoodItem != null)
        {
            editUI.SetActive(true);
            AmendUI.Instance.SetIconInf(editFoodItem);
        }
    }

    //修改了食物的数量
    public void EditBackFood(FoodKindItemData foodKind)
    {
        switch (foodKind.mealPeriod)
        {
            case "0":
                EditFoodItem(foodBreakfastList, foodKind);
                break;
            case "1":
                EditFoodItem(foodLunchList, foodKind);
                break;
            case "2":
                EditFoodItem(foodDinnerList, foodKind);
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
        if (editFoodItem != null)
        {
            switch (editFoodItem.mealPeriod)
            {
                case "0":
                    DelFoodItem(foodBreakfastList, editFoodItem);
                    break;
                case "1":
                    DelFoodItem(foodLunchList, editFoodItem);
                    break;
                case "2":
                    DelFoodItem(foodDinnerList, editFoodItem);
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
        if (foodDic.Count > 1)
        {
            List<FoodKindItemData> targetList = foodDic;
            targetList.Remove(foodKind);
            SendServerCon();
        }
        else
        {
            delectTipText.SetActive(true);
            StartCoroutine(WaitCloseText(delectTipText));

        }

        //RefreshMealUI();
    }

    public void RefreshMealUI()
    {
        //SumTotalBreakfastFood(foodBreakfastList, foodBreakfastParent, "Zao");
        //SumTotalBreakfastFood(foodLunchList, foodLunchParent, "Zhong");
        //SumTotalBreakfastFood(foodDinnerList, foodDinnerParent, "Wan");
        SumTotalEveryMeal();

    }



    //点击选完所有餐的按钮 然后发送服务器所有数据
    private void SelectEndUI()
    {
        if (FoodChooseKind() && lastValue == 2)
        {
            UIManager.Instance.CloseUICaoZuo("ChooseShanShiUI");
            //ShanShiCon.Instance.conditionMet = true;
            GameManager.Instance.SetStepDetection(true);  //打开营养师的提示位置
            SendServerCon();
            goYingYiangShiText.SetActive(true);
            StartCoroutine(WaitCloseText(goYingYiangShiText));

            return;
        }
        selectEndTipText.SetActive(true);
        StartCoroutine(WaitCloseText(selectEndTipText));
    }

    public void SendServerCon()
    {
        FoodSendConverDay foodSendConverDay = new FoodSendConverDay();
        foodSendConverDay.breakfast = AddFoodSendConverList(foodBreakfastList);
        foodSendConverDay.lunch = AddFoodSendConverList(foodLunchList);
        foodSendConverDay.dinner = AddFoodSendConverList(foodDinnerList);
        foodSendConverDay.userInfo = userInfo;
        ServerCon.Instance.ConverToJson(foodSendConverDay);
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
        if (FoodChooseKind() && newValue != lastValue)
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

        Debug.Log(foodBreakfastList.Count + "  zao" + foodLunchList.Count + "  zhong" + foodDinnerList.Count + "  wan");
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

    int foodChooseCount = 0;

    public void AddFoodDic(List<FoodKindItemData> foodLsit, string kindName, int count)
    {
        foodChooseCount = count;
        // 创建新列表并复制数据（浅拷贝）
        List<FoodKindItemData> copiedList = new List<FoodKindItemData>(foodLsit);
        switch (dropdown.value)
        {
            case 0:
                //foodBreakfastList.Remove(kindName);
                //foodBreakfastList.Add(kindName, foodLsit);
                foodBreakfastList = copiedList;
                //Debug.Log(foodBreakfastList.Count());
                totalFoodAllLists.RemoveAll(item => item.foodName == "Zao");


                break;
            case 1:
                //foodLunchList.Remove(kindName);
                //foodLunchList.Add(kindName, foodLsit);
                foodLunchList = copiedList;

                //Debug.Log(foodLunchList.Count());
                totalFoodAllLists.RemoveAll(item => item.foodName == "Zhong");


                break;
            case 2:
                //foodDinnerList.Remove(kindName);
                //foodDinnerList.Add(kindName, foodLsit);
                foodDinnerList = copiedList;

                //Debug.Log(foodDinnerList.Count());
                totalFoodAllLists.RemoveAll(item => item.foodName == "Wan");

                break;
        }

    }

    public void DeleteFoodKind(string foodKindNmae)
    {
        switch (foodKindNmae)
        {
            case "Zao":
                dropdown.value = 0;
                foodBreakfastList.Clear();
                totalFoodAllLists.RemoveAll(item => item.foodName == foodKindNmae);
                break;
            case "Zhong":
                dropdown.value = 1;

                foodLunchList.Clear();
                totalFoodAllLists.RemoveAll(item => item.foodName == foodKindNmae);
                break;
            case "Wan":
                dropdown.value = 2;

                foodDinnerList.Clear();
                totalFoodAllLists.RemoveAll(item => item.foodName == foodKindNmae);
                break;
        }
    }

    //判断是否3餐都选择完了
    public bool TotalFoodShow()
    {
        //Debug.Log(totalFoodAllLists.Count+" totalfoodall");
        if (totalFoodAllLists.Count < 3) return false;

        return true;
    }

    //进行食物选择的判断 如果本餐的食物选择数量大于6 就可以切换哪餐
    public bool FoodChooseKind()
    {
        switch (lastValue)
        {
            case 0:
                if (foodBreakfastList.Count < 6) return false;
                return true;
            case 1:
                if (foodLunchList.Count < 6) return false;
                return true;

            case 2:
                if (foodDinnerList.Count < 6) return false;
                return true;

            default:
                return false;
        }


        Debug.Log(foodChooseCount);
        if (foodChooseCount < 6) return false;
        return true;

    }



    //出报告显示
    //public void SumTotalBreakfastFood(Dictionary<string, List<FoodKindItemData>> foodDic, Transform foodParent, string mealName)
    public void SumTotalBreakfastFood(List<FoodKindItemData> foodDic, Transform foodParent, string mealName)
    {
        //totalFoodAllLists.Clear();
        // 清空现有Toggle
        foreach (Transform child in foodParent)
        {
            Destroy(child.gameObject);
        }
        TotalFoodAll totalFood = new TotalFoodAll();
        //foreach (KeyValuePair<string, List<FoodKindItemData>> entry in foodDic)
        //{
        //string category = entry.Key; // 获取当前分类（如 "Fruits"）
        //List<FoodKindItemData> foodList = entry.Value; // 获取当前分类下的食物列表
        List<FoodKindItemData> foodList = foodDic; // 获取当前分类下的食物列表
        TotalFoodAll totalFoodAll = new TotalFoodAll();
        // 遍历当前分类下的所有食物
        foreach (FoodKindItemData foodItem in foodList)
        {
            GameObject toggleObj = Instantiate(foodPrefab, foodParent);
            toggleObj.SetActive(true);
            SetFoodItemText(toggleObj, "Name", foodItem.foodName);
            SetFoodItemText(toggleObj, "Amount", foodItem.count.ToString());
            //SetFoodItemText(toggleObj, "Unit", BackMultiplyuantity(foodItem.count, float.Parse(foodItem.water)));
            SetFoodItemText(toggleObj, "EnergyKcal", BackMultiplyuantity(foodItem.count, float.Parse(foodItem.heat)));
            //SetFoodItemText(toggleObj, "energyKj", BackMultiplyuantity(foodItem.count, float.Parse(foodItem.energyKj)));
            SetFoodItemText(toggleObj, "Protein", BackMultiplyuantity(foodItem.count, float.Parse(foodItem.protein)));
            SetFoodItemText(toggleObj, "Fat", BackMultiplyuantity(foodItem.count, float.Parse(foodItem.fat)));
            SetFoodItemText(toggleObj, "Cho", BackMultiplyuantity(foodItem.count, float.Parse(foodItem.cho)));
            //toggleObj.transform.Find("Name").GetComponent<Text>().text = foodItem.foodName;
            //toggleObj.transform.Find("Amount").GetComponent<Text>().text = foodItem.count.ToString();
            //toggleObj.transform.Find("Unit").GetComponent<Text>().text = BackMultiplyuantity(foodItem.count, float.Parse(foodItem.water));
            //toggleObj.transform.Find("EnergyKcal").GetComponent<Text>().text = foodItem.energyKcal;
            //toggleObj.transform.Find("Protein").GetComponent<Text>().text = foodItem.protein;
            //toggleObj.transform.Find("Fat").GetComponent<Text>().text = foodItem.fat;
            //toggleObj.transform.Find("Cho").GetComponent<Text>().text = foodItem.cho;

            //totalFood.totalEnergyKca += float.Parse(foodItem.energyKcal);
            //totalFood.protein += float.Parse(foodItem.protein);
            //totalFood.fat += float.Parse(foodItem.fat);
            //totalFood.cho += float.Parse(foodItem.cho);
            Toggle toggle = toggleObj.GetComponent<Toggle>();

            toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn) OnAlterFoodSelected(foodItem, toggle, isOn);
            });
            //}
        }
        //totalFood.foodName = mealName;
        //totalFood.totalEnergyKcaStandard = Compare(totalFood.totalEnergyKca, 5);
        //totalFoodAllLists.Add(totalFood);
        //Debug.Log(totalFoodAllLists.Count + "zongshu ");
    }
    //传入text名字 设置text文本
    public void SetFoodItemText(GameObject tgl, string textName, string foodInfor)
    {
        //if (tgl.transform.Find(textName) == null) return;
        tgl.transform.Find(textName).GetComponent<Text>().text = foodInfor;

    }

    public string BackMultiplyuantity(float quantity, float parameters)
    {
        return (quantity * parameters).ToString("F2");
    }

    void OnAlterFoodSelected(FoodKindItemData food, Toggle tglItem, bool isOn)
    {
        if (isOn)
        {
            editFoodItem = food;
            //AmendUI.Instance.SetIconInf(food);
        }
        else
        {
            editFoodItem = null;
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
        everyMealEnergies.Clear();
        everyMealEnergies.Add(response.data.breakfastEnergy);
        everyMealEnergies.Add(response.data.lunchEnergy);
        everyMealEnergies.Add(response.data.dinnerEnergy);
        elementResult = response.data.elementResult;
        compareResult = response.data.compareResult;
        totalEnergy = response.data.totalEnergy;
        recEnergyIntake = response.data.recEnergyIntake;
        ExcelExporter.Instance.response = response;
        RefreshMealUI();

    }
    EveryMealEnergy totalEnergy = new EveryMealEnergy();
    EveryMealEnergy recEnergyIntake = new EveryMealEnergy();
    CompareResult compareResult = new CompareResult();
    private List<EveryMealEnergy> everyMealEnergies = new List<EveryMealEnergy>();
    private List<ElementResult> elementResult = new List<ElementResult>();
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
        SumTotalBreakfastFood(foodBreakfastList, foodBreakfastParent, "Zao");

        SumTotalBreakfastFood(foodLunchList, foodLunchParent, "Zhong");
        SumTotalBreakfastFood(foodDinnerList, foodDinnerParent, "Wan");
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
        GameObject totalTgl = Instantiate(everyMealObj, allFoodPartent);
        totalTgl.SetActive(true);
        totalTgl.transform.GetChild(2).GetComponent<Text>().text = totalEnergy.totalEnergyKcal;
        totalTgl.transform.GetChild(5).GetComponent<Text>().text = totalEnergy.protein;
        totalTgl.transform.GetChild(8).GetComponent<Text>().text = totalEnergy.fat;
        totalTgl.transform.GetChild(11).GetComponent<Text>().text = totalEnergy.cho;


        foreach (var item in elementResult)
        {
            GameObject elementItem = Instantiate(allFoodItemprefab, allFoodPartent);
            elementItem.SetActive(true);
            elementItem.transform.GetChild(0).GetComponent<Text>().text = item.zhName;
            elementItem.transform.GetChild(1).GetComponent<Text>().text = item.totalContent;
        }
        bool fl = true;
        if (compareResult.choDiff < 0)
        {
            tipEnergyList[0].SetActive(true);
            fl = false;
        }
        else if (compareResult.choDiff > 0)
        {
            tipEnergyList[1].SetActive(true);
            fl = false;

        }
        if (compareResult.fatDiff < 0)
        {
            tipEnergyList[2].SetActive(true);
            fl = false;

        }
        else if (compareResult.fatDiff > 0)
        {
            tipEnergyList[3].SetActive(true);
            fl = false;

        }
        if (compareResult.energyDiff < 0)
        {
            tipEnergyList[4].SetActive(true);
            fl = false;

        }
        else if (compareResult.energyDiff > 0)
        {
            tipEnergyList[5].SetActive(true);
            fl = false;

        }
        if (compareResult.proteinDiff < 0)
        {
            tipEnergyList[6].SetActive(true);
            fl = false;
        }
        else if (compareResult.proteinDiff > 0)
        {
            tipEnergyList[7].SetActive(true);
            fl = false;
        }
        if (fl)
        {
            tipEnergyList[8].SetActive(true);

        }

    }



    public void EnableInform()
    {
        SetChooseMealActive(false);

        SelectEnd();
    }

    private void SelectEnd()
    {
        //if (TotalFoodShow())
        {
            totalFoodAllLists.Clear();

            //UIManager.Instance.OpenUICaoZuo("MealReportUI");
            GameObjMan.Instance.CLoseFirst();
            SumTotalEveryMeal();
            //ShanShiCon.Instance.conditionMet = true;

            //return;
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
