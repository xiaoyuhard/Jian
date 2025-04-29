using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// ������ʵ��
/// </summary>
public class AnjisuanCon : MonoSingletonBase<AnjisuanCon>
{
    //public GameObject anObj;
    public GameObject workbench;
    public List<GameObject> modelList;
    public List<GameObject> listObj;
    public List<GameObject> timeList;

    public GameObject timelineObj;
    public GameObject sepuObj;

    //int index = 0;

    //public void JoinAnjisuan()
    //{

    //    anObj.SetActive(true);
    //    //ƿ�Ӹ���
    //    LabSystemManager.Instance.HighlightObject(listObj[0]);
    //}

    private void Start()
    {


        //MessageCenter.Instance.Register("SendMouseToAnjisuanPing", MouseAnjisuanPing); //�򿪰�����UI���

        //MessageCenter.Instance.Register("SendMouseToChromatograph", MouseChromatograph); //�����Ṥ��̨
        //MessageCenter.Instance.Register("SendMouseToDaoChu", MouseDaoChu); //�����Ṥ��̨
        //MessageCenter.Instance.Register("SendPreToWorkbench", PreToWorkbench); //�����Ṥ��̨����
        //MessageCenter.Instance.Register("SendExamineToAnjisuan", ExamineToAnjisuan); //������PreparationUI��
        //MessageCenter.Instance.Register("SendMouseToWorkbench", MouseWorkbench); //�����Ṥ��̨
        //MessageCenter.Instance.Register("SendMouseToAnjisuanComputer", MouseToAnjisuanComputer); //���������

        //MessageCenter.Instance.Register("SendDianNaoToAn", DianNaoToAn); //�����Ṥ��̨

        //MessageCenter.Instance.Register("SendWearToAn", WearToAnTag);


    }
    //private void MouseToAnjisuanComputer(string obj)
    //{
    //    UIManager.Instance.OpenUICaoZuo("ChromatographUI");

    //}
    //private void MouseWorkbench(string obj)
    //{
    //    UIManager.Instance.OpenUICaoZuo("WorkbenchUI");
    //    LabSystemManager.Instance.HighlightObject(listObj[4]);

    //}
    //private void ExamineToAnjisuan(string obj)
    //{
    //    UIManager.Instance.OpenUICaoZuo("PreparationUI");
    //    LabSystemManager.Instance.HighlightObject(listObj[1]);

    //}

    //private void DianNaoToAn(string obj)
    //{


    //}


    //private void PreToWorkbench(string obj)
    //{
    //    //workbench.GetComponent<Outline>().enabled = true;
    //    //LabSystemManager.Instance.HighlightObject(workbench);

    //}

    //////���յ���������ǰ����� �������д洢 �������������������� ���з��� �������������
    ////private void WearToAnTag(string obj)
    ////{
    ////    MessageCenter.Instance.Send("CaozuoName", obj);

    ////}
    //private void MouseDaoChu(string obj)
    //{
    //    UIManager.Instance.OpenUICaoZuo("GenLianUIMan");

    //}

    //private void MouseChromatograph(string obj)
    //{
    //    //UIManager.Instance.OpenUICaoZuo("ChromatographUI");
    //    LabSystemManager.Instance.HighlightObject(listObj[5]);

    //}



    //private void MouseAnjisuanPing(string obj)
    //{
    //    UIManager.Instance.OpenUICaoZuo("AnjisuanExamineUI");

    //}



    IEnumerator ConUp()
    {
        //while (true)
        //{
        // �ȴ�������������
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� �����ҵ��˹���ѡ���·�

        LabSystemManager.Instance.OnExitLockerClicked();
        IsAssessmentMode(1); //��ʾ��ʾ����
                             //LabSystemManager.Instance.HighlightObject(listObj[8]);

        //for (int i = 0; i < 10; i++)
        //{
        //    IsAssessmentMode(i + 1); //��ʾ��ʾ����
        //    yield return new WaitForSeconds(3f);
        //}

        GameManager.Instance.SetStepDetection(false);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� ����������ʵ������ AccQ-TagUltra�����Լ����� ������Ӧģ�� ����ʾ��ʾ
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(2); //��ʾ��ʾ����
        ShowObj(0);//��˳����ʾ ��1������
        //LabSystemManager.Instance.HighlightObject(listObj[8]);

        LabSystemManager.Instance.HighlightObject(listObj[0]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� �Ѹ����������� ���˺� �հ���Ʒ���Ʊ� ���� ����ʾ
        Debug.Log($"�ȴ�ʱ��: {OpenTimePlay(0)}"); // �鿴����̨���
        yield return SkipableWait(OpenTimePlay(0));//����󲥷Ŷ���  1
        //index++;//1
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(3);
        ShowObj(1);//��˳����ʾ ��2������
        LabSystemManager.Instance.HighlightObject(listObj[1]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� �������׼Ʒ���Ʊ�

        yield return SkipableWait(OpenTimePlay(1));//���Ŷ���  2
        //index++;//2
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(4);
        ShowObj(2);//��˳����ʾ ����������
        LabSystemManager.Instance.HighlightObject(listObj[2]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� ��׼Ʒ���������ܼ�

        yield return SkipableWait(OpenTimePlay(2));//���Ŷ���  3
        //index++;//3
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(5);
        ShowObj(3);//��˳����ʾ ��4������
        LabSystemManager.Instance.HighlightObject(listObj[3]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� ��׼Ʒ������ˮԡ����

        yield return SkipableWait(OpenTimePlay(3));//���Ŷ���  4
        //index++;//4
        GameManager.Instance.SetStepDetection(false);

        IsAssessmentMode(6);
        ShowObj(4);//��˳����ʾ ��5������
        LabSystemManager.Instance.HighlightObject(listObj[4]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� �����Ʊ�

        yield return SkipableWait(OpenTimePlay(4));//���Ŷ���  5
                                                   //index++;//5
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(7);
        ShowObj(5);//��˳����ʾ ��6������
        LabSystemManager.Instance.HighlightObject(listObj[5]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� �Լ�����

        yield return SkipableWait(OpenTimePlay(5));//���Ŷ���  6
                                                   //index++;//6
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(8);
        ShowObj(6);//��˳����ʾ ��7������
        LabSystemManager.Instance.HighlightObject(listObj[5]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� ˮ��ܼ�����ͱ���

        yield return SkipableWait(OpenTimePlay(6));//���Ŷ���  7
                                                   //index++;//7
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(9);
        ShowObj(7);//��˳����ʾ ��8������
        LabSystemManager.Instance.HighlightObject(listObj[6]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� ˮ����䶳����

        yield return SkipableWait(OpenTimePlay(7));//���Ŷ���  8
                                                   //index++;//8
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(10);
        ShowObj(8);//��˳����ʾ ��9������
        LabSystemManager.Instance.HighlightObject(listObj[7]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� ˮ��ܺ��������

        yield return SkipableWait(OpenTimePlay(7));//���Ŷ���  9
                                                   //index++;//9
        DoorClickCon.Instance.SetHighlight(11);

        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(11);
        ShowObj(9);//��˳����ʾ ��10������
        LabSystemManager.Instance.HighlightObject(listObj[8]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� ˮ��Һ���˶���

        yield return SkipableWait(OpenTimePlay(9));//���Ŷ���  10
        DoorClickCon.Instance.CloseDoorHigh(11);

        //index++;//10
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(12);
        ShowObj(10);//��˳����ʾ ��11������
        LabSystemManager.Instance.HighlightObject(listObj[9]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� ˮ��Һ����ת��

        yield return SkipableWait(OpenTimePlay(10));//���Ŷ���  11
                                                    //index++;//11
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(13);
        ShowObj(11);//��˳����ʾ ��12������
        LabSystemManager.Instance.HighlightObject(listObj[10]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� �Թ�Ũ���Ǽ�ѹ

        yield return SkipableWait(OpenTimePlay(11));//���Ŷ���  12
                                                    //index++;//12
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(14);
        ShowObj(12);//��˳����ʾ ��13������
        LabSystemManager.Instance.HighlightObject(listObj[11]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� ���������ܽ���Ĥ����

        yield return SkipableWait(OpenTimePlay(12));//���Ŷ���  13
                                                    //index++;//13
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(15);
        ShowObj(13);//��˳����ʾ ��14������
        LabSystemManager.Instance.HighlightObject(listObj[12]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� ��Ʒ����������Լ�

        yield return SkipableWait(OpenTimePlay(13));//���Ŷ���  14
                                                    //index++;//14
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(16);
        ShowObj(14);//��˳����ʾ ��15������
        LabSystemManager.Instance.HighlightObject(listObj[13]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� ��Ʒ������ˮԡ����

        yield return SkipableWait(OpenTimePlay(14));//���Ŷ���  15
                                                    //index++;//15
        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(17);
        ShowObj(15);//��˳����ʾ ��16������
        LabSystemManager.Instance.HighlightObject(listObj[14]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� ��Ʒ��Һ��ƿ

        yield return SkipableWait(OpenTimePlay(15));//���Ŷ���  16
        GameManager.Instance.SetStepDetection(false);
        //index++;//16
        IsAssessmentMode(18);
        ShowObj(16);//��˳����ʾ ��17������
        LabSystemManager.Instance.HighlightObject(listObj[15]);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� ��Ʒ��Һ��ƿ

        yield return SkipableWait(OpenTimePlay(16));//���Ŷ���  17

        GameManager.Instance.SetStepDetection(false);
        //index++;//17
        IsAssessmentMode(19);
        sepuObj.SetActive(true);

        LabSystemManager.Instance.HighlightObject(listObj[16]);

        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� ����ģ�����
        GameManager.Instance.SetStepDetection(false);

        LabSystemManager.Instance.HighlightObject(listObj[17]);

        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� ��ģ�����
        LabSystemManager.Instance.HighlightObject(listObj[18]);
        GameManager.Instance.SetStepDetection(false);
        //index++;//18

        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� �Զ�����ģ�����
        LabSystemManager.Instance.HighlightObject(listObj[19]);
        GameManager.Instance.SetStepDetection(false);
        //index++;//19

        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� ������ģ�����
        LabSystemManager.Instance.HighlightObject(listObj[20]);
        GameManager.Instance.SetStepDetection(false);
        //index++;//20

        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� �����ģ�����

        LabSystemManager.Instance.HighlightObject(listObj[22]);
        GameManager.Instance.SetStepDetection(false);
        //index++;//21
        IsAssessmentMode(20);
        ShowObj(18);//��˳����ʾ ��19������

        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� ���Կ���
        yield return SkipableWait(OpenTimePlay(18));//���Ŷ���  19

        LabSystemManager.Instance.HighlightObject(listObj[24]);
        GameManager.Instance.SetStepDetection(false);
        //index++;//20

        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� ���������ͼ�������

        UIManager.Instance.OpenUICaoZuo("ComputerUI");
        //ComputerUI.Instance.ShowComUI(0);
        GameManager.Instance.SetStepDetection(false);
        yield return new WaitForSeconds(3);//��ʾ��������������

        LabSystemManager.Instance.HighlightObject(listObj[21]);
        ComputerUI.Instance.CloseUI();
        UIManager.Instance.CloseUICaoZuo("ComputerUI");
        ShowObj(17);//��˳����ʾ ��18������

        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� ������Ļ��ʾUI Ȼ������������

        GameManager.Instance.SetStepDetection(false);
        IsAssessmentMode(21);
        yield return SkipableWait(OpenTimePlay(17));//���Ŷ���  18

        UIManager.Instance.OpenUICaoZuo("ComputerUI");
        ComputerUI.Instance.ShowComUI(0);
        GameManager.Instance.SetStepDetection(false);

        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� ���������� ���зŶ��� ��UI���в��� 
        //yield return new WaitForSeconds(3);//��ʾ��������������
        LabSystemManager.Instance.HighlightObject(listObj[21]);
        ComputerUI.Instance.CloseUI();
        UIManager.Instance.CloseUICaoZuo("ComputerUI");

        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� ������Ļ��ʾUI Ȼ������������
        UIManager.Instance.OpenUICaoZuo("ComputerUI");

        ComputerUI.Instance.ShowComUI(1);
        GameManager.Instance.SetStepDetection(false);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� ���������� ���зŶ��� ��UI���в��� 

        ComputerUI.Instance.ShowComUI(2);
        GameManager.Instance.SetStepDetection(false);
        yield return new WaitForSeconds(3);//��ʾ��������������
        //LabSystemManager.Instance.HighlightObject(listObj[21]);
        ComputerUI.Instance.CloseUI();

        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� ������Ļ��ʾUI Ȼ������������

        ComputerUI.Instance.ShowComUI(3);
        GameManager.Instance.SetStepDetection(false);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� 

        ComputerUI.Instance.ShowComUI(4);
        GameManager.Instance.SetStepDetection(false);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ����

        ComputerUI.Instance.ShowComUI(5);
        GameManager.Instance.SetStepDetection(false);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� 

        ComputerUI.Instance.ShowComUI(6);
        GameManager.Instance.SetStepDetection(false);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� 

        ComputerUI.Instance.ShowComUI(7);
        GameManager.Instance.SetStepDetection(false);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� 

        ComputerUI.Instance.ShowComUI(8);
        GameManager.Instance.SetStepDetection(false);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� 

        ComputerUI.Instance.ShowComUI(9);
        GameManager.Instance.SetStepDetection(false);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� 
        //UIManager.Instance.CloseUICaoZuo("ComputerUI");
        ComputerUI.Instance.ShowComUI(10);
        GameManager.Instance.SetStepDetection(false);
        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection()); // �� conditionMet == true ʱ���� 
                                                                                    //UIManager.Instance.OpenUICaoZuo("ChromatographUI");


    }

    public void IsAssessmentMode(int index)
    {
        if (GameManager.Instance.tipStalkBl)
        {
            StalkProcedureManager.Instance.UpdateUIInf(index);
        }
    }

    bool isSkip = false;
    public float OpenTimePlay(int index)
    {
        timeList[index].GetComponent<PlayableDirector>().Play();

        return (float)timeList[index].GetComponent<PlayableDirector>().duration;
        //return 3;
    }

    /// <summary>
    /// �������ĵȴ�Э��
    /// </summary>
    /// <param name="waitTime">���ȴ�ʱ�䣨�룩</param>
    /// <param name="skipCondition">��������������trueʱ���������ȴ���</param>
    /// <returns></returns>
    public static IEnumerator SkipableWait(float waitTime)
    {
        if (waitTime <= 0)
        {
            yield break; // ʱ����Чֱ������
        }

        float timer = 0f;
        while (timer < waitTime)
        {
            timer += Time.deltaTime;

            // ����Ƿ�������������
            if (Input.GetKeyDown(KeyCode.Y))
            {
                Debug.Log("�ȴ�����������");
                yield break; // ��ǰ�˳�
            }

            yield return null; // ��������һ֡
        }

        Debug.Log("�ȴ���Ȼ����");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            isSkip = true;
        }
        else
        {
            isSkip = false;
        }
    }

    bool isTriOnce = true;
    private void OnTriggerEnter(Collider other)
    {
        if (!isTriOnce) return;
        //if (other.tag != "Player") return;
        //LabSystemManager.Instance.HighlightObject(anObj);
        //anObj.SetActive(true);
        DoorClickCon.Instance.CloseHighlightAll();

        LabSystemManager.Instance.HighlightObject(listObj[0]);
        IsAssessmentMode(2);
        GameManager.Instance.SetStepDetection(true);
        isTriOnce = false;
    }
    public void ShowObj(int index)
    {
        foreach (var item in modelList)
        {
            //item.SetActive(true);
        }
        foreach (var item in timeList)
        {
            item.SetActive(false);
            //item.GetComponent<PlayableDirector>().enabled = false;
            item.GetComponent<PlayableDirector>().time = 0;
            item.GetComponent<PlayableDirector>().Stop();
            //item.GetComponent<PlayableDirector>().Evaluate(); // ǿ��Ӧ�õ�0֡״̬

        }
        timeList[index].SetActive(true);
        timeList[index].GetComponent<PlayableDirector>().time = 5f;
        timeList[index].GetComponent<PlayableDirector>().Stop();
        timeList[index].GetComponent<PlayableDirector>().Evaluate(); // ǿ��Ӧ�õ�0֡״̬
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
            //item.GetComponent<PlayableDirector>().Evaluate(); // ǿ��Ӧ�õ�0֡״̬
            item.SetActive(false);
        }
        foreach (var item in modelList)
        {
            //item.SetActive(false);
        }
        timelineObj.SetActive(true);
        isTriOnce = true;

        StartCoroutine(ConUp());

    }
    private void OnDisable()
    {
        timelineObj.SetActive(false);
    }
}
