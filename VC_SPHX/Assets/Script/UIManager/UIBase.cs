using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// UI基类
public abstract class UIBase : MonoBehaviour
{
    [HideInInspector] public string UIName;

    public void Initialize(string uiName)
    {
        this.UIName = uiName;
    }

    public virtual void OnOpen() { } // 打开时调用
    public virtual void OnClose() { } // 关闭时调用
    // 关闭自身
    //public void CloseSelf()
    //{
    //    UIManager.Instance.CloseUI(UIName);
    //}
}

public abstract class UICaoZuoBase : MonoBehaviour
{
    [HideInInspector] public string UIName;

    public void Initialize(string uiName)
    {
        this.UIName = uiName;
    }

    public virtual void OnOpen() { } // 打开时调用
    public virtual void OnClose() { } // 关闭时调用
    // 关闭自身
    //public void CloseSelf()
    //{
    //    UIManager.Instance.CloseUI(UIName);
    //}
}

