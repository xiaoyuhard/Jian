using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//选择实验方法
public class UI_ExpMethod : UIBase
{
    public Button btnClose;

    public GameObject windShaChongJi;
    public GameObject winZhiFang;
    public GameObject winDanBaiZhi;

    public Button[] btn_ShaChongJi;
    public Button[] btn_ZhiFang;
    public Button[] btn_DanBaiZhi;

    private void OnEnable()
    {
        winZhiFang.SetActive(false);
        winDanBaiZhi.SetActive(false);
        windShaChongJi.SetActive(false);

        var curExp = GameData.Instance.CurrentExperiment;
     
        if (curExp == Experiment.ZhiFang)
            winZhiFang.SetActive(true);
        else if (curExp == Experiment.DanBaiZhi)
            winDanBaiZhi.SetActive(true);
        else if (curExp == Experiment.ShaChongJi)
            windShaChongJi.SetActive(true);
    }

    private void OnDisable()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        btnClose.onClick.AddListener(CloseUI);

        for (int i = 0; i < btn_ShaChongJi.Length; i++)
        {
            var idx = i;

            btn_ShaChongJi[i].onClick.AddListener(() =>
            {
                OnClickBtnShaChongJi(idx);
            });
        }

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

    //进入杀虫剂场景
    void OnClickBtnShaChongJi(int index)
    {
        print("OnClickBtnShaChongJi " + index);

        GameData.Instance.CurExpSubIndex = index;

        if (index == 0)
        {
            SceneMgr.LoadScene(GameScene.Exp4_ShaChongJi_GuoShu);
        }
        else if (index == 1)
        {
            SceneMgr.LoadScene(GameScene.Exp4_ShaChongJi_XiangLiao);
        }
        else if (index == 2)
        {
            SceneMgr.LoadScene(GameScene.Exp4_ShaChongJi_DongWu);
        }
        else if (index == 3)
        {
            SceneMgr.LoadScene(GameScene.Exp4_ShaChongJi_YeTai);
        }
    }

    //进入脂肪场景
    void OnClickBtnZhifang(int index)
    {
        print("OnClickBtnZhifang " + index);

        GameData.Instance.CurExpSubIndex = index;
        if (index == 0)
        {
            SceneMgr.LoadScene(GameScene.Exp6_ZhiFang_ChuanTong);
        }
        else if (index == 1)
        {
            SceneMgr.LoadScene(GameScene.Exp6_ZhiFang_ShiYanShi);
        }
        else if (index == 2)
        {
            SceneMgr.LoadScene(GameScene.Exp6_ZhiFang_GaiBo);
        }
    }

    //进入蛋白质场景
    void OnClickBtnDanbaizhi(int index)
    {
        print("OnClickBtnDanbaizhi " + index);

        GameData.Instance.CurExpSubIndex = index;
        if (index == 0)
        {
            SceneMgr.LoadScene(GameScene.Exp7_DanBaiZhi_ShouDong);
        }
        else if (index == 1)
        {
            SceneMgr.LoadScene(GameScene.Exp7_DanBaiZhi_ZiDong);
        }
    }


    void CloseUI()
    {
        UIManager.Instance.CloseUI(UINameType.UI_ExpMethod);
    }

}
