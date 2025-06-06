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

    public Dropdown groupDrop;      //选择人群dropdown
    public InputField peopleInput;  //人数输入inputfield
    public Dropdown sexDrop;        //性别dropdown
    public Dropdown levelDrop;      //强度dropdown
    public InputField kcalInput;    //摄入热量inputfield
    public InputField partText;           //饮食份数显示text 
    public Button verifyBtn;        //登记btn
    public Text kcalText;           //推荐摄入量显示text

    public Dropdown biliDrop;       //配餐比例

    public UserInfo userInfo = new UserInfo();


    void Awake()
    {
        InitializeManager();
    }

    // 显式初始化方法（供编辑器调用）
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
        // 方法一：使用 Unity 内置 JsonUtility（需要包装类）
        Wrapper<SendHeatIntake> wrapper = new Wrapper<SendHeatIntake> { items = data };
        //return JsonUtility.ToJson(wrapper);

        // 方法二：使用 Newtonsoft.Json（直接序列化列表）
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
        // 先进行基础验证
        if (!IsValidInput(inputText, out float inputValue))
        {
            ClearInvalidInput();
            return;
        }

        // 获取有效范围
        if (!TryGetValidRange(out float minValue, out float maxValue))
        {
            ClearInvalidInput();
            return;
        }

        // 范围判断
        if (/*inputValue >= minValue &&*/ inputValue <= maxValue)
        {
            UpdatePartText(inputValue);
        }
        else
        {
            ClearInvalidInput();
        }
    }

    // 基础输入验证
    private bool IsValidInput(string text, out float value)
    {
        value = 0f;

        if (string.IsNullOrWhiteSpace(text)) return false;
        if (!float.TryParse(text, out float parsedValue)) return false;
        if (parsedValue <= 0) return false;

        value = parsedValue;
        return true;
    }

    // 获取有效范围
    private bool TryGetValidRange(out float min, out float max)
    {
        min = max = 0f;

        if (string.IsNullOrWhiteSpace(kcalText.text)) return false;

        string[] rangeParts = kcalText.text.Split('~');
        if (rangeParts.Length == 0 || rangeParts.Length > 2) return false;

        // 处理单值范围
        if (rangeParts.Length == 1)
        {
            if (!float.TryParse(rangeParts[0], out float singleValue)) return false;
            min = max = singleValue;
            return true;
        }

        // 处理范围值
        if (!float.TryParse(rangeParts[0], out float lower) ||
            !float.TryParse(rangeParts[1], out float upper))
        {
            return false;
        }

        min = Mathf.Round(lower * 100) / 100;
        max = Mathf.Round(upper * 100) / 100;
        return true;
    }

    // 更新显示内容
    private void UpdatePartText(float value)
    {
        int result = Mathf.RoundToInt(value / 90f);
        partText.text = result.ToString();
    }

    // 清空无效输入
    private void ClearInvalidInput()
    {
        // 避免循环触发事件
        kcalInput.onValueChanged.RemoveListener(OnChangedInput);
        kcalInput.text = "";
        kcalInput.onValueChanged.AddListener(OnChangedInput);
    }
}
