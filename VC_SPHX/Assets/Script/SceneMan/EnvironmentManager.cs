using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineTriggerAction.ActionSettings;

public class EnvironmentManager : MonoBehaviour
{
    #region Singleton Pattern
    public static EnvironmentManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeCorridor();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    // 新增实验室门引用数组
    [SerializeField] private LabDoorController[] _labDoors;

    /// <summary>
    /// 初始化走廊状态
    /// </summary>
    public void InitializeCorridor()
    {
        foreach (var door in _labDoors)
        {
            door.InitializeDoor();
        }

        // 其他初始化逻辑...
    }

    #region Scene References
    [Header("区域根节点")]
    [Tooltip("走廊区域根物体")]
    public GameObject CorridorRoot;

    [Tooltip("更衣室区域根物体")]
    public GameObject DressingRoomRoot;

    [Tooltip("实验室区域根物体")]
    public GameObject LabRoot;

    [Header("玩家出生点")]
    [Tooltip("走廊出生点数组：0-默认位置 1-从更衣室返回")]
    public Transform[] CorridorSpawnPoints = new Transform[2];

    [Tooltip("更衣室出生点")]
    public Transform DressingRoomSpawnPoint;

    [Tooltip("实验室出生点")]
    public Transform LabSpawnPoint;
    #endregion

    #region Area Transition
    /// <summary>
    /// 切换当前显示的区域
    /// </summary>
    /// <param name="targetArea">目标区域</param>
    /// <param name="spawnIndex">出生点索引（仅走廊有效）</param>
    public void SwitchArea(string targetArea, int spawnIndex = 0)
    {
        // 关闭所有区域
        SetAreaActive(CorridorRoot, false);
        SetAreaActive(DressingRoomRoot, false);
        SetAreaActive(LabRoot, false);

        // 处理目标区域
        switch (targetArea.ToLower())
        {
            case "corridor":
                HandleCorridorTransition(spawnIndex);
                break;

            case "dressingroom":
                HandleDressingRoomTransition();
                break;

            case "lab":
                HandleLabTransition();
                break;

            default:
                Debug.LogError($"未知区域: {targetArea}");
                break;
        }

        UpdateHighlights();
    }

    /// <summary>
    /// 处理走廊区域切换逻辑
    /// </summary>
    /// <param name="spawnIndex">出生点索引</param>
    private void HandleCorridorTransition(int spawnIndex)
    {
        SetAreaActive(CorridorRoot, true);
        TeleportPlayer(CorridorSpawnPoints[spawnIndex].position);
        Debug.Log($"Entered corridor at spawn point {spawnIndex}");
    }

    /// <summary>
    /// 处理更衣室区域切换逻辑
    /// </summary>
    private void HandleDressingRoomTransition()
    {
        SetAreaActive(DressingRoomRoot, true);
        TeleportPlayer(DressingRoomSpawnPoint.position);
        Debug.Log("Entered dressing room");
    }

    /// <summary>
    /// 处理实验室区域切换逻辑
    /// </summary>
    private void HandleLabTransition()
    {
        SetAreaActive(LabRoot, true);
        TeleportPlayer(LabSpawnPoint.position);
        Debug.Log("Entered lab");
    }
    #endregion

    #region Utility Methods
    /// <summary>
    /// 设置区域物体的激活状态
    /// </summary>
    /// <param name="area">区域根物体</param>
    /// <param name="state">激活状态</param>
    private void SetAreaActive(GameObject area, bool state)
    {
        if (area != null)
        {
            area.SetActive(state);
        }
    }

    /// <summary>
    /// 传送玩家到指定位置
    /// </summary>
    /// <param name="targetPosition">目标坐标</param>
    private void TeleportPlayer(Vector3 targetPosition)
    {
        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.Teleport(targetPosition);
        }
        else
        {
            Debug.LogWarning("PlayerController instance not found!");
        }
    }

    /// <summary>
    /// 更新所有高亮物体的显示状态
    /// </summary>
    public void UpdateHighlights()
    {
        // 实现具体的物体高亮逻辑
        // 根据游戏状态控制不同物体的Outline组件
    }

    /// <summary>
    /// 禁用所有高亮效果
    /// </summary>
    public void DisableAllHighlights()
    {
        // 遍历所有可高亮物体并禁用Outline组件
    }
    #endregion
}
