using RTS;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEditor.Progress;
//using static UnityEngine.Rendering.DebugUI;


/// <summary>
/// 选择食物界面
/// </summary>
public class FoodChooseUI : UICaoZuoBase
{
    // UI元素
    //public Transform categoryButtonsParent;
    public Transform foodToggleParent;
    public GameObject foodPrefab;
    public ToggleGroup toggleGroup;
    public InputField searchInput;
    public GameObject alterUI;
    public Button affirmBtn;

    public Button chooseClose;

    public string foodSort;

    public Transform addFoodToggleParent;
    public GameObject addFoodPrefab;
    public ToggleGroup addToggleGroup;

    public Dropdown chooseLevel2;



    // 数据存储
    //private Dictionary<string, List<FoodKindItemData>> foodDictionary = new Dictionary<string, List<FoodKindItemData>>();
    private FoodKindItemData currentSelectedFood;
    //private Toggle tgl;
    //private string currentCategory;
    Dictionary<string, List<FoodKindItemData>> endFoodDataDic = new Dictionary<string, List<FoodKindItemData>>();
    Dictionary<string, List<FoodKindItemData>> foodChooseData = new Dictionary<string, List<FoodKindItemData>>();
    //   FoodKindItemData[] dataDic = new FoodKindItemData[6];
    List<FoodKindItemData> dataRaw = new List<FoodKindItemData>();
    List<FoodKindItemData> data = new List<FoodKindItemData>();

    List<FoodKindItemData> addFoodList = new List<FoodKindItemData>();
    List<FoodKindItemData> thisAddFoodList = new List<FoodKindItemData>();

    void Start()
    {
        //searchInput.onValueChanged.AddListener(OnSearch);
        affirmBtn.onClick.AddListener(OnSearch);
        MessageCenter.Instance.Register("SendAlterConfirmFood", OnConfirm);
        MessageCenter.Instance.Register("SendAlterDeletFood", AlterConfirmFood);
        chooseClose.onClick.AddListener(ChooseCloseUI);
        //InitDropDown();
        chooseLevel2.onValueChanged.AddListener(OnDropdownValueChanged);

    }

    private void Update()
    {


    }


    private void ChooseCloseUI()
    {
        foreach (var item in addFoodList)
        {
            if (endFoodDataDic.ContainsKey(item.categoryName))
            {
                endFoodDataDic[item.categoryName].Add(item);
            }
            else
            {
                endFoodDataDic.Add(item.categoryName, new List<FoodKindItemData> { item });
            }
        }

        ChooseFoodAllInformCon.Instance.AddFoodDic(addFoodList, foodSort, endFoodDataDic.Count);
        GameObjMan.Instance.OpenFirst();

        UIManager.Instance.CloseUICaoZuo("ChooseShanShiUI");
        thisAddFoodList.Clear();
        endFoodDataDic.Clear();
    }

    private void OnEnable()
    {
        MessageCenter.Instance.Register("SendChooseItemToShanShiUI", ChooseItemToShanShiUI);
        chooseLevel2.value = 0;
       
        //StartCoroutine(WaitShow());
        //InitDropDown();
        searchInput.text = "";
    }

    public void OnDropdownValueChanged(int value)
    {
        data = GetItemById(chooseLevel2.options[value].text);
        RefreshFoodList(data);

    }

    IEnumerator WaitShow()
    {
        ShowCategory("");
        yield return new WaitForSeconds(0.01f);

        //addFoodList.Clear();
    }

    private void ChooseItemToShanShiUI(string obj)
    {
        //data.Clear();
        foodSort = obj;
        //Debug.Log(foodSort);

        if (ChooseFoodAllInformCon.Instance.isSame /*|| ChooseFoodAllInformCon.Instance.againBl*/)
        {
            ChooseFoodAllInformCon.Instance.isSame = false;
            //ChooseFoodAllInformCon.Instance.againBl = false;
            addFoodList.Clear();
            //Debug.Log("false shuaxin");
            dataRaw = FoodManager.Instance.GetItemById(obj);

            StartCoroutine(WaitShow());

            //addFoodList.Clear();
            Debug.Log(55);
            return;
        }

        dataRaw = FoodManager.Instance.GetItemById(obj);
        StartCoroutine(WaitShow());

    }

    /// <summary>
    /// 初始化 下拉列表
    /// </summary>
    private void InitDropDown()
    {
        chooseLevel2.ClearOptions();//清空
        // 动态添加选项
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

        foreach (var item in foodChooseData)
        {

            options.Add(new Dropdown.OptionData(item.Key));
        }
        chooseLevel2.AddOptions(options);

        // 设置默认选中第一个选项（可选）
        Debug.Log(222);
        chooseLevel2.value = 0;
        chooseLevel2.RefreshShownValue();
        data = GetItemById(chooseLevel2.options[0].text);
        RefreshFoodList(data);
    }


