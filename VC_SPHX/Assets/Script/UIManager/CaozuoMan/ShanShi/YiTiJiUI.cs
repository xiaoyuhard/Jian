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

    // �������ֶ��Ƿ�Ϊ��
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

    public Text errorText;      // ������ʾ�ı�
    private bool isFormatting = false;     // ��ֹ�ݹ鴥���¼�

    bool birInpBl = false;
    // ����ʱ�Զ���ʽ��
    private void OnDateValueChanged(string newValue)
    {
        if (isFormatting) return;
        isFormatting = true;

        // 1. �Ƴ��������ַ�
        string numbersOnly = Regex.Replace(newValue, @"[^\d]", "");
        if (numbersOnly.Length > 8) numbersOnly = numbersOnly.Substring(0, 8);

        // 2. �Զ�����ָ���
        string formatted = "";
        for (int i = 0; i < numbersOnly.Length; i++)
        {
            if (i == 4 || i == 6) formatted += "-";
            formatted += numbersOnly[i];
        }

        // 3. ����������ı�
        inFBirthday.text = formatted;
        inFBirthday.caretPosition = formatted.Length; // �������λ��

        isFormatting = false;
    }

    // �����༭ʱ��֤����
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
            errorText.text = "���ڸ�ʽ������ȷʾ����2000-01-01";
            inFBirthday.image.color = Color.red;
            birInpBl = false;

        }
    }

    // ��֤���ںϷ���
    private bool IsValidDate(string date)
    {
        if (date.Length != 10) return false; // ���ȱ���Ϊ 10��YYYY-MM-DD��

        // ������ʽ��֤��ʽ
        Regex regex = new Regex(@"^\d{4}-\d{2}-\d{2}$");
        if (!regex.IsMatch(date)) return false;

        // ����ת��Ϊ DateTime ����
        return DateTime.TryParseExact(
            date,
            "yyyy-MM-dd",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None,
            out _
        );
    }

    //�����������
    [Header("�����Χ")]
    public int minYear = 1900;
    public int maxYear = 2025;
    public int minHeight = 100;
    public int maxHeight = 200;
    public int minWeight = 30;
    public int maxWeight = 150;

    // Ԥ����ѡ��
    private string[] genders = { "��", "Ů" };
    private string[] levelTypes = { "������", "������", "������" };
    private string[] physiqueTypes = { "����", "����", "����", "����", "��л�Լ�������", "��������������", "����Ѫ�ܼ�������", "�ε��ȼ�������", "����ϵͳ��������", "���༲������", "ѪҺϵͳ��������" };


    // ����������ݲ�����UI
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

    // ���ɺϷ����ڣ�YYYY-MM-DD��
    private string GenerateRandomDate()
    {
        int year = UnityEngine.Random.Range(minYear, maxYear + 1);
        int month = UnityEngine.Random.Range(1, 13);
        int day = UnityEngine.Random.Range(1, DateTime.DaysInMonth(year, month) + 1);
        return $"{year}-{month:00}-{day:00}";
    }
}
