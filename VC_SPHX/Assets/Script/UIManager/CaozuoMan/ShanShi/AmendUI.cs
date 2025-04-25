using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmendUI : MonoSingletonBase<AmendUI>
{
    public Image icon;
    public Text itemName;
    public Text code;
    public Text amount;
    public InputField inputField;
    public Button ackBtn;

    private FoodKindItemData foodKind;

    public void SetIconInf(FoodKindItemData foodKindItemData)
    {
        icon.sprite = Resources.Load<Sprite>(/*data[i].id */"Icons" + "/" + foodKindItemData.foodCode);
        itemName.text = foodKindItemData.foodName;
        code.text = foodKindItemData.foodCode;
        amount.text = foodKindItemData.count.ToString();
        foodKind = foodKindItemData;
    }
    // Start is called before the first frame update
    void Start()
    {
        ackBtn.onClick.AddListener(AmendClick);
    }
    private void OnEnable()
    {
        inputField.text = "";
    }

    private void AmendClick()
    {
        string count = inputField.text;
        foodKind.count = int.Parse(count);
        ChooseFoodAllInformCon.Instance.EditBackFood(foodKind);
        gameObject.SetActive(false);
    }

    //public FoodKindItemData BackFooditem()
    //{
    //    return foodKind;
    //}

    // Update is called once per frame
    void Update()
    {

    }
}
