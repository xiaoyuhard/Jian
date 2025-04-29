using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//一些拓展方法
public static class GameExt
{
    /// <summary>
    /// 获取或添加组件
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
