using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AlterRecipeUI : MonoSingletonBase<AlterRecipeUI>
{
    public Image icon;
    public Text foodName;
    public Text code;
    public InputField inputField;
    public Button confirm;
    public Button delet;
    public GameObject tipText;
    public Text heat;//����
    public Text protein;//������
    public Text fat;//֬��
    public Text carbohydrate;//̼ˮ������

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
        FoodChooseUI.Instance.AlterReciopeCount(int.Parse(inputField.text), code.text);
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
