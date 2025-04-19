using RTS;
using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class ChooseFoodAllInformCon : MonoSingletonBase<ChooseFoodAllInformCon>
{
    //private Dictionary<string, List<FoodKindItemData>> foodBreakfastDic = new Dictionary<string, List<FoodKindItemData>>();
    //private Dictionary<string, List<FoodKindItemData>> foodLunchDic = new Dictionary<string, List<FoodKindItemData>>();
    //private Dictionary<string, List<FoodKindItemData>> foodDinnerDic = new Dictionary<string, List<FoodKindItemData>>();
    //private Dictionary<string, List<FoodKindItemData>> foodChooseBlDic = new Dictionary<string, List<FoodKindItemData>>();

    private List<FoodKindItemData> foodBreakfastDic = new List<FoodKindItemData>();
    private List<FoodKindItemData> foodLunchDic = new List<FoodKindItemData>();
    private List<FoodKindItemData> foodDinnerDic = new List<FoodKindItemData>();

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
    public Text selectEndTipText;

    public Toggle amendTgl;
    public Toggle deltgl;
    public GameObject editUI;

    private FoodKindItemData editFoodItem;

    private void OnEnable()
    {
        foodBreakfastDic.Clear();
        foodLunchDic.Clear();
        foodDinnerDic.Clear();
        totalFoodAllLists.Clear();
        dropdown.value = 0;
        editUI.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        //MessageCenter.Instance.Register()
        choossFoodAllBtn.onClick.AddListener(SelectEnd);
        lastValue = dropdown.value;
        selectEndBtn.onClick.AddListener(SelectEndUI);

        // 绑定事件
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        amendTgl.onValueChanged.AddListener(AmendTglClick);//修改按钮
        deltgl.onValueChanged.AddListener(DelTglClick);//删除按钮

    }

    private void AmendTglClick(bool arg0)
    {
        if (editFoodItem != null)
        {
            editUI.SetActive(true);
            AmendUI.Instance.SetIconInf(editFoodItem);
        }
    }

    public void EditBackFood(FoodKindItemData foodKind)
    {
        switch (foodKind.mealPeriod)
        {
            case "0":
                EditFoodItem(foodBreakfastDic, foodKind);
                break;
            case "1":
                EditFoodItem(foodLunchDic, foodKind);
                break;
            case "2":
                EditFoodItem(foodDinnerDic, foodKind);
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
            if (food.code == foodKind.code)
            {
                food.count = foodKind.count;
                // 如果只需修改第一个匹配项，可在此处break
                break;
            }
        }
        RefreshMealUI();

    }

    private void DelTglClick(bool arg0)
    {
        if (editFoodItem != null)
        {
            switch (editFoodItem.mealPeriod)
            {
                case "0":
                    DelFoodItem(foodBreakfastDic, editFoodItem);
                    break;
                case "1":
                    DelFoodItem(foodLunchDic, editFoodItem);
                    break;
                case "2":
                    DelFoodItem(foodDinnerDic, editFoodItem);
                    break;
                default:
                    break;
            }
        }
    }

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
        List<FoodKindItemData> targetList = foodDic;
        targetList.Remove(foodKind);
        RefreshMealUI();
    }

    public void RefreshMealUI()
    {
        SumTotalBreakfastFood(foodBreakfastDic, foodBreakfastParent, "Zao");
        SumTotalBreakfastFood(foodLunchDic, foodLunchParent, "Zhong");
        SumTotalBreakfastFood(foodDinnerDic, foodDinnerParent, "Wan");
        SumTotalEveryMeal();

    }




    private void SelectEndUI()
    {
        if (FoodChooseKind() && lastValue==2)
        {
            UIManager.Instance.CloseUICaoZuo("ChooseShanShiUI");
            //ShanShiCon.Instance.conditionMet = true;
            EnableInform();

            return;
        }
        selectEndTipText.gameObject.SetActive(true);
        StartCoroutine(WaitCloseTipText());
    }
    IEnumerator WaitCloseTipText()
    {
        yield return new WaitForSeconds(1);
        selectEndTipText.gameObject.SetActive(false);

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
            return;
        }

        Debug.Log(foodBreakfastDic.Count + "  zao" + foodLunchDic.Count + "  zhong" + foodDinnerDic.Count + "  wan");
        dropdown.value = lastValue;
        textTip.SetActive(true);
        StartCoroutine(WaitCloseText());
    }

    public int BackDropValue()
    {
        return dropdown.value;
    }
    IEnumerator WaitCloseText()
    {
        yield return new WaitForSeconds(1);
        textTip.SetActive(false);
    }

    public bool isSame = false;

    int foodChooseCount = 0;

    public void AddFoodDic(List<FoodKindItemData> foodLsit, string kindName, int count)
    {
        foodChooseCount = count;

        switch (dropdown.value)
        {
            case 0:
                //foodBreakfastDic.Remove(kindName);
                //foodBreakfastDic.Add(kindName, foodLsit);
                foodBreakfastDic = foodLsit;
                //Debug.Log(foodBreakfastDic.Count());
                totalFoodAllLists.RemoveAll(item => item.foodName == "Zao");


                break;
            case 1:
                //foodLunchDic.Remove(kindName);
                //foodLunchDic.Add(kindName, foodLsit);
                foodLunchDic = foodLsit;

                //Debug.Log(foodLunchDic.Count());
                totalFoodAllLists.RemoveAll(item => item.foodName == "Zhong");


                break;
            case 2:
                //foodDinnerDic.Remove(kindName);
                //foodDinnerDic.Add(kindName, foodLsit);
                foodDinnerDic = foodLsit;

                //Debug.Log(foodDinnerDic.Count());
                totalFoodAllLists.RemoveAll(item => item.foodName == "Wan");

                break;
        }

    }

    public void DeleteFoodKind(string foodKindNmae)
    {
        switch (foodKindNmae)
        {
            case "Zao":
                foodBreakfastDic.Clear();
                totalFoodAllLists.RemoveAll(item => item.foodName == foodKindNmae);
                break;
            case "Zhong":
                foodLunchDic.Clear();
                totalFoodAllLists.RemoveAll(item => item.foodName == foodKindNmae);
                break;
            case "Wan":
                foodDinnerDic.Clear();
                totalFoodAllLists.RemoveAll(item => item.foodName == foodKindNmae);
                break;
        }
    }

    public bool TotalFoodShow()
    {
        //Debug.Log(totalFoodAllLists.Count+" totalfoodall");
        if (totalFoodAllLists.Count < 3) return false;

        return true;
    }

    public bool FoodChooseKind()
    {
        //switch (lastValue)
        //{
        //    case 0:
        //        if (foodBreakfastDic.Count < 6) return false;
        //        return true;
        //    case 1:
        //        if (foodLunchDic.Count < 6) return false;
        //        return true;

        //    case 2:
        //        if (foodDinnerDic.Count < 6) return false;
        //        return true;

        //    default:
        //        return false;
        //}


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
            toggleObj.transform.Find("Name").GetComponent<Text>().text = foodItem.iconName;
            toggleObj.transform.Find("Amount").GetComponent<Text>().text = foodItem.count.ToString();
            toggleObj.transform.Find("Unit").GetComponent<Text>().text = foodItem.unit;
            toggleObj.transform.Find("Heat").GetComponent<Text>().text = foodItem.heat;
            toggleObj.transform.Find("Protein").GetComponent<Text>().text = foodItem.protein;
            toggleObj.transform.Find("Fat").GetComponent<Text>().text = foodItem.fat;
            toggleObj.transform.Find("Carbohydrate").GetComponent<Text>().text = foodItem.carbohydrate;

            totalFood.heat += float.Parse(foodItem.heat);
            totalFood.protein += float.Parse(foodItem.protein);
            totalFood.fat += float.Parse(foodItem.fat);
            totalFood.carbohydrate += float.Parse(foodItem.carbohydrate);
            Toggle toggle = toggleObj.GetComponent<Toggle>();

            toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn) OnAlterFoodSelected(foodItem, toggle, isOn);
            });
            //}
        }
        totalFood.foodName = mealName;
        totalFood.heatStandard = Compare(totalFood.heat, 5);
        totalFoodAllLists.Add(totalFood);
        //Debug.Log(totalFoodAllLists.Count + "zongshu ");
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

    //一天总量
    public void SumTotalEveryMeal()
    {
        foreach (Transform child in everyMealParent)
        {
            Destroy(child.gameObject);
        }
        SumTotalBreakfastFood(foodBreakfastDic, foodBreakfastParent, "Zao");

        SumTotalBreakfastFood(foodLunchDic, foodLunchParent, "Zhong");
        SumTotalBreakfastFood(foodDinnerDic, foodDinnerParent, "Wan");
        foreach (TotalFoodAll foodItem in totalFoodAllLists)
        {
            GameObject toggleObj = Instantiate(everyMealUIPrefab, everyMealParent);
            toggleObj.SetActive(true);
            toggleObj.transform.GetChild(3).GetComponent<Text>().text = foodItem.heat.ToString();
            toggleObj.transform.GetChild(3).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(/*data[i].id */"Icons" + "/" + foodItem.heatStandard);
            toggleObj.transform.GetChild(4).GetComponent<Text>().text = foodItem.protein.ToString();
            toggleObj.transform.GetChild(4).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(/*data[i].id */"Icons" + "/" + foodItem.proteinStandard);
            toggleObj.transform.GetChild(5).GetComponent<Text>().text = foodItem.fat.ToString();
            toggleObj.transform.GetChild(5).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(/*data[i].id */"Icons" + "/" + foodItem.fatStandard);
            toggleObj.transform.GetChild(6).GetComponent<Text>().text = foodItem.carbohydrate.ToString();
            toggleObj.transform.GetChild(6).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(/*data[i].id */"Icons" + "/" + foodItem.carbohydrateStandard);
            // 显示食物名称
            totalFoodAll.heat += foodItem.heat;
            totalFoodAll.heat += foodItem.protein;
            totalFoodAll.heat += foodItem.fat;
            totalFoodAll.heat += foodItem.carbohydrate;
        }
        everyMealObj.transform.GetChild(3).GetComponent<Text>().text = totalFoodAll.heat.ToString();
        everyMealObj.transform.GetChild(3).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(/*data[i].id */"Icons" + "/" + totalFoodAll.heatStandard);
        everyMealObj.transform.GetChild(4).GetComponent<Text>().text = totalFoodAll.protein.ToString();
        everyMealObj.transform.GetChild(4).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(/*data[i].id */"Icons" + "/" + totalFoodAll.proteinStandard);
        everyMealObj.transform.GetChild(5).GetComponent<Text>().text = totalFoodAll.fat.ToString();
        everyMealObj.transform.GetChild(5).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(/*data[i].id */"Icons" + "/" + totalFoodAll.fatStandard);
        everyMealObj.transform.GetChild(6).GetComponent<Text>().text = totalFoodAll.carbohydrate.ToString();
        everyMealObj.transform.GetChild(6).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(/*data[i].id */"Icons" + "/" + totalFoodAll.carbohydrateStandard);
    }



    public void EnableInform()
    {
        SelectEnd();
    }

    private void SelectEnd()
    {
        //if (TotalFoodShow())
        {
            totalFoodAllLists.Clear();

            UIManager.Instance.OpenUICaoZuo("MealReportUI");
            GameObjMan.Instance.CLoseFirst();
            SumTotalEveryMeal();
            //ShanShiCon.Instance.conditionMet = true;

            return;
        }


    }

    public void EditAmend(FoodKindItemData foodKind)
    {

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
    public float heat;//热量
    public float heatStandard = 0;//热量标准 0为达到 1为超过 2为低于
    public float protein;//蛋白质
    public float proteinStandard = 0;//蛋白质标准 0为达到 1为超过 2为低于

    public float fat;//脂肪
    public float fatStandard = 0;//脂肪标准 0为达到 1为超过 2为低于

    public float carbohydrate;//碳水化合物
    public float carbohydrateStandard = 0;//碳水化合物标准 0为达到 1为超过 2为低于


}
