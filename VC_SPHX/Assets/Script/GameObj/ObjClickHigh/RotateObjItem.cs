using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjItem : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 30f; // 旋转速度（度/秒）
    [SerializeField] private Vector3 axis = Vector3.up; // 旋转轴（默认绕Y轴）

    void Update()
    {
        // 绕指定轴以固定速度旋转
        transform.Rotate(axis, rotateSpeed * Time.deltaTime);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }


}
