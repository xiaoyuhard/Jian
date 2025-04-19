using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZhishiManager : UIBase
{
    [System.Serializable]
    public class ToggleButton
    {
        public Toggle toggle;
        public Text label;
        public Image background;
        public Color normalColor = Color.white;
        public Color selectedColor;
        //public string normalText;
        public string selectedText;
        public Transform uiPanel;
        //public ToggleGroup toggleGroup;
        [HideInInspector] public bool isSelected;
    }

    public List<ToggleButton> toggleButtons;
    private ToggleGroup toggleGroup;
    public Color colorBack;

    void Awake()
    {
        toggleGroup = GetComponent<ToggleGroup>();
        int index = 1;

        foreach (var tButton in toggleButtons)
        {
            tButton.toggle = transform.GetChild(index).GetComponent<Toggle>();
            tButton.label = transform.GetChild(index).transform.GetChild(1).GetComponent<Text>();
            tButton.background = transform.GetChild(index).transform.GetChild(0).GetComponent<Image>();
            tButton.uiPanel = transform.GetChild(index).transform.GetChild(2).transform.GetComponent<Transform>();
            index++;
            //tButton.normalColor = Color.white;
            //tButton.normalColor.a = 100;
            //tButton.selectedColor = colorBack;
            //tButton.selectedColor.a = 100;
            tButton.toggle.group = toggleGroup;
            tButton.toggle.onValueChanged.AddListener((isOn) =>
                UpdateButtonAppearance(tButton, isOn));

            // ³õÊ¼»¯×´Ì¬
            UpdateButtonAppearance(tButton, tButton.toggle.isOn);
        }

    }

    void UpdateButtonAppearance(ToggleButton tButton, bool isOn)
    {
        //tButton.background.color = isOn ? tButton.selectedColor : tButton.normalColor;
        //tButton.label.color = isOn ? tButton.normalColor : Color.black;
        tButton.uiPanel.gameObject.SetActive(isOn);
    }

    void OnDestroy()
    {
        foreach (var tButton in toggleButtons)
        {
            tButton.toggle.onValueChanged.RemoveAllListeners();
        }
    }
}
