using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinGameManager : MonoBehaviour
{
    // ����ʵ��������״̬
    private int _selectedLabID = -1; // -1��ʾδѡ��
    private bool _isLabSelected = false;

    /// <summary>
    /// ���õ�ǰѡ���ʵ���ң���UI��ť���ã�
    /// </summary>
    public void SelectLaboratory(int labID)
    {
        if (!_isLabSelected)
        {
            _selectedLabID = labID;
            _isLabSelected = true;
            Debug.Log($"��ѡ��ʵ���ң�Lab_{labID}");

            // ��������ʵ���Ұ�ť
            LaboratoryUI.Instance.DisableOtherButtons(labID);
        }
    }

    /// <summary>
    /// ����Ƿ���Խ���ָ��ʵ����
    /// </summary>
    public bool CanAccessLab(int labID)
    {
        return _isLabSelected && labID == _selectedLabID;
    }

    /// <summary>
    /// ����ʵ����ѡ��״̬���������˵�ʱ���ã�
    /// </summary>
    public void ResetLabSelection()
    {
        _selectedLabID = -1;
        _isLabSelected = false;
        LaboratoryUI.Instance.EnableAllButtons();
    }
    public static JoinGameManager Instance { get; private set; }

    /// <summary>
    /// ��ʼ������ʵ��
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


    [Tooltip("��ǰ��Ϸģʽ")]
    public GameMode CurrentMode = GameMode.Training;

    [Tooltip("��ҽ���״̬")]
    public ProgressState CurrentProgress = ProgressState.Initial;

    /// <summary>
    /// ��ʼ����Ϸ״̬��ÿ��ģʽ�л�ʱ���ã�
    /// </summary>
    public void InitializeGameState()
    {
        CurrentProgress = ProgressState.Initial;
        Debug.Log($"Game state initialized for {CurrentMode} mode");
    }
    #endregion

    #region Mode Management
    /// <summary>
    /// �л�������ģʽ����������״̬
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