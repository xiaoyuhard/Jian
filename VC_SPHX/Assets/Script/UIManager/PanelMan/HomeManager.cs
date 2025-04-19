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
    private ToggleGroup toggleGroup;
    //public Color colorBack;

    void Awake()
    {
        toggleGroup = GetComponent<ToggleGroup>();
        int index = 1;

        foreach (var tButton in toggleButtons)
        {
            tButton.toggle = transform.GetChild(index).GetComponent<Toggle>();
            tButton.label = transform.GetChild(index).transform.GetChild(1).GetComponent<Text>();
            tButton.background = transform.GetChild(index).transform.GetChild(0).GetComponent<Image>();
            //tButton.uiPanel = transform.GetChild(index).transform.GetChild(2).transform.GetComponent<GameObject>();
            index++;
            //tButton.normalColor = Color.white;
            //tButton.normalColor.a = 100;
            //tButton.selectedColor = colorBack;
            //tButton.selectedColor.a = 100;
            tButton.toggle.group = toggleGroup;
            tButton.toggle.isOn = false;
            tButton.toggle.onValueChanged.AddListener((isOn) =>
                UpdateButtonAppearance(tButton, isOn));

            // 初始化状态
            //UpdateButtonAppearance(tButton, tButton.toggle.isOn);
        }

        foreach (var tButton in toggleButtons)
        {
            tButton.toggle.isOn = false;
            //Debug.Log(tButton.toggle.isOn+"   "+tButton.uiPanel.name);
        }
    }

    private void Start()
    {
        MessageCenter.Instance.Register("SendHomeReset", HomeReset); //氨基酸工作台

    }

    void UpdateButtonAppearance(ToggleButton tButton, bool isOn)
    {
        if (isOn)
        {
            UIManager.Instance.OpenUI(tButton.uiPanel.name);
            UIManager.Instance.OpenUI("BackMan");
            UIManager.Instance.CloseAllUICaoZuo();
            GameManager.Instance.CloseAllCon();
            //GameManager.Instance.SetGameObj(false);
            GameObjMan.Instance.CLoseFirst();
            GameObjMan.Instance.UpObjPosCon();
            DoorClickCon.Instance.ResDoorItem();

        }
        //tButton.background.color = isOn ? tButton.selectedColor : tButton.normalColor;
        //tButton.label.color = isOn ? tButton.normalColor : Color.black;
        tButton.uiPanel.SetActive(isOn);
        CaozuoTipsPanel.Instance.ClosePan();
    }

    private void HomeReset(string obj)
    {
        toggleButtons[2].toggle.isOn = true;

    }

    void OnDestroy()
    {
        foreach (var tButton in toggleButtons)
        {
            tButton.toggle.onValueChanged.RemoveAllListeners();
        }
    }

    IEnumerator YieldGetFromPool()
    {
        yield return new WaitForSeconds(0.1f);
        UIManager.Instance.OpenUI("ZhishiManager");
        toggleButtons[0].toggle.isOn = true;

    }
    public override void OnOpen()
    {

    }

    public override void OnClose()
    {

    }

    private void OnEnable()
    {
        StartCoroutine(YieldGetFromPool());

    }

}
