using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(BoxCollider))]

public class AddBoxCollider : MonoBehaviour
{
    void Start()
    {
        // 动态添加 BoxCollider
        if (gameObject.GetComponent<BoxCollider>() == null)
        {
            BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
            MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

            if (renderers.Length == 0)
            {
                Debug.LogError("未找到 MeshRenderer！");
                return;
            }

            // 计算合并后的包围盒
            Bounds combinedBounds = renderers[0].bounds;
            foreach (MeshRenderer renderer in renderers)
            {
                combinedBounds.Encapsulate(renderer.bounds);
            }

            // 设置 Collider 的中心和尺寸
            boxCollider.center = combinedBounds.center - transform.position;    
            boxCollider.size = combinedBounds.size;

            //fix 有些模型旋转导致collider位置不对
            if (boxCollider.center.y < 0)
            {
                var newCenter = boxCollider.center;
                newCenter.y *= -1;
                boxCollider.center = newCenter;
            }

            // 考虑物体缩放（可选）
            boxCollider.size = new Vector3(
                boxCollider.size.x / transform.lossyScale.x,
                boxCollider.size.y / transform.lossyScale.y,
                boxCollider.size.z / transform.lossyScale.z
            );

        }

        // 获取模型所有子物体的 MeshRenderer（确保子物体已激活）
      
    }

    // Update is called once per frame
    void Update()
    {

    }
}
