using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AnjisuanCon : MonoSingletonBase<AnjisuanCon>
{
    public GameObject anObj;
    public GameObject workbench;
    public List<GameObject> modelList;
    public List<GameObject> listObj;
    public List<GameObject> timeList;

    public GameObject timelineObj;

    //int index = 0;

    //public void JoinAnjisuan()
    //{

    //    anObj.SetActive(true);
    //    //瓶子高亮
    //    LabSystemManager.Instance.HighlightObject(listObj[0]);
    //}

    private void Start()
    {

        StartCoroutine(ConUp());

        MessageCenter.Instance.Register("SendMouseToAnjisuanPing", MouseAnjisuanPing); //打开氨基酸UI面板

        MessageCenter.Instance.Register("SendMouseToChromatograph", MouseChromatograph); //氨基酸工作台
        MessageCenter.Instance.Register("SendMouseToDaoChu", MouseDaoChu); //氨基酸工作台
        MessageCenter.Instance.Register("SendPreToWorkbench", PreToWorkbench); //氨基酸工作台高亮
        MessageCenter.Instance.Register("SendExamineToAnjisuan", ExamineToAnjisuan); //氨基酸PreparationUI打开
        MessageCenter.Instance.Register("SendMouseToWorkbench", MouseWorkbench); //氨基酸工作台
        MessageCenter.Instance.Register("SendMouseToAnjisuanComputer", MouseToAnjisuanComputer); //氨基酸电脑

        //MessageCenter.Instance.Register("SendDianNaoToAn", DianNaoToAn); //氨基酸工作台

        //MessageCenter.Instance.Register("SendWearToAn", WearToAnTag);


    }
    private void MouseToAnjisuanComputer(string obj)
    {
        UIManager.Instance.OpenUICaoZuo("ChromatographUI");

    }
    private void MouseWorkbench(string obj)
    {
        UIManager.Instance.OpenUICaoZuo("WorkbenchUI");
        LabSystemManager.Instance.HighlightObject(listObj[4]);

    }
    private void ExamineToAnjisuan(string obj)
    {
        UIManager.Instance.OpenUICaoZuo("PreparationUI");
        LabSystemManager.Instance.HighlightObject(listObj[1]);

    }

    private void DianNaoToAn(string obj)
    {


    }


    private void PreToWorkbench(string obj)
    {
        //workbench.GetComponent<Outline>().enabled = true;
        //LabSystemManager.Instance.HighlightObject(workbench);

    }

    ////接收到操作点的是氨基酸 进入后进行存储 跟练结束后如果点击考核 进行发送 高亮氨基酸的门
    //private void WearToAnTag(string obj)
    //{
    //    MessageCenter.Instance.Send("CaozuoName", obj);

    //}
    private void MouseDaoChu(string obj)
    {
        UIManager.Instance.OpenUICaoZuo("GenLianUIMan");

    }

    private void MouseChromatograph(string obj)
    {
        //UIManager.Instance.OpenUICaoZuo("ChromatographUI");
        LabSystemManager.Instance.HighlightObject(listObj[5]);

    }



    private void MouseAnjisuanPing(string obj)
    {
        UIManager.Instance.OpenUICaoZuo("AnjisuanExamineUI");

    }



    IEnumerator ConUp()
    {
        //while (true)
        //{
        // 等待条件满足后继续
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 更衣室点了柜子选完衣服

        LabSystemManager.Instance.OnExitLockerClicked();
        IsAssessmentMode(1); //显示提示操作
        //LabSystemManager.Instance.HighlightObject(listObj[8]);

        //for (int i = 0; i < 10; i++)
        //{
        //    IsAssessmentMode(i + 1); //显示提示操作
        //    yield return new WaitForSeconds(3f);
        //}

        GameManager.Instance.SetStepDetection(false);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 碰到氨基酸实验室门 AccQ-TagUltra衍生试剂配制 高亮对应模型 并显示提示
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(2); //显示提示操作
        ShowObj(0);//按顺序显示 第1个动画
        //LabSystemManager.Instance.HighlightObject(listObj[8]);

        LabSystemManager.Instance.HighlightObject(listObj[0]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 把高亮的物体点击 点了后 空白样品的制备 高亮 换提示
        Debug.Log($"等待时间: {OpenTimePlay(0)}"); // 查看控制台输出
        yield return new WaitForSeconds(OpenTimePlay(0));//点击后播放动画  1
        //index++;//1
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(3);
        ShowObj(1);//按顺序显示 第2个动画
        LabSystemManager.Instance.HighlightObject(listObj[1]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 氨基酸标准品的制备

        yield return new WaitForSeconds(OpenTimePlay(1));//播放动画  2
        //index++;//2
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(4);
        ShowObj(2);//按顺序显示 第三个动画
        LabSystemManager.Instance.HighlightObject(listObj[2]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 标准品的衍生加溶剂

        yield return new WaitForSeconds(OpenTimePlay(2));//播放动画  3
        //index++;//3
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(5);
        ShowObj(3);//按顺序显示 第4个动画
        LabSystemManager.Instance.HighlightObject(listObj[3]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 标准品的衍生水浴加热

        yield return new WaitForSeconds(OpenTimePlay(3));//播放动画  4
        //index++;//4
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(6);
        ShowObj(4);//按顺序显示 第5个动画
        LabSystemManager.Instance.HighlightObject(listObj[4]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 试样制备

        yield return new WaitForSeconds(OpenTimePlay(4));//播放动画  5
        //index++;//5
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(7);
        ShowObj(5);//按顺序显示 第6个动画
        LabSystemManager.Instance.HighlightObject(listObj[5]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 试剂称量

        yield return new WaitForSeconds(OpenTimePlay(5));//播放动画  6
        //index++;//6
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(8);
        ShowObj(6);//按顺序显示 第7个动画
        LabSystemManager.Instance.HighlightObject(listObj[5]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 水解管加盐酸和苯酚

        yield return new WaitForSeconds(OpenTimePlay(6));//播放动画  7
        //index++;//7
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(9);
        ShowObj(7);//按顺序显示 第8个动画
        LabSystemManager.Instance.HighlightObject(listObj[6]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 水解管冷冻抽气

        yield return new WaitForSeconds(OpenTimePlay(7));//播放动画  8
        //index++;//8
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(10);
        ShowObj(8);//按顺序显示 第9个动画
        LabSystemManager.Instance.HighlightObject(listObj[7]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 水解管恒温箱加热

        yield return new WaitForSeconds(OpenTimePlay(7));//播放动画  9
        //index++;//9
        DoorClickCon.Instance.SetHighlight(11);

        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(11);
        ShowObj(9);//按顺序显示 第10个动画
        LabSystemManager.Instance.HighlightObject(listObj[8]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 水解液过滤定容

        yield return new WaitForSeconds(OpenTimePlay(9));//播放动画  10
        //index++;//10
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(12);
        ShowObj(10);//按顺序显示 第11个动画
        LabSystemManager.Instance.HighlightObject(listObj[9]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 水解液过滤转移

        yield return new WaitForSeconds(OpenTimePlay(10));//播放动画  11
        //index++;//11
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(13);
        ShowObj(11);//按顺序显示 第12个动画
        LabSystemManager.Instance.HighlightObject(listObj[10]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 试管浓缩仪加压

        yield return new WaitForSeconds(OpenTimePlay(11));//播放动画  12
        //index++;//12
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(14);
        ShowObj(12);//按顺序显示 第13个动画
        LabSystemManager.Instance.HighlightObject(listObj[11]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 柠檬酸钠溶解滤膜过滤

        yield return new WaitForSeconds(OpenTimePlay(12));//播放动画  13
        //index++;//13
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(15);
        ShowObj(13);//按顺序显示 第14个动画
        LabSystemManager.Instance.HighlightObject(listObj[12]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 样品的衍生添加试剂

        yield return new WaitForSeconds(OpenTimePlay(13));//播放动画  14
        //index++;//14
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(16);
        ShowObj(14);//按顺序显示 第15个动画
        LabSystemManager.Instance.HighlightObject(listObj[13]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 样品的衍生水浴加热

        yield return new WaitForSeconds(OpenTimePlay(14));//播放动画  15
        //index++;//15
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(17);
        ShowObj(15);//按顺序显示 第16个动画
        LabSystemManager.Instance.HighlightObject(listObj[14]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 样品溶液进瓶

        yield return new WaitForSeconds(OpenTimePlay(15));//播放动画  16
        GameManager.Instance.SetStepDetection(false);
        //index++;//16
        IsAssessmentMode(18);
        ShowObj(16);//按顺序显示 第17个动画
        LabSystemManager.Instance.HighlightObject(listObj[15]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 样品溶液进瓶

        yield return new WaitForSeconds(OpenTimePlay(16));//播放动画  17

        GameManager.Instance.SetStepDetection(false);
        //index++;//17
        IsAssessmentMode(19);
        ShowObj(17);//按顺序显示 第18个动画

        LabSystemManager.Instance.HighlightObject(listObj[16]);

        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 脱气模块高亮
        GameManager.Instance.SetStepDetection(false);

        LabSystemManager.Instance.HighlightObject(listObj[17]);

        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 泵模块高亮
        LabSystemManager.Instance.HighlightObject(listObj[18]);
        GameManager.Instance.SetStepDetection(false);
        //index++;//18

        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 自动进样模块高亮
        LabSystemManager.Instance.HighlightObject(listObj[19]);
        GameManager.Instance.SetStepDetection(false);
        //index++;//19

        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 柱温箱模块高亮
        LabSystemManager.Instance.HighlightObject(listObj[20]);
        GameManager.Instance.SetStepDetection(false);
        //index++;//20

        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 检测器模块高亮

        LabSystemManager.Instance.HighlightObject(listObj[22]);
        GameManager.Instance.SetStepDetection(false);
        //index++;//21
        IsAssessmentMode(20);
        ShowObj(18);//按顺序显示 第19个动画

        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 电脑开关
        yield return new WaitForSeconds(OpenTimePlay(18));//播放动画  19

        LabSystemManager.Instance.HighlightObject(listObj[24]);
        GameManager.Instance.SetStepDetection(false);
        //index++;//20

        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 点击电脑上图标软件后

        UIManager.Instance.OpenUICaoZuo("ComputerUI");
        ComputerUI.Instance.ShowComUI(0);
        GameManager.Instance.SetStepDetection(false);
        yield return new WaitForSeconds(3);//提示上面排气阀高亮
        LabSystemManager.Instance.HighlightObject(listObj[21]);
        ComputerUI.Instance.CloseUI();

        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 电脑屏幕显示UI 然后排气阀高亮

        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(21);
        yield return new WaitForSeconds(OpenTimePlay(17));//播放动画  18

        ComputerUI.Instance.ShowComUI(1);
        GameManager.Instance.SetStepDetection(false);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 点了排气阀 进行放动画 出UI进行操作 

        ComputerUI.Instance.ShowComUI(2);
        GameManager.Instance.SetStepDetection(false);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 点了排气阀 进行放动画 出UI进行操作 

        ComputerUI.Instance.ShowComUI(3);
        GameManager.Instance.SetStepDetection(false);
        yield return new WaitForSeconds(3);//提示上面排气阀高亮
        LabSystemManager.Instance.HighlightObject(listObj[21]);
        ComputerUI.Instance.CloseUI();

        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // 当 conditionMet == true 时继续 电脑屏幕显示UI 然后排气阀高亮




        UIManager.Instance.OpenUICaoZuo("ChromatographUI");

        //}
    }

    public void IsAssessmentMode(int index)
    {
        if (GameManager.Instance.tipStalkBl)
        {
            StalkProcedureManager.Instance.UpdateUIInf(index);
        }
    }


    public float OpenTimePlay(int index)
    {
        timeList[index].GetComponent<PlayableDirector>().Play();

        return (float)timeList[index].GetComponent<PlayableDirector>().duration;
    }

    // Update is called once per frame
    void Update()
    {

    }

    bool isTriOnce = true;
    private void OnTriggerEnter(Collider other)
    {
        if (!isTriOnce) return;
        //if (other.tag != "Player") return;
        //LabSystemManager.Instance.HighlightObject(anObj);
        //anObj.SetActive(true);
        LabSystemManager.Instance.HighlightObject(listObj[0]);
        IsAssessmentMode(2);
        GameManager.Instance.SetStepDetection(true);
        isTriOnce = false;
    }
    public void ShowObj(int index)
    {
        foreach (var item in modelList)
        {
            item.SetActive(false);
        }
        foreach (var item in timeList)
        {
            item.SetActive(false);
            //item.GetComponent<PlayableDirector>().enabled = false;
            item.GetComponent<PlayableDirector>().time = 0;
            item.GetComponent<PlayableDirector>().Stop();
            //item.GetComponent<PlayableDirector>().Evaluate(); // 强制应用第0帧状态

        }
        timeList[index].SetActive(true);
        timeList[index].GetComponent<PlayableDirector>().time = 5f;
        timeList[index].GetComponent<PlayableDirector>().Stop();
        timeList[index].GetComponent<PlayableDirector>().Evaluate(); // 强制应用第0帧状态
    }



    private void OnEnable()
    {
        foreach (var item in listObj)
        {
            item.GetComponent<InteractableObject>().enabled = false;

        }
        foreach (var item in timeList)
        {
            //item.GetComponent<PlayableDirector>().enabled = false;
            item.GetComponent<PlayableDirector>().time = 0;
            item.GetComponent<PlayableDirector>().Stop();
            //item.GetComponent<PlayableDirector>().Evaluate(); // 强制应用第0帧状态
            item.SetActive(false);
        }
        foreach (var item in modelList)
        {
            item.SetActive(false);
        }
        timelineObj.SetActive(true);
    }
}
