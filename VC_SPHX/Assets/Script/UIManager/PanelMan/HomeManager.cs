using System;
using System.Collections;
using System.Collections.Generic;
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
    //public Color colorBack;

    /// <summary>
    /// 当前点击的哪个菜单(顶部)
    /// </summary>
    public static int ClickIndex = -1;

    void Awake()
    {

    }

    void OnDestroy()
    {
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
            // 初始化状态
            //UpdateButtonAppearance(tButton, tButton.toggle.isOn);
        }

        //foreach (var tButton in toggleButtons)
        //{
        //    tButton.toggle.isOn = false;
        //    //Debug.Log(tButton.toggle.isOn+"   "+tButton.uiPanel.name);
        //}

        MessageCenter.Instance.Register("SendHomeReset", HomeReset); //氨基酸工作台

        if (SceneMgr.CurSceneName == GameScene.Exp_HuaXue)
        {
            UIManager.Instance.OpenUI(UINameType.UI_ZhishiManager);
            toggleButtons[0].toggle.isOn = true;
        }
        else
        {
            //toggleButtons[2].toggle.isOn = true;
            toggleButtons[2].toggle.SetIsOnWithoutNotify(true);
        }
    }

    void UpdateButtonAppearance(int index,ToggleButton tButton, bool isOn)
    {
        if (isOn)
        {
            ClickIndex = index;
            UIManager.Instance.OpenUI(tButton.uiPanel.name);
            UIManager.Instance.OpenUI(UINameType.UI_BackMan);
            UIManager.Instance.CloseAllUICaoZuo();
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
}
