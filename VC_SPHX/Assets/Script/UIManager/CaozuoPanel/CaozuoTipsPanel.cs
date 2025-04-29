using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//ѡ��ʵ��ģʽ���� ���� ����
public class CaozuoTipsPanel : MonoSingletonBase<CaozuoTipsPanel>
{
    public Button btnKaohe;
    public Button btnGensui;
    public Button btnClose;
    public GameObject obj;

    public string joinIdtag;

    //��ʾ������� һ���ǿ��� һ���Ǹ���

    // Start is called before the first frame update
    void Start()
    {
        btnKaohe.onClick.AddListener(PassStalking);
        btnGensui.onClick.AddListener(PassExamine);
        btnClose.onClick.AddListener(() =>
        {
            ClosePanel();
        });
        //MessageCenter.Instance.Register("CaozuoName", GetCaozuoName);//Ӧ�ò���Ҫ
        //MessageCenter.Instance.Register("JoinKaoHe", JoinKaoHe);//Ӧ�ò���Ҫ
        ClosePanel();
    }

    private void JoinKaoHe(string obj)
    {
        UIManager.Instance.CloseAllUI();
        GameManager.Instance.SetGameObj(true);
        GameObjMan.Instance.OpenFirst();
    }

    //����
    void PassStalking()
    {
        JoinKaoHe("");
        //MessageCenter.Instance.Send("SendGengToDoorKaoHe", "WearPos");//Ӧ�ò���Ҫ
        //GameManager.Instance.SetTrainingMode(false);

        ClosePanel();

        var curExp = (Experiment)CaozuoManager.ClickIndex;
        var curScene = SceneMgr.CurSceneName;

        GameData.Instance.IsTestMode = false;
        GameData.Instance.CurrentExperiment = curExp;

        if (curExp == Experiment.AnJiSuan || curExp == Experiment.XiangQi)
        {
            if (curScene != GameScene.Exp_HuaXue)
                SceneMgr.LoadScene(GameScene.Exp_HuaXue);
        }
        else if (curExp == Experiment.ZhongJinShu)
        {
            if (curScene != GameScene.Exp3_ZhongJinShu)
                SceneMgr.LoadScene(GameScene.Exp3_ZhongJinShu);
        }

        LabSystemManager.Instance.SelectAssessmentMode();  //���뿼��
    }

    //����
    void PassExamine()
    {
        //MessageCenter.Instance.Send("SendCaozuoToWear", joinIdtag);//Ӧ�ò���Ҫ
        //GameManager.Instance.SetTrainingMode(true);

        JoinKaoHe("");
        //MessageCenter.Instance.Send("SendGengToDoor", "WearPos");//Ӧ�ò���Ҫ
        //GameObjMan.Instance.SetPosition(0);
        ClosePanel();

        var curExp = (Experiment)CaozuoManager.ClickIndex;
        var curScene = SceneMgr.CurSceneName;

        GameData.Instance.IsTestMode = false;
        GameData.Instance.CurrentExperiment = curExp;

        if (curExp == Experiment.AnJiSuan || curExp == Experiment.XiangQi)
        {
            if (curScene != GameScene.Exp_HuaXue)
                SceneMgr.LoadScene(GameScene.Exp_HuaXue);
        }
        else if (curExp == Experiment.ZhongJinShu)
        {
            if (curScene != GameScene.Exp3_ZhongJinShu)
                SceneMgr.LoadScene(GameScene.Exp3_ZhongJinShu);
        }
        else if (curExp == Experiment.ZhiFang)
        {
            //if (curScene != GameScene.Exp3_ZhongJinShu)
            //    SceneMgr.LoadScene(GameScene.Exp3_ZhongJinShu);
            UIManager.Instance.OpenUI(UINameType.UI_ExpMethod);
        }
        else if (curExp == Experiment.DanBaiZhi)
        {
            //if (curScene != GameScene.Exp3_ZhongJinShu)
            //    SceneMgr.LoadScene(GameScene.Exp3_ZhongJinShu);
            UIManager.Instance.OpenUI(UINameType.UI_ExpMethod);
        }

        LabSystemManager.Instance.SelectPracticeMode();//�������
    }

    public void ClosePanel()
    {
        obj.SetActive(false);
    }

    //����ʵ��
    void EnterExperiment()
    {

    }
}
