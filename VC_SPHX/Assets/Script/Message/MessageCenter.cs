using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageCenter : MonoBehaviour
{
    #region 单例实现
    private static MessageCenter _instance;
    public static MessageCenter Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("MessageCenter");
                _instance = obj.AddComponent<MessageCenter>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }
    #endregion

    #region 核心数据结构
    // 存储事件监听器的字典
    private Dictionary<string, Action<string>> eventDictionary =
        new Dictionary<string, Action<string>>();
    #endregion

    #region 公共接口
    /// <summary>
    /// 注册事件监听
    /// </summary>
    /// <param name="eventName">事件名称</param>
    /// <param name="callback">回调函数</param>
    public void Register(string eventName, Action<string> callback)
    {
        if (string.IsNullOrEmpty(eventName))
        {
            Debug.LogError("事件名不能为空！");
            return;
        }

        if (callback == null)
        {
            Debug.LogError("回调函数不能为空！");
            return;
        }

        if (!eventDictionary.ContainsKey(eventName))
        {
            eventDictionary.Add(eventName, null);
        }

        eventDictionary[eventName] += callback;
    }

    /// <summary>
    /// 注销事件监听
    /// </summary>
    public void Unregister(string eventName, Action<string> callback)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] -= callback;
        }
    }

    /// <summary>
    /// 发送消息（带参数）
    /// </summary>
    public void Send(string eventName, string message = null)
    {
        if (eventDictionary.TryGetValue(eventName, out Action<string> callback))
        {
            callback?.Invoke(message);
        }
        else
        {
            Debug.LogWarning($"未注册的事件: {eventName}");
        }
    }

    /// <summary>
    /// 清空所有事件监听
    /// </summary>
    public void ClearAll()
    {
        eventDictionary.Clear();
    }
    #endregion

    #region 自动清理扩展（可选）
    // 为MonoBehaviour对象提供的自动注销功能
    public void RegisterWithCleanup(string eventName, Action<string> callback, MonoBehaviour owner)
    {
        Register(eventName, callback);

        // 当宿主对象被销毁时自动注销
        owner.StartCoroutine(CleanupOnDestroy(eventName, callback, owner));
    }

    private System.Collections.IEnumerator CleanupOnDestroy(
        string eventName,
        Action<string> callback,
        MonoBehaviour owner)
    {
        yield return new WaitUntil(() => owner == null);
        Unregister(eventName, callback);
    }

    internal void Send(string v, object mouseToAnjisuanComputer)
    {
        throw new NotImplementedException();
    }
    #endregion
}
