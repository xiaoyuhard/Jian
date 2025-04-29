using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI ѡ��ʵ�����
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
    //ѡ�����ģʽ������ ���ˣ�
    public GameObject panelCaozuoObj;

    /// <summary>
    /// ������ĸ�ʵ��˵�
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

            // ��ʼ��״̬
            UpdateButtonAppearance(cIndex, tButton, tButton.toggle.isOn);
        }

    }

    //������� ������ʾ ������������ĸ����
    void UpdateButtonAppearance(int index, ToggleButton tButton, bool isOn)
    {
        if (isOn)
        {
            GameObjMan.Instance.CloseObjCon(1);

            LabSystemManager.Instance.OnLabButtonClicked(tButton.index + 1, tButton.name);

            ClickIndex = index;
            print("UpdateButtonAppearance: " + index);

            //��ʳ������Ӫ�����ʵϰ��Ŀ ����Ӫ����� �������ֽ���
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
                //�ؽ������
                
            }

            //if (isOn)
            //{
            //    LabSystemManager.Instance.OnLabButtonClicked(tButton.index + 1, tButton.name);
            //    //UIManager.Instance.OpenUI(panelCaozuoObj.name);
            //    //MessageCenter.Instance.Send("CaozuoName", tButton.label.tag);//Ӧ�ò���Ҫ
            //    tButton.toggle.isOn = false;
            //}

            tButton.toggle.isOn = false;
        }

        //��ʾѡ��ģʽ���
        panelCaozuoObj.SetActive(isOn);

        //UIManager.Instance.CloseUI("CaozuoManager");

    }
}
