using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaozuoTipsPanel : MonoSingletonBase<CaozuoTipsPanel>
{
    public Button btn1;
    public Button btn2;
    public GameObject obj;

    public string joinIdtag;

    //提示操作面板 一个是考核 一个是跟练

    // Start is called before the first frame update
    void Start()
    {
        btn1.onClick.AddListener(PassStalking);
        btn2.onClick.AddListener(PassExamine);
        //MessageCenter.Instance.Register("CaozuoName", GetCaozuoName);//应该不需要
        //MessageCenter.Instance.Register("JoinKaoHe", JoinKaoHe);//应该不需要

    }

    private void JoinKaoHe(string obj)
    {
        UIManager.Instance.CloseUI("ZhishiManager");
        UIManager.Instance.CloseUI("MoxingManager");
        UIManager.Instance.CloseUI("CaozuoManager");
        UIManager.Instance.CloseUI("BaogaoManager");
        UIManager.Instance.CloseUI("BackMan");
        GameManager.Instance.SetGameObj(true);
        GameObjMan.Instance.OpenFirst();

    }


    void PassStalking()
    {
        JoinKaoHe("");
        //MessageCenter.Instance.Send("SendGengToDoorKaoHe", "WearPos");//应该不需要
        //GameManager.Instance.SetTrainingMode(false);
        LabSystemManager.Instance.SelectAssessmentMode();  //进入考核

        ClosePan();
    }

    void PassExamine()
    {
        //MessageCenter.Instance.Send("SendCaozuoToWear", joinIdtag);//应该不需要
        //GameManager.Instance.SetTrainingMode(true);

        JoinKaoHe("");
        //MessageCenter.Instance.Send("SendGengToDoor", "WearPos");//应该不需要
        LabSystemManager.Instance.SelectPracticeMode();//进入跟练
        //GameObjMan.Instance.SetPosition(0);
        ClosePan();
    }

    public void ClosePan()
    {
        obj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
