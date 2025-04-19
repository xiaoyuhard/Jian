using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssessmentManager : MonoBehaviour
{
    #region Singleton Pattern
    public static AssessmentManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    #endregion

    #region Assessment Data
    private int _successCount = 0;
    private int _failureCount = 0;
    private float _startTime;
    #endregion

    #region Public Methods
    /// <summary>
    /// ��ʼ���˼�ʱ
    /// </summary>
    public void StartAssessmentTimer()
    {
        _startTime = Time.time;
        Debug.Log("Assessment timer started");
    }

    /// <summary>
    /// ��¼�ɹ�����
    /// </summary>
    public void RecordSuccessfulInteraction()
    {
        _successCount++;
        UpdateScoreDisplay();
    }

    /// <summary>
    /// ��¼ʧ�ܽ���
    /// </summary>
    public void RecordFailedInteraction()
    {
        _failureCount++;
        UpdateScoreDisplay();
    }
    #endregion

    #region Score Calculation
    /// <summary>
    /// �������տ��˷���
    /// </summary>
    /// <returns>���˷�����0-100��</returns>
    public float CalculateFinalScore()
    {
        float timeTaken = Time.time - _startTime;
        float timeScore = Mathf.Clamp(1 - (timeTaken / 300f), 0, 1) * 40f; // ʱ���ռ40%
        float accuracyScore = (_successCount / (float)(_successCount + _failureCount)) * 60f; // ׼ȷ��ռ60%
        return Mathf.Clamp(timeScore + accuracyScore, 0, 100);
    }
    #endregion

    #region UI Updates
    /// <summary>
    /// ���·�����ʾ
    /// </summary>
    private void UpdateScoreDisplay()
    {
        // ʵ�־����UI�����߼�
        Debug.Log($"Current Score - Success: {_successCount}, Failures: {_failureCount}");
    }
    #endregion
}
