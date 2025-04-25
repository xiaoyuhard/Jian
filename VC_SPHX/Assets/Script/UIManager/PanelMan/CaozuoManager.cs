using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UI 实验操作界面
public class CaozuoManager : UIBase
{
    [System.Serializable]
    public class ToggleButton
    {
        public string name;
        public Toggle toggle;
        public Text label;
        public int index;
    }

    public List<ToggleButton> toggleButtons;
    private ToggleGroup toggleGroup;
    //选择操作模式（跟随 考核）
    public GameObject panelCaozuoObj;

    void Awake()
    {
        toggleGroup = GetComponent<ToggleGroup>();
        int index = 0;
        foreach (var tButton in toggleButtons)
        {
            tButton.toggle = transform.GetChild(index).GetComponent<Toggle>();
            tButton.label = transform.GetChild(index).transform.GetChild(1).GetComponent<Text>();
            tButton.name = transform.GetChild(index).transform.GetChild(2).name;
            tButton.index = index;

            index++;

            //tButton.toggle.group = toggleGroup;

            tButton.toggle.onValueChanged.AddListener((isOn) =>
                UpdateButtonAppearance(tButton, isOn));

            // 初始化状态
            UpdateButtonAppearance(tButton, tButton.toggle.isOn);
        }

    }

    //点击操作 出现提示 并传点击的是哪个面板
    void UpdateButtonAppearance(ToggleButton tButton, bool isOn)
    {
        if (isOn)
        {
            LabSystemManager.Instance.OnLabButtonClicked(tButton.index + 1, tButton.name);

            //膳食分析与营养配餐实习项目 个人营养配餐 人体数字解剖
            if (tButton.label.name == "ShanshiFenxi" || tButton.label.name == "GerenYinyang" || tButton.label.name == "RentiShuzi")
            {
                //LabSystemManager.Instance.OnLabButtonClicked(tButton.index + 1, tButton.name);

                UIManager.Instance.CloseUI(UINameType.UI_ZhishiManager);
                UIManager.Instance.CloseUI(UINameType.UI_MoxingManager);
                UIManager.Instance.CloseUI(UINameType.UI_CaozuoManager);
                UIManager.Instance.CloseUI(UINameType.UI_BaogaoManager);
                UIManager.Instance.CloseUI(UINameType.UI_BackMan);
                GameManager.Instance.SetGameObj(true);
                GameObjMan.Instance.OpenFirst();

                LabSystemManager.Instance.SelectAssessmentMode();
                //tButton.toggle.isOn = false;

                //return;
            }
            else if (tButton.index == 2)
            {
                //重金属检测
                UIManager.Instance.CloseUI("ZhishiManager");
                UIManager.Instance.CloseUI("MoxingManager");
                UIManager.Instance.CloseUI("CaozuoManager");
                UIManager.Instance.CloseUI("BaogaoManager");
                UIManager.Instance.CloseUI("BackMan");
                GameObjMan.Instance.OpenFirst();
                LabSystemManager.Instance.SelectAssessmentMode();
            }

            //if (isOn)
            //{
            //    LabSystemManager.Instance.OnLabButtonClicked(tButton.index + 1, tButton.name);
            //    //UIManager.Instance.OpenUI(panelCaozuoObj.name);
            //    //MessageCenter.Instance.Send("CaozuoName", tButton.label.tag);//应该不需要
            //    tButton.toggle.isOn = false;
            //}

            tButton.toggle.isOn = false;
        }
        panelCaozuoObj.SetActive(isOn);

        //UIManager.Instance.CloseUI("CaozuoManager");

    }

    void OnDestroy()
    {
        foreach (var tButton in toggleButtons)
        {
            tButton.toggle.onValueChanged.RemoveAllListeners();
        }
    }
    private void OnEnable()
    {
        foreach (var tButton in toggleButtons)
        {
            tButton.toggle.isOn = false;
        }
    }
}
