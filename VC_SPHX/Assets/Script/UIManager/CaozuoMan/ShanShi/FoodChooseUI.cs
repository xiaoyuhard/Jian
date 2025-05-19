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
/// ѡ��ʳ�����
/// </summary>
public class FoodChooseUI : UICaoZuoBase
{
    public static FoodChooseUI Instance { get; private set; }
    // UIԪ��
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
    GameObject foodRecpeItemObj;//���ʳ�����ʳ��Ļ���


    // ���ݴ洢
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

    private Queue<GameObject> toggleFoodPool = new Queue<GameObject>();  //ʳ������
    private Queue<GameObject> toggleFoodRecipePool = new Queue<GameObject>();  //ʳ�׶����

    List<RecipeItem> recipeItems = new List<RecipeItem>();


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
    /// ��ʼ�� �����б�
    /// </summary>
    private void InitDropDown()
    {
        chooseLevel2.ClearOptions();//���
        // ��̬���ѡ��
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

        foreach (var item in foodChooseData)
        {

            options.Add(new Dropdown.OptionData(item.Key));
        }
        chooseLevel2.AddOptions(options);

        // ����Ĭ��ѡ�е�һ��ѡ���ѡ��
        Debug.Log(222);
        chooseLevel2.value = 0;
        chooseLevel2.RefreshShownValue();
        data = GetItemById(chooseLevel2.options[0].text);
        RefreshFoodList(data);

    }


    // ��ʾָ�������ʳ��
    public void ShowCategory(string categoryName)
    {
        //currentCategory = categoryName;
        //dataRaw = FoodManager.Instance.GetItemById(foodSort);
        foodChooseData.Clear();
        foreach (var item in dataRaw)
        {
            // ���Ի�ȡ�����б����������򴴽�
            if (!foodChooseData.TryGetValue(item.subCategoryName, out List<FoodKindItemData> itemList))
            {
                itemList = new List<FoodKindItemData>();
                foodChooseData.Add(item.subCategoryName, itemList);
            }
            // �����Ŀ���б�
            itemList.Add(item);
        }

        //Debug.Log(foodSort);
        //RefreshFoodList(data);
        //RefreshAddFoodList();
        InitDropDown();

    }


    // ˢ��ʳ���б�
    //void RefreshFoodList(List<FoodKindItemData> foods, string filter = "")
    //{
    //    // �������Toggle
    //    foreach (Transform child in foodToggleParent)
    //    {
    //        Destroy(child.gameObject);
    //    }
    //    // �����µ�Toggle
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

    //        // ��ʾʳ������
    //        //toggleObj.GetComponentInChildren<Text>().text = food.name;

    //        // ��ӵ���¼�
    //        toggle.onValueChanged.AddListener((isOn) =>
    //        {
    //            if (isOn) OnFoodSelected(food, toggle, isOn);
    //        });
    //    }
    //}

    void RefreshFoodList(List<FoodKindItemData> foods, string filter = "")
    {
        // �����������е�Toggle�������
        while (foodToggleParent.childCount > 0)
        {
            Transform child = foodToggleParent.GetChild(0);
            child.gameObject.SetActive(false);
            child.SetParent(null); // �����ڸ�����仯ʱӰ�첼��
            toggleFoodPool.Enqueue(child.gameObject);
        }

        // ����/����Toggle
        foreach (var food in foods)
        {
            // �ӳ��л�ȡ��ʵ������Toggle
            GameObject toggleObj = GetPooledToggle();

            Toggle toggle = toggleObj.GetComponent<Toggle>();
            Text text1 = toggleObj.transform.Find("Text1").GetComponent<Text>();
            Text code = toggleObj.transform.Find("Code").GetComponent<Text>();
            StartCoroutine(LoadImage(food, toggle.transform.Find("Background").GetComponent<Image>()));

            // ����Toggle״̬
            toggle.onValueChanged.RemoveAllListeners();
            toggle.interactable = !food.isOnClick;
            toggle.isOn = food.isOnClick;
            text1.text = food.foodName;
            code.text = food.foodCode;

            // �����¼�
            toggle.onValueChanged.AddListener(isOn =>
            {
                if (isOn) OnFoodSelected(food, toggle, isOn);
            });
        }
    }



