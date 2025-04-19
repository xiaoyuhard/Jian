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
    // ����ʵ��������������
    [SerializeField] private LabDoorController[] _labDoors;

    /// <summary>
    /// ��ʼ������״̬
    /// </summary>
    public void InitializeCorridor()
    {
        foreach (var door in _labDoors)
        {
            door.InitializeDoor();
        }

        // ������ʼ���߼�...
    }

    #region Scene References
    [Header("������ڵ�")]
    [Tooltip("�������������")]
    public GameObject CorridorRoot;

    [Tooltip("���������������")]
    public GameObject DressingRoomRoot;

    [Tooltip("ʵ�������������")]
    public GameObject LabRoot;

    [Header("��ҳ�����")]
    [Tooltip("���ȳ��������飺0-Ĭ��λ�� 1-�Ӹ����ҷ���")]
    public Transform[] CorridorSpawnPoints = new Transform[2];

    [Tooltip("�����ҳ�����")]
    public Transform DressingRoomSpawnPoint;

    [Tooltip("ʵ���ҳ�����")]
    public Transform LabSpawnPoint;
    #endregion

    #region Area Transition
    /// <summary>
    /// �л���ǰ��ʾ������
    /// </summary>
    /// <param name="targetArea">Ŀ������</param>
    /// <param name="spawnIndex">��������������������Ч��</param>
    public void SwitchArea(string targetArea, int spawnIndex = 0)
    {
        // �ر���������
        SetAreaActive(CorridorRoot, false);
        SetAreaActive(DressingRoomRoot, false);
        SetAreaActive(LabRoot, false);

        // ����Ŀ������
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
                Debug.LogError($"δ֪����: {targetArea}");
                break;
        }

        UpdateHighlights();
    }

    /// <summary>
    /// �������������л��߼�
    /// </summary>
    /// <param name="spawnIndex">����������</param>
    private void HandleCorridorTransition(int spawnIndex)
    {
        SetAreaActive(CorridorRoot, true);
        TeleportPlayer(CorridorSpawnPoints[spawnIndex].position);
        Debug.Log($"Entered corridor at spawn point {spawnIndex}");
    }

    /// <summary>
    /// ��������������л��߼�
    /// </summary>
    private void HandleDressingRoomTransition()
    {
        SetAreaActive(DressingRoomRoot, true);
        TeleportPlayer(DressingRoomSpawnPoint.position);
        Debug.Log("Entered dressing room");
    }

    /// <summary>
    /// ����ʵ���������л��߼�
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
    /// ������������ļ���״̬
    /// </summary>
    /// <param name="area">���������</param>
    /// <param name="state">����״̬</param>
    private void SetAreaActive(GameObject area, bool state)
    {
        if (area != null)
        {
            area.SetActive(state);
        }
    }

    /// <summary>
    /// ������ҵ�ָ��λ��
    /// </summary>
    /// <param name="targetPosition">Ŀ������</param>
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
    /// �������и����������ʾ״̬
    /// </summary>
    public void UpdateHighlights()
    {
        // ʵ�־������������߼�
        // ������Ϸ״̬���Ʋ�ͬ�����Outline���
    }

    /// <summary>
    /// �������и���Ч��
    /// </summary>
    public void DisableAllHighlights()
    {
        // �������пɸ������岢����Outline���
    }
    #endregion
}
