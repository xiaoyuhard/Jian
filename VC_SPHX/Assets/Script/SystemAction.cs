using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemAction : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if (SystemManager.systemMode == SystemMode.Experiment1) 
        {
            Experiment1();
        } else
        if (SystemManager.systemMode == SystemMode.Experiment9)
        {
            Experiment9();
        } else
        if (SystemManager.systemMode == SystemMode.Experiment10)
        {
            Experiment10();
        }

    }
    /// <summary>
    /// ������
    /// </summary>
    public void Experiment1()
    {
        //UIManager.Instance.CloseUI(UINameType.UI_ZhishiManager);
        //UIManager.Instance.CloseUI(UINameType.UI_MoxingManager);
        //UIManager.Instance.CloseUI(UINameType.UI_CaozuoManager);
        //UIManager.Instance.CloseUI(UINameType.UI_BaogaoManager);
        //UIManager.Instance.CloseUI(UINameType.UI_BackMan);
        GameObjMan.Instance.CloseObjCon(1);
        LabSystemManager.Instance.OnLabButtonClicked(1, "1������");
        //GameManager.Instance.SetGameObj(true);
        //GameObjMan.Instance.OpenFirst();
        LabSystemManager.Instance.SelectPracticeMode();
    }
    /// <summary>
    /// ����Ӫ��
    /// </summary>
    public void Experiment9()
    {
        Debug.LogError("Experiment9");
        //UIManager.Instance.CloseUI(UINameType.UI_ZhishiManager);
        //UIManager.Instance.CloseUI(UINameType.UI_MoxingManager);
        //UIManager.Instance.CloseUI(UINameType.UI_CaozuoManager);
        //UIManager.Instance.CloseUI(UINameType.UI_BaogaoManager);
        //UIManager.Instance.CloseUI(UINameType.UI_BackMan);Ӫ����ʾ
        GameObjMan.Instance.CloseObjCon(1);
        LabSystemManager.Instance.OnLabButtonClicked(9, "Ӫ����ʾ");
        //GameManager.Instance.SetGameObj(true);
        GameObjMan.Instance.OpenFirst();
        LabSystemManager.Instance.SelectAssessmentMode();
    }
    /// <summary>
    /// ����
    /// </summary>
    public void Experiment10()
    {
        Debug.LogError("Experiment10");
        //UIManager.Instance.CloseUI(UINameType.UI_ZhishiManager);
        //UIManager.Instance.CloseUI(UINameType.UI_MoxingManager);
        //UIManager.Instance.CloseUI(UINameType.UI_CaozuoManager);
        //UIManager.Instance.CloseUI(UINameType.UI_BaogaoManager);
        //UIManager.Instance.CloseUI(UINameType.UI_BackMan);
        GameObjMan.Instance.CloseObjCon(1);
        LabSystemManager.Instance.OnLabButtonClicked(10, "GameObject");
        //GameManager.Instance.SetGameObj(true);
        GameObjMan.Instance.OpenFirst();
        LabSystemManager.Instance.SelectAssessmentMode();
    }
}
