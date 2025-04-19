using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// UI����
public abstract class UIBase : MonoBehaviour
{
    [HideInInspector] public string UIName;

    public void Initialize(string uiName)
    {
        this.UIName = uiName;
    }

    public virtual void OnOpen() { } // ��ʱ����
    public virtual void OnClose() { } // �ر�ʱ����
    // �ر�����
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

    public virtual void OnOpen() { } // ��ʱ����
    public virtual void OnClose() { } // �ر�ʱ����
    // �ر�����
    //public void CloseSelf()
    //{
    //    UIManager.Instance.CloseUI(UIName);
    //}
}

