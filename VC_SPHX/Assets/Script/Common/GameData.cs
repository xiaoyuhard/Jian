using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ��������
/// </summary>
public class GameData : Singleton<GameData>
{
    public int index = 0;
    /// <summary>
    /// �Ƿ񿼺�ģʽ
    /// </summary>
    public bool IsTestMode = false;
    /// <summary>
    /// ��ǰ���е�ʵ��
    /// </summary>
    public Experiment CurrentExperiment = Experiment.Unknown;
}

public class Singleton<T> where T : new()
{
    private static T _instance;
    static readonly object lockObj = new object();

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (lockObj)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                }
            }

            return _instance;
        }
    }
}