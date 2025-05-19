using Newtonsoft.Json;
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
/// 选择食物界面
/// </summary>
public class FoodChooseUI : UICaoZuoBase
{
    public static FoodChooseUI Instance { get; private set; }
    // UI元素
    //public Transform categoryButtonsParent;
    public Transform foodToggleParent;
    public GameObject foodPrefab;
    public ToggleGroup toggleGroup;
    public InputField searchInput;
    public GameObject alterUI;
    public Button affirmBtn;

    public Toggle recipeTgl;

    public Button chooseClose;

    public string foodSort;

    public Transform addFoodToggleParent;
    public GameObject addFoodPrefab;
    public ToggleGroup addToggleGroup;
    public GameObject addFoodRecipeContentPrefab;

    public GameObject addFoodRecipeToggleParent;
    //public GameObject addFoodRecipePrefab;
    public ToggleGroup addRecipeToggleGroup;

    public Dropdown chooseLevel2;

    public TMP_InputField recipeInput;
    public Button recipeBtn;
    public GameObject chooseRecipeUI;

    public GameObject contentRecipe;
    public GameObject recipeItem;

    public GameObject alterRecipeUI;
    GameObject foodRecpeItemObj;//点击食谱里的食物的缓存


    // 数据存储
    //private Dictionary<string, List<FoodKindItemData>> foodDictionary = new Dictionary<string, List<FoodKindItemData>>();
    private FoodKindItemData currentSelectedFood;
    private RecipeItem currentSelectedFoodRecipe;
    //private Toggle tgl;
    //private string currentCategory;
    Dictionary<string, List<FoodKindItemData>> endFoodDataDic = new Dictionary<string, List<FoodKindItemData>>();
    Dictionary<string, List<FoodKindItemData>> foodChooseData = new Dictionary<string, List<FoodKindItemData>>();
    //   FoodKindItemData[] dataDic = new FoodKindItemData[6];
    List<FoodKindItemData> dataRaw = new List<FoodKindItemData>();
    List<FoodKindItemData> data = new List<FoodKindItemData>();

    //List<FoodKindItemData> addFoodList = new List<FoodKindItemData>();
    ChooseFoodData addFoodList = new ChooseFoodData();
    //List<FoodKindItemData> thisAddFoodList = new List<FoodKindItemData>();
    //List<FoodKindItemData> chooseEndAllFoodList = new List<FoodKindItemData>();

    private Queue<GameObject> toggleFoodPool = new Queue<GameObject>();  //食物对象池
    private Queue<GameObject> toggleFoodRecipePool = new Queue<GameObject>();  //食谱对象池

    List<RecipeItem> recipeItems = new List<RecipeItem>();


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

    void Start()
    {
        //searchInput.onValueChanged.AddListener(OnSearch);
        affirmBtn.onClick.AddListener(OnSearch);
        recipeBtn.onClick.AddListener(OnSearchRecipe);
        MessageCenter.Instance.Register("SendAlterConfirmFood", OnConfirm);
        MessageCenter.Instance.Register("SendAlterDeletFood", AlterConfirmFood);
        chooseClose.onClick.AddListener(ChooseCloseUI);
        //InitDropDown();
        chooseLevel2.onValueChanged.AddListener(OnDropdownValueChanged);
        if (YiTiJiUI.Instance.BackUserInfo().isBaby)
        {
            recipeTgl.gameObject.SetActive(false);
        }
        else
        {
            recipeTgl.gameObject.SetActive(true);
        }
    }



    private void Update()
    {


    }


    private void ChooseCloseUI()
    {
        foreach (var item in addFoodList.allFoods)
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
        //thisAddFoodList.Clear();
        endFoodDataDic.Clear();
    }

