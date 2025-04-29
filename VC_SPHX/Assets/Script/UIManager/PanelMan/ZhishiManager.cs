using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ZhishiManager : UIBase
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
        public Transform uiPanel;
        //public ToggleGroup toggleGroup;
        [HideInInspector] public bool isSelected;
    }
    public GameObject videoPlayerObj;
    public List<ToggleButton> toggleButtons;
    private ToggleGroup toggleGroup;
    public Color colorBack;

    void Awake()
    {
        toggleGroup = GetComponent<ToggleGroup>();
        int index = 3;

        foreach (var tButton in toggleButtons)
        {
            tButton.toggle = transform.GetChild(index).GetComponent<Toggle>();
            tButton.label = transform.GetChild(index).transform.GetChild(1).GetComponent<Text>();
            tButton.background = transform.GetChild(index).transform.GetChild(0).GetComponent<Image>();
            //tButton.uiPanel = transform.GetChild(index).transform.GetChild(2).transform.GetComponent<Transform>();
            index++;
            //tButton.normalColor = Color.white;
            //tButton.normalColor.a = 100;
            //tButton.selectedColor = colorBack;
            //tButton.selectedColor.a = 100;
            tButton.toggle.group = toggleGroup;
            tButton.toggle.onValueChanged.AddListener((isOn) =>
                UpdateButtonAppearance(tButton, isOn));

            // 初始化状态
            UpdateButtonAppearance(tButton, tButton.toggle.isOn);
        }

    }

    void UpdateButtonAppearance(ToggleButton tButton, bool isOn)
    {
        //tButton.background.color = isOn ? tButton.selectedColor : tButton.normalColor;
        //tButton.label.color = isOn ? tButton.normalColor : Color.black;
        //tButton.uiPanel.gameObject.SetActive(isOn);
        videoPlayerObj.GetComponent<VideoPlayer>().clip = Resources.Load<VideoClip>("安全知识视频/" + tButton.label.text);
    }

    void OnDestroy()
    {
        foreach (var tButton in toggleButtons)
        {
            tButton.toggle.onValueChanged.RemoveAllListeners();
        }
    }
}
