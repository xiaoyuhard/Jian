using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


/// <summary>
/// ��ʳʵ��-һ���������Ϣ�������
/// </summary>
public class YiTiJiUI : UICaoZuoBase
{
    public static YiTiJiUI Instance { get; private set; }

    public Button randomBtn;
    public Button oneselfBtn;
    public GameObject selectUIObj;

    public Toggle normalTgl; //�����˰�ť
    public Toggle diseaseTgl;//���˰�ť

    public Toggle childNorTgl;//�Ӽ��������˰�ť
    //public Toggle childPregnantTgl;//�Ӽ����и���ť
    public Toggle childNurseTgl;//�Ӽ�����ĸ��ť
    public Dropdown pregnantDrop;
    public Dropdown diseaseDrop;
    public Dropdown crowdDrop;

    //public TMP_InputField inFName;
    //public InputField inFBirthday;
    public Dropdown inFGender;
    public InputField inFHeight;
    public InputField inFWeight;
    public Dropdown workDrop;
    public Button verifyBtn;
    public GameObject informUIObj;

    UserInfo userInfo = new UserInfo();

    public GameObject tipObjUI;

    public Button biLiQueRenBtn;
    public Dropdown biLiDrop;
    public Transform biLiContent;
    public GameObject biLiPrefab;


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
        selectUIObj.SetActive(true);
        informUIObj.SetActive(false);
        pregnantDrop.value = 0;
        crowdDrop.value = 0;
        //inFName.text = "";
        //inFBirthday.text = "";
        inFHeight.text = "";
        inFWeight.text = "";
        workDrop.value = 0;
        normalTgl.isOn = true;
        tipObjUI.SetActive(false);
        userInfo.physique = "����";

    }
    // Start is called before the first frame update
    void Start()
    {
        userInfo.physique = "����";

        randomBtn.onClick.AddListener(RandomInform);
        oneselfBtn.onClick.AddListener(OneselfInform);
        verifyBtn.onClick.AddListener(VerifylfInform);
        biLiQueRenBtn.onClick.AddListener(BiLiClickTOServer);

        //inFBirthday.onValueChanged.AddListener(OnDateValueChanged);
        //inFBirthday.onEndEdit.AddListener(ValidateDate);
        childNorTgl.onValueChanged.AddListener(ClickNorTgl);
        childNurseTgl.onValueChanged.AddListener(ClickNurseTgl);

        crowdDrop.onValueChanged.AddListener(OnCrowdChanged);

    }
    bool isNormal = true;
    private void OnCrowdChanged(int index)
    {
        if (index == 0)
        {
            isNormal = true;
        }
        else
        {
            isNormal = false;
        }
    }

    public UserInfo BackUserInfo()
    {
        return userInfo;
    }

    bool biLiBtnOclick = false;
    private void BiLiClickTOServer()
    {
        biLiBtnOclick = true;

        if (CheckRequiredFields())
        {
            if (!DatePicker.Instance.BackYear())
            {
                biLiDrop.value = 3;
                userInfo.isBaby = true;
                workDrop.value = 1;
            }
            else
            {
                if (biLiDrop.value == 3)
                {
                    biLiDrop.value = 0;
                }
                userInfo.isBaby = false;

            }
            userInfo.level = workDrop.options[workDrop.value].text;

            userInfo.proportion = biLiDrop.options[biLiDrop.value].text;
            userInfo.birthday = DatePicker.Instance.GetSelectedDate().ToString("yyyy-MM-dd");

            ServerCon.Instance.ConverToJsonPost(SerializeData(userInfo), "/analyse/nutrition/plan");
        }
        else
        {
            tipObjUI.SetActive(true);
            StartCoroutine(WaitCloseTip());
            biLiBtnOclick = false;
        }
    }
    private string SerializeData(UserInfo data)
    {
        // ����һ��ʹ�� Unity ���� JsonUtility����Ҫ��װ�ࣩ
        Wrapper<UserInfo> wrapper = new Wrapper<UserInfo> { items = data };
        //return JsonUtility.ToJson(wrapper);

        // ��������ʹ�� Newtonsoft.Json��ֱ�����л��б�
        return JsonConvert.SerializeObject(data);
    }

    public void ShowBiLiScroll(ThreeMeals plan)
    {
        isValid = true;

        // �������Toggle
        for (int i = biLiContent.childCount - 1; i >= 0; i--)
        {
            Destroy(biLiContent.GetChild(i).gameObject);
        }

        InstantObj(plan.data.breakfastPlan);
        InstantObj(plan.data.lunchPlan);
        InstantObj(plan.data.dinnerPlan);

    }
    public void InstantObj(Plan plan)
    {
        GameObject obj = Instantiate(biLiPrefab, biLiContent);
        obj.SetActive(true);
        obj.transform.GetChild(0).GetComponent<Text>().text = plan.protein;
        obj.transform.GetChild(1).GetComponent<Text>().text = plan.fat;
        obj.transform.GetChild(2).GetComponent<Text>().text = plan.cho;
        obj.transform.GetChild(3).GetComponent<Text>().text = plan.totalEnergyKcal;
    }

    bool isSex = false;
    private void ClickNurseTgl(bool arg0)
    {
        userInfo.physique = "��ĸ";
        inFGender.value = 1;
        isSex = true;
    }

    private void ClickNorTgl(bool arg0)
    {
        userInfo.physique = "����";
        isSex = false;

    }

    private void VerifylfInform()
    {
        if (!CheckRequiredFields())
        {
            tipObjUI.SetActive(true);
            StartCoroutine(WaitCloseTip());
            return;
        }
        userInfo.birthday = DatePicker.Instance.GetSelectedDate().ToString("yyyy-MM-dd");
        informUIObj.SetActive(false);
        GameManager.Instance.stepDetection = true;
        UIManager.Instance.CloseUICaoZuo("YiTiJiUI");
        ChooseFoodAllInformCon.Instance.userInfo = userInfo;
        GameObjMan.Instance.OpenFirst();
        FoodManager.Instance.LoadFoodData();
    }

    IEnumerator WaitCloseTip()
    {
        yield return new WaitForSeconds(2f);
        tipObjUI.SetActive(false);

    }
    public bool isValid = false;

    // �������ֶ��Ƿ�Ϊ��
    private bool CheckRequiredFields()
    {
        isValid = true;
        //if (string.IsNullOrEmpty(userInfo.birthday))
        //{
        //    isValid = false;
        //}
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
        if (!biLiBtnOclick)
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
        GameObjMan.Instance.OpenFirst();

        selectUIObj.SetActive(false);
        GameManager.Instance.stepDetection = true;
        UIManager.Instance.CloseUICaoZuo("YiTiJiUI");
        FoodManager.Instance.LoadFoodData();
    }



    // Update is called once per frame
    void Update()
    {
        if (crowdDrop.gameObject.activeSelf)
        {
            if (!isNormal)
            {
                userInfo.physique = crowdDrop.options[crowdDrop.value].text;

                inFGender.value = 1;
                isSex = true;

            }


        }
        if (diseaseDrop.gameObject.activeSelf)
        {
            userInfo.physique = diseaseDrop.options[diseaseDrop.value].text;
            isSex = false;

        }

    }
    private void LateUpdate()
    {
        //if (inFBirthday.text != "" && birInpBl)
        //{
        //    userInfo.birthday = inFBirthday.text;
        //}
        //userInfo.birthday = inFBirthday.text;

        if (inFHeight.text != "" && IsInputText(inFHeight.text))
        {
            userInfo.height = inFHeight.text;
        }
        if (inFWeight.text != "" && IsInputText(inFWeight.text))
        {
            userInfo.weight = inFWeight.text;
        }
        userInfo.sex = inFGender.options[inFGender.value].text;
        userInfo.level = workDrop.options[workDrop.value].text;
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

    //public Text errorText;      // ������ʾ�ı�
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
        //inFBirthday.text = formatted;
        //inFBirthday.caretPosition = formatted.Length; // �������λ��

        isFormatting = false;
    }

    // �����༭ʱ��֤����
    private void ValidateDate(string date)
    {
        //    if (IsValidDate(date))
        //    {
        //        errorText.text = "";
        //        inFBirthday.image.color = Color.white;
        //        birInpBl = true;
        //    }
        //    else
        //    {
        //        errorText.text = "���ڸ�ʽ������ȷʾ����2000-01-01";
        //        inFBirthday.image.color = Color.red;
        //        birInpBl = false;

        //    }
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
    public int minYear/* = DateTime.Now.Year - 150*/;
    public int maxYear/* = DateTime.Now.Year*/;
    public int minHeight = 100;
    public int maxHeight = 200;
    public int minWeight = 30;
    public int maxWeight = 150;

    // Ԥ����ѡ��
    private string[] genders = { "��", "Ů" };
    private string[] levelTypes = { "��", "��", "��" };
    private string[] physiqueTypes = { "����",/* "�и�",*/ "��ĸ", "������", "������", "������", "��л�Լ�������", "��������������", "����Ѫ�ܼ�������", "�ε��ȼ�������", "����ϵͳ��������", "���༲������", "ѪҺϵͳ��������" };
    private string[] bilis = { "3:4:3", "2:4:4", "4:4:2" };

    string bili;
    // ����������ݲ�����UI
    public void GenerateRandomData()
    {
        maxYear = DateTime.Now.Year; // ��ȡ��ǰ���
        minYear = maxYear - 100; // ������С���Ϊ��ǰ��ݵ�ǰ100��
        bili = bilis[UnityEngine.Random.Range(0, bilis.Length)];

        string date = GenerateRandomDate();
        int height = UnityEngine.Random.Range(minHeight, maxHeight + 1);
        int weight = UnityEngine.Random.Range(minWeight, maxWeight + 1);
        string sex = genders[UnityEngine.Random.Range(0, genders.Length)];
        string levelType = levelTypes[UnityEngine.Random.Range(0, levelTypes.Length)];
        string physique = physiqueTypes[UnityEngine.Random.Range(0, physiqueTypes.Length)];
        if (physique == "��ĸ" || physique == "������" || physique == "������" || physique == "������")
        {
            sex = "Ů";

        }

        if (bili == "3")
        {
            biLiDrop.value = 3;
            userInfo.isBaby = true;
            levelType = levelTypes[1];
        }
        else
        {
            if (biLiDrop.value == 3)
            {
                biLiDrop.value = 0;
            }
            userInfo.isBaby = false;

        }
        //if (physique == "�и�")
        //{
        //    sex = "Ů";
        //}

        userInfo.birthday = date;
        userInfo.height = height.ToString();
        userInfo.weight = weight.ToString();
        userInfo.sex = sex;
        userInfo.level = levelType;
        userInfo.physique = physique;
        userInfo.proportion = bili;
        ServerCon.Instance.ConverToJsonPost(SerializeData(userInfo), "/analyse/nutrition/plan");

    }

    public void BackYear(int year, int month, int day)
    {
        DateTime now = DateTime.Now.Date;

        DateTime dateTime = new DateTime(year, month, day);

        DateTime fiveYear = dateTime.AddYears(5);


        if (now < fiveYear)
        {
            bili = "3";
        }

    }


    // ���ɺϷ����ڣ�YYYY-MM-DD��
    private string GenerateRandomDate()
    {
        int year = UnityEngine.Random.Range(minYear, maxYear + 1);
        int month;
        int day;
        if (year == DateTime.Now.Year)
        {
            month = UnityEngine.Random.Range(1, DateTime.Now.Month + 1);
            if (month == DateTime.Now.Month)
            {
                day = UnityEngine.Random.Range(1, DateTime.Now.Day + 1);
            }
            else
            {
                day = UnityEngine.Random.Range(1, DateTime.DaysInMonth(year, month) + 1);

            }

        }
        else
        {
            month = UnityEngine.Random.Range(1, DateTime.Now.Month + 1);
            day = UnityEngine.Random.Range(1, DateTime.DaysInMonth(year, month) + 1);

        }
        BackYear(year, month, day);
        return $"{year}-{month:00}-{day:00}";
    }
}