    private void OnEnable()
    {
        MessageCenter.Instance.Register("SendChooseItemToShanShiUI", ChooseItemToShanShiUI);
        chooseLevel2.value = 0;

        //StartCoroutine(WaitShow());
        //InitDropDown();
        searchInput.text = "";
        recipeInput.text = "";
        /*List<RecipeItem>*/
        recipeItems = FoodManager.Instance.response.rows;
        RefreshFoodRecipeList(recipeItems);

        if (YiTiJiUI.Instance.BackUserInfo().isBaby)
        {
            recipeTgl.gameObject.SetActive(false);
        }
        else
        {
            recipeTgl.gameObject.SetActive(true);
        }
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
        //wwww
        if (ChooseFoodAllInformCon.Instance.isSame || ChooseFoodAllInformCon.Instance.chooseValue /*|| ChooseFoodAllInformCon.Instance.againBl*/)
        {
            ChooseFoodAllInformCon.Instance.isSame = false;
            ChooseFoodAllInformCon.Instance.chooseValue = false;
            ChooseFoodAllInformCon.Instance.endChooseValue = true;
            //ChooseFoodAllInformCon.Instance.againBl = false;
            addFoodList.foods.Clear();
            addFoodList.recipes.Clear();
            //Debug.Log("false shuaxin");
            dataRaw = FoodManager.Instance.GetItemById(obj);
            addFoodList.allFoods.Clear();
            foreach (Transform child in addFoodToggleParent)
            {
                Destroy(child.gameObject);
            }
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
        //RefreshAddFoodList();
        InitDropDown();

    }


    // 刷新食物列表
    //void RefreshFoodList(List<FoodKindItemData> foods, string filter = "")
    //{
    //    // 清空现有Toggle
    //    foreach (Transform child in foodToggleParent)
    //    {
    //        Destroy(child.gameObject);
    //    }
    //    // 创建新的Toggle
    //    foreach (var food in foods)
    //    {
    //        GameObject toggleObj = Instantiate(foodPrefab, foodToggleParent);
    //        toggleObj.SetActive(true);
    //        Toggle toggle = toggleObj.GetComponent<Toggle>();
    //        //toggle.group = toggleGroup;
    //        toggle.interactable = !food.isOnClick;
    //        toggle.isOn = food.isOnClick;
    //        toggle.transform.Find("Text1").GetComponent<Text>().text = food.foodName;
    //        toggle.transform.Find("Code").GetComponent<Text>().text = food.foodCode;
    //        //toggle.transform.Find("Background").GetComponent<Image>().sprite = Resources.Load<Sprite>(/*data[i].id */"Icons" + "/" + food.foodCode);
    //        StartCoroutine(LoadImage(food, toggle.transform.Find("Background").GetComponent<Image>()));

    //        // 显示食物名称
    //        //toggleObj.GetComponentInChildren<Text>().text = food.name;

    //        // 添加点击事件
    //        toggle.onValueChanged.AddListener((isOn) =>
    //        {
    //            if (isOn) OnFoodSelected(food, toggle, isOn);
    //        });
    //    }
    //}

    void RefreshFoodList(List<FoodKindItemData> foods, string filter = "")
    {
        // 回收所有现有的Toggle到对象池
        while (foodToggleParent.childCount > 0)
        {
            Transform child = foodToggleParent.GetChild(0);
            child.gameObject.SetActive(false);
            child.SetParent(null); // 避免在父对象变化时影响布局
            toggleFoodPool.Enqueue(child.gameObject);
        }

        // 创建/复用Toggle
        foreach (var food in foods)
        {
            // 从池中获取或实例化新Toggle
            GameObject toggleObj = GetPooledToggle();

            Toggle toggle = toggleObj.GetComponent<Toggle>();
            Text text1 = toggleObj.transform.Find("Text1").GetComponent<Text>();
            Text code = toggleObj.transform.Find("Code").GetComponent<Text>();
            StartCoroutine(LoadImage(food, toggle.transform.Find("Background").GetComponent<Image>()));

            // 重置Toggle状态
            toggle.onValueChanged.RemoveAllListeners();
            toggle.interactable = !food.isOnClick;
            toggle.isOn = food.isOnClick;
            text1.text = food.foodName;
            code.text = food.foodCode;

            // 绑定新事件
            toggle.onValueChanged.AddListener(isOn =>
            {
                if (isOn) OnFoodSelected(food, toggle, isOn);
            });
        }
    }



    GameObject GetPooledToggle()
    {
        // 尝试从池中获取可用对象
        while (toggleFoodPool.Count > 0)
        {
            GameObject obj = toggleFoodPool.Dequeue();
            if (obj != null)
            {
                obj.transform.SetParent(foodToggleParent);
                obj.SetActive(true);
                return obj;
            }
        }

        // 没有可用对象时创建新实例
        GameObject newToggle = Instantiate(foodPrefab, foodToggleParent);
        newToggle.SetActive(true);
        return newToggle;
    }
    // 使用协程加载图片
    private IEnumerator LoadImage(FoodKindItemData food, Image back)
    {
        // 发送请求以从 URL 加载图片
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(food.imageUrl);

        // 等待请求完成
        yield return request.SendWebRequest();

        // 检查是否请求成功
        if (request.result == UnityWebRequest.Result.Success)
        {
            // 获取返回的纹理
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            // 将纹理转换为 Sprite
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            // 将 Sprite 赋值给 Image 组件
            back.sprite = sprite;
        }
        else
        {
            back.sprite = Resources.Load<Sprite>("暂无图片");
            Debug.LogError("Failed to load image: " + request.error + "  " + food.foodCode + " " + food.foodName);
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
        currentSelectedFood.foodCount = float.Parse(obj);
        currentSelectedFood.count = currentSelectedFood.foodCount;
        currentSelectedFood.isOnClick = true;
        currentSelectedFood.mealPeriod = ChooseFoodAllInformCon.Instance.BackDropValue().ToString();
        addFoodList.foods.Add(currentSelectedFood);
        //thisAddFoodList.Add(currentSelectedFood);
        //tgl.interactable = false;
        ShowEditPanel(false, null);
        RefreshFoodList(data);
        //RefreshAddFoodList();
        RefreshAddFoodList(currentSelectedFood);
        bool foodBl = false;
        foreach (var item in addFoodList.allFoods)
        {
            if (currentSelectedFood.foodCode == item.foodCode)
            {
                foodBl = true;
                item.foodCount = currentSelectedFood.foodCount;
                item.count = item.foodCount + item.recipeCount;
            }
        }
        if (!foodBl)
        {
            addFoodList.allFoods.Add(currentSelectedFood);
        }
    }



    /// <summary>
    /// 食谱的刷新加载
    /// </summary>
    /// <param name="foods"></param>
    /// <param name="filter"></param>

    void RefreshFoodRecipeList(List<RecipeItem> recipeItems, string filter = "")
    {
        // 回收所有现有的Toggle到对象池
        //while (foodToggleParent.childCount > 0)
        //{
        //    Transform child = foodToggleParent.GetChild(0);
        //    child.gameObject.SetActive(false);
        //    child.SetParent(null); // 避免在父对象变化时影响布局
        //    toggleFoodRecipePool.Enqueue(child.gameObject);
        //}
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
        //while (toggleFoodRecipePool.Count > 0)
        //{
        //    GameObject obj = toggleFoodRecipePool.Dequeue();
        //    if (obj != null)
        //    {
        //        obj.transform.SetParent(contentRecipe.transform);
        //        obj.SetActive(true);
        //        return obj;
        //    }
        //}
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
            if (food.isOnClick) return;

            currentSelectedFoodRecipe = food;
            //tgl = tglItem;
            ShowRecipeEditPanel(true, food);
            //quantityInput.text = "";
            ChooseRecipeUI.Instance.UpFoodItem(food, foodSort);

        }
    }

    // 显示/隐藏编辑面板
    void ShowRecipeEditPanel(bool show, RecipeItem food)
    {
        chooseRecipeUI.SetActive(show);

    }

    /// <summary>
    /// 食谱确认选择后发id那所用的食材
    /// </summary>
    public void OnRecipeConfirm()
    {
        ServerCon.Instance.LoadRecipe("/cookbook/getRecipeFood", $"?recipeId={currentSelectedFoodRecipe.id}");

    }
    /// <summary>
    /// 接收到从服务器发回来的食材数据
    /// </summary>
    /// <param name="food"></param>
    public void ReceiveRecipeItem(List<FoodKindItemData> food)
    {
        currentSelectedFoodRecipe.isOnClick = true;
        foreach (var item in food)
        {
            item.mealPeriod = ChooseFoodAllInformCon.Instance.BackDropValue().ToString();
        }
        currentSelectedFoodRecipe.foodKindItems = food;
        addFoodList.recipes.Add(currentSelectedFoodRecipe);
        //addFoodList.Add(currentSelectedFoodRecipe);
        //thisAddFoodList.Add(currentSelectedFoodRecipe);
        //tgl.interactable = false;
        ShowEditPanel(false, null);
        //RefreshFoodRecipeList(recipeItems);
        chooseRecipeUI.SetActive(false);
        foreach (var item in food)
        {
            item.recipeCount = 1;
            item.count = 1;
            bool foodBl = false;
            foreach (var tem in addFoodList.allFoods)
            {
                if (item.foodCode == tem.foodCode)
                {
                    foodBl = true;
                    tem.count = item.recipeCount + tem.foodCount;
                }
            }
            if (!foodBl)
            {
                addFoodList.allFoods.Add(item);
            }
        }
        RefreshAddFoodRecipeList(food);

    }
    private string SerializeData(UserInfo data)
    {
        // 方法一：使用 Unity 内置 JsonUtility（需要包装类）
        Wrapper<UserInfo> wrapper = new Wrapper<UserInfo> { items = data };
        //return JsonUtility.ToJson(wrapper);

        // 方法二：使用 Newtonsoft.Json（直接序列化列表）
        return JsonConvert.SerializeObject(data);
    }
    #region //刷新食物列表

    //// 刷新选择后食物列表
    //void RefreshAddFoodList()
    //{
    //    // 清空现有Toggle
    //    foreach (Transform child in addFoodToggleParent)
    //    {
    //        Destroy(child.gameObject);
    //    }
    //    // 创建新的Toggle
    //    foreach (var food in addFoodList.foods)
    //    {
    //        GameObject toggleObj = Instantiate(addFoodPrefab, addFoodToggleParent);
    //        toggleObj.SetActive(true);
    //        Toggle toggle = toggleObj.transform.GetChild(0).GetComponent<Toggle>();
    //        toggle.group = toggleGroup;
    //        toggle.interactable = !food.isOnClick;
    //        toggle.isOn = food.isOnClick;
    //        toggleObj.transform.Find("id").GetComponent<Text>().text = food.foodName;
    //        toggleObj.transform.Find("Count").GetComponent<Text>().text = food.count.ToString();
    //        //toggleObj.transform.Find("Unit").GetComponent<Text>().text = food.water;
    //        toggleObj.transform.Find("Heat").GetComponent<Text>().text = BackMultiplyuantity(food.count, float.Parse(food.heat));
    //        toggleObj.transform.Find("Protein").GetComponent<Text>().text = BackMultiplyuantity(food.count, float.Parse(food.protein));
    //        toggleObj.transform.Find("Fat").GetComponent<Text>().text = BackMultiplyuantity(food.count, float.Parse(food.fat));
    //        toggleObj.transform.Find("carbohydrate").GetComponent<Text>().text = BackMultiplyuantity(food.count, float.Parse(food.cho));

    //    }
    //    // 创建新的Toggle
    //    foreach (var food in addFoodList.recipes)
    //    {
    //        GameObject toggleObj = Instantiate(addFoodRecipeContentPrefab, addFoodToggleParent);
    //        toggleObj.SetActive(true);
    //        Toggle toggle = toggleObj.transform.GetChild(0).GetComponent<Toggle>();
    //        toggle.group = toggleGroup;
    //        toggle.interactable = !food.isOnClick;
    //        toggle.isOn = food.isOnClick;
    //        toggleObj.transform.Find("id").GetComponent<Text>().text = food.foodName;
    //        toggleObj.transform.Find("Count").GetComponent<Text>().text = food.count.ToString();
    //        //toggleObj.transform.Find("Unit").GetComponent<Text>().text = food.water;
    //        toggleObj.transform.Find("Heat").GetComponent<Text>().text = BackMultiplyuantity(food.count, float.Parse(food.heat));
    //        toggleObj.transform.Find("Protein").GetComponent<Text>().text = BackMultiplyuantity(food.count, float.Parse(food.protein));
    //        toggleObj.transform.Find("Fat").GetComponent<Text>().text = BackMultiplyuantity(food.count, float.Parse(food.fat));
    //        toggleObj.transform.Find("carbohydrate").GetComponent<Text>().text = BackMultiplyuantity(food.count, float.Parse(food.cho));

    //    }
    //}
    #endregion
    void RefreshAddFoodList(FoodKindItemData food)
    {

        // 创建新的Toggle
        //foreach (var food in addFoodList.foods)
        {
            GameObject toggleObj = Instantiate(addFoodPrefab, addFoodToggleParent);
            toggleObj.SetActive(true);
            Toggle toggle = toggleObj.transform.GetChild(0).GetComponent<Toggle>();
            //toggle.group = toggleGroup;
            //toggle.interactable = !food.isOnClick;
            //toggle.isOn = food.isOnClick;
            toggleObj.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = food.foodName;
            toggleObj.transform.Find("Count").GetComponent<Text>().text = food.count.ToString();
            //toggleObj.transform.Find("Unit").GetComponent<Text>().text = food.water;
            toggleObj.transform.Find("Heat").GetComponent<Text>().text = BackMultiplyuantity(food.count, float.Parse(food.heat));
            toggleObj.transform.Find("Protein").GetComponent<Text>().text = BackMultiplyuantity(food.count, float.Parse(food.protein));
            toggleObj.transform.Find("Fat").GetComponent<Text>().text = BackMultiplyuantity(food.count, float.Parse(food.fat));
            toggleObj.transform.Find("carbohydrate").GetComponent<Text>().text = BackMultiplyuantity(food.count, float.Parse(food.cho));

        }

    }
    void RefreshAddFoodRecipeList(List<FoodKindItemData> foods)
    {
        GameObject obj = Instantiate(addFoodRecipeToggleParent, addFoodToggleParent);
        obj.SetActive(true);
        // 创建新的Toggle
        foreach (var food in foods)
        {
            GameObject toggleObj = Instantiate(addFoodPrefab, obj.transform.GetChild(0).GetChild(0));
            toggleObj.SetActive(true);
            Toggle toggle = toggleObj.transform.GetChild(0).GetComponent<Toggle>();
            //toggle.group = addRecipeToggleGroup;
            //toggle.interactable = !food.isOnClick;
            //toggle.isOn = food.isOnClick;
            //toggleObj.transform.Find("id").GetComponent<Text>().text = food.foodName;
            toggleObj.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = food.foodName;
            toggleObj.transform.Find("Count").GetComponent<Text>().text = food.count.ToString();
            //toggleObj.transform.Find("Unit").GetComponent<Text>().text = food.water;
            toggleObj.transform.Find("Heat").GetComponent<Text>().text = BackMultiplyuantity(food.count, float.Parse(food.heat));
            toggleObj.transform.Find("Protein").GetComponent<Text>().text = BackMultiplyuantity(food.count, float.Parse(food.protein));
            toggleObj.transform.Find("Fat").GetComponent<Text>().text = BackMultiplyuantity(food.count, float.Parse(food.fat));
            toggleObj.transform.Find("carbohydrate").GetComponent<Text>().text = BackMultiplyuantity(food.count, float.Parse(food.cho));
            // 绑定新事件
            toggle.onValueChanged.AddListener(isOn =>
            {
                if (isOn) OnAlterFoodCount(food, toggleObj, isOn);
            });
        }
    }
    private void OnAlterFoodCount(FoodKindItemData food, GameObject toggleObj, bool isOn)
    {
        foodRecpeItemObj = toggleObj;
        alterRecipeUI.SetActive(true);
        AlterRecipeUI.Instance.UpFoodItem(food, foodSort);

    }

    public void AlterReciopeCount(int count, string foodCode)
    {
        foreach (var item in addFoodList.allFoods)
        {
            if (foodCode == item.foodCode)
            {
                item.recipeCount = count;
                item.count = item.foodCount + count;
                foodRecpeItemObj.transform.Find("Count").GetComponent<Text>().text = count.ToString();
                foodRecpeItemObj.transform.Find("Heat").GetComponent<Text>().text = BackMultiplyuantity(count, float.Parse(item.heat));
                foodRecpeItemObj.transform.Find("Protein").GetComponent<Text>().text = BackMultiplyuantity(count, float.Parse(item.protein));
                foodRecpeItemObj.transform.Find("Fat").GetComponent<Text>().text = BackMultiplyuantity(count, float.Parse(item.fat));
                foodRecpeItemObj.transform.Find("carbohydrate").GetComponent<Text>().text = BackMultiplyuantity(count, float.Parse(item.cho));
            }
        }
        //addFoodList.allFoods.Add(currentSelectedFood);
    }

    public string BackMultiplyuantity(float quantity, float parameters)
    {
        return ((quantity * parameters) /*/ 100*/).ToString("F2");
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
    private void OnSearchRecipe()
    {
        string keyword = recipeInput.text;
        if (string.IsNullOrEmpty(keyword))
        {
            RefreshFoodRecipeList(recipeItems);

        }
        else
        {
            var filtered = recipeItems.Where(f => f.recipeName.Contains(keyword)).ToList();
            RefreshFoodRecipeList(filtered);

        }
    }
}
