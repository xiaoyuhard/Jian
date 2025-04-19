using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
            panel.gameObject.SetActive(true);
            panel.OnOpen();
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
            panel.gameObject.SetActive(true);
            panel.OnOpen();
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
            panel.OnClose();
            panel.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError($"δ�ҵ� UI ����: {uiName}");
        }
    }
    public void CloseUICaoZuo(string uiName)
    {
        if (uiCaoZuoPanels.TryGetValue(uiName, out UICaoZuoBase panel))
        {
            panel.OnClose();
            panel.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError($"δ�ҵ� UI ����: {uiName}");
        }
    }

    // �ر����� UI
    public void CloseAllUI()
    {
        foreach (var panel in uiPanels.Values)
        {
            panel.OnClose();
            panel.gameObject.SetActive(false);
        }
    }

    public void CloseAllUICaoZuo()
    {
        foreach (var panel in uiCaoZuoPanels.Values)
        {
            panel.OnClose();
            panel.gameObject.SetActive(false);
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
}



