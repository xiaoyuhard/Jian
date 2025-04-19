using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(BoxCollider))]

public class AddBoxCollider : MonoBehaviour
{
    void Start()
    {
        // ��̬��� BoxCollider
        if (gameObject.GetComponent<BoxCollider>() == null)
        {
            BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
            MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

            if (renderers.Length == 0)
            {
                Debug.LogError("δ�ҵ� MeshRenderer��");
                return;
            }

            // ����ϲ���İ�Χ��
            Bounds combinedBounds = renderers[0].bounds;
            foreach (MeshRenderer renderer in renderers)
            {
                combinedBounds.Encapsulate(renderer.bounds);
            }

            // ���� Collider �����ĺͳߴ�
            boxCollider.center = combinedBounds.center - transform.position;
            boxCollider.size = combinedBounds.size;

            // �����������ţ���ѡ��
            boxCollider.size = new Vector3(
                boxCollider.size.x / transform.lossyScale.x,
                boxCollider.size.y / transform.lossyScale.y,
                boxCollider.size.z / transform.lossyScale.z
            );

        }

        // ��ȡģ������������� MeshRenderer��ȷ���������Ѽ��
      
    }

    // Update is called once per frame
    void Update()
    {

    }
}
