using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameConst
{

}

/// <summary>
/// 场景名字
/// </summary>
public class GameScene
{
    public const string Exp_HuaXue = "食品化学";
    public const string Exp3_ZhongJinShu = "Exp3-重金属测定";
    public const string Exp4_ShaChongJi = "Exp4_ShaChongJi";
    public const string Exp6_ZhiFang1 = "Exp6_ZhiFang1";
    public const string Exp6_ZhiFang2 = "Exp6_ZhiFang1";
    public const string Exp6_ZhiFang3 = "Exp6_ZhiFang1";
    public const string Exp7_DanBaiZhi_ShouDong = "Exp7-蛋白质测定-凯氏定氮法之手动法";
    public const string Exp7_DanBaiZhi_ZiDong = "Exp7-蛋白质测定-自动凯氏定氮法";
}

public class GameTag
{
    public const string Player = "Player";
}

public class EventName
{
    public const string Main = "食品化学";

    /// <summary>
    /// 显示当前步骤的图片
    /// </summary>
    public const string UI_ShowPicture = "UI_ShowPicture";

    /// <summary>
    /// 实验，进行下一步
    /// </summary>
    public const string Exp_NextStep = "Exp_NextStep";
}

/// <summary>
/// 实验操作类型
/// </summary>
public enum Experiment
{
    /// <summary>
    /// 未指定
    /// </summary>
    Unknown = -1,
    /// <summary>
    /// 食品中氨基酸含量检测
    /// </summary>
    AnJiSuan,
    /// <summary>
    /// 食品中香气成分分析
    /// </summary>
    XiangQi,
    /// <summary>
    /// 农产品重金属含量检测
    /// </summary>
    ZhongJinShu,
    /// <summary>
    /// 农产品有机磷杀虫剂残留检测
    /// </summary>
    ShaChongJi,
    /// <summary>
    /// 还原滴定法测定食品中还原糖含量
    /// </summary>
    Tang,
    /// <summary>
    /// 还原索氏提取法测定脂肪含量
    /// </summary>
    ZhiFang,
    /// <summary>
    /// 还原蛋白质含量的测定
    /// </summary>
    DanBaiZhi,
    /// <summary>
    /// 膳食分析与营养配餐实习项目
    /// </summary>
    PeiCan,
    /// <summary>
    /// 个人营养配餐
    /// </summary>
    GeRenYingYang,
    /// <summary>
    /// 人体数字解剖
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
    /// 碰撞/点击触发的物体
    /// </summary>
    public GameObject triggerObj;
    [HideInInspector]
    public bool triggerObjSelfActive;
    /// <summary>
    /// 高亮显示的物体
    /// </summary>
    public GameObject lightObj;
    /// <summary>
    /// 播放动画timeline文件
    /// </summary>
    public PlayableDirector director;
    /// <summary>
    /// 需要展示的图片
    /// </summary>
    public Sprite[] pictures;
}

/// <summary>
/// 实验操作类型
/// </summary>
public enum ExpActionType
{
    None,
    /// <summary>
    /// 碰撞触发
    /// </summary>
    TriggerObject,
    /// <summary>
    /// 点击打开更衣室柜子
    /// </summary>
    ClickOpenCabinet,
    /// <summary>
    /// 点击物体播放动画
    /// </summary>
    ClickPlayAnim,
    /// <summary>
    /// 点击物体显示图片
    /// </summary>
    ClickShowImage,
}