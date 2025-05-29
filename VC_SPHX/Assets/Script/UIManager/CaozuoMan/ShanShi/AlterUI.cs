using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
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
        //icon.sprite = Resources.Load<Sprite>("Icons" + "/" + data.foodCode);
        foodName.text = data.foodName;
        code.text = data.foodCode;
        //unit.text = data.water;
        heat.text = data.heat;
        protein.text = data.protein;
        fat.text = data.fat;
        carbohydrate.text = data.cho;
        StartCoroutine(LoadImage(data, icon));

    }
    private IEnumerator LoadImage(FoodKindItemData food, Image back)
    {
        // 发送请求以从 URL 加载图片
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(food.imageUrl);

        // 等待请求完成
        yield return request.SendWebRequest();

        // 检查是否请求成功
        if (request.result == UnityWebRequest.Result.Success)
        {
            // 获取返回的纹理
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            // 将纹理转换为 Sprite
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            // 将 Sprite 赋值给 Image 组件
            back.sprite = sprite;
        }
        else
        {
            Debug.LogError("Failed to load image: " + request.error);
            back.sprite = Resources.Load<Sprite>("暂无图片");
            Debug.LogError("Failed to load image: " + request.error + "  " + food.foodCode + " " + food.foodName);
        }
    }

    private void DeletFoodItem()
    {
        gameObject.SetActive(false);
        MessageCenter.Instance.Send("SendAlterDeletFood", "");

    }

    private void ConfirmFoodItem()
    {
        if (inputField.text == "" && !IsInputText(inputField.text))
        {
            tipText.gameObject.SetActive(true);
            StartCoroutine(WaitCloseTip());

            return;
        }
        gameObject.SetActive(false);
        MessageCenter.Instance.Send("SendAlterConfirmFood", inputField.text);

    }
    public bool IsInputText(string text)
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
