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
    [Header("交互设置")]
    [Tooltip("最大交互距离（米）")]
    [SerializeField] private float _interactionRange = 5f;

    [Tooltip("可交互物体层级")]
    [SerializeField] private LayerMask _interactableLayer;
    #endregion

    #region Core Logic
    /// <summary>
    /// 每帧检测鼠标点击事件
    /// </summary>
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)&& !EventSystem.current.IsPointerOverGameObject())
        {
            AttemptInteraction();
        }
    }

    /// <summary>
    /// 尝试进行交互操作
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
    /// 处理交互物体逻辑
    /// </summary>
    /// <param name="interactedCollider">碰撞到的物体碰撞器</param>
    private void ProcessInteraction(Collider interactedCollider)
    {
        if (interactedCollider.CompareTag("LabDoor"))
        {
            // 直接由LabDoorController处理
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
                Debug.Log($"未知交互物体: {interactedCollider.name}");
                break;
        }
    }
    #endregion

    #region Interaction Handlers
    /// <summary>
    /// 处理门类型的交互
    /// </summary>
    /// <param name="doorObject">门物体</param>
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
                Debug.Log("需要先完成更衣才能进入实验室");
            }
        }
    }

    /// <summary>
    /// 处理柜子交互逻辑
    /// </summary>
    /// <param name="cabinet">柜子组件</param>
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
    /// 更新游戏进度状态
    /// </summary>
    /// <param name="newState">新的进度状态</param>
    private void UpdateGameProgress(ProgressState newState)
    {
        JoinGameManager.Instance.CurrentProgress = newState;
        Debug.Log($"Progress updated to: {newState}");
    }
    #endregion
}