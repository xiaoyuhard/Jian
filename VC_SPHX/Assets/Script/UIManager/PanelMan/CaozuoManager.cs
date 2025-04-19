using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaozuoManager : UIBase
{
    [System.Serializable]
    public class ToggleButton
    {
        public Toggle toggle;
        public string name;
        public Text label;
        public int index;
    }

    public List<ToggleButton> toggleButtons;
    private ToggleGroup toggleGroup;

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
            if (tButton.label.name == "ShanshiFenxi" || tButton.label.name == "GerenYinyang" || tButton.label.name == "RentiShuzi")
            {
                LabSystemManager.Instance.OnLabButtonClicked(tButton.index + 1, tButton.name);

                UIManager.Instance.CloseUI("ZhishiManager");
                UIManager.Instance.CloseUI("MoxingManager");
                UIManager.Instance.CloseUI("CaozuoManager");
                UIManager.Instance.CloseUI("BaogaoManager");
                UIManager.Instance.CloseUI("BackMan");
                GameManager.Instance.SetGameObj(true);
                GameObjMan.Instance.OpenFirst();

                LabSystemManager.Instance.SelectAssessmentMode();
                tButton.toggle.isOn = false;

                return;
            }

            if (isOn)
            {
                LabSystemManager.Instance.OnLabButtonClicked(tButton.index + 1, tButton.name);
                //UIManager.Instance.OpenUI(panelCaozuoObj.name);
                //MessageCenter.Instance.Send("CaozuoName", tButton.label.tag);//应该不需要
                tButton.toggle.isOn = false;
            }
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
