using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjMove : MonoBehaviour
{
    [Header("移动范围限制")]
    public float minX = -5f;  // X轴最小值
    public float maxX = 5f;   // X轴最大值
    public float minY = -5f;  // Y轴最小值（2D游戏可用）
    public float maxY = 5f;   // Y轴最大值（2D游戏可用）
    public float minZ = -5f;  // Z轴最小值（3D游戏）
    public float maxZ = 5f;   // Z轴最大值（3D游戏）

    void Update()
    {
        Vector3 currentPos = transform.position;

        // 限制各轴坐标在设定范围内
        float clampedX = Mathf.Clamp(currentPos.x, minX, maxX);
        float clampedY = Mathf.Clamp(currentPos.y, minY, maxY);
        float clampedZ = Mathf.Clamp(currentPos.z, minZ, maxZ);

        // 应用限制后的坐标
        transform.position = new Vector3(clampedX, clampedY, clampedZ);
    }
}
