using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//选择实验模式界面 考核 跟练
public class CaozuoTipsPanel : MonoSingletonBase<CaozuoTipsPanel>
{
    public Button btnKaohe;
    public Button btnGensui;
    public GameObject obj;

    public string joinIdtag;

    //提示操作面板 一个是考核 一个是跟练

    // Start is called before the first frame update
    void Start()
    {
        btnKaohe.onClick.AddListener(PassStalking);
        btnGensui.onClick.AddListener(PassExamine);
        //MessageCenter.Instance.Register("CaozuoName", GetCaozuoName);//应该不需要
        //MessageCenter.Instance.Register("JoinKaoHe", JoinKaoHe);//应该不需要
        ClosePanel();
    }

    private void JoinKaoHe(string obj)
    {
        //UIManager.Instance.CloseUI(UINameType.UI_ZhishiManager);
        //UIManager.Instance.CloseUI(UINameType.UI_MoxingManager);
        //UIManager.Instance.CloseUI(UINameType.UI_CaozuoManager);
        //UIManager.Instance.CloseUI(UINameType.UI_BaogaoManager);
        //UIManager.Instance.CloseUI(UINameType.UI_BackMan);
        UIManager.Instance.CloseAllUI();
        //UIManager.Instance.OpenUI(UINameType.UI_HomeManager);
        GameManager.Instance.SetGameObj(true);
        GameObjMan.Instance.OpenFirst();

    }

    //考核
    void PassStalking()
    {
        JoinKaoHe("");
        //MessageCenter.Instance.Send("SendGengToDoorKaoHe", "WearPos");//应该不需要
        //GameManager.Instance.SetTrainingMode(false);
        LabSystemManager.Instance.SelectAssessmentMode();  //进入考核

        ClosePanel();
        //SceneMgr.LoadScene(GameScene.Demo3);

    }

    //跟练
    void PassExamine()
    {
        //MessageCenter.Instance.Send("SendCaozuoToWear", joinIdtag);//应该不需要
        //GameManager.Instance.SetTrainingMode(true);

        JoinKaoHe("");
        //MessageCenter.Instance.Send("SendGengToDoor", "WearPos");//应该不需要
        LabSystemManager.Instance.SelectPracticeMode();//进入跟练
        //GameObjMan.Instance.SetPosition(0);
        ClosePanel();

        //SceneMgr.LoadScene(GameScene.Demo3);

    }

    public void ClosePanel()
    {
        obj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
