using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ѡ��������ӵ�ʳ�� ѡ�����
/// </summary>
public class FoodGroupRecipeItem : MonoSingletonBase<FoodGroupRecipeItem>
{
    public Button alterBtn;         //�޸İ�ť
    public Button delBtn;           //ɾ����ť
    public Button swopBtn;          //������ť

    public RecipeItem recipeItem = new RecipeItem();

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
        FoodGroupChooseUI.Instance.swopRecipeGroupUI.SetActive(true);
        SwopRecipeGroupUI.Instance.UpOldFoodItem(recipeItem);
    }
    /// <summary>
    /// ɾ����ʳ��
    /// </summary>
    private void ClickDelRecipe()
    {
        FoodGroupChooseUI.Instance.DelRecipeGroup(recipeItem);

    }
    /// <summary>
    /// �޸�ʳ����ÿ��ʳ�������
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
