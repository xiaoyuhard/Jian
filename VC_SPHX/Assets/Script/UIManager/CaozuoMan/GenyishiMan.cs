using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenyishiMan : UICaoZuoBase
{
    public List<Toggle> toggles = new List<Toggle>();
    public GameObject image;
    public Button chuanDiaBtn;
    public string caozuoTag;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Toggle toggle in toggles)
        {
            toggle.onValueChanged.AddListener((isOn) =>
               UpdateButtonAppearance(isOn));
        }

        chuanDiaBtn.onClick.AddListener(ClosePanel);
        //MessageCenter.Instance.Register("SendWearToGenTag", ToGenMan);
        foreach (var info in toggleInfosFu)
        {
            info.toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                    UpdateImageDisplayFu(info.row, info.column, isOn);
            });
            info.toggle.isOn = false;
        }
        foreach (var info in toggleInfosShou)
        {
            info.toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                    UpdateImageDisplayShou(info.row, info.column, isOn);
            });
            info.toggle.isOn = false;
        }
        foreach (var info in toggleInfosKou)
        {
            info.toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                    UpdateImageDisplayKou(info.row, info.column, isOn);
            });
            info.toggle.isOn = false;
        }
        GetLabIndex();
        //OnToggleSelected += HandleSelection;
    }

    public void GetLabIndex()
    {
        int labIndex = LabSystemManager.Instance.ReutrnCurrIndex();
        switch (labIndex)
        {
            case 1:
                fuIndex = 1;
                shouIndex = 2;
                kouIndex = 2;
                break;
            case 2:
                fuIndex = 1;
                shouIndex = 2;
                kouIndex = 2;
                break;
            case 3:
                fuIndex = 1;
                shouIndex = 2;
                kouIndex = 2;
                break;
            case 4:
                fuIndex = 1;
                shouIndex = 2;
                kouIndex = 2;
                break;
            case 5:
                fuIndex = 1;
                shouIndex = 2;
                kouIndex = 2;
                break;
            case 6:
                fuIndex = 1;
                shouIndex = 2;
                kouIndex = 2;
                break;
            case 7:
                fuIndex = 1;
                shouIndex = 2;
                kouIndex = 2;
                break;
            default:
                break;
        }
    }

    [System.Serializable]
    public class ToggleInfo
    {
        public Toggle toggle;
        public int row;
        public int column;
        public GameObject targetSprite;
    }

    bool fuBl = false;
    bool shouBl = false;
    bool kouBl = false;

    public Sprite beginSprite;
    [Header("图片显示组件")]
    public Image displayImage; // UI中用于显示图片的Image组件

    public ToggleInfo[] toggleInfosFu; // 在Inspector中设置每个Toggle的行列
    public ToggleInfo[] toggleInfosShou; // 在Inspector中设置每个Toggle的行列
    public ToggleInfo[] toggleInfosKou; // 在Inspector中设置每个Toggle的行列

    public int fuIndex = 0;
    public int shouIndex = 0;
    public int kouIndex = 0;

    public GameObject tipUI;

    void UpdateImageDisplayFu(int row, int column, bool isOn)
    {
        foreach (var mapping in toggleInfosFu)
        {
            if (mapping.row == row && mapping.column == column)
            {
                mapping.targetSprite.SetActive(true);
                if (mapping.row == fuIndex)
                {
                    fuBl = true;
                }
                else
                {
                    fuBl = false;

                }
            }
            else
            {
                mapping.targetSprite.SetActive(false);

            }
        }
    }
    void UpdateImageDisplayShou(int row, int column, bool isOn)
    {
        foreach (var mapping in toggleInfosShou)
        {
            if (mapping.row == row && mapping.column == column)
            {
                mapping.targetSprite.SetActive(true);
                if (mapping.row == shouIndex)
                {
                    shouBl = true;
                }
                else
                {
                    shouBl = false;
                }
            }
            else
            {
                mapping.targetSprite.SetActive(false);

            }
        }
    }
    void UpdateImageDisplayKou(int row, int column, bool isOn)
    {
        foreach (var mapping in toggleInfosKou)
        {
            if (mapping.row == row && mapping.column == column)
            {
                mapping.targetSprite.SetActive(true);
                if (mapping.row == kouIndex)
                {
                    kouBl = true;
                }
                else
                {
                    kouBl = false;
                }
            }
            else
            {
                mapping.targetSprite.SetActive(false);

            }
        }
    }

    public Image fu1;
    public Image fu2;
    public Image shou1;
    public Image shou2;
    public Image kou1;
    public Image kou2;


    private void ClosePanel()
    {
        if (fuBl && shouBl && kouBl)
        {
            WearCon.Instance.ResetObj();
            //image.SetActive(false);
            GameManager.Instance.SetStepDetection(true);  //穿了衣服点击了退出更衣室 就传到下一个实验室Con 高亮对应实验室的门
            LabSystemManager.Instance.OnExitLockerClicked();
            UIManager.Instance.CloseUICaoZuo("GenyishiMan");
        }

        else
        {
            tipUI.SetActive(true);
            if (fuIndex == 1)
            {
                fu1.gameObject.SetActive(true);
                fu2.gameObject.SetActive(false);
            }
            else
            {
                fu2.gameObject.SetActive(true);
                fu1.gameObject.SetActive(false);

            }
            if (shouIndex == 1)
            {
                shou1.gameObject.SetActive(true);
                shou2.gameObject.SetActive(false);
            }
            else
            {
                shou2.gameObject.SetActive(true);
                shou1.gameObject.SetActive(false);
            }
            if (kouIndex == 1)
            {
                kou1.gameObject.SetActive(true);
                kou2.gameObject.SetActive(false);
            }
            else
            {
                kou2.gameObject.SetActive(true);
                kou1.gameObject.SetActive(false);
            }
            StartCoroutine(ShowUI());

        }

    }

    IEnumerator ShowUI()
    {
        yield return new WaitForSeconds(3);//点击后播放动画  1
        tipUI.SetActive(false);

    }


    //在更衣室中穿戴安全防护用品 点击后进行保存 穿戴了哪些
    private void UpdateButtonAppearance(bool isOn)
    {

    }

    //打开时 重新刷新数据
    private void OnEnable()
    {
        foreach (Toggle toggle in toggles)
        {
            toggle.isOn = false;

        }
        //displayImage.sprite = beginSprite;
        foreach (var mapping in toggleInfosFu)
        {
            mapping.targetSprite.SetActive(false);

            mapping.toggle.isOn = false;

        }
        foreach (var mapping in toggleInfosShou)
        {
            mapping.targetSprite.SetActive(false);

            mapping.toggle.isOn = false;

        }
        foreach (var mapping in toggleInfosKou)
        {
            mapping.targetSprite.SetActive(false);

            mapping.toggle.isOn = false;

        }
        GetLabIndex();

    }


}
