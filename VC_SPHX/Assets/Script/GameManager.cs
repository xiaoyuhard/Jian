using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoSingletonBase<GameManager>
{
    public Button btn;
    public GameObject gameObj;
    public bool stepDetection = false;
    public bool tipStalkBl = false;

    public void SetStepDetection(bool stepBl)
    {
        Debug.Log("gai wei "+ stepBl);
        stepDetection = stepBl;
    }

    public bool BackStepDetection()
    {
        Debug.Log("返回 " + stepDetection);

        return stepDetection;
    }

    //Start is called before the first frame update
    void Start()
    {
        if (SceneMgr.CurSceneName == GameScene.Exp_HuaXue)
        {
            //GameObjMan.Instance.CLoseFirst();
            UIManager.Instance.DonotCloseUI(UINameType.UI_HomeManager);

            //btn.gameObject.SetActive(true);
            //btn.onClick.AddListener(OnOpen);
            OnOpen();

            //LabSystemManager.Instance.OnLabButtonClicked(1, "1氨基酸");
            ////LabSystemManager.Instance.OnLabButtonClicked(8, "膳食提示");
            //LabSystemManager.Instance.SelectPracticeMode();//进入跟练
            //GameObjMan.Instance.OpenFirst();
        }
    }

    void OnOpen()
    {
        UIManager.Instance.CloseWholeUI();
        UIManager.Instance.OpenUI(UINameType.UI_BackMan);
        UIManager.Instance.OpenUI(UINameType.UI_HomeManager);
        btn.gameObject.SetActive(false);
    }

    public void SetGameObj(bool active)
    {
        //gameObj.SetActive(active);
    }

    public void CloseAllCon()
    {
        LabSystemManager.Instance.HighlightObject(null);
        //CaozuoSceneCon.Instance.CloseObj();
    }

    // Update is called once per frame
    void Update()
    {
    }

}

