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

        MessageCenter.Instance.Register("SendMouseToXiangQiGui", MouseToXiangQiGui); //������
        MessageCenter.Instance.Register("SendMouseToXiangQiNiu", MouseToXiangQiNiu); //����Ť
        MessageCenter.Instance.Register("SendMouseToXiangQiFa", MouseToXiangQiFa); //������
        MessageCenter.Instance.Register("SendMouseToXiangQiRongye", MouseToXiangQiRongye); //������Һ
        MessageCenter.Instance.Register("SendMouseToXiangQiCuiquyi", MouseToXiangQiCuiquyi); //����������

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
        // �ȴ�������������
        yield return new WaitUntil(() => GameManager.Instance.stepDetection); // �� conditionMet == true ʱ���� ����ƿ��
        LabSystemManager.Instance.HighlightObject(listObj[index]);
        index++;
        GameManager.Instance.stepDetection = false;
        yield return new WaitUntil(() => GameManager.Instance.stepDetection); // �� conditionMet == true ʱ���� ��������ʵ����
        DoorClickCon.Instance.OpenDoorHigh(2);
        GameManager.Instance.stepDetection = false;
        yield return new WaitUntil(() => GameManager.Instance.stepDetection); // �� conditionMet == true ʱ���� ѡ��ҩƷ��ʼʵ��
        LabSystemManager.Instance.HighlightObject(listObj[index]);
        index++;
        GameManager.Instance.stepDetection = false;
        yield return new WaitUntil(() => GameManager.Instance.stepDetection); // �� conditionMet == true ʱ���� ��Һ���������ȷ��
        LabSystemManager.Instance.HighlightObject(listObj[index]); //��������һ������
        index++;
        GameManager.Instance.stepDetection = false;
        yield return new WaitUntil(() => GameManager.Instance.stepDetection); // �� conditionMet == true ʱ���� �����
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
