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
        MessageCenter.Instance.Register("SendMouseToAnjisuanChaoSheng", MouseToAnjisuanChaoSheng); //�����ᳬ��������

    }

    private void MouseToAnjisuanChaoSheng(string obj)
    {
        objUI.SetActive(true);
    }

 
    //ȷ�Ϻ� ���е�ɫ����
    private void TiShiClose()
    {
        UIManager.Instance.CloseUICaoZuo("WorkbenchUI");
        MessageCenter.Instance.Send("SendMouseToChromatograph", ""); //�����Ṥ��̨

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
