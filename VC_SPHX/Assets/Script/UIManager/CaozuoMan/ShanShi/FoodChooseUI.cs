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
/// ѡ��ʳ�����
/// </summary>
public class FoodChooseUI : UICaoZuoBase
{
    // UIԪ��
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



    // ���ݴ洢
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
        RefreshAddFoodList();
        InitDropDown();

    }


    // ˢ��ʳ���б�
    void RefreshFoodList(List<FoodKindItemData> foods, string filter = "")
    {
        // �������Toggle
        foreach (Transform child in foodToggleParent)
        {
            Destroy(child.gameObject);
        }
        // �����µ�Toggle
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
            // ��ʾʳ������
            //toggleObj.GetComponentInChildren<Text>().text = food.name;

            // ��ӵ���¼�
            toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn) OnFoodSelected(food, toggle, isOn);
            });
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


    // ˢ��ѡ���ʳ���б�
    void RefreshAddFoodList()
    {
        // �������Toggle
        foreach (Transform child in addFoodToggleParent)
        {
            Destroy(child.gameObject);
        }
        // �����µ�Toggle
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
            // ��ʾʳ������
            //toggleObj.GetComponentInChildren<Text>().text = food.name;

            //// ��ӵ���¼�
            //toggle.onValueChanged.AddListener((isOn) =>
            //{
            //    if (isOn) OnFoodSelected(food, toggle, isOn);
            //});
        }
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
}
