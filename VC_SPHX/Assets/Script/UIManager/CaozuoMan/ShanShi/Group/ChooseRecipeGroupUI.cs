using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// 膳食选择食谱输入数量界面
/// </summary>
public class ChooseRecipeGroupUI : MonoSingletonBase<ChooseRecipeGroupUI>
{
    public Text foodName;
    public Button confirm;

    public Text recipeItemNameText;

    public GameObject content;
    public GameObject itemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        confirm.onClick.AddListener(ConfirmFoodItem);
    }

    private void OnEnable()
    {

    }

    public void UpFoodItem(RecipeItem data, string foodSort)
    {
        foodName.text = data.recipeName;
        recipeItemNameText.text = data.includingFood;
    }


    private void ConfirmFoodItem()
    {
        //gameObject.SetActive(false);
        FoodChooseUI.Instance.OnRecipeConfirm();

    }


}
