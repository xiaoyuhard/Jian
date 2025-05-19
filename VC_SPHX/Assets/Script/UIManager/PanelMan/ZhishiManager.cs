using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using RenderHeads.Media.AVProVideo;

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
    public VideoPlayer vp;
    public List<ToggleButton> toggleButtons;
    private ToggleGroup toggleGroup;
    public Color colorBack;
    public MediaPlayer mediaPlayer;//ʳƷ����

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

            // ��ʼ��״̬
            //UpdateButtonAppearance(tButton, false);

        }
        //UpdateButtonAppearance(toggleButtons[0], true);
        //toggleButtons[0].toggle.isOn = true;
    }

    void UpdateButtonAppearance(ToggleButton tButton, bool isOn)
    {
        if (isOn)
        {
            //tButton.background.color = isOn ? tButton.selectedColor : tButton.normalColor;
            //tButton.label.color = isOn ? tButton.normalColor : Color.black;
            //tButton.uiPanel.gameObject.SetActive(isOn);
            //videoPlayerObj.GetComponent<VideoPlayer>().clip = Resources.Load<VideoClip>("��ȫ֪ʶ��Ƶ/" + tButton.label.text);
            mediaPlayer.Loop = true;
            //mediaPlayer.OpenMedia(MediaPathType.AbsolutePathOrURL, Application.streamingAssetsPath + "/��ȫ֪ʶ��Ƶ/" + tButton.label.text + ".mp4");
            var url = Application.streamingAssetsPath + "/��ȫ֪ʶ��Ƶ/" + tButton.label.text + ".mp4";
            //mediaPlayer.OpenMedia(MediaPathType.RelativeToStreamingAssetsFolder, url);
            //mediaPlayer.Control.Play();
            print("VideoPlayer url: " + url);
            vp.url = url;
            vp.Play();
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
