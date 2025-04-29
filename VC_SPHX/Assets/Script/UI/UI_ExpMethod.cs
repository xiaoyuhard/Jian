using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//选择实验方法
public class UI_ExpMethod : UIBase
{
    public Button btnClose;
    public GameObject winZhiFang;
    public GameObject winDanBaiZhi;

    public Button[] btn_ZhiFang;
    public Button[] btn_DanBaiZhi;

    private void OnEnable()
    {
        winZhiFang.SetActive(false);
        winDanBaiZhi.SetActive(false);

        var curExp = GameData.Instance.CurrentExperiment;
        if (curExp == Experiment.ZhiFang)
            winZhiFang.SetActive(true);
        else if (curExp == Experiment.DanBaiZhi)
            winDanBaiZhi.SetActive(true);

        MessageCenter.Instance.Register(EventName.UI_ShowPicture, ShowStepPic);
    }

    private void OnDisable()
    {
        MessageCenter.Instance.Unregister(EventName.UI_ShowPicture, ShowStepPic);
    }

    // Start is called before the first frame update
    void Start()
    {
        btnClose.onClick.AddListener(CloseUI);

        for (int i = 0; i < btn_ZhiFang.Length; i++)
        {
            var idx = i;

            btn_ZhiFang[i].onClick.AddListener(() =>
            {
                OnClickBtnZhifang(idx);
            });
        }

        for (int i = 0; i < btn_DanBaiZhi.Length; i++)
        {
            var idx = i;

            btn_DanBaiZhi[i].onClick.AddListener(() =>
            {
                OnClickBtnDanbaizhi(idx);
            });
        }
    }

    void ShowStepPic(string msg)
    {

    }

    //进入脂肪场景
    void OnClickBtnZhifang(int index)
    {
        print("OnClickBtnZhifang " + index);

        if (index == 0)
        {
            SceneMgr.LoadScene(GameScene.Exp6_ZhiFang1);
        }
        else if (index == 1)
        {
            SceneMgr.LoadScene(GameScene.Exp6_ZhiFang2);
        }
        else if (index == 2)
        {
            SceneMgr.LoadScene(GameScene.Exp6_ZhiFang3);
        }
    }

    //进入蛋白质场景
    void OnClickBtnDanbaizhi(int index)
    {
        print("OnClickBtnDanbaizhi " + index);

        if (index == 0)
        {
            SceneMgr.LoadScene(GameScene.Exp7_DanBaiZhi_ShouDong);
        }
        else if (index == 1)
        {
            SceneMgr.LoadScene(GameScene.Exp7_DanBaiZhi_ShouDong);
        }
    }


    void CloseUI()
    {
        UIManager.Instance.CloseUI(UINameType.UI_ExpMethod);
    }

}
