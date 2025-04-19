using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjItem : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 30f; // ��ת�ٶȣ���/�룩
    [SerializeField] private Vector3 axis = Vector3.up; // ��ת�ᣨĬ����Y�ᣩ

    void Update()
    {
        // ��ָ�����Թ̶��ٶ���ת
        transform.Rotate(axis, rotateSpeed * Time.deltaTime);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }


}
