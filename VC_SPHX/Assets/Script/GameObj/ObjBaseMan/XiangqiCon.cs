using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XiangqiCon : MonoSingletonBase<XiangqiCon>
{
    public GameObject qiTiShi;
    public GameObject XiangQiObj;
    public List<GameObject> listObj;
 
    int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ConUp());

        MessageCenter.Instance.Register("SendMouseToXiangQiGui", MouseToXiangQiGui); //香气柜
        MessageCenter.Instance.Register("SendMouseToXiangQiNiu", MouseToXiangQiNiu); //香气扭
        MessageCenter.Instance.Register("SendMouseToXiangQiFa", MouseToXiangQiFa); //香气阀
        MessageCenter.Instance.Register("SendMouseToXiangQiRongye", MouseToXiangQiRongye); //香气溶液
        MessageCenter.Instance.Register("SendMouseToXiangQiCuiquyi", MouseToXiangQiCuiquyi); //香气质谱仪

    }

    private void MouseToXiangQiCuiquyi(string obj)
    {
        UIManager.Instance.OpenUICaoZuo("XiangqiCuiquyiTipUI");

    }

    private void MouseToXiangQiRongye(string obj)
    {
        UIManager.Instance.OpenUICaoZuo("XiangqiRongyeTipUI");

    }

    private void MouseToXiangQiFa(string obj)
    {
        UIManager.Instance.OpenUICaoZuo("XiangqiExamineUI");
    }

    private void MouseToXiangQiNiu(string obj)
    {
        LabSystemManager.Instance.HighlightObject(listObj[index]);
        index++;

    }

    private void MouseToXiangQiGui(string obj)
    {
        LabSystemManager.Instance.HighlightObject(listObj[index]);
        index++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator ConUp()
    {
        // 等待条件满足后继续
        yield return new WaitUntil(() => GameManager.Instance.stepDetection); // 当 conditionMet == true 时继续 进气瓶室
        LabSystemManager.Instance.HighlightObject(listObj[index]);
        index++;
        GameManager.Instance.stepDetection = false;
        yield return new WaitUntil(() => GameManager.Instance.stepDetection); // 当 conditionMet == true 时继续 到香气的实验室
        DoorClickCon.Instance.OpenDoorHigh(2);
        GameManager.Instance.stepDetection = false;
        yield return new WaitUntil(() => GameManager.Instance.stepDetection); // 当 conditionMet == true 时继续 选完药品开始实验
        LabSystemManager.Instance.HighlightObject(listObj[index]);
        index++;
        GameManager.Instance.stepDetection = false;
        yield return new WaitUntil(() => GameManager.Instance.stepDetection); // 当 conditionMet == true 时继续 溶液点击后出面板确认
        LabSystemManager.Instance.HighlightObject(listObj[index]); //开启气相一质谱仪
        index++;
        GameManager.Instance.stepDetection = false;
        yield return new WaitUntil(() => GameManager.Instance.stepDetection); // 当 conditionMet == true 时继续 体检室
        listObj[4].SetActive(true);

    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag != "Player") return;
        //LabSystemManager.Instance.HighlightObject(anObj);
        qiTiShi.SetActive(true);
        LabSystemManager.Instance.HighlightObject(listObj[0]);

    }

}
