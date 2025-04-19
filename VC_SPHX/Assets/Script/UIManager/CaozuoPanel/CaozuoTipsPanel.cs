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

    //��ʾ������� һ���ǿ��� һ���Ǹ���

    // Start is called before the first frame update
    void Start()
    {
        btn1.onClick.AddListener(PassStalking);
        btn2.onClick.AddListener(PassExamine);
        //MessageCenter.Instance.Register("CaozuoName", GetCaozuoName);//Ӧ�ò���Ҫ
        //MessageCenter.Instance.Register("JoinKaoHe", JoinKaoHe);//Ӧ�ò���Ҫ

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
        //MessageCenter.Instance.Send("SendGengToDoorKaoHe", "WearPos");//Ӧ�ò���Ҫ
        //GameManager.Instance.SetTrainingMode(false);
        LabSystemManager.Instance.SelectAssessmentMode();  //���뿼��

        ClosePan();
    }

    void PassExamine()
    {
        //MessageCenter.Instance.Send("SendCaozuoToWear", joinIdtag);//Ӧ�ò���Ҫ
        //GameManager.Instance.SetTrainingMode(true);

        JoinKaoHe("");
        //MessageCenter.Instance.Send("SendGengToDoor", "WearPos");//Ӧ�ò���Ҫ
        LabSystemManager.Instance.SelectPracticeMode();//�������
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
