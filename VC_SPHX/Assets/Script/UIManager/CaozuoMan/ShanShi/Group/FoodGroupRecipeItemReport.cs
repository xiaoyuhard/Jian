using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ѡ��������ӵ�ʳ�� ���������
/// </summary>
public class FoodGroupRecipeItemReport : MonoSingletonBase<FoodGroupRecipeItemReport>
{
    public Button alterBtn;         //�޸İ�ť
    public Button delBtn;           //ɾ����ť
    public Button swopBtn;          //������ť

    public RecipeItem recipeItem;

    // Start is called before the first frame update
    void Start()
    {
        alterBtn.onClick.AddListener(ClickAlterFoodCount);
        delBtn.onClick.AddListener(ClickDelRecipe);
        swopBtn.onClick.AddListener(ClickSwopRecipe);
    }
    /// <summary>
    /// �������ʳ�׽���
    /// </summary>
    private void ClickSwopRecipe()
    {
        MealGroupReportUI.Instance.swopRecipeGroupUI.SetActive(true);
        SwopRecipeGroupReportUI.Instance.UpOldFoodItem(recipeItem);
    }
    /// <summary>
    /// ɾ����ʳ��
    /// </summary>
    private void ClickDelRecipe()
    {
        MealGroupReportUI.Instance.DelRecipeGroup(recipeItem);

    }
    /// <summary>
    /// �޸�ʳ����ÿ��ʳ�������
    /// </summary>
    private void ClickAlterFoodCount()
    {
        MealGroupReportUI.Instance.modifyRecipeGroupUI.SetActive(true);
        ModifyRecipeGroupReportUI.Instance.UpFoodItem(recipeItem);

    }

    // Update is called once per frame
    void Update()
    {

    }


}
