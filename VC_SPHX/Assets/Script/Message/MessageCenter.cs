using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageCenter : MonoBehaviour
{
    #region ����ʵ��
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

    #region �������ݽṹ
    // �洢�¼����������ֵ�
    private Dictionary<string, Action<string>> eventDictionary =
        new Dictionary<string, Action<string>>();
    #endregion

    #region �����ӿ�
    /// <summary>
    /// ע���¼�����
    /// </summary>
    /// <param name="eventName">�¼�����</param>
    /// <param name="callback">�ص�����</param>
    public void Register(string eventName, Action<string> callback)
    {
        if (string.IsNullOrEmpty(eventName))
        {
            Debug.LogError("�¼�������Ϊ�գ�");
            return;
        }

        if (callback == null)
        {
            Debug.LogError("�ص���������Ϊ�գ�");
            return;
        }

        if (!eventDictionary.ContainsKey(eventName))
        {
            eventDictionary.Add(eventName, null);
        }

        eventDictionary[eventName] += callback;
    }

    /// <summary>
    /// ע���¼�����
    /// </summary>
    public void Unregister(string eventName, Action<string> callback)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] -= callback;
        }
    }

    /// <summary>
    /// ������Ϣ����������
    /// </summary>
    public void Send(string eventName, string message = null)
    {
        if (eventDictionary.TryGetValue(eventName, out Action<string> callback))
        {
            callback?.Invoke(message);
        }
        else
        {
            Debug.LogWarning($"δע����¼�: {eventName}");
        }
    }

    /// <summary>
    /// ��������¼�����
    /// </summary>
    public void ClearAll()
    {
        eventDictionary.Clear();
    }
    #endregion

    #region �Զ�������չ����ѡ��
    // ΪMonoBehaviour�����ṩ���Զ�ע������
    public void RegisterWithCleanup(string eventName, Action<string> callback, MonoBehaviour owner)
    {
        Register(eventName, callback);

        // ��������������ʱ�Զ�ע��
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