    // 显示指定分类的食物
    public void ShowCategory(string categoryName)
    {
        //currentCategory = categoryName;
        //dataRaw = FoodManager.Instance.GetItemById(foodSort);
        foodChooseData.Clear();
        foreach (var item in dataRaw)
        {
            // 尝试获取现有列表，若不存在则创建
            if (!foodChooseData.TryGetValue(item.subCategoryName, out List<FoodKindItemData> itemList))
            {
                itemList = new List<FoodKindItemData>();
                foodChooseData.Add(item.subCategoryName, itemList);
            }
            // 添加条目到列表
            itemList.Add(item);
        }

        //Debug.Log(foodSort);
        //RefreshFoodList(data);
        RefreshAddFoodList();
        InitDropDown();

    }


    // 刷新食物列表
    void RefreshFoodList(List<FoodKindItemData> foods, string filter = "")
    {
        // 清空现有Toggle
        foreach (Transform child in foodToggleParent)
        {
            Destroy(child.gameObject);
        }
        // 创建新的Toggle
        foreach (var food in foods)
        {
            GameObject toggleObj = Instantiate(foodPrefab, foodToggleParent);
            toggleObj.SetActive(true);
            Toggle toggle = toggleObj.GetComponent<Toggle>();
            //toggle.group = toggleGroup;
            toggle.interactable = !food.isOnClick;
            toggle.isOn = food.isOnClick;
            toggle.transform.Find("Text1").GetComponent<Text>().text = food.foodName;
            toggle.transform.Find("Code").GetComponent<Text>().text = food.foodCode;
            toggle.transform.Find("Background").GetComponent<Image>().sprite = Resources.Load<Sprite>(/*data[i].id */"Icons" + "/" + food.foodCode);
            // 显示食物名称
            //toggleObj.GetComponentInChildren<Text>().text = food.name;

            // 添加点击事件
            toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn) OnFoodSelected(food, toggle, isOn);
            });
        }
    }





    // 食物被选中时
    void OnFoodSelected(FoodKindItemData food, Toggle tglItem, bool isOn)
    {
        if (isOn)
        {
            if (food.isOnClick) return;

            currentSelectedFood = food;
            //tgl = tglItem;
            ShowEditPanel(true, food);
            //quantityInput.text = "";
            AlterUI.Instance.UpFoodItem(food, foodSort);

        }
    }

    // 显示/隐藏编辑面板
    void ShowEditPanel(bool show, FoodKindItemData food)
    {
        alterUI.SetActive(show);

    }

    // 确认修改
    public void OnConfirm(string obj)
    {
        currentSelectedFood.count = int.Parse(obj);
        currentSelectedFood.isOnClick = true;
        currentSelectedFood.mealPeriod = ChooseFoodAllInformCon.Instance.BackDropValue().ToString();
        addFoodList.Add(currentSelectedFood);
        thisAddFoodList.Add(currentSelectedFood);
        //tgl.interactable = false;
        ShowEditPanel(false, null);
        RefreshFoodList(data);
        RefreshAddFoodList();
    }


    // 刷新选择后食物列表
    void RefreshAddFoodList()
    {
        // 清空现有Toggle
        foreach (Transform child in addFoodToggleParent)
        {
            Destroy(child.gameObject);
        }
        // 创建新的Toggle
        foreach (var food in addFoodList)
        {
            GameObject toggleObj = Instantiate(addFoodPrefab, addFoodToggleParent);
            toggleObj.SetActive(true);
            Toggle toggle = toggleObj.transform.GetChild(0).GetComponent<Toggle>();
            toggle.group = toggleGroup;
            toggle.interactable = !food.isOnClick;
            toggle.isOn = food.isOnClick;
            toggleObj.transform.Find("id").GetComponent<Text>().text = food.foodName;
            toggleObj.transform.Find("Count").GetComponent<Text>().text = food.count.ToString();
            toggleObj.transform.Find("Unit").GetComponent<Text>().text = food.water;
            toggleObj.transform.Find("Heat").GetComponent<Text>().text = food.energyKcal;
            toggleObj.transform.Find("Protein").GetComponent<Text>().text = food.protein;
            toggleObj.transform.Find("Fat").GetComponent<Text>().text = food.fat;
            toggleObj.transform.Find("carbohydrate").GetComponent<Text>().text = food.cho;
            // 显示食物名称
            //toggleObj.GetComponentInChildren<Text>().text = food.name;

            //// 添加点击事件
            //toggle.onValueChanged.AddListener((isOn) =>
            //{
            //    if (isOn) OnFoodSelected(food, toggle, isOn);
            //});
        }
    }

    //通过已经按亚类储存好的字典继续亚类名字查询 返回list
    public List<FoodKindItemData> GetItemById(string id)
    {
        if (foodChooseData.TryGetValue(id, out List<FoodKindItemData> data))
        {
            return data;
        }
        Debug.Log($"未找到 ID 为 {id} 的数据");
        return null;
    }


    // 删除食物
    public void AlterConfirmFood(string obj)
    {
        currentSelectedFood.isOnClick = false;
        //tgl.interactable = true;

        ShowEditPanel(false, null);
        RefreshFoodList(data);
    }

    // 搜索功能
    void OnSearch()
    {
        string keyword = searchInput.text;
        if (string.IsNullOrEmpty(keyword))
        {
            RefreshFoodList(data);
        }
        else
        {
            var filtered = data.Where(f => f.foodCode.Contains(keyword)).ToList();
            RefreshFoodList(filtered, keyword);
        }
    }
}
