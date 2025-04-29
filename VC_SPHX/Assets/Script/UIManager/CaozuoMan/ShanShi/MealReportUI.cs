using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 膳食报告界面 控制可以重新选择哪餐
/// </summary>
public class MealReportUI : UICaoZuoBase
{
    public Button reselectBtn1;
    public Button reselectBtn2;
    public Button reselectBtn3;


    // Start is called before the first frame update
    void Start()
    {
        reselectBtn1.onClick.AddListener(ReselectClick1);
        reselectBtn2.onClick.AddListener(ReselectClick2);
        reselectBtn3.onClick.AddListener(ReselectClick3);


    }

    private void ReselectClick3()
    {
        ChooseFoodAllInformCon.Instance.DeleteFoodKind("Wan");
        UIManager.Instance.CloseUICaoZuo("MealReportUI");
        GameObjMan.Instance.OpenFirst();

    }

    private void ReselectClick2()
    {
        ChooseFoodAllInformCon.Instance.DeleteFoodKind("Zhong");
        UIManager.Instance.CloseUICaoZuo("MealReportUI");
        GameObjMan.Instance.OpenFirst();

    }

    private void ReselectClick1()
    {
        ChooseFoodAllInformCon.Instance.DeleteFoodKind("Zao");
        UIManager.Instance.CloseUICaoZuo("MealReportUI");
        GameObjMan.Instance.OpenFirst();

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
