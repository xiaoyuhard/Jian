using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameConst
{

}

/// <summary>
/// ��������
/// </summary>
public class GameScene
{
    public const string Exp_HuaXue = "ʳƷ��ѧ";
    public const string Exp3_ZhongJinShu = "Exp3-�ؽ����ⶨ";
    public const string Exp4_ShaChongJi = "Exp4_ShaChongJi";
    public const string Exp6_ZhiFang1 = "Exp6_ZhiFang1";
    public const string Exp6_ZhiFang2 = "Exp6_ZhiFang1";
    public const string Exp6_ZhiFang3 = "Exp6_ZhiFang1";
    public const string Exp7_DanBaiZhi_ShouDong = "Exp7-�����ʲⶨ-���϶�����֮�ֶ���";
    public const string Exp7_DanBaiZhi_ZiDong = "Exp7-�����ʲⶨ-�Զ����϶�����";
}

public class GameTag
{
    public const string Player = "Player";
}

public class EventName
{
    public const string Main = "ʳƷ��ѧ";

    /// <summary>
    /// ��ʾ��ǰ�����ͼƬ
    /// </summary>
    public const string UI_ShowPicture = "UI_ShowPicture";

    /// <summary>
    /// ʵ�飬������һ��
    /// </summary>
    public const string Exp_NextStep = "Exp_NextStep";
}

/// <summary>
/// ʵ���������
/// </summary>
public enum Experiment
{
    /// <summary>
    /// δָ��
    /// </summary>
    Unknown = -1,
    /// <summary>
    /// ʳƷ�а����Ậ�����
    /// </summary>
    AnJiSuan,
    /// <summary>
    /// ʳƷ�������ɷַ���
    /// </summary>
    XiangQi,
    /// <summary>
    /// ũ��Ʒ�ؽ����������
    /// </summary>
    ZhongJinShu,
    /// <summary>
    /// ũ��Ʒ�л���ɱ����������
    /// </summary>
    ShaChongJi,
    /// <summary>
    /// ��ԭ�ζ����ⶨʳƷ�л�ԭ�Ǻ���
    /// </summary>
    Tang,
    /// <summary>
    /// ��ԭ������ȡ���ⶨ֬������
    /// </summary>
    ZhiFang,
    /// <summary>
    /// ��ԭ�����ʺ����Ĳⶨ
    /// </summary>
    DanBaiZhi,
    /// <summary>
    /// ��ʳ������Ӫ�����ʵϰ��Ŀ
    /// </summary>
    PeiCan,
    /// <summary>
    /// ����Ӫ�����
    /// </summary>
    GeRenYingYang,
    /// <summary>
    /// �������ֽ���
    /// </summary>
    RenTi,
}

[Serializable]
public class ExpStep
{
    public string name;
    [HideInInspector]
    public int index;
    public ExpActionType type = ExpActionType.None;
    /// <summary>
    /// ��ײ/�������������
    /// </summary>
    public GameObject triggerObj;
    [HideInInspector]
    public bool triggerObjSelfActive;
    /// <summary>
    /// ������ʾ������
    /// </summary>
    public GameObject lightObj;
    /// <summary>
    /// ���Ŷ���timeline�ļ�
    /// </summary>
    public PlayableDirector director;
    /// <summary>
    /// ��Ҫչʾ��ͼƬ
    /// </summary>
    public Sprite[] pictures;
}

/// <summary>
/// ʵ���������
/// </summary>
public enum ExpActionType
{
    None,
    /// <summary>
    /// ��ײ����
    /// </summary>
    TriggerObject,
    /// <summary>
    /// ����򿪸����ҹ���
    /// </summary>
    ClickOpenCabinet,
    /// <summary>
    /// ������岥�Ŷ���
    /// </summary>
    ClickPlayAnim,
    /// <summary>
    /// ���������ʾͼƬ
    /// </summary>
    ClickShowImage,
}