using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildFacingController : MonoBehaviour
{
    [Header("ȫ�ֳ�������")]
    public Vector3 WorldFacingDirection = Vector3.forward; // �������곯��
    public bool UseLocalSpace = false;                    // ʹ�ñ�������ϵ
    public bool LockYAxis = true;                        // ����Y����ת

    public Transform childrenTran;

    void Start()
    {

        childrenTran = transform.GetComponentInChildren<Canvas>().transform;

        UpdateAllChildrenRotation();

    }

    void Update()
    {
        // �����������ת�仯ʱ����
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
            // ��������ϵ�µķ�������ʼ�ճ�������ǰ����
            return transform.TransformDirection(WorldFacingDirection);
        }
        else
        {
            // ��������ϵ�̶�����
            return WorldFacingDirection;
        }
    }

    void ApplyRotation(Transform child, Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // ����Y�ᴦ��
        if (LockYAxis)
        {
            Vector3 euler = targetRotation.eulerAngles;
            euler.y = child.eulerAngles.y; // ����ԭ��Y��
            targetRotation = Quaternion.Euler(euler);
        }

        child.rotation = targetRotation;
    }

    // �༭����ݲ���
    [ContextMenu("����ˢ�����������峯��")]
    public void RefreshAll()
    {
        UpdateAllChildrenRotation();
    }
}
