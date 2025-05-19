using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
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
        //icon.sprite = Resources.Load<Sprite>(/*data[i].id */"Icons" + "/" + foodKindItemData.foodCode);
        itemName.text = foodKindItemData.foodName;
        code.text = foodKindItemData.foodCode;
        amount.text = foodKindItemData.count.ToString();
        foodKind = foodKindItemData;
        StartCoroutine(LoadImage(foodKindItemData, icon));

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
