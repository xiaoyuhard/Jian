using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenLianUIMan : UICaoZuoBase
{
    public Button quXiaoBtn;
    public Button queRenBtn;
    // Start is called before the first frame update
    void Start()
    {
        quXiaoBtn.onClick.AddListener(QuXiao);
        queRenBtn.onClick.AddListener(QueRen);

    }

    //发送消息到提示选择模式面板CaozuoTipsPanel 调用进入考核模式 
    private void QueRen()
    {
        //MessageCenter.Instance.Send("OpenDoor", "WearPos");
        //CaozuoSceneCon.Instance.EnterLab(0);
        UIManager.Instance.CloseUICaoZuo(UINameType.UI_GenLianUIMan);
        LabSystemManager.Instance.SelectAssessmentMode();
    }

    //返回实验操作界面
    private void QuXiao()
    {
        UIManager.Instance.OpenUI(UINameType.UI_CaozuoManager);
        UIManager.Instance.OpenUI(UINameType.UI_BackMan);
        //GameManager.Instance.SetGameObj(false);
        UIManager.Instance.CloseAllUICaoZuo();
        //GameManager.Instance.SetGameObj(false);
        GameObjMan.Instance.CLoseFirst();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
