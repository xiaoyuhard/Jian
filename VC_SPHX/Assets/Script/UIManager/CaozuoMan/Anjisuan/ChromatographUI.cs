using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChromatographUI : UICaoZuoBase
{
    public GameObject tiShiObj;
    public Button quXiaoBtn;
    public Button queRenBtn;

    public GameObject daoChu;
    public Button daoChuBtn;


    // Start is called before the first frame update
    void Start()
    {
        quXiaoBtn.onClick.AddListener(QuXiao);
        queRenBtn.onClick.AddListener(QueRen);
        daoChuBtn.onClick.AddListener(DaoChu);


    }

    private void DaoChu()
    {
        MessageCenter.Instance.Send("SendMouseToDaoChu", ""); //�����Ṥ��̨

    }

    //���в��Ŷ����Ȳ���
    private void QueRen()
    {
        tiShiObj.SetActive(false);

        MessageCenter.Instance.Send("SendDianNaoToAn", ""); //�����Ṥ��̨
        LabSystemManager.Instance.SelectAssessmentMode();  //���뿼��
        UIManager.Instance.CloseAllUICaoZuo();


    }

    private void QuXiao()
    {
        tiShiObj.SetActive(false);
        UIManager.Instance.CloseAllUICaoZuo();
        MessageCenter.Instance.Send("SendHomeReset", ""); //�����Ṥ��̨

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDisable()
    {
        tiShiObj.SetActive(false);
        daoChu.SetActive(false);
    }
}