    GameObject GetPooledToggle()
    {
        // ���Դӳ��л�ȡ���ö���
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

        // û�п��ö���ʱ������ʵ��
        GameObject newToggle = Instantiate(foodPrefab, foodToggleParent);
        newToggle.SetActive(true);
        return newToggle;
    }
    // ʹ��Э�̼���ͼƬ
    private IEnumerator LoadImage(FoodKindItemData food, Image back)
    {
        // ���������Դ� URL ����ͼƬ
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(food.imageUrl);

        // �ȴ��������
        yield return request.SendWebRequest();

        // ����Ƿ�����ɹ�
        if (request.result == UnityWebRequest.Result.Success)
        {
            // ��ȡ���ص�����
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            // ������ת��Ϊ Sprite
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            // �� Sprite ��ֵ�� Image ���
            back.sprite = sprite;
        }
        else
        {
            back.sprite = Resources.Load<Sprite>("����ͼƬ");
            Debug.LogError("Failed to load image: " + request.error + "  " + food.foodCode + " " + food.foodName);
        }
    }

    // ʳ�ﱻѡ��ʱ
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

    // ��ʾ/���ر༭���
    void ShowEditPanel(bool show, FoodKindItemData food)
    {
        alterUI.SetActive(show);

    }

    // ȷ���޸�
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
    /// ʳ�׵�ˢ�¼���
    /// </summary>
    /// <param name="foods"></param>
    /// <param name="filter"></param>

    void RefreshFoodRecipeList(List<RecipeItem> recipeItems, string filter = "")
    {
        // �����������е�Toggle�������
        //while (foodToggleParent.childCount > 0)
        //{
        //    Transform child = foodToggleParent.GetChild(0);
        //    child.gameObject.SetActive(false);
        //    child.SetParent(null); // �����ڸ�����仯ʱӰ�첼��
        //    toggleFoodRecipePool.Enqueue(child.gameObject);
        //}
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
            if (food.isOnClick) return;

            currentSelectedFoodRecipe = food;
            //tgl = tglItem;
            ShowRecipeEditPanel(true, food);
            //quantityInput.text = "";
            ChooseRecipeUI.Instance.UpFoodItem(food, foodSort);

        }
    }

    // ��ʾ/���ر༭���
    void ShowRecipeEditPanel(bool show, RecipeItem food)
    {
        chooseRecipeUI.SetActive(show);

    }

    /// <summary>
    /// ʳ��ȷ��ѡ���id�����õ�ʳ��
    /// </summary>
    public void OnRecipeConfirm()
    {
        ServerCon.Instance.LoadRecipe("/cookbook/getRecipeFood", $"?recipeId={currentSelectedFoodRecipe.id}");

    }
    /// <summary>
    /// ���յ��ӷ�������������ʳ������
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
        // ����һ��ʹ�� Unity ���� JsonUtility����Ҫ��װ�ࣩ
        Wrapper<UserInfo> wrapper = new Wrapper<UserInfo> { items = data };
        //return JsonUtility.ToJson(wrapper);

        // ��������ʹ�� Newtonsoft.Json��ֱ�����л��б�
        return JsonConvert.SerializeObject(data);
    }
    #region //ˢ��ʳ���б�

    //// ˢ��ѡ���ʳ���б�
    //void RefreshAddFoodList()
    //{
    //    // �������Toggle
    //    foreach (Transform child in addFoodToggleParent)
    //    {
    //        Destroy(child.gameObject);
    //    }
    //    // �����µ�Toggle
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
    //    // �����µ�Toggle
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

        // �����µ�Toggle
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
        // �����µ�Toggle
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
            // �����¼�
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

    //ͨ���Ѿ������ഢ��õ��ֵ�����������ֲ�ѯ ����list
    public List<FoodKindItemData> GetItemById(string id)
    {
        if (foodChooseData.TryGetValue(id, out List<FoodKindItemData> data))
        {
            return data;
        }
        Debug.Log($"δ�ҵ� ID Ϊ {id} ������");
        return null;
    }


    // ɾ��ʳ��
    public void AlterConfirmFood(string obj)
    {
        currentSelectedFood.isOnClick = false;
        //tgl.interactable = true;

        ShowEditPanel(false, null);
        RefreshFoodList(data);
    }

    // ��������
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
