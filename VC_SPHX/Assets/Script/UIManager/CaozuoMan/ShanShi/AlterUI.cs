using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 膳食选择食物输入数量界面
/// </summary>
public class AlterUI : MonoSingletonBase<AlterUI>
{
    public Image icon;
    public Text foodName;
    public Text code;
    public InputField inputField;
    public Text unit;
    public Button confirm;
    public Button delet;
    public GameObject tipText;
    public Text heat;//热量
    public Text protein;//蛋白质
    public Text fat;//脂肪
    public Text carbohydrate;//碳水化合物

    // Start is called before the first frame update
    void Start()
    {
        confirm.onClick.AddListener(ConfirmFoodItem);
        delet.onClick.AddListener(DeletFoodItem);
    }

    private void OnEnable()
    {
        inputField.text = "";
        tipText.gameObject.SetActive(false);

    }

    public void UpFoodItem(FoodKindItemData data, string foodSort)
    {
        icon.sprite = Resources.Load<Sprite>("Icons" + "/" + data.foodCode);
        foodName.text = data.foodName;
        code.text = data.foodCode;
        //unit.text = data.water;
        heat.text = data.heat;
        protein.text = data.protein;
        fat.text = data.fat;
        carbohydrate.text = data.cho;
    }

    private void DeletFoodItem()
    {
        gameObject.SetActive(false);
        MessageCenter.Instance.Send("SendAlterDeletFood", "");

    }

    private void ConfirmFoodItem()
    {
        if (inputField.text == "")
        {
            tipText.gameObject.SetActive(true);
            StartCoroutine(WaitCloseTip());

            return;
        }
        gameObject.SetActive(false);
        MessageCenter.Instance.Send("SendAlterConfirmFood", inputField.text);

    }

    IEnumerator WaitCloseTip()
    {
        yield return new WaitForSeconds(2f);
        tipText.gameObject.SetActive(false);

    }



    // Update is called once per frame
    void Update()
    {

    }
}
