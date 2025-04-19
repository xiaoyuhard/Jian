using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogIntroduceItem : MonoSingletonBase<TogIntroduceItem>
{
    [System.Serializable]
    public class ToggleButton
    {
        public Toggle toggle;
        public Text label;
        public Image background;
        public Color normalColor = Color.white;
        public Color selectedColor;
        public string sprName;
        public string introduceTex;
        //public string normalText;
        //public string selectedText;
        //public Transform uiPanel;
        //public ToggleGroup toggleGroup;
        [HideInInspector] public bool isSelected;
    }
    public string idName = "";
    public List<ToggleButton> toggleButtons = new List<ToggleButton>();
    private ToggleGroup toggleGroup;
    public Color colorBack;
    private void Awake()
    {
    }

    void Start()
    {
        //int index = 1;

        foreach (var tButton in toggleButtons)
        {

            //tButton.toggle = transform.GetChild(index).GetComponent<Toggle>();
            //tButton.label = transform.GetChild(index).transform.GetChild(1).GetComponent<Text>();
            //tButton.background = transform.GetChild(index).transform.GetChild(0).GetComponent<Image>();
            ////tButton.uiPanel = transform.GetChild(index).transform.GetChild(2).transform.GetComponent<Transform>();
            //index++;
            tButton.normalColor = Color.white;
            tButton.normalColor.a = 100;
            tButton.selectedColor = colorBack;
            tButton.selectedColor.a = 100;
            tButton.toggle.onValueChanged.AddListener((isOn) =>
                UpdateButtonAppearance(tButton, isOn));

            // 初始化状态
            UpdateButtonAppearance(tButton, tButton.toggle.isOn);
        }

    }

    //更新toggle显示 和储存
    public void UpdateUIInf(string idName, List<EquipmentItemData> date)
    {
        for (int i = 0; i < date.Count; i++)
        {
            toggleButtons[i].label.text = date[i].parent;
            toggleButtons[i].sprName = date[i].iconName;
            toggleButtons[i].introduceTex = date[i].introduce;
        }
    }

    //当toggle不够时进行扩容
    public void AddTog(int count)
    {
        toggleGroup = GetComponent<ToggleGroup>();

        for (int i = 0; i < count; i++)
        {
            ToggleButton togBtn = new ToggleButton();
            togBtn.toggle = transform.GetChild(i).GetComponent<Toggle>();
            togBtn.label = transform.GetChild(i).transform.GetChild(1).GetComponent<Text>();
            togBtn.background = transform.GetChild(i).transform.GetChild(0).GetComponent<Image>();
            togBtn.toggle.group = toggleGroup.GetComponent<ToggleGroup>();
            toggleButtons.Add(togBtn);
        }
    }

    //当点击对应的模型toggle时 显示对应的图片和介绍
    void UpdateButtonAppearance(ToggleButton tButton, bool isOn)
    {
        IntroducePanel.Instance.SetInformation(tButton.introduceTex, tButton.sprName, tButton.sprName);
        //tButton.background.color = isOn ? tButton.selectedColor : tButton.normalColor;
        //tButton.label.color = isOn ? tButton.normalColor : Color.black;
        MoXingObjCon.Instance.ShowObj(isOn ? tButton.sprName : "");
        //tButton.uiPanel.gameObject.SetActive(isOn);
        if (isOn) MoXingObjCon.Instance.ShowObj(tButton.label.text);
    }

    public void CloseTogIsOn()
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

}
