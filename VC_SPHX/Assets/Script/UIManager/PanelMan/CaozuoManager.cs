using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI 选择实验界面
/// </summary>
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

    /// <summary>
    /// 点击的哪个实验菜单
    /// </summary>
    public static int ClickIndex = -1;

    private void OnEnable()
    {
        foreach (var tButton in toggleButtons)
        {
            tButton.toggle.isOn = false;
        }
    }

    void OnDestroy()
    {
        foreach (var tButton in toggleButtons)
        {
            tButton.toggle.onValueChanged.RemoveAllListeners();
        }
    }

    void Awake()
    {
        toggleGroup = GetComponent<ToggleGroup>();
        int index = 0;

        foreach (var tButton in toggleButtons)
        {
            var trf = transform.GetChild(index + 1);
            tButton.toggle = trf.GetComponent<Toggle>();
            tButton.label = trf.GetChild(1).GetComponent<Text>();
            tButton.name = trf.GetChild(2).name;
            tButton.index = index;

            int cIndex = index;
            index++;

            //tButton.toggle.group = toggleGroup;

            tButton.toggle.onValueChanged.AddListener((isOn) =>
                UpdateButtonAppearance(cIndex, tButton, isOn));

            // 初始化状态
            UpdateButtonAppearance(cIndex, tButton, tButton.toggle.isOn);
        }

    }

    //点击操作 出现提示 并传点击的是哪个面板
    void UpdateButtonAppearance(int index, ToggleButton tButton, bool isOn)
    {
        if (isOn)
        {
            GameObjMan.Instance.CloseObjCon(1);

            LabSystemManager.Instance.OnLabButtonClicked(tButton.index + 1, tButton.name);

            ClickIndex = index;
            print("UpdateButtonAppearance: " + index);

            //膳食分析与营养配餐实习项目 个人营养配餐 人体数字解剖
            if (tButton.label.name == "ShanshiFenxi" || tButton.label.name == "GerenYinyang" || tButton.label.name == "RentiShuzi")
            {
                UIManager.Instance.CloseUI(UINameType.UI_ZhishiManager);
                UIManager.Instance.CloseUI(UINameType.UI_MoxingManager);
                UIManager.Instance.CloseUI(UINameType.UI_CaozuoManager);
                UIManager.Instance.CloseUI(UINameType.UI_BaogaoManager);
                UIManager.Instance.CloseUI(UINameType.UI_BackMan);
                GameManager.Instance.SetGameObj(true);
                GameObjMan.Instance.OpenFirst();

                LabSystemManager.Instance.SelectAssessmentMode();
                //tButton.toggle.isOn = false;
                tButton.toggle.isOn = false;

                return;
            }
            else if (tButton.index == 2)
            {
                //重金属检测
                
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

        //显示选择模式面板
        panelCaozuoObj.SetActive(isOn);

        //UIManager.Instance.CloseUI("CaozuoManager");

    }
}
