using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//һЩ��չ����
public static class GameExt
{
    /// <summary>
    /// ��ȡ��������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        var comp = go.GetComponent<T>();

        if (comp == null)
            comp= go.AddComponent<T>();

        return comp;
    }
}
