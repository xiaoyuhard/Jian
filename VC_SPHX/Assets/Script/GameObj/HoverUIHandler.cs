using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AddBoxCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class HoverUIHandler : MonoBehaviour
{
    public GameObject hoverUI; // 拖入需要显示的UI对象
    public float maxDistance = 100f; // 射线检测最大距离
    public float sphereRadius = 0.5f; // 检测球体半径
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

        //// 检测射线是否击中3D模型
        //if (Physics.Raycast(ray, out hit, maxDistance))
        ////if (Physics.SphereCast(ray, sphereRadius, out hit))
        //{
        //    if (hit.transform.name == transform.name)
        //    {
        //        // 如果击中目标模型，显示UI
        //        hoverUI.SetActive(true);
        //    }
        //    else
        //    {
        //        hoverUI.SetActive(false);
        //    }
        //    // 更新UI位置到鼠标位置（可选）
        //    //hoverUI.transform.position = Input.mousePosition + new Vector3(20, -20, 0); // 偏移防止遮挡
        //}

    }
}
