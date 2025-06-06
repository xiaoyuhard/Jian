using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ⱥ����ʳʳ�׽�������
/// </summary>
public class SwopRecipeGroupUI : MonoSingletonBase<SwopRecipeGroupUI>
{
    public Text nameTextOld;

    public Text weightOld;//����
    public Text proteinOld;//������
    public Text fatOld;//֬��
    public Text choOld;//̼ˮ������

    public Dropdown dropFoodRecipe;

    public Transform contentOld;


    public Text nameText;
    public Button confirm;

    public Text weight;//����
    public Text protein;//������
    public Text fat;//֬��
    public Text cho;//̼ˮ������

    public Transform content;
    public GameObject foodItemPrefab;

    public TMP_InputField inputField;       //������в�ѯ
    public Button btn;                  //�����ѯȷ��

    public GameObject tipText;

    List<FoodRecipeGroupItem> recipeGroupItems = new List<FoodRecipeGroupItem>();
    List<RecipeItem> foodRecipeMesses = new List<RecipeItem>();
    RecipeItem oldRecipe;
    RecipeItem newRecipe;

    float countRecipeOld = 0;      //ʳ�׾��ܵķ���
    float countRecipeNew = 0;      //ʳ�����ܵķ���
    RecipeItem recipeItemCache = new RecipeItem();

    // Start is called before the first frame update
    void Start()
    {
        confirm.onClick.AddListener(ConfirmFoodItem);
        btn.onClick.AddListener(ClickFindRecipe);
        dropFoodRecipe.onValueChanged.AddListener(OnDropdownValueChanged);

    }
    public void OnDropdownValueChanged(int value)
    {
        if (foodRecipeMesses != null)
        {
            SendRecipeId(foodRecipeMesses[value]);
        }
    }

    private void SendRecipeId(RecipeItem recipeItem)
    {
        ServerCon.Instance.LoadRecipe("/cookbook/getGroupRecipeFood", $"?recipeId={recipeItem.id}", "Swop");
        RecipeItem recipe = new RecipeItem();

        recipeItemCache = recipeItem;
        //recipeItemCache.id = recipeItem.id;
        //recipeItemCache.includingFood = recipeItem.includingFood;
        //recipeItemCache.itemObj = recipeItem.itemObj;
        //recipeItemCache.recipeName = recipeItem.recipeName;

    }

    /// <summary>
    /// ���յ��ӷ�������������ʳ������
    /// </summary>
    /// <param name="food"></param>
    public void ReceiveRecipeItem(List<FoodRecipeGroupItem> food)
    {
        recipeItemCache.foodRecipeGroupItems = food;

        UpNewFoodItem(recipeItemCache);
    }

    private void ClickFindRecipe()
    {
        List<RecipeItem> recipeItem = FoodGroupChooseUI.Instance.BackRecipeGroupItem(inputField.text);
        foodRecipeMesses.Clear();
        if (recipeItem != null)
        {
            foodRecipeMesses = new List<RecipeItem>(recipeItem);
        }
        else
        {
            foodRecipeMesses.Clear();
        }
        PopulateDropdown(dropFoodRecipe, foodRecipeMesses);
        OnDropdownValueChanged(0);
        //recipeItemCache = null;
    }

    // ͨ�÷��������Dropdownѡ��
    private void PopulateDropdown(Dropdown dropdown, List<RecipeItem> options)
    {
        dropdown.ClearOptions();
        List<string> optionStrings = new List<string>();
        foreach (var option in options)
        {
            optionStrings.Add(option.recipeName.ToString());
        }
        dropdown.AddOptions(optionStrings);
    }

    private void OnEnable()
    {

        ResetNewFood();
    }

