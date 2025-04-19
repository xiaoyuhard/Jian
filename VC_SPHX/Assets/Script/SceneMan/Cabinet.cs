using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabinet : MonoBehaviour
{
    #region Configuration
    [Header("��������")]
    [Tooltip("���ſ�������")]
    [SerializeField] private Animation _doorAnimation;

    [Tooltip("ѵ��ģʽ��������")]
    [SerializeField] private Material _trainingHighlight;
    #endregion

    #region Interaction Logic
    /// <summary>
    /// ������ӽ�������Ҫ����
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
    /// ����ѵ��ģʽ�ı�׼����
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
    /// ������ģʽ�����⽻��
    /// </summary>
    private void HandleAssessmentInteraction()
    {
        System.Random rand = new System.Random();
        int randomOutcome = rand.Next(1, 101);

        if (randomOutcome > 30) // 70%�ɹ���
        {
            Debug.Log("������Ʒ��ȡ�ɹ�");
            AssessmentManager.Instance.RecordSuccessfulInteraction();
        }
        else
        {
            Debug.Log("������Ʒ��ȡʧ��");
            AssessmentManager.Instance.RecordFailedInteraction();
        }
    }
    #endregion

    #region Visual Effects
    /// <summary>
    /// Ӧ�ø����Ӿ�Ч��
    /// </summary>
    /// <param name="enable">�Ƿ����ø���</param>
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
