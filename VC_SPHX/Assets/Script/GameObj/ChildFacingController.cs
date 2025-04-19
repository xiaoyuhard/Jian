using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildFacingController : MonoBehaviour
{
    [Header("全局朝向设置")]
    public Vector3 WorldFacingDirection = Vector3.forward; // 世界坐标朝向
    public bool UseLocalSpace = false;                    // 使用本地坐标系
    public bool LockYAxis = true;                        // 锁定Y轴旋转

    public Transform childrenTran;

    void Start()
    {

        childrenTran = transform.GetComponentInChildren<Canvas>().transform;

        UpdateAllChildrenRotation();

    }

    void Update()
    {
        // 如果父物体旋转变化时更新
        if (transform.hasChanged)
        {
            UpdateAllChildrenRotation();
            transform.hasChanged = false;
        }
    }


    void UpdateAllChildrenRotation()
    {

        Vector3 targetDirection = GetTargetDirection(childrenTran);
        ApplyRotation(childrenTran, targetDirection);

    }

    Vector3 GetTargetDirection(Transform child)
    {
        if (UseLocalSpace)
        {
            // 本地坐标系下的方向（例如始终朝父物体前方）
            return transform.TransformDirection(WorldFacingDirection);
        }
        else
        {
            // 世界坐标系固定方向
            return WorldFacingDirection;
        }
    }

    void ApplyRotation(Transform child, Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // 锁定Y轴处理
        if (LockYAxis)
        {
            Vector3 euler = targetRotation.eulerAngles;
            euler.y = child.eulerAngles.y; // 保持原有Y轴
            targetRotation = Quaternion.Euler(euler);
        }

        child.rotation = targetRotation;
    }

    // 编辑器快捷操作
    [ContextMenu("立即刷新所有子物体朝向")]
    public void RefreshAll()
    {
        UpdateAllChildrenRotation();
    }
}