    private void ResetNewFood()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);

        }
        weight.text = "";
        protein.text = "";
        fat.text = "";
        cho.text = "";
        inputField.text = "";
        dropFoodRecipe.ClearOptions();
        nameText.text = "";
        recipeGroupItems.Clear();
    }
    public void UpSwopRecipe(RecipeItem oldRecipe, RecipeItem newRecipe)
    {


    }


    /// <summary>
    /// ������ʾʳ�����µ�ʳ��
    /// </summary>
    private void UpNewFoodItem(RecipeItem recipeItem)
    {
        newRecipe = recipeItem;
        countRecipeNew = 0;
        nameText.text = recipeItem.recipeName;
        // �������Toggle
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        recipeGroupItems = recipeItem.foodRecipeGroupItems;
        foreach (var item in recipeItem.foodRecipeGroupItems)
        {
            GameObject foodItem = Instantiate(foodItemPrefab, content);
            foodItem.SetActive(true);
            item.part = "1";
            foodItem.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = item.foodName;
            //foodItem.transform.Find("Count").GetComponent<Text>().text = item.part.ToString();
            foodItem.transform.Find("Heat").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(item.part), float.Parse(item.heat));

            foodItem.transform.Find("Weight").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(item.part), float.Parse(item.weight));
            foodItem.transform.Find("Protein").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(item.part), float.Parse(item.protein));
            foodItem.transform.Find("Fat").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(item.part), float.Parse(item.fat));
            foodItem.transform.Find("carbohydrate").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(item.part), float.Parse(item.cho));
            foodItem.transform.Find("CountInput").GetComponent<InputField>().onValueChanged.AddListener(
                (inputText) => HandleInputWithData(inputText, foodItem, item)
            );
        }
    }

    /// <summary>
    /// ������ʾʳ���оɵ�ʳ��
    /// </summary>
    public void UpOldFoodItem(RecipeItem recipeItem)
    {
        oldRecipe = recipeItem;
        nameTextOld.text = recipeItem.recipeName;

        float weightNum = 0;
        float proteinNum = 0;
        float fatNum = 0;
        float choNum = 0;
        // �������Toggle
        foreach (Transform child in contentOld)
        {
            Destroy(child.gameObject);
        }
        foreach (var item in recipeItem.foodRecipeGroupItems)
        {
            GameObject foodItem = Instantiate(foodItemPrefab, contentOld);
            foodItem.SetActive(true);
            //item.part = "1";
            foodItem.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = item.foodName;
            foodItem.transform.Find("Count").GetComponent<Text>().text = item.part.ToString();
            foodItem.transform.Find("Heat").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(item.part), float.Parse(item.heat));

            foodItem.transform.Find("Weight").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(item.part), float.Parse(item.weight));
            foodItem.transform.Find("Protein").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(item.part), float.Parse(item.protein));
            foodItem.transform.Find("Fat").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(item.part), float.Parse(item.fat));
            foodItem.transform.Find("carbohydrate").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(item.part), float.Parse(item.cho));
            foodItem.transform.Find("CountInput").GetComponent<InputField>().text = item.part;
            foodItem.transform.Find("CountInput").GetComponent<InputField>().interactable = false;
            countRecipeOld += float.Parse(item.part);

            weightNum += float.Parse(BackMultiplyuantity(float.Parse(item.part), float.Parse(item.weight)));
            proteinNum += float.Parse(BackMultiplyuantity(float.Parse(item.part), float.Parse(item.protein)));
            fatNum += float.Parse(BackMultiplyuantity(float.Parse(item.part), float.Parse(item.fat)));
            choNum += float.Parse(BackMultiplyuantity(float.Parse(item.part), float.Parse(item.cho)));
        }
        weightOld.text = weightNum.ToString();
        proteinOld.text = proteinNum.ToString();
        fatOld.text = fatNum.ToString();
        choOld.text = choNum.ToString();
    }


    public void SetText()
    {
        float weightNum = 0;
        float proteinNum = 0;
        float fatNum = 0;
        float choNum = 0;
        foreach (var item in recipeGroupItems)
        {
            weightNum += float.Parse(BackMultiplyuantity(float.Parse(item.part), float.Parse(item.weight)));
            proteinNum += float.Parse(BackMultiplyuantity(float.Parse(item.part), float.Parse(item.protein)));
            fatNum += float.Parse(BackMultiplyuantity(float.Parse(item.part), float.Parse(item.fat)));
            choNum += float.Parse(BackMultiplyuantity(float.Parse(item.part), float.Parse(item.cho)));
        }
        weight.text = weightNum.ToString();
        protein.text = proteinNum.ToString();
        fat.text = fatNum.ToString();
        cho.text = choNum.ToString();
    }

    /// <summary>
    /// ͨ�����������ж��Ƿ������� Ȼ���ٽ����޸�
    /// </summary>
    private void HandleInputWithData(string newText, GameObject obj, FoodRecipeGroupItem item)
    {
        // ���������ݵ�ʱ�� ����ˢ��
        if (!string.IsNullOrEmpty(newText) && IsInputText(newText))
        {
            item.part = newText;
            obj.transform.Find("Count").GetComponent<Text>().text = newText;
            obj.transform.Find("Weight").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(newText), float.Parse(item.weight));
            obj.transform.Find("Heat").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(item.part), float.Parse(item.heat));

            obj.transform.Find("Protein").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(newText), float.Parse(item.protein));
            obj.transform.Find("Fat").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(newText), float.Parse(item.fat));
            obj.transform.Find("carbohydrate").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(newText), float.Parse(item.cho));
            item.isInput = true;
        }
        else
        {
            item.isInput = false;
        }
    }
    private bool IsInputText(string text)
    {
        if (text == "")
        {
            return false;
        }
        if (float.Parse(text) > 0)
        {
            return true;
        }
        return false;
    }

    private bool IsInputList()
    {
        countRecipeOld = 0;

        foreach (var item in recipeGroupItems)
        {
            if (!item.isInput)
            {
                countRecipeOld += float.Parse(item.part);
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// ȷ�ϰ�ť ������ʳ�״���ȥ
    /// </summary>
    private void ConfirmFoodItem()
    {

        if (IsInputList() && (countRecipeOld == countRecipeNew))
        {
            newRecipe.mealPeriod = oldRecipe.mealPeriod;
            FoodGroupChooseUI.Instance.SwopRecipeGroup(oldRecipe, newRecipe);
            return;
        }
        tipText.gameObject.SetActive(true);
        StartCoroutine(WaitCloseTip());


    }

    IEnumerator WaitCloseTip()
    {
        yield return new WaitForSeconds(2f);
        tipText.gameObject.SetActive(false);

    }


    public string BackMultiplyuantity(float quantity, float parameters)
    {
        return ((quantity * parameters) /*/ 100*/).ToString("F2");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        SetText();
    }
}
