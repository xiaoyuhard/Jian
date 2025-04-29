using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class BodyConUI : UICaoZuoBase
{
    public GameObject man;
    public GameObject woman;


    public Button manBtn;
    public Button womanBtn;
    public List<Toggle> tglManList;
    public List<Toggle> tglWomanList;

    public GameObject explanationObj;
    public Text explanationText;

    public Button backBtn;
    private void OnEnable()
    {
        // 正确获取所有 Toggle 组件（包含子物体）
        Toggle[] togglesMan = man.GetComponentsInChildren<Toggle>(true); // true 包含未激活的物体

        // 将数组转换为列表
        tglManList = new List<Toggle>(togglesMan);
        // 正确获取所有 Toggle 组件（包含子物体）
        Toggle[] togglesWoman = woman.GetComponentsInChildren<Toggle>(true); // true 包含未激活的物体

        // 将数组转换为列表
        tglWomanList = new List<Toggle>(togglesWoman);
        foreach (Toggle toggle in tglManList)
        {
            toggle.onValueChanged.AddListener((isOn) =>
            {
                /* if (isOn) */
                OnClickShowManBody(toggle, isOn);

            });
            toggle.isOn = false;
        }
        foreach (Toggle toggle in tglWomanList)
        {
            toggle.onValueChanged.AddListener((isOn) =>
            {
                /*if (isOn)*/
                OnClickShowWomanBody(toggle, isOn);

            });
            toggle.isOn = false;

        }
        backBtn.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        manBtn.onClick.AddListener(EnterManPanel);
        womanBtn.onClick.AddListener(EnterWomanPanel);
        backBtn.onClick.AddListener(BackChoosePanel);
     
    }

    private void BackChoosePanel()
    {
        womanBtn.gameObject.SetActive(true);
        manBtn.gameObject.SetActive(true);
        backBtn.gameObject.SetActive(false);
        RenTiCon.Instance.ResetTimeline();
        man.SetActive(false);
        woman.SetActive(false);

    }

    private void OnClickShowWomanBody(Toggle toggle, bool isOn)
    {
        bool bl = false;
        if (isOn)
        {
            string bodyName = RenTiCon.Instance.ShowBodyWomanModel(toggle.transform.GetChild(1).GetComponent<Text>().text);
            if (GetBodyExplText("Man", bodyName) != "")
            {
                bl = true;
                explanationText.text = GetBodyExplText("Man", bodyName);

            }
            else
            {
                bl = false;
                explanationText.text = "";

            }
        }
        else
        {
            RenTiCon.Instance.ShowBodyWomanModel("");

        }
        if (bl && isOn)
        {
            explanationObj.SetActive(isOn);
        }
        else
        {
            explanationObj.SetActive(false);

        }

        StartCoroutine(RefreshLayout(toggle));

    }
    IEnumerator RefreshLayout(Toggle toggle)
    {
        yield return null; // 等待一帧
        if (toggle.transform.parent.name == "Content")
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(toggle.transform.parent as RectTransform);
        }
        if (toggle.transform.parent.parent.name == "Content")
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(toggle.transform.parent.parent as RectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(toggle.transform.parent as RectTransform);
        }
        if (toggle.transform.parent.parent.parent.name == "Content")
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(toggle.transform.parent.parent.parent as RectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(toggle.transform.parent.parent as RectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(toggle.transform.parent as RectTransform);

        }
        if (toggle.transform.parent.parent.parent.parent.name == "Content")
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(toggle.transform.parent.parent.parent.parent as RectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(toggle.transform.parent.parent.parent as RectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(toggle.transform.parent.parent as RectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(toggle.transform.parent as RectTransform);

        }

        Debug.Log(toggle.transform.parent + "sss");

    }
    private void OnClickShowManBody(Toggle toggle, bool isOn)
    {
        bool bl = false;
        if (isOn)
        {
            string bodyName = RenTiCon.Instance.ShowBodyManModel(toggle.transform.GetChild(1).GetComponent<Text>().text);
            if (GetBodyExplText("Man", bodyName) != "")
            {
                bl = true;
                explanationText.text = GetBodyExplText("Man", bodyName);

            }
            else
            {
                bl = false;
                explanationText.text = "";

            }
        }
        else
        {
             RenTiCon.Instance.ShowBodyManModel("");

        }
        if (bl && isOn)
        {
            explanationObj.SetActive(isOn);
        }
        else
        {
            explanationObj.SetActive(false);

        }

        StartCoroutine(RefreshLayout(toggle));

    }

    public string GetBodyExplText(string sex, string bodyName)
    {
        List<BodyItem> bodyItems = BodyManager.Instance.GetBodyExplItemById(sex);
        foreach (BodyItem item in bodyItems)
        {
            if (item.bodyId == bodyName)
            {
                return item.bodyExplanation;
            }
        }
        return "";
    }

    private void EnterWomanPanel()
    {
        womanBtn.gameObject.SetActive(false);
        manBtn.gameObject.SetActive(false);
        RenTiCon.Instance.ClickBtnShowBody(1);
        StartCoroutine(OpenWomanUIPanel());
        backBtn.gameObject.SetActive(true);

    }
    //打开女模型面板
    IEnumerator OpenWomanUIPanel()
    {
        yield return new WaitForSeconds(5);//播放动画  18
        woman.SetActive(true);

    }

    private void EnterManPanel()
    {
        womanBtn.gameObject.SetActive(false);

        manBtn.gameObject.SetActive(false);
        RenTiCon.Instance.ClickBtnShowBody(0);

        StartCoroutine(OpenManUIPanel());
        backBtn.gameObject.SetActive(true);

    }
    //打开男模型面板
    IEnumerator OpenManUIPanel()
    {
        yield return new WaitForSeconds(5);//播放动画  18
        man.SetActive(true);

    }


    // Update is called once per frame
    void Update()
    {
       

    }

    private void OnDisable()
    {
        man.SetActive(false);
        woman.SetActive(false);
        womanBtn.gameObject.SetActive(true);
        manBtn.gameObject.SetActive(true);
        explanationObj.SetActive(false);

    }
}
