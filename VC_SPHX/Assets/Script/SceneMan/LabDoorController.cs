using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabDoorController : MonoBehaviour
{
    [SerializeField] private int _labID; // 在Inspector中设置对应的实验室编号
    [SerializeField] private Collider _doorCollider;
    [SerializeField] private Outline _outline;

    /// <summary>
    /// 初始化门状态
    /// </summary>
    public void InitializeDoor()
    {
        bool isSelectedLab = JoinGameManager.Instance.CanAccessLab(_labID);

        _doorCollider.enabled = isSelectedLab;
        _outline.enabled = isSelectedLab;

        // 设置材质颜色
        GetComponent<Renderer>().material.color = isSelectedLab ?
            Color.green : new Color(0.5f, 0.5f, 0.5f, 0.5f);
    }

    /// <summary>
    /// 处理门点击事件（挂载EventTrigger）
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
        //    "请先完成当前选择的实验室流程",
        //    transform.position + Vector3.up * 2,
        //    3f
        //);
    }
}
