using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UserInfo;


public class GroupRegisterUI : UICaoZuoBase
{
    public static GroupRegisterUI Instance { get; private set; }

    public Dropdown groupDrop;      //ѡ����Ⱥdropdown
    public InputField peopleInput;  //��������inputfield
    public Dropdown sexDrop;        //�Ա�dropdown
    public Dropdown levelDrop;      //ǿ��dropdown
    public InputField kcalInput;    //��������inputfield
    public InputField partText;           //��ʳ������ʾtext 
    public Button verifyBtn;        //�Ǽ�btn
    public Text kcalText;           //�Ƽ���������ʾtext

    public Dropdown biliDrop;       //��ͱ���

    public UserInfo userInfo = new UserInfo();


    void Awake()
    {
        InitializeManager();
    }

    // ��ʽ��ʼ�����������༭�����ã�
    public void InitializeManager()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void OnEnable()
    {
        peopleInput.text = "";
        kcalInput.text = "";
        groupDrop.value = 0;
        sexDrop.value = 0;
        levelDrop.value = 0;
        ServerCon.Instance.LoadRecipe("/group/analyse/getAllPhysique", "", "");
    }

    public void ServerGetAllPhysique(List<ReceiveAllPhysique> allPhysique)
    {
        groupDrop.ClearOptions();
        List<string> optionStrings = new List<string>();
        foreach (var option in allPhysique)
        {
            optionStrings.Add(option.physique);
        }
        groupDrop.AddOptions(optionStrings);
        SendHeatIntakeValue(0);

    }

    // Start is called before the first frame update
    void Start()
    {
        verifyBtn.onClick.AddListener(ClickVerifyClose);
        groupDrop.onValueChanged.AddListener(OnDropdownValueChanged);

        //kcalInput.onValueChanged.AddListener(
        //       (inputText) => OnChangedInput(inputText)
        //   );
        kcalInput.onValueChanged.AddListener(OnChangedInput);
    }

    public void OnDropdownValueChanged(int value)
    {
        if (groupDrop.value == 7 || groupDrop.value == 8 || groupDrop.value == 9 || groupDrop.value == 10)
        {
            sexDrop.value = 1;

        }
        SendHeatIntakeValue(value);
    }

    private void SendHeatIntakeValue(int value)
    {
        SendHeatIntake heatIntake = new SendHeatIntake();
        heatIntake.physique = groupDrop.options[value].text;
        heatIntake.sex = sexDrop.options[sexDrop.value].text;
        heatIntake.level = levelDrop.options[levelDrop.value].text;
        ServerCon.Instance.ConverToJsonPost(SerializeData(heatIntake), "/group/analyse/heatIntake");
    }
    private string SerializeData(SendHeatIntake data)
    {
        // ����һ��ʹ�� Unity ���� JsonUtility����Ҫ��װ�ࣩ
        Wrapper<SendHeatIntake> wrapper = new Wrapper<SendHeatIntake> { items = data };
        //return JsonUtility.ToJson(wrapper);

        // ��������ʹ�� Newtonsoft.Json��ֱ�����л��б�
        return JsonConvert.SerializeObject(data);
    }

    public void ReceiveRecIntake(HeatIntakeItem heatIntake)
    {
        kcalText.text = heatIntake.recIntake;
    }

    //private void OnChangedInput(string inputText)
    //{
    //    if (!string.IsNullOrEmpty(inputText) && IsInputText(inputText) && (RecEnergyIntakeSplit(kcalText.text, 0) <= float.Parse(inputText)) && (RecEnergyIntakeSplit(kcalText.text, 1) >= float.Parse(inputText)))
    //    {
    //        partText.text = (int.Parse(inputText) / 90).ToString();
    //    }
    //    else
    //    {
    //        inputText = "";
    //    }
    //}

    private void ClickVerifyClose()
    {
        if (kcalInput.text != "" && !IsInputText(kcalInput.text))
        {
            return;
        }
        if (peopleInput.text != "" && !IsInputText(peopleInput.text))
        {
            return;
        }
        userInfo.sex = sexDrop.options[sexDrop.value].text;
        userInfo.level = levelDrop.options[levelDrop.value].text;
        userInfo.physique = groupDrop.options[groupDrop.value].text;
        userInfo.proportion = biliDrop.options[biliDrop.value].text;
        userInfo.recIntake = kcalText.text;
        UIManager.Instance.OpenUICaoZuo("ChooseGroupShanShiUI");
        FoodGroupChooseUI.Instance.userInfo = userInfo;

        UIManager.Instance.CloseUICaoZuo("GroupRegisterUI");


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

    // Update is called once per frame
    void Update()
    {
        if (groupDrop.value == 7 || groupDrop.value == 8 || groupDrop.value == 9 || groupDrop.value == 10)
        {
            sexDrop.value = 1;

        }
    }


    //public bool IsInputText(string text)
    //{
    //    if (text == "")
    //    {
    //        return false;
    //    }
    //    if (float.Parse(text) > 0)
    //    {
    //        return true;
    //    }
    //    return false;
    //}
    //public float RecEnergyIntakeSplit(string recEnergyIntake, int index)
    //{
    //    char separator = '~';
    //    string[] result = recEnergyIntake.Split(separator);
    //    if (result.Length == 1)
    //    {
    //        return float.Parse(result[0]);
    //    }
    //    return Mathf.Round(float.Parse(result[index]) * 100) / 100;
    //}
    private void OnChangedInput(string inputText)
    {
        // �Ƚ��л�����֤
        if (!IsValidInput(inputText, out float inputValue))
        {
            ClearInvalidInput();
            return;
        }

        // ��ȡ��Ч��Χ
        if (!TryGetValidRange(out float minValue, out float maxValue))
        {
            ClearInvalidInput();
            return;
        }

        // ��Χ�ж�
        if (/*inputValue >= minValue &&*/ inputValue <= maxValue)
        {
            UpdatePartText(inputValue);
        }
        else
        {
            ClearInvalidInput();
        }
    }

    // ����������֤
    private bool IsValidInput(string text, out float value)
    {
        value = 0f;

        if (string.IsNullOrWhiteSpace(text)) return false;
        if (!float.TryParse(text, out float parsedValue)) return false;
        if (parsedValue <= 0) return false;

        value = parsedValue;
        return true;
    }

    // ��ȡ��Ч��Χ
    private bool TryGetValidRange(out float min, out float max)
    {
        min = max = 0f;

        if (string.IsNullOrWhiteSpace(kcalText.text)) return false;

        string[] rangeParts = kcalText.text.Split('~');
        if (rangeParts.Length == 0 || rangeParts.Length > 2) return false;

        // ����ֵ��Χ
        if (rangeParts.Length == 1)
        {
            if (!float.TryParse(rangeParts[0], out float singleValue)) return false;
            min = max = singleValue;
            return true;
        }

        // ����Χֵ
        if (!float.TryParse(rangeParts[0], out float lower) ||
            !float.TryParse(rangeParts[1], out float upper))
        {
            return false;
        }

        min = Mathf.Round(lower * 100) / 100;
        max = Mathf.Round(upper * 100) / 100;
        return true;
    }

    // ������ʾ����
    private void UpdatePartText(float value)
    {
        int result = Mathf.RoundToInt(value / 90f);
        partText.text = result.ToString();
    }

    // �����Ч����
    private void ClearInvalidInput()
    {
        // ����ѭ�������¼�
        kcalInput.onValueChanged.RemoveListener(OnChangedInput);
        kcalInput.text = "";
        kcalInput.onValueChanged.AddListener(OnChangedInput);
    }
}
