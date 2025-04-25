using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class YiTiJiUI : UICaoZuoBase
{
    public Button randomBtn;
    public Button oneselfBtn;
    public GameObject selectUIObj;

    public Toggle normalTgl;
    public Toggle diseaseTgl;
    public Dropdown normalDrop;
    public Dropdown diseaseDrop;

    public InputField inFName;
    public InputField inFBirthday;
    public Dropdown inFGender;
    public InputField inFHeight;
    public InputField inFWeight;
    public Dropdown workDrop;
    public Button verifyBtn;
    public GameObject informUIObj;

    UserInfo userInfo = new UserInfo();

    public GameObject tipObjUI;


    private void OnEnable()
    {
        selectUIObj.SetActive(true);
        informUIObj.SetActive(false);
        normalDrop.value = 0;
        inFName.text = "";
        inFBirthday.text = "";
        inFHeight.text = "";
        inFWeight.text = "";
        workDrop.value = 0;
        normalTgl.isOn = true;
        tipObjUI.SetActive(false);

    }
    // Start is called before the first frame update
    void Start()
    {
        randomBtn.onClick.AddListener(RandomInform);
        oneselfBtn.onClick.AddListener(OneselfInform);
        verifyBtn.onClick.AddListener(VerifylfInform);

        inFBirthday.onValueChanged.AddListener(OnDateValueChanged);
        inFBirthday.onEndEdit.AddListener(ValidateDate);
    }



    private void VerifylfInform()
    {
        if (!CheckRequiredFields())
        {
            tipObjUI.SetActive(true);
            StartCoroutine(WaitCloseTip());
            return;
        }
        informUIObj.SetActive(false);
        GameManager.Instance.stepDetection = true;
        UIManager.Instance.CloseUICaoZuo("YiTiJiUI");
        ChooseFoodAllInformCon.Instance.userInfo = userInfo;

    }

    IEnumerator WaitCloseTip()
    {
        yield return new WaitForSeconds(2f);
        tipObjUI.SetActive(false);

    }

    // 检查必填字段是否为空
    private bool CheckRequiredFields()
    {
        bool isValid = true;

        if (string.IsNullOrEmpty(userInfo.birthday))
        {
            isValid = false;
        }
        if (string.IsNullOrEmpty(userInfo.height))
        {
            isValid = false;
        }
        if (string.IsNullOrEmpty(userInfo.weight))
        {
            isValid = false;
        }
        if (string.IsNullOrEmpty(userInfo.sex))
        {
            isValid = false;
        }
        if (string.IsNullOrEmpty(userInfo.level))
        {
            isValid = false;
        }
        if (string.IsNullOrEmpty(userInfo.physique))
        {
            isValid = false;
        }

        return isValid;
    }

    private void OneselfInform()
    {
        informUIObj.SetActive(true);
        selectUIObj.SetActive(false);

    }

    private void RandomInform()
    {
        GenerateRandomData();
        ChooseFoodAllInformCon.Instance.userInfo = userInfo;

        selectUIObj.SetActive(false);
        GameManager.Instance.stepDetection = true;
        UIManager.Instance.CloseUICaoZuo("YiTiJiUI");
    }



    // Update is called once per frame
    void Update()
    {
        if (normalDrop.gameObject.activeSelf)
        {
            userInfo.physique = normalDrop.options[normalDrop.value].text;
        }
        if (diseaseDrop.gameObject.activeSelf)
        {
            userInfo.physique = diseaseDrop.options[diseaseDrop.value].text;
        }

    }
    private void LateUpdate()
    {
        if (inFBirthday.text != ""&& birInpBl)
        {
            userInfo.birthday = inFBirthday.text;
        }
        if (inFHeight.text != "")
        {
            userInfo.height = inFHeight.text;
        }
        if (inFWeight.text != "")
        {
            userInfo.weight = inFWeight.text;
        }
        userInfo.sex = inFGender.options[inFGender.value].text;
        userInfo.level = workDrop.options[workDrop.value].text;
    }

    public Text errorText;      // 错误提示文本
    private bool isFormatting = false;     // 防止递归触发事件

    bool birInpBl = false;
    // 输入时自动格式化
    private void OnDateValueChanged(string newValue)
    {
        if (isFormatting) return;
        isFormatting = true;

        // 1. 移除非数字字符
        string numbersOnly = Regex.Replace(newValue, @"[^\d]", "");
        if (numbersOnly.Length > 8) numbersOnly = numbersOnly.Substring(0, 8);

        // 2. 自动插入分隔符
        string formatted = "";
        for (int i = 0; i < numbersOnly.Length; i++)
        {
            if (i == 4 || i == 6) formatted += "-";
            formatted += numbersOnly[i];
        }

        // 3. 更新输入框文本
        inFBirthday.text = formatted;
        inFBirthday.caretPosition = formatted.Length; // 调整光标位置

        isFormatting = false;
    }

    // 结束编辑时验证日期
    private void ValidateDate(string date)
    {
        if (IsValidDate(date))
        {
            errorText.text = "";
            inFBirthday.image.color = Color.white;
            birInpBl = true;
        }
        else
        {
            errorText.text = "日期格式错误，正确示例：2000-01-01";
            inFBirthday.image.color = Color.red;
            birInpBl = false;

        }
    }

    // 验证日期合法性
    private bool IsValidDate(string date)
    {
        if (date.Length != 10) return false; // 长度必须为 10（YYYY-MM-DD）

        // 正则表达式验证格式
        Regex regex = new Regex(@"^\d{4}-\d{2}-\d{2}$");
        if (!regex.IsMatch(date)) return false;

        // 尝试转换为 DateTime 对象
        return DateTime.TryParseExact(
            date,
            "yyyy-MM-dd",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None,
            out _
        );
    }

    //进行随机定义
    [Header("随机范围")]
    public int minYear = 1900;
    public int maxYear = 2025;
    public int minHeight = 100;
    public int maxHeight = 200;
    public int minWeight = 30;
    public int maxWeight = 150;

    // 预定义选项
    private string[] genders = { "男", "女" };
    private string[] levelTypes = { "轻体力", "中体力", "重体力" };
    private string[] physiqueTypes = { "正常", "肥胖", "消瘦", "特殊", "代谢性疾病患者", "消化道疾病患者", "心脑血管疾病患者", "肝胆胰疾病患者", "呼吸系统疾病患者", "肾脏疾病患者", "血液系统疾病患者" };


    // 生成随机数据并更新UI
    public void GenerateRandomData()
    {
        string date = GenerateRandomDate();
        int height = UnityEngine.Random.Range(minHeight, maxHeight + 1);
        int weight = UnityEngine.Random.Range(minWeight, maxWeight + 1);
        string sex = genders[UnityEngine.Random.Range(0, genders.Length)];
        string levelType = levelTypes[UnityEngine.Random.Range(0, levelTypes.Length)];
        string physique = physiqueTypes[UnityEngine.Random.Range(0, physiqueTypes.Length)];

        userInfo.birthday = date;
        userInfo.height = height.ToString();
        userInfo.weight = weight.ToString();
        userInfo.sex = sex;
        userInfo.level = levelType;
        userInfo.physique = physique;
    }

    // 生成合法日期（YYYY-MM-DD）
    private string GenerateRandomDate()
    {
        int year = UnityEngine.Random.Range(minYear, maxYear + 1);
        int month = UnityEngine.Random.Range(1, 13);
        int day = UnityEngine.Random.Range(1, DateTime.DaysInMonth(year, month) + 1);
        return $"{year}-{month:00}-{day:00}";
    }
}
