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
    /// 开始考核计时
    /// </summary>
    public void StartAssessmentTimer()
    {
        _startTime = Time.time;
        Debug.Log("Assessment timer started");
    }

    /// <summary>
    /// 记录成功交互
    /// </summary>
    public void RecordSuccessfulInteraction()
    {
        _successCount++;
        UpdateScoreDisplay();
    }

    /// <summary>
    /// 记录失败交互
    /// </summary>
    public void RecordFailedInteraction()
    {
        _failureCount++;
        UpdateScoreDisplay();
    }
    #endregion

    #region Score Calculation
    /// <summary>
    /// 计算最终考核分数
    /// </summary>
    /// <returns>考核分数（0-100）</returns>
    public float CalculateFinalScore()
    {
        float timeTaken = Time.time - _startTime;
        float timeScore = Mathf.Clamp(1 - (timeTaken / 300f), 0, 1) * 40f; // 时间分占40%
        float accuracyScore = (_successCount / (float)(_successCount + _failureCount)) * 60f; // 准确率占60%
        return Mathf.Clamp(timeScore + accuracyScore, 0, 100);
    }
    #endregion

    #region UI Updates
    /// <summary>
    /// 更新分数显示
    /// </summary>
    private void UpdateScoreDisplay()
    {
        // 实现具体的UI更新逻辑
        Debug.Log($"Current Score - Success: {_successCount}, Failures: {_failureCount}");
    }
    #endregion
}
