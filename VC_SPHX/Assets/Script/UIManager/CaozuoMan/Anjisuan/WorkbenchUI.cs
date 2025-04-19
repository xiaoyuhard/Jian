using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkbenchUI : UICaoZuoBase
{
    public Button verifyBtn;
    public GameObject objUI;
    // Start is called before the first frame update
    void Start()
    {
        verifyBtn.onClick.AddListener(TiShiClose);
        MessageCenter.Instance.Register("SendMouseToAnjisuanChaoSheng", MouseToAnjisuanChaoSheng); //氨基酸超声脱气机

    }

    private void MouseToAnjisuanChaoSheng(string obj)
    {
        objUI.SetActive(true);
    }

 
    //确认后 进行到色谱仪
    private void TiShiClose()
    {
        UIManager.Instance.CloseUICaoZuo("WorkbenchUI");
        MessageCenter.Instance.Send("SendMouseToChromatograph", ""); //氨基酸工作台

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnDisable()
    {
        objUI.SetActive(false);

    }
}
