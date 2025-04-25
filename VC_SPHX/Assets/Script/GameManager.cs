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
        Debug.Log("·µ»Ø " + stepDetection);

        return stepDetection;
    }

    //Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(OnOpen);
        GameObjMan.Instance.CLoseFirst();
        UIManager.Instance.DonotCloseUI(UINameType.UI_HomeManager);
    }

    void OnOpen()
    {
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

