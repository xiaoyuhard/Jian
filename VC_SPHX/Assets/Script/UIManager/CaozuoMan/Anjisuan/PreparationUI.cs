using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreparationUI : UICaoZuoBase
{
    public Button zhuangBtn;
    public GameObject ui;

    public GameObject zhen;
    public GameObject BiaoZhun;

    // Start is called before the first frame update
    void Start()
    {
        zhuangBtn.onClick.AddListener(ZhuangYang);
        MessageCenter.Instance.Register("SendMouseToAnjisuanBiaoZhun", MouseAnjisuanYangpin); //打开氨基酸标准样品的制备UI面板
        MessageCenter.Instance.Register("SendMouseToAnjisuanZhen", MouseToAnjisuanZhen); //氨基酸针
    }

    //点击确认 进行下一步
    private void ZhuangYang()
    {
        UIManager.Instance.CloseUICaoZuo("PreparationUI");
        MessageCenter.Instance.Send("SendPreToWorkbench", ""); //氨基酸工作台高亮

    }

    private void MouseToAnjisuanZhen(string obj)
    {
        LabSystemManager.Instance.HighlightObject(BiaoZhun);

    }


    private void MouseAnjisuanYangpin(string obj)
    {
        ui.SetActive(true);
    }


    public void SetUI(bool isBl)
    {
        ui.SetActive(isBl);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDisable()
    {
        ui.SetActive(false);
    }
}
