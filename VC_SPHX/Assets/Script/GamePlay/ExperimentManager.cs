using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//重金属测定
public class ExperimentManager : MonoBehaviour
{
    public Button btnStart;
    //动画文件
    public Transform directorParent;
    //触发动画的物体，高亮
    //public GameObject[] triggerObjs;
    public ExpStepCtrl stepCtrl;
    public TextAsset csvFile;

    //实验进行到哪一个步骤
    int stepIndex = 0;
    bool isPlayingAnim = false;

    private void Awake()
    {
        MessageCenter.Instance.Register(EventName.Exp_NextStep, NextStep);
    }

    private void OnDestroy()
    {
        MessageCenter.Instance.Unregister(EventName.Exp_NextStep, NextStep);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartExperiment();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartExperiment()
    {
        UIManager.Instance.DonotCloseUI(UINameType.UI_HomeManager);
        UIManager.Instance.CloseWholeUI();
        UIManager.Instance.OpenUI(UINameType.UI_HomeManager);

        if (GameData.Instance.IsTestMode)
            UIManager.Instance.CloseUICaoZuo(UINameType.UI_ProTipsMan);
        else
            UIManager.Instance.OpenUICaoZuo(UINameType.UI_ProTipsMan);

        btnStart.gameObject.SetActive(false);
        GameObjMan.Instance.OpenFirst();

        InitAnimation();

        //读取提示配置，显示提示
        //MessageCenter.Instance.Send("SendTiShiUIName", "重金属提示");
        StalkProcedureManager.Instance.TiShiUIName(csvFile.name);

        // 高亮更衣室门
        //DoorClickCon.Instance.SetHighlight(0);
        stepCtrl.StartExp();
        ExpObjPicker.Instance.OnHitObj = OnHitObj;
    }

    void OnHitObj(RaycastHit hit)
    {
        print("OnHitObj: " + hit.transform);

        //directors[0].gameObject.SetActive(true);
        //directors[0].Play();
        //print(directors[0].duration);
        //isPlayingAnim = true;
    }

    //隐藏动画，避免自动播放
    void InitAnimation()
    {
        for (int i = 0; i < directorParent.childCount; i++)
        {
            directorParent.GetChild(i).gameObject.SetActive(false);
        }
    }

    void NextStep(string arg)
    {
        print("===========开始进行下一步");
        //print("cur stepIndex: " + stepIndex);

        //if (stepIndex == 0)
        //{
        //    //打开 重金属门
        //    DoorClickCon.Instance.SetHighlight(3);
        //    //打开 重金属实验 触发器
        //    GameObjMan.Instance.OpenObjCon(4);
        //}
        //else if (stepIndex == 1)
        //{
        //    //高亮点击物体
        //    triggerObjs[0].GetComponent<Outline>().enabled = true;
        //}

        //stepIndex++;
        //StalkProcedureManager.Instance.UpdateUIInf(stepIndex);

        stepCtrl.NextStep();
    }

}