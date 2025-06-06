using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeManager : UIBase
{
    [System.Serializable]
    public class ToggleButton
    {
        public Toggle toggle;
        public Text label;
        public Image background;
        //public Color normalColor = Color.white;
        //public Color selectedColor;
        //public string normalText;
        public string selectedText;
        public GameObject uiPanel;

        [HideInInspector] public bool isSelected;
    }

    public List<ToggleButton> toggleButtons;
    public ToggleGroup toggleGroup;
    public Button btnMain;
    public GameObject expNamePanel;
    public TextMeshProUGUI txtExpName;

    [Header("����ָ��")]
    public GameObject expOperPanel;
    public GameObject expGuidePanel;
    public GameObject expInfoPanel;

    [Header("��ť")]
    public Button btnOperGuide;
    public Button btnExpInfo;
    public Button btnExpInfo2;
    public Button btnCloseGuide;
    public Button btnCloseInfo;
    public TextMeshProUGUI txtExpInfo;

    //public Color colorBack;

    /// <summary>
    /// ��ǰ������ĸ��˵�(����)
    /// </summary>
    public static int ClickIndex = -1;

    void Awake()
    {
       
    }
    private void OnEnable()
    {
        //UpdateButtonAppearance(0, toggleButtons[0], true);
        //toggleButtons[2].toggle.isOn = true;
        //UpdateButtonAppearance(2, toggleButtons[2], true);

    }
    void OnDestroy()
    {
        //toggleButtons[0].toggle.isOn = true;

        foreach (var tButton in toggleButtons)
        {
            tButton.toggle.onValueChanged.RemoveAllListeners();
        }
    }

    private void Start()
    {
        int index = 1;
        int cIndex = 0;

        foreach (var tButton in toggleButtons)
        {
            int idx = cIndex;
            var trf = toggleGroup.transform.GetChild(index - 1);
            tButton.toggle = trf.GetComponent<Toggle>();
            tButton.background = trf.GetChild(0).GetComponent<Image>();
            tButton.label = trf.GetChild(1).GetComponent<Text>();
            //tButton.uiPanel = transform.GetChild(index).transform.GetChild(2).transform.GetComponent<GameObject>();
            index++;
            //tButton.normalColor = Color.white;
            //tButton.normalColor.a = 100;
            //tButton.selectedColor = colorBack;
            //tButton.selectedColor.a = 100;
            tButton.toggle.group = toggleGroup;
            tButton.toggle.isOn = false;
            tButton.toggle.onValueChanged.AddListener((isOn) =>
                UpdateButtonAppearance(idx, tButton, isOn));
            cIndex++;
            // ��ʼ��״̬
            //UpdateButtonAppearance(idx, tButton, false);
        }
        //Mark:2025.5.6
        //UpdateButtonAppearance(0, toggleButtons[0], true);

        //toggleButtons[0].toggle.isOn = true;

        //foreach (var tButton in toggleButtons)
        //{
        //    tButton.toggle.isOn = false;
        //    //Debug.Log(tButton.toggle.isOn+"   "+tButton.uiPanel.name);
        //}

        //Mark:2025.5.6
        MessageCenter.Instance.Register("SendHomeReset", HomeReset); //�����Ṥ��̨

        //if (SceneMgr.CurSceneName == GameScene.Exp_HuaXue)
        //{
        //    UIManager.Instance.OpenUI(UINameType.UI_ZhishiManager);
        //    toggleButtons[0].toggle.isOn = true;
        //}
        //else
        //{
        //    //toggleButtons[2].toggle.isOn = true;
        //    toggleButtons[2].toggle.SetIsOnWithoutNotify(true);
        //}

        btnMain.onClick.AddListener(() =>
        {
            SceneMgr.LoadScene(GameScene.Main);
            //toggleButtons[0].toggle.isOn = true;
        });

        InitUI();

        if (SceneMgr.CurSceneName == GameScene.Main)
        {
            btnMain.gameObject.SetActive(false);
            expNamePanel.SetActive(false);
            toggleGroup.gameObject.SetActive(true);
        }
        else
        {
            btnMain.gameObject.SetActive(true);
            expNamePanel.SetActive(true);
            toggleGroup.gameObject.SetActive(false);

            var curExp = GameData.Instance.CurrentExperiment;
            if (curExp == Experiment.Unknown)
                expOperPanel.SetActive(false);
            else if ((int)GameData.Instance.CurrentExperiment < 7)
                expOperPanel.SetActive(true);
            else
                expOperPanel.SetActive(false);

            SetName();
        }

        btnOperGuide.onClick.AddListener(() =>
        {
            expGuidePanel.SetActive(true);
            expInfoPanel.SetActive(false);
        });
        btnExpInfo.onClick.AddListener(() =>
        {
            expInfoPanel.SetActive(true);
            expGuidePanel.SetActive(false);
            txtExpInfo.text = ExpInfo.Instance.expInfo;
        });
        btnExpInfo2.onClick.AddListener(() =>
        {
            expInfoPanel.SetActive(true);
            expGuidePanel.SetActive(false);
            txtExpInfo.text = ExpInfo.Instance.expInfo2;
        });
        btnCloseGuide.onClick.AddListener(() =>
        {
            expGuidePanel.SetActive(false);
        });
        btnCloseInfo.onClick.AddListener(() =>
        {
            expInfoPanel.SetActive(false);
        });
    }

    void UpdateButtonAppearance(int index, ToggleButton tButton, bool isOn)
    {
        if (isOn)
        {
            ClickIndex = index;
            UIManager.Instance.CloseWholeUI();
            UIManager.Instance.OpenUI(tButton.uiPanel.name);
            UIManager.Instance.OpenUI(UINameType.UI_BackMan);
            //UIManager.Instance.CloseAllUICaoZuo();
            GameManager.Instance.CloseAllCon();
            //GameManager.Instance.SetGameObj(false);
            GameObjMan.Instance.UpObjPosCon();

            GameObjMan.Instance.CLoseFirst();
            DoorClickCon.Instance.ResDoorItem();

        }
        //tButton.background.color = isOn ? tButton.selectedColor : tButton.normalColor;
        //tButton.label.color = isOn ? tButton.normalColor : Color.black;
        tButton.uiPanel.SetActive(isOn);
        //CaozuoTipsPanel.Instance.ClosePan();
    }

    private void HomeReset(string obj)
    {
        toggleButtons[2].toggle.isOn = true;

    }

    public override void OnOpen()
    {

    }

    public override void OnClose()
    {

    }

    void InitUI()
    {
        expOperPanel.SetActive(false);
        expGuidePanel.SetActive(false);
        expInfoPanel.SetActive(false);
    }

    void SetName()
    {
        string title = string.Empty;
        var curExp = GameData.Instance.CurrentExperiment;
        var subIndex = GameData.Instance.CurExpSubIndex;

        if (curExp == Experiment.AnJiSuan)
        {
            title = "ʳƷ�а����Ậ�����";
        }
        else if (curExp == Experiment.XiangQi)
        {
            title = "ʳƷ�������ɷַ���";
        }
        else if (curExp == Experiment.ZhongJinShu)
        {
            title = "ũ��Ʒ�ؽ����������";
        }
        else if (curExp == Experiment.ShaChongJi)
        {
            title = "ũ��Ʒ�л���ɱ����������";

            if (subIndex == 0)
            {
                title += " ������";
            }
            else if (subIndex == 1)
            {
                title += " ������";
            }
            else if (subIndex == 2)
            {
                title += " ��������Ʒ";
            }
            else if (subIndex == 3)
            {
                title += " Һ̬��Ʒ";
            }
        }
        else if (curExp == Experiment.Tang)
        {
            title = "��ԭ�ζ����ⶨʳƷ�л�ԭ�Ǻ���";
        }
        else if (curExp == Experiment.ZhiFang)
        {
            title = "��ԭ������ȡ���ⶨ֬������";


            if (subIndex == 0)
            {
                title += " ��ͳ����";
            }
            else if (subIndex == 1)
            {
                title += " ʵ���ҷ���";
            }
            else if (subIndex == 2)
            {
                title += " �ǲ����ⶨ";
            }
        }
        else if (curExp == Experiment.DanBaiZhi)
        {
            title = "��ԭ�����ʺ����Ĳⶨ";

            if (subIndex == 0)
            {
                title += " ���϶�����֮�ֶ���";
            }
            else if (subIndex == 1)
            {
                title += " �Զ����϶�����";
            }
        }
        //else if (curExp == Experiment.PeiCan)
        //{
        //    title = "��ʳ������Ӫ�����ʵϰ��Ŀ";
        //}
        //else if (curExp == Experiment.GeRenYingYang)
        //{
        //    title = "����Ӫ�����";
        //}
        //else if (curExp == Experiment.RenTi)
        //{
        //    title = "�������ֽ���";
        //}
        else
        {
            expNamePanel.SetActive(false);
        }

        txtExpName.text = title;
    }

}
