using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjMove : MonoBehaviour
{
    [Header("�ƶ���Χ����")]
    public float minX = -5f;  // X����Сֵ
    public float maxX = 5f;   // X�����ֵ
    public float minY = -5f;  // Y����Сֵ��2D��Ϸ���ã�
    public float maxY = 5f;   // Y�����ֵ��2D��Ϸ���ã�
    public float minZ = -5f;  // Z����Сֵ��3D��Ϸ��
    public float maxZ = 5f;   // Z�����ֵ��3D��Ϸ��

    void Update()
    {
        Vector3 currentPos = transform.position;

        // ���Ƹ����������趨��Χ��
        float clampedX = Mathf.Clamp(currentPos.x, minX, maxX);
        float clampedY = Mathf.Clamp(currentPos.y, minY, maxY);
        float clampedZ = Mathf.Clamp(currentPos.z, minZ, maxZ);

        // Ӧ�����ƺ������
        transform.position = new Vector3(clampedX, clampedY, clampedZ);
    }
}
