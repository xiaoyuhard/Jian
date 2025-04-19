using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Singleton Pattern
    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    #endregion

    #region Movement Components
    [Tooltip("角色控制器组件")]
    [SerializeField] private CharacterController _characterController;
    #endregion

    #region Teleportation
    /// <summary>
    /// 传送玩家到指定位置
    /// </summary>
    /// <param name="targetPosition">目标坐标</param>
    public void Teleport(Vector3 targetPosition)
    {
        if (_characterController != null)
        {
            _characterController.enabled = false;
            transform.position = targetPosition;
            _characterController.enabled = true;
            Debug.Log($"Player teleported to: {targetPosition}");
        }
        else
        {
            Debug.LogError("CharacterController component missing!");
        }
    }
    #endregion
}
