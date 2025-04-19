using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinGameManager : MonoBehaviour
{
    // 新增实验室锁定状态
    private int _selectedLabID = -1; // -1表示未选择
    private bool _isLabSelected = false;

    /// <summary>
    /// 设置当前选择的实验室（从UI按钮调用）
    /// </summary>
    public void SelectLaboratory(int labID)
    {
        if (!_isLabSelected)
        {
            _selectedLabID = labID;
            _isLabSelected = true;
            Debug.Log($"已选择实验室：Lab_{labID}");

            // 禁用其他实验室按钮
            LaboratoryUI.Instance.DisableOtherButtons(labID);
        }
    }

    /// <summary>
    /// 检查是否可以进入指定实验室
    /// </summary>
    public bool CanAccessLab(int labID)
    {
        return _isLabSelected && labID == _selectedLabID;
    }

    /// <summary>
    /// 重置实验室选择状态（返回主菜单时调用）
    /// </summary>
    public void ResetLabSelection()
    {
        _selectedLabID = -1;
        _isLabSelected = false;
        LaboratoryUI.Instance.EnableAllButtons();
    }
    public static JoinGameManager Instance { get; private set; }

    /// <summary>
    /// 初始化单例实例
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGameState();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Game State
    //public enum GameMode { Training, Assessment }


    [Tooltip("当前游戏模式")]
    public GameMode CurrentMode = GameMode.Training;

    [Tooltip("玩家进度状态")]
    public ProgressState CurrentProgress = ProgressState.Initial;

    /// <summary>
    /// 初始化游戏状态（每次模式切换时调用）
    /// </summary>
    public void InitializeGameState()
    {
        CurrentProgress = ProgressState.Initial;
        Debug.Log($"Game state initialized for {CurrentMode} mode");
    }
    #endregion

    #region Mode Management
    /// <summary>
    /// 切换到考核模式并重置所有状态
    /// </summary>
    public void SwitchToAssessmentMode()
    {
        CurrentMode = GameMode.Assessment;
        InitializeGameState();
        EnvironmentManager.Instance.DisableAllHighlights();
        Debug.Log("Switched to assessment mode");
    }
    #endregion
}

public enum GameMode { Training, Assessment }
public enum ProgressState
{
    Initial,
    EnteredDressingRoom,
    Dressed,
    EnteredLab
}