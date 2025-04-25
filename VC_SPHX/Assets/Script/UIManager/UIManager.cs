using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


public class UIManager : MonoBehaviour
{
    // ����ģʽ
    public static UIManager Instance { get; private set; }

    // �洢����ע��� UI ���
    private Dictionary<string, UIBase> uiPanels = new Dictionary<string, UIBase>();
    private Dictionary<string, UICaoZuoBase> uiCaoZuoPanels = new Dictionary<string, UICaoZuoBase>();
    public List<UICaoZuoBase> uICaoZuoBaseList;
    public List<UIBase> uIBaseList;

    private Canvas mainCanvas;
    List<string> mNotCloseList = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            mainCanvas = GetComponentInParent<Canvas>(); // ���� UIManager ���� Canvas ��
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        AutoRegisterAllUI();
    }

    private void Start()
    {
        //Debug.Log(GetComponentInChildren<HomeManager>());
        //UIManager.Instance.RegisterUI<HomeManager>(transform.GetComponentInChildren<HomeManager>());
        //UIManager.Instance.RegisterUI<ZhishiManager>(GetComponentInChildren<ZhishiManager>());
    }

    // ע�� UI ��壨�ֶ��󶨻��Զ����ң�
    public void RegisterUI(string uiName, UIBase panel)
    {
        if (!uiPanels.ContainsKey(uiName))
        {
            uiPanels.Add(uiName, panel);
            panel.gameObject.SetActive(false); // Ĭ�Ϲر�
        }
        else
        {
            Debug.LogWarning($"�ظ�ע��� UI ����: {uiName}");
        }
    }

    public void RegisterCaoZuoUI(string uiName, UICaoZuoBase panel)
    {
        if (!uiCaoZuoPanels.ContainsKey(uiName))
        {
            uiCaoZuoPanels.Add(uiName, panel);
            panel.gameObject.SetActive(false); // Ĭ�Ϲر�
            //Debug.Log(uiName);
        }
        else
        {
            Debug.LogWarning($"�ظ�ע��� UI ����: {uiName}");
        }
    }

    // �Զ�ע�������Ӷ����е� UI ��壨ʹ�ö�������Ϊ����
    public void AutoRegisterAllUI()
    {
        //UIBase[] panels = GetComponentsInChildren<UIBase>(true);
        foreach (UIBase panel in uIBaseList)
        {
            string uiName = panel.gameObject.name;
            RegisterUI(uiName, panel);
        }

        //UICaoZuoBase[] caoZUoPanels = GetComponentsInChildren<UICaoZuoBase>();
        foreach (UICaoZuoBase panel in uICaoZuoBaseList)
        {
            string uiName = panel.gameObject.name;
            RegisterCaoZuoUI(uiName, panel);
        }
    }

    // �� UI ��壨ͨ�����ƣ�
    public void OpenUI(string uiName)
    {
        if (uiPanels.TryGetValue(uiName, out UIBase panel))
        {
            if (!panel.gameObject.activeInHierarchy)
            {
                panel.gameObject.SetActive(true);
                panel.OnOpen();
                print("OpenUI:" + panel);
            }
        }
        else
        {
            Debug.LogError($"δ�ҵ� UI ����: {uiName}");
        }
    }

    public void OpenUICaoZuo(string uiName)
    {
        if (uiCaoZuoPanels.TryGetValue(uiName, out UICaoZuoBase panel))
        {
            if (!panel.gameObject.activeInHierarchy)
            {
                panel.gameObject.SetActive(true);
                panel.OnOpen();
                print("OpenUICaoZuo:" + panel);
            }
        }
        else
        {
            Debug.LogError($"δ�ҵ� UI ����: {uiName}");
        }
    }

    // �ر� UI ��壨ͨ�����ƣ�
    public void CloseUI(string uiName)
    {
        if (uiPanels.TryGetValue(uiName, out UIBase panel))
        {
            CloseUI(panel);
        }
    }

    void CloseUI(UIBase ui)
    {
        if (ui != null)
        {
            if (mNotCloseList.Contains(ui.name))
                return;

            if (ui.gameObject.activeInHierarchy)
            {
                ui.OnClose();
                ui.gameObject.SetActive(false);
                print("CloseUI:" + ui);
            }
        }
        else
        {
            Debug.LogError($"δ�ҵ� UI ����: {ui.name}");
        }
    }

    public void CloseUICaoZuo(string uiName)
    {
        if (uiCaoZuoPanels.TryGetValue(uiName, out UICaoZuoBase panel))
        {
            CloseUICaoZuo(panel);
        }
    }

    void CloseUICaoZuo(UICaoZuoBase ui)
    {
        if (ui != null)
        {
            if (mNotCloseList.Contains(ui.name))
                return;

            if (ui.gameObject.activeInHierarchy)
            {
                ui.OnClose();
                ui.gameObject.SetActive(false);
                print("CloseUICaoZuo:" + ui);
            }
        }
        else
        {
            Debug.LogError($"δ�ҵ� UI ����: {ui.name}");
        }
    }

    // �ر����� UI
    public void CloseAllUI()
    {
        foreach (var panel in uiPanels.Values)
        {
            CloseUI(panel);
        }
    }

    public void CloseAllUICaoZuo()
    {
        foreach (var panel in uiCaoZuoPanels.Values)
        {
            CloseUICaoZuo(panel);
        }
    }
    //public void BackHome()
    //{
    //    CloseUI("ProTipsMan");
    //    CloseUI("GenyishiMan");
    //    CloseUI("ExamineUI");
    //    CloseUI("PreparationUI");
    //    CloseUI("WorkbenchUI");
    //    CloseUI("ChromatographUI");
    //    CloseUI("GenLianUIMan");
    //    CloseUI("ChromatographUI");
    //    CloseUI("ChromatographUI");
    //}

    /// <summary>
    /// �趨��Ҫ�ر�UI���κ�ʱ��
    /// </summary>
    /// <param name="name"></param>
    public void DonotCloseUI(string name)
    {
        if (!mNotCloseList.Contains(name))
            mNotCloseList.Add(name);
    }
}

public class UINameType
{
    public const string UI_HomeManager = "UI_HomeManager";
    public const string UI_ZhishiManager = "UI_ZhishiManager";
    public const string UI_MoxingManager = "UI_MoxingManager";
    public const string UI_CaozuoManager = "UI_CaozuoManager";
    public const string UI_BaogaoManager = "UI_BaogaoManager";
    public const string UI_BackMan = "UI_BackMan";
    public const string UI_GenLianUIMan = "UI_GenLianUIMan";
    public const string UI_GenyishiMan = "UI_GenyishiMan";
    public const string UI_ProTipsMan = "UI_ProTipsMan";



}
