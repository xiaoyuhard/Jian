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
        // ���������Դ� URL ����ͼƬ
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(food.imageUrl);

        // �ȴ��������
        yield return request.SendWebRequest();

        // ����Ƿ�����ɹ�
        if (request.result == UnityWebRequest.Result.Success)
        {
            // ��ȡ���ص�����
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            // ������ת��Ϊ Sprite
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            // �� Sprite ��ֵ�� Image ���
            back.sprite = sprite;
        }
        else
        {
            Debug.LogError("Failed to load image: " + request.error);
            back.sprite = Resources.Load<Sprite>("����ͼƬ");
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
