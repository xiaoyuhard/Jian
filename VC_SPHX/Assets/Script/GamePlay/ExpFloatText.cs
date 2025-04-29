using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

/// <summary>
/// ʵ��������Ư��
/// <see cref="ShowModelTextCon"/>
/// </summary>
public class ExpFloatText : MonoBehaviour
{
    public TextMeshPro txt;
    public GameObject arrow;

    private Camera mainCamera;
    Vector3 offsetRatio = new Vector3(0, 1, 0);

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        //transform.localPosition = Vector3.zero;
        //// ��������ߴ磨�� MeshRenderer ʱʹ�� Collider ���ֶ��ߴ磩
        //Vector3 colliderSize = CalculateObjectSize();
        //Vector3 newPos = Vector3.zero;
        //newPos.y = colliderSize.y;
        //newPos.y += 0.1f;


        //if (Mathf.Abs(transform.parent.localRotation.x) > 90 || Mathf.Abs(transform.parent.localRotation.z) > 90)
        //    newPos.y *= -1;

        //// ���� Text �ı���ƫ��λ��
        //Vector3 finalOffset = new Vector3(
        //    offsetRatio.x * colliderSize.x,
        //    offsetRatio.y * colliderSize.y,
        //    offsetRatio.z * colliderSize.z
        //);

        //transform.localPosition = newPos;
        arrow.transform.DOLocalMoveY(0.074f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// <see cref="FloatingText"/>
    /// </summary>
    private void LateUpdate()
    {
        // ʹText������ʼ�ճ������
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
        // ��ѡ������X��Z����ת������ˮƽ
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    Vector3 CalculateObjectSize()
    {
        // ���� 1������ Collider��ʹ�� Collider �ĳߴ�
        Collider collider = GetComponentInParent<Collider>();

        if (collider != null)
        {
            if (collider is BoxCollider box)
                return box.size;
            else if (collider is SphereCollider sphere)
                return Vector3.one * sphere.radius * 2;
            else if (collider is CapsuleCollider capsule)
                return new Vector3(capsule.radius * 2, capsule.height, capsule.radius * 2);
        }

        // ���� 2�����ֶ����óߴ磬ͨ���ű���������
        return new Vector3(0, 1, 0); // Ĭ�Ϸ��ص�λ�ߴ�
    }

}
