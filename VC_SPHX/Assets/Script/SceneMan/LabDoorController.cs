using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabDoorController : MonoBehaviour
{
    [SerializeField] private int _labID; // ��Inspector�����ö�Ӧ��ʵ���ұ��
    [SerializeField] private Collider _doorCollider;
    [SerializeField] private Outline _outline;

    /// <summary>
    /// ��ʼ����״̬
    /// </summary>
    public void InitializeDoor()
    {
        bool isSelectedLab = JoinGameManager.Instance.CanAccessLab(_labID);

        _doorCollider.enabled = isSelectedLab;
        _outline.enabled = isSelectedLab;

        // ���ò�����ɫ
        GetComponent<Renderer>().material.color = isSelectedLab ?
            Color.green : new Color(0.5f, 0.5f, 0.5f, 0.5f);
    }

    /// <summary>
    /// �����ŵ���¼�������EventTrigger��
    /// </summary>
    public void OnDoorClicked()
    {
        if (JoinGameManager.Instance.CanAccessLab(_labID))
        {
            EnvironmentManager.Instance.SwitchArea("Lab");
        }
        //else
        //{
        //    ShowAccessDeniedMessage();
        //}
    }

    private void ShowAccessDeniedMessage()
    {
        //FloatingTextUI.Instance.ShowMessage(
        //    "������ɵ�ǰѡ���ʵ��������",
        //    transform.position + Vector3.up * 2,
        //    3f
        //);
    }
}
