using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);

        // 绘制朝向指示
        Gizmos.color = Color.blue;
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 2;
        Gizmos.DrawRay(transform.position, forward);
    }
}
