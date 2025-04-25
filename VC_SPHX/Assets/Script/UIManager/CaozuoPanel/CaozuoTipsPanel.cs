using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//ѡ��ʵ��ģʽ���� ���� ����
public class CaozuoTipsPanel : MonoSingletonBase<CaozuoTipsPanel>
{
    public Button btnKaohe;
    public Button btnGensui;
    public GameObject obj;

    public string joinIdtag;

    //��ʾ������� һ���ǿ��� һ���Ǹ���

    // Start is called before the first frame update
    void Start()
    {
        btnKaohe.onClick.AddListener(PassStalking);
        btnGensui.onClick.AddListener(PassExamine);
        //MessageCenter.Instance.Register("CaozuoName", GetCaozuoName);//Ӧ�ò���Ҫ
        //MessageCenter.Instance.Register("JoinKaoHe", JoinKaoHe);//Ӧ�ò���Ҫ
        ClosePanel();
    }

    private void JoinKaoHe(string obj)
    {
        //UIManager.Instance.CloseUI(UINameType.UI_ZhishiManager);
        //UIManager.Instance.CloseUI(UINameType.UI_MoxingManager);
        //UIManager.Instance.CloseUI(UINameType.UI_CaozuoManager);
        //UIManager.Instance.CloseUI(UINameType.UI_BaogaoManager);
        //UIManager.Instance.CloseUI(UINameType.UI_BackMan);
        UIManager.Instance.CloseAllUI();
        //UIManager.Instance.OpenUI(UINameType.UI_HomeManager);
        GameManager.Instance.SetGameObj(true);
        GameObjMan.Instance.OpenFirst();

    }

    //����
    void PassStalking()
    {
        JoinKaoHe("");
        //MessageCenter.Instance.Send("SendGengToDoorKaoHe", "WearPos");//Ӧ�ò���Ҫ
        //GameManager.Instance.SetTrainingMode(false);
        LabSystemManager.Instance.SelectAssessmentMode();  //���뿼��

        ClosePanel();
        //SceneMgr.LoadScene(GameScene.Demo3);

    }

    //����
    void PassExamine()
    {
        //MessageCenter.Instance.Send("SendCaozuoToWear", joinIdtag);//Ӧ�ò���Ҫ
        //GameManager.Instance.SetTrainingMode(true);

        JoinKaoHe("");
        //MessageCenter.Instance.Send("SendGengToDoor", "WearPos");//Ӧ�ò���Ҫ
        LabSystemManager.Instance.SelectPracticeMode();//�������
        //GameObjMan.Instance.SetPosition(0);
        ClosePanel();

        //SceneMgr.LoadScene(GameScene.Demo3);

    }

    public void ClosePanel()
    {
        obj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
