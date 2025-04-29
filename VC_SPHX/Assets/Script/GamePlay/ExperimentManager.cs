using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�ؽ����ⶨ
public class ExperimentManager : MonoBehaviour
{
    public Button btnStart;
    //�����ļ�
    public Transform directorParent;
    //�������������壬����
    //public GameObject[] triggerObjs;
    public ExpStepCtrl stepCtrl;
    public TextAsset csvFile;

    //ʵ����е���һ������
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

        //��ȡ��ʾ���ã���ʾ��ʾ
        //MessageCenter.Instance.Send("SendTiShiUIName", "�ؽ�����ʾ");
        StalkProcedureManager.Instance.TiShiUIName(csvFile.name);

        // ������������
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

    //���ض����������Զ�����
    void InitAnimation()
    {
        for (int i = 0; i < directorParent.childCount; i++)
        {
            directorParent.GetChild(i).gameObject.SetActive(false);
        }
    }

    void NextStep(string arg)
    {
        print("===========��ʼ������һ��");
        //print("cur stepIndex: " + stepIndex);

        //if (stepIndex == 0)
        //{
        //    //�� �ؽ�����
        //    DoorClickCon.Instance.SetHighlight(3);
        //    //�� �ؽ���ʵ�� ������
        //    GameObjMan.Instance.OpenObjCon(4);
        //}
        //else if (stepIndex == 1)
        //{
        //    //�����������
        //    triggerObjs[0].GetComponent<Outline>().enabled = true;
        //}

        //stepIndex++;
        //StalkProcedureManager.Instance.UpdateUIInf(stepIndex);

        stepCtrl.NextStep();
    }

}