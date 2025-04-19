using RTS;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
//using static UnityEngine.Rendering.DebugUI;

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




    // ���ݴ洢
    //private Dictionary<string, List<FoodKindItemData>> foodDictionary = new Dictionary<string, List<FoodKindItemData>>();
    private FoodKindItemData currentSelectedFood;
    //private Toggle tgl;
    //private string currentCategory;
    Dictionary<string, List<FoodKindItemData>> dataDic = new Dictionary<string, List<FoodKindItemData>>();
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
    }

    private void Update()
    {


    }


    private void ChooseCloseUI()
    {
        foreach (var item in addFoodList)
        {
            if (dataDic.ContainsKey(item.id))
            {
                dataDic[item.id].Add(item);
            }
            else
            {
                dataDic.Add(item.id, new List<FoodKindItemData> { item });
            }
        }

        ChooseFoodAllInformCon.Instance.AddFoodDic(addFoodList, foodSort, dataDic.Count);
        GameObjMan.Instance.OpenFirst();

        UIManager.Instance.CloseUICaoZuo("ChooseShanShiUI");
        thisAddFoodList.Clear();
        dataDic.Clear();
    }

    private void OnEnable()
    {
        MessageCenter.Instance.Register("SendChooseItemToShanShiUI", ChooseItemToShanShiUI);

        if (ChooseFoodAllInformCon.Instance.isSame)
        {
            FoodManager.Instance.ResFoodOnClick();
            ChooseFoodAllInformCon.Instance.isSame = false;
            addFoodList.Clear();
            //Debug.Log("false shuaxin");
            StartCoroutine(WaitShow());

            //addFoodList.Clear();
            Debug.Log(55);
            return;
        }
        StartCoroutine(WaitShow());

    }
    IEnumerator WaitShow()
    {
        yield return new WaitForSeconds(0.01f);
        ShowCategory("");

        //addFoodList.Clear();
    }

    private void ChooseItemToShanShiUI(string obj)
    {
        //data.Clear();
        foodSort = obj;
        //Debug.Log(foodSort);

        dataRaw = FoodManager.Instance.GetItemById(obj);
    }



    // ��ʾָ�������ʳ��
    public void ShowCategory(string categoryName)
    {
        //currentCategory = categoryName;
        dataRaw = FoodManager.Instance.GetItemById(foodSort);
        data = dataRaw;
        //Debug.Log(foodSort);
        RefreshFoodList(data);
        RefreshAddFoodList();
    }

    // ˢ��ʳ���б�
    void RefreshFoodList(List<FoodKindItemData> foods, string filter = "")
    {
        // �������Toggle
        foreach (Transform child in foodToggleParent)
        {
            Destroy(child.gameObject);
        }
        if (filter != "")
        {
            // �����µ�Toggle
            foreach (var food in foods)
            {
                GameObject toggleObj = Instantiate(foodPrefab, foodToggleParent);
                toggleObj.SetActive(true);
                Toggle toggle = toggleObj.GetComponent<Toggle>();
                toggle.interactable = !food.isOnClick;

                toggle.group = toggleGroup;
                toggle.isOn = food.isOnClick;
                toggle.transform.Find("Text1").GetComponent<Text>().text = food.iconName;
                toggle.transform.Find("Code").GetComponent<Text>().text = food.code;
                toggle.transform.Find("Background").GetComponent<Image>().sprite = Resources.Load<Sprite>(/*data[i].id */"Icons" + "/" + food.code);
                // ��ʾʳ������
                //toggleObj.GetComponentInChildren<Text>().text = food.name;

                // ��ӵ���¼�
                toggle.onValueChanged.AddListener((isOn) =>
                {
                    if (isOn) OnFoodSelected(food, toggle, isOn);
                });
            }
            return;
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
            toggle.transform.Find("Text1").GetComponent<Text>().text = food.iconName;
            toggle.transform.Find("Code").GetComponent<Text>().text = food.code;
            toggle.transform.Find("Background").GetComponent<Image>().sprite = Resources.Load<Sprite>(/*data[i].id */"Icons" + "/" + food.code);
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
        currentSelectedFood.count = float.Parse(obj);
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
            //toggle.group = toggleGroup;
            toggle.interactable = !food.isOnClick;
            toggle.isOn = food.isOnClick;
            toggleObj.transform.Find("id").GetComponent<Text>().text = food.iconName;
            toggleObj.transform.Find("Count").GetComponent<Text>().text = food.count.ToString();
            toggleObj.transform.Find("Unit").GetComponent<Text>().text = food.unit;
            toggleObj.transform.Find("Heat").GetComponent<Text>().text = food.heat;
            toggleObj.transform.Find("Protein").GetComponent<Text>().text = food.protein;
            toggleObj.transform.Find("Fat").GetComponent<Text>().text = food.fat;
            toggleObj.transform.Find("carbohydrate").GetComponent<Text>().text = food.carbohydrate;
            // ��ʾʳ������
            //toggleObj.GetComponentInChildren<Text>().text = food.name;

            //// ��ӵ���¼�
            //toggle.onValueChanged.AddListener((isOn) =>
            //{
            //    if (isOn) OnFoodSelected(food, toggle, isOn);
            //});
        }
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
            var filtered = data.Where(f => f.code.Contains(keyword)).ToList();
            RefreshFoodList(filtered, keyword);
        }
    }
}
