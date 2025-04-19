using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyConUI : UICaoZuoBase
{
    public GameObject man;
    public GameObject woman;
    

    public Button manBtn;
    public Button womanBtn;
    public List<Toggle> tglManList;
    public List<Toggle> tglWomanList;
    // Start is called before the first frame update
    void Start()
    {
        manBtn.onClick.AddListener(EnterManPanel);
        womanBtn.onClick.AddListener(EnterWomanPanel);
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
               /* if (isOn) */OnClickShowManBody(toggle, isOn);

            });
        }
        foreach (Toggle toggle in tglWomanList)
        {
            toggle.onValueChanged.AddListener((isOn) =>
            {
                /*if (isOn)*/ OnClickShowWomanBody(toggle, isOn);

            });
        }
    }

    private void OnClickShowWomanBody(Toggle toggle, bool isOn)
    {
        if (isOn)
        {
            //RenTiCon.Instance.ShowBodyWomanModel(toggle.transform.GetChild(1).GetComponent<Text>().text);
            Debug.Log(toggle.transform.parent + "ddsss");

        }
        //LayoutRebuilder.ForceRebuildLayoutImmediate(toggle.transform.parent as RectTransform);
        StartCoroutine(RefreshLayout(toggle));
    }
    IEnumerator RefreshLayout(Toggle toggle)
    {
        yield return null; // 等待一帧
        if(toggle.transform.parent.name== "Content")
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

        Debug.Log(toggle.transform.parent+"sss");

    }
    private void OnClickShowManBody(Toggle toggle, bool isOn)
    {
        if (isOn)
        {
            //RenTiCon.Instance.ShowBodyManModel(toggle.transform.GetChild(1).GetComponent<Text>().text);
            Debug.Log(toggle.transform.parent + "aasss");

        }
        StartCoroutine(RefreshLayout(toggle));

    }

    private void EnterWomanPanel()
    {
        woman.SetActive(true);
        womanBtn.gameObject.SetActive(false);
        
    }

    private void EnterManPanel()
    {
        man.SetActive(true);
        manBtn.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnDisable()
    {
        //man.SetActive(true);
        //woman.SetActive(true);
        womanBtn.gameObject.SetActive(true);
        manBtn.gameObject.SetActive(true);

    }
}
