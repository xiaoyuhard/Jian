using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabinet : MonoBehaviour
{
    #region Configuration
    [Header("动画设置")]
    [Tooltip("柜门开启动画")]
    [SerializeField] private Animation _doorAnimation;

    [Tooltip("训练模式高亮材质")]
    [SerializeField] private Material _trainingHighlight;
    #endregion

    #region Interaction Logic
    /// <summary>
    /// 处理柜子交互的主要方法
    /// </summary>
    public void ProcessInteraction()
    {
        if (JoinGameManager.Instance.CurrentMode == GameMode.Training)
        {
            PlayTrainingAnimation();
        }
        else
        {
            HandleAssessmentInteraction();
        }
    }

    /// <summary>
    /// 播放训练模式的标准动画
    /// </summary>
    private void PlayTrainingAnimation()
    {
        if (_doorAnimation != null)
        {
            _doorAnimation.Play("CabinetOpen");
            ApplyHighlightEffect(true);
        }
    }

    /// <summary>
    /// 处理考核模式的特殊交互
    /// </summary>
    private void HandleAssessmentInteraction()
    {
        System.Random rand = new System.Random();
        int randomOutcome = rand.Next(1, 101);

        if (randomOutcome > 30) // 70%成功率
        {
            Debug.Log("考核物品获取成功");
            AssessmentManager.Instance.RecordSuccessfulInteraction();
        }
        else
        {
            Debug.Log("考核物品获取失败");
            AssessmentManager.Instance.RecordFailedInteraction();
        }
    }
    #endregion

    #region Visual Effects
    /// <summary>
    /// 应用高亮视觉效果
    /// </summary>
    /// <param name="enable">是否启用高亮</param>
    private void ApplyHighlightEffect(bool enable)
    {
        if (JoinGameManager.Instance.CurrentMode == GameMode.Training)
        {
            GetComponent<Renderer>().material = enable ?
                _trainingHighlight :
                GetComponent<Renderer>().material;
        }
    }
    #endregion
}
