using OfficeOpenXml.Packaging;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    [Header("������")]
    public Transform targetModel;  // ��Ҫ�����3Dģ��
    public Text uiText;           // UI�������
    public GameObject uiObj; //UI���ֺͱ���

    [Header("λ������")]
    public float verticalOffset = 0.05f; // ������ֱƫ��
    public float scaleFactor = 0.1f;    // �ߴ�Ӱ��ϵ��

    private Camera mainCamera;
    private Renderer modelRenderer;

    void Start()
    {
        targetModel = transform.parent.parent.parent;
        mainCamera = Camera.main;
        modelRenderer = targetModel.GetComponent<Renderer>();
        //Debug.Log(transform.parent.transform.eulerAngles.y + "   "+ transform.parent.transform .name+ "   " + transform.parent.parent.transform.eulerAngles.y);
        //Debug.Log(transform.parent.transform.localEulerAngles.y + "   "+ transform.parent.transform.name + "   " + transform.parent.parent.transform.localEulerAngles.y);

        //transform.parent.eulerAngles = new Vector3(0, -(transform.parent.parent.transform.eulerAngles.y), 0);
        UpdateWorldSpacePosition();
        uiText.text = GetMiddleChars(targetModel.gameObject);
        if (transform.parent.parent.parent.GetComponent<ChildFacingController>() != null)
        {
            transform.parent.parent.parent.GetComponent<ChildFacingController>().enabled = true;

        }


        // ʵ���� Text �����ص���ǰ����
        //textInstance = Instantiate(textPrefab, transform);
        uiObj.transform.localPosition = Vector3.zero;

        // ��������ߴ磨�� MeshRenderer ʱʹ�� Collider ���ֶ��ߴ磩
        Vector3 objectSize = CalculateObjectSize();

        // ���� Text �ı���ƫ��λ��
        Vector3 finalOffset = new Vector3(
            offsetRatio.x * objectSize.x,
            offsetRatio.y * objectSize.y,
            offsetRatio.z * objectSize.z
        );
        uiObj.transform.localPosition = finalOffset;

        // ��ѡ�����������С���� Text ����
        Text textComponent = uiText.GetComponent<Text>();
        //textComponent.fontSize = (int)(objectSize.magnitude * 10); // ����ʵ���������
        //uiText.fontSize = 8;

    }


    // ʾ������ȡ��4-6���ַ���������0��ʼ��
    string GetMiddleChars(GameObject obj)
    {

        return obj.name.Substring(3);

    }

    void Update()
    {
        UpdateWorldSpacePosition();
    }

    void UpdateWorldSpacePosition()
    {
        //float modelHeight = modelRenderer.bounds.size.y;
        //Vector3 targetPosition = targetModel.position +
        //                        Vector3.up * (modelHeight + verticalOffset);

        //// ֱ������3Dλ��
        //transform.position = targetPosition;
        // ����������������꣬������ƫ�ƣ�����߶�ƫ�ƣ�
        //Vector3 worldPosition = targetModel.position + new Vector3(0, 1, 0);
        //uiText.transform.position = worldPosition;

        //// ʼ���������
        //transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
        //                mainCamera.transform.rotation * Vector3.up);
    }

    public GameObject textPrefab; // ���� Text Ԥ����
    public Vector3 offsetRatio = new Vector3(0, 1, 0); // ƫ�Ʊ�������������߶ȣ�

    //private GameObject textInstance;


    Vector3 CalculateObjectSize()
    {
        // ���� 1������ Collider��ʹ�� Collider �ĳߴ�
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            if (collider is BoxCollider box)
                return box.size;
            else if (collider is SphereCollider sphere)
                return Vector3.one * sphere.radius * 2;
            else if (collider is CapsuleCollider capsule)
                return new Vector3(capsule.radius * 2, capsule.height, capsule.radius * 2);
        }

        // ���� 2�����ֶ����óߴ磬ͨ���ű���������
        return Vector3.one; // Ĭ�Ϸ��ص�λ�ߴ�
    }

    void LateUpdate()
    {
        // ʹText������ʼ�ճ������
        uiObj.transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
        // ��ѡ������X��Z����ת������ˮƽ
        // transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}
