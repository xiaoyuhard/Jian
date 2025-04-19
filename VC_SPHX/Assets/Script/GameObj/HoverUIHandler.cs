using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AddBoxCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class HoverUIHandler : MonoBehaviour
{
    public GameObject hoverUI; // ������Ҫ��ʾ��UI����
    public float maxDistance = 100f; // ���߼��������
    public float sphereRadius = 0.5f; // �������뾶
    private void Awake()
    {
        hoverUI=transform.GetComponentInChildren<Text>().gameObject;
    }
    private void Start()
    {
    }

    private void Update()
    {
        //if(Camera.main == null) return;
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;

        //// ��������Ƿ����3Dģ��
        //if (Physics.Raycast(ray, out hit, maxDistance))
        ////if (Physics.SphereCast(ray, sphereRadius, out hit))
        //{
        //    if (hit.transform.name == transform.name)
        //    {
        //        // �������Ŀ��ģ�ͣ���ʾUI
        //        hoverUI.SetActive(true);
        //    }
        //    else
        //    {
        //        hoverUI.SetActive(false);
        //    }
        //    // ����UIλ�õ����λ�ã���ѡ��
        //    //hoverUI.transform.position = Input.mousePosition + new Vector3(20, -20, 0); // ƫ�Ʒ�ֹ�ڵ�
        //}

    }
}
