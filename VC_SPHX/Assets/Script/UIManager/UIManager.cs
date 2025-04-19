using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIManager : MonoBehaviour
{
    // 单例模式
    public static UIManager Instance { get; private set; }

    // 存储所有注册的 UI 面板
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
            mainCanvas = GetComponentInParent<Canvas>(); // 假设 UIManager 挂在 Canvas 下
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

    // 注册 UI 面板（手动绑定或自动查找）
    public void RegisterUI(string uiName, UIBase panel)
    {
        if (!uiPanels.ContainsKey(uiName))
        {
            uiPanels.Add(uiName, panel);
            panel.gameObject.SetActive(false); // 默认关闭
        }
        else
        {
            Debug.LogWarning($"重复注册的 UI 名称: {uiName}");
        }
    }

    public void RegisterCaoZuoUI(string uiName, UICaoZuoBase panel)
    {
        if (!uiCaoZuoPanels.ContainsKey(uiName))
        {
            uiCaoZuoPanels.Add(uiName, panel);
            panel.gameObject.SetActive(false); // 默认关闭
            //Debug.Log(uiName);
        }
        else
        {
            Debug.LogWarning($"重复注册的 UI 名称: {uiName}");
        }
    }

    // 自动注册所有子对象中的 UI 面板（使用对象名作为键）
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

    // 打开 UI 面板（通过名称）
    public void OpenUI(string uiName)
    {
        if (uiPanels.TryGetValue(uiName, out UIBase panel))
        {
            panel.gameObject.SetActive(true);
            panel.OnOpen();
        }
        else
        {
            Debug.LogError($"未找到 UI 名称: {uiName}");
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
            Debug.LogError($"未找到 UI 名称: {uiName}");
        }
    }

    // 关闭 UI 面板（通过名称）
    public void CloseUI(string uiName)
    {
        if (uiPanels.TryGetValue(uiName, out UIBase panel))
        {
            panel.OnClose();
            panel.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError($"未找到 UI 名称: {uiName}");
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
            Debug.LogError($"未找到 UI 名称: {uiName}");
        }
    }

    // 关闭所有 UI
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



