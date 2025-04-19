using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoxingManager : UIBase
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
        //public Transform uiPanel;
        //public ToggleGroup toggleGroup;
        [HideInInspector] public bool isSelected;
    }

    public List<EquipmentItemData> equipmentItems = new List<EquipmentItemData>();

    public List<ToggleButton> toggleButtons;
    private ToggleGroup toggleGroup;
    public Color colorBack;

    public GameObject modelObj;

    

    void Awake()
    {
        toggleGroup = GetComponent<ToggleGroup>();
        int index = 1;
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
            //UpdateButtonAppearance(tButton, tButton.toggle.isOn);
        }

    }

    void UpdateButtonAppearance(ToggleButton tButton, bool isOn)
    {
        DynamicScrollView.Instance.ClosePool();
        DynamicScrollView.Instance.GetFromPool(DataManager.Instance.GetItemById(tButton.label.name).Count);//调用显示点击对应的设备按钮
        TogIntroduceItem.Instance.UpdateUIInf(tButton.label.name, DataManager.Instance.GetItemById(tButton.label.name));
        //tButton.background.color = isOn ? tButton.selectedColor : tButton.normalColor;
        //tButton.label.color = isOn ? tButton.normalColor : Color.black;
        //tButton.uiPanel.gameObject.SetActive(isOn);
    }

    public void SetInformation()
    {

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
        modelObj.SetActive(true);

        StartCoroutine(YieldGetFromPool());
        DynamicScrollView.Instance.ClosePool();
        TogIntroduceItem.Instance.CloseTogIsOn();
        //DynamicScrollView.Instance.ShowPool();
        DynamicScrollView.Instance.GetFromPool(DataManager.Instance.GetItemById(toggleButtons[0].label.name).Count);//调用显示点击对应的设备按钮
        TogIntroduceItem.Instance.UpdateUIInf(toggleButtons[0].label.name, DataManager.Instance.GetItemById(toggleButtons[0].label.name));

    }
    IEnumerator YieldGetFromPool()
    {
        yield return new WaitForSeconds(0.5f);
    }

    public void ShowStartUI()
    {
        DynamicScrollView.Instance.GetFromPool(DataManager.Instance.GetItemById(toggleButtons[0].label.name).Count);//调用显示点击对应的设备按钮
        TogIntroduceItem.Instance.UpdateUIInf(toggleButtons[0].label.name, DataManager.Instance.GetItemById(toggleButtons[0].label.name));
    }
    private void OnDisable()
    {
        modelObj.SetActive(false);
    }
}
