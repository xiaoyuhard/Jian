using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Cinemachine.CinemachineTriggerAction.ActionSettings;
using static GameManager;

public class InteractionDetector : MonoSingletonBase<InteractionDetector>
{
    #region Configuration
    [Header("��������")]
    [Tooltip("��󽻻����루�ף�")]
    [SerializeField] private float _interactionRange = 5f;

    [Tooltip("�ɽ�������㼶")]
    [SerializeField] private LayerMask _interactableLayer;
    #endregion

    #region Core Logic
    /// <summary>
    /// ÿ֡���������¼�
    /// </summary>
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)&& !EventSystem.current.IsPointerOverGameObject())
        {
            AttemptInteraction();
        }
    }

    /// <summary>
    /// ���Խ��н�������
    /// </summary>
    private void AttemptInteraction()
    {
        Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(interactionRay, out RaycastHit hitInfo, _interactionRange, _interactableLayer))
        {
            ProcessInteraction(hitInfo.collider);
        }
    }

    /// <summary>
    /// �����������߼�
    /// </summary>
    /// <param name="interactedCollider">��ײ����������ײ��</param>
    private void ProcessInteraction(Collider interactedCollider)
    {
        if (interactedCollider.CompareTag("LabDoor"))
        {
            // ֱ����LabDoorController����
            interactedCollider.GetComponent<LabDoorController>().OnDoorClicked();
        }
        switch (interactedCollider.tag)
        {
            case "DressingRoomDoor":
                HandleDoorInteraction(interactedCollider.gameObject);
                break;

            case "LabDoor":
                HandleDoorInteraction(interactedCollider.gameObject);
                break;

            case "Cabinet":
                HandleCabinetInteraction(interactedCollider.GetComponent<Cabinet>());
                break;

            default:
                Debug.Log($"δ֪��������: {interactedCollider.name}");
                break;
        }
    }
    #endregion

    #region Interaction Handlers
    /// <summary>
    /// ���������͵Ľ���
    /// </summary>
    /// <param name="doorObject">������</param>
    private void HandleDoorInteraction(GameObject doorObject)
    {
        if (doorObject.CompareTag("DressingRoomDoor"))
        {
            EnvironmentManager.Instance.SwitchArea("DressingRoom");
            UpdateGameProgress(ProgressState.EnteredDressingRoom);
        }
        else if (doorObject.CompareTag("LabDoor"))
        {
            if (JoinGameManager.Instance.CurrentProgress >= ProgressState.Dressed)
            {
                EnvironmentManager.Instance.SwitchArea("Lab");
                UpdateGameProgress(ProgressState.EnteredLab);
            }
            else
            {
                Debug.Log("��Ҫ����ɸ��²��ܽ���ʵ����");
            }
        }
    }

    /// <summary>
    /// ������ӽ����߼�
    /// </summary>
    /// <param name="cabinet">�������</param>
    private void HandleCabinetInteraction(Cabinet cabinet)
    {
        if (cabinet != null)
        {
            cabinet.ProcessInteraction();
            if (JoinGameManager.Instance.CurrentMode == GameMode.Training)
            {
                EnvironmentManager.Instance.SwitchArea("Corridor", 1);
                UpdateGameProgress(ProgressState.Dressed);
            }
        }
    }
    #endregion

    #region Progress Tracking
    /// <summary>
    /// ������Ϸ����״̬
    /// </summary>
    /// <param name="newState">�µĽ���״̬</param>
    private void UpdateGameProgress(ProgressState newState)
    {
        JoinGameManager.Instance.CurrentProgress = newState;
        Debug.Log($"Progress updated to: {newState}");
    }
    #endregion
}