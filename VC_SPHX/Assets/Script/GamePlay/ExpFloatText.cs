using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

/// <summary>
/// 实验点击物体漂字
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
        //// 计算物体尺寸（无 MeshRenderer 时使用 Collider 或手动尺寸）
        //Vector3 colliderSize = CalculateObjectSize();
        //Vector3 newPos = Vector3.zero;
        //newPos.y = colliderSize.y;
        //newPos.y += 0.1f;


        //if (Mathf.Abs(transform.parent.localRotation.x) > 90 || Mathf.Abs(transform.parent.localRotation.z) > 90)
        //    newPos.y *= -1;

        //// 设置 Text 的本地偏移位置
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
        // 使Text的正面始终朝向相机
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
        // 可选：锁定X和Z轴旋转，保持水平
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    Vector3 CalculateObjectSize()
    {
        // 方法 1：若有 Collider，使用 Collider 的尺寸
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

        // 方法 2：若手动设置尺寸，通过脚本参数传入
        return new Vector3(0, 1, 0); // 默认返回单位尺寸
    }

}
