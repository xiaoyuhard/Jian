using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//选择实验模式界面 考核 跟练
public class CaozuoTipsPanel : MonoSingletonBase<CaozuoTipsPanel>
{
    public Button btnKaohe;
    public Button btnGensui;
    public Button btnClose;
    public GameObject obj;

    public string joinIdtag;

    //提示操作面板 一个是考核 一个是跟练

    // Start is called before the first frame update
    void Start()
    {
        btnKaohe.onClick.AddListener(PassStalking);
        btnGensui.onClick.AddListener(PassExamine);
        btnClose.onClick.AddListener(() =>
        {
            ClosePanel();
        });
        //MessageCenter.Instance.Register("CaozuoName", GetCaozuoName);//应该不需要
        //MessageCenter.Instance.Register("JoinKaoHe", JoinKaoHe);//应该不需要
        ClosePanel();
    }

    private void JoinKaoHe(string obj)
    {
        //UIManager.Instance.CloseAllUI();
        //GameManager.Instance.SetGameObj(true);
        GameObjMan.Instance.OpenFirst();
    }

    //考核
    void PassStalking()
    {
        JoinKaoHe("");
        //MessageCenter.Instance.Send("SendGengToDoorKaoHe", "WearPos");//应该不需要
        //GameManager.Instance.SetTrainingMode(false);

        ClosePanel();
        EnterExperiment(true);
        //LabSystemManager.Instance.SelectAssessmentMode();  //进入考核
    }

    //跟练
    void PassExamine()
    {
        //MessageCenter.Instance.Send("SendCaozuoToWear", joinIdtag);//应该不需要
        //GameManager.Instance.SetTrainingMode(true);

        JoinKaoHe("");
        //MessageCenter.Instance.Send("SendGengToDoor", "WearPos");//应该不需要
        //GameObjMan.Instance.SetPosition(0);
        ClosePanel();
        EnterExperiment(false);
        //LabSystemManager.Instance.SelectPracticeMode();//进入跟练
    }

    public void ClosePanel()
    {
        obj.SetActive(false);
    }

    //进入实验
    void EnterExperiment(bool isTestMode)
    {
        var curExp = (Experiment)CaozuoManager.ClickIndex;
        var curScene = SceneMgr.CurSceneName;

        GameData.Instance.IsTestMode = isTestMode;
        GameData.Instance.CurrentExperiment = curExp;

        if (curExp == Experiment.AnJiSuan)
        {
            SceneMgr.LoadScene(GameScene.Exp_HuaXue);
        }
        else if (curExp == Experiment.XiangQi)
        {
            SceneMgr.LoadScene(GameScene.Exp2_XiangQi);
        }
        else if (curExp == Experiment.ZhongJinShu)
        {
            SceneMgr.LoadScene(GameScene.Exp3_ZhongJinShu);
        }
        else if (curExp == Experiment.ShaChongJi)
        {
            UIManager.Instance.OpenUI(UINameType.UI_ExpMethod);
        }
        else if (curExp == Experiment.Tang)
        {
            SceneMgr.LoadScene(GameScene.Exp5_Tang);
        }
        else if (curExp == Experiment.ZhiFang)
        {
            UIManager.Instance.OpenUI(UINameType.UI_ExpMethod);
        }
        else if (curExp == Experiment.DanBaiZhi)
        {
            UIManager.Instance.OpenUI(UINameType.UI_ExpMethod);
        }
    }
}
