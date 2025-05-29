using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// 群体膳食食谱修改界面  出报告中
/// </summary>
public class ModifyRecipeGroupReportUI : MonoSingletonBase<ModifyRecipeGroupReportUI>
{

    public Text nameText;
    public Button confirm;

    public Text weight;//重量
    public Text protein;//蛋白质
    public Text fat;//脂肪
    public Text cho;//碳水化合物

    public Transform content;
    public GameObject foodItemPrefab;

    public GameObject tipText;

    RecipeItem recipeGroupItems;

    // Start is called before the first frame update
    void Start()
    {
        confirm.onClick.AddListener(ConfirmFoodItem);
    }

    private void OnEnable()
    {
        weight.text = "";
        protein.text = "";
        fat.text = "";
        cho.text = "";

    }


    /// <summary>
    /// 更新显示食谱中的食物
    /// </summary>
    public void UpFoodItem(RecipeItem recipeItem)
    {
        nameText.text = recipeItem.recipeName;
        // 清空现有Toggle
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        recipeGroupItems = recipeItem;
        foreach (var item in recipeGroupItems.foodRecipeGroupItems)
        {
            GameObject foodItem = Instantiate(foodItemPrefab, content);
            foodItem.SetActive(true);
            item.part = "1";
            foodItem.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = item.foodName;
            //foodItem.transform.Find("Count").GetComponent<Text>().text = item.part.ToString();
            foodItem.transform.Find("Heat").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(item.part), float.Parse(item.weight));

            foodItem.transform.Find("Weight").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(item.part), float.Parse(item.weight));
            foodItem.transform.Find("Protein").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(item.part), float.Parse(item.protein));
            foodItem.transform.Find("Fat").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(item.part), float.Parse(item.fat));
            foodItem.transform.Find("carbohydrate").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(item.part), float.Parse(item.cho));
            foodItem.transform.Find("CountInput").GetComponent<InputField>().onValueChanged.AddListener(
                (inputText) => HandleInputWithData(inputText, foodItem, item)
            );
        }
    }

    public void SetText()
    {
        float weightNum = 0;
        float proteinNum = 0;
        float fatNum = 0;
        float choNum = 0;
        foreach (var item in recipeGroupItems.foodRecipeGroupItems)
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
    /// 通过输入框进行判断是否有输入 然后再进行修改
    /// </summary>
    private void HandleInputWithData(string newText, GameObject obj, FoodRecipeGroupItem item)
    {
        // 有输入内容的时候 进行刷新
        if (!string.IsNullOrEmpty(newText) && IsInputText(newText))
        {
            item.part = newText;

            obj.transform.Find("Count").GetComponent<Text>().text = newText;
            obj.transform.Find("Weight").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(newText), float.Parse(item.weight));
            obj.transform.Find("Heat").GetComponent<Text>().text = BackMultiplyuantity(float.Parse(item.part), float.Parse(item.weight));

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
        foreach (var item in recipeGroupItems.foodRecipeGroupItems)
        {
            if (!item.isInput)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 确认按钮 把整个食谱传回去
    /// </summary>
    private void ConfirmFoodItem()
    {

        if (IsInputList())
        {
            MealGroupReportUI.Instance.ReciveModifyRecipeGroup(recipeGroupItems);
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
