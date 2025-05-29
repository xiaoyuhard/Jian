using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 选择完已添加的食谱 选择界面
/// </summary>
public class FoodGroupRecipeItem : MonoSingletonBase<FoodGroupRecipeItem>
{
    public Button alterBtn;         //修改按钮
    public Button delBtn;           //删除按钮
    public Button swopBtn;          //交换按钮

    public RecipeItem recipeItem = new RecipeItem();

    // Start is called before the first frame update
    void Start()
    {
        alterBtn.onClick.AddListener(ClickAlterFoodCount);
        delBtn.onClick.AddListener(ClickDelRecipe);
        swopBtn.onClick.AddListener(ClickSwopRecipe);
    }
    /// <summary>
    /// 点击进行食谱交换
    /// </summary>
    private void ClickSwopRecipe()
    {
        FoodGroupChooseUI.Instance.swopRecipeGroupUI.SetActive(true);
        SwopRecipeGroupUI.Instance.UpOldFoodItem(recipeItem);
    }
    /// <summary>
    /// 删除本食谱
    /// </summary>
    private void ClickDelRecipe()
    {
        FoodGroupChooseUI.Instance.DelRecipeGroup(recipeItem);

    }
    /// <summary>
    /// 修改食谱里每到食物的数量
    /// </summary>
    private void ClickAlterFoodCount()
    {
        FoodGroupChooseUI.Instance.modifyRecipeGroupUI.SetActive(true);
        ModifyRecipeGroupUI.Instance.UpFoodItem(recipeItem);

    }

    // Update is called once per frame
    void Update()
    {

    }


}
