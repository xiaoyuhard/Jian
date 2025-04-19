using UnityEngine;
using UnityEngine.EventSystems;

namespace RTS 
{
    public class AdvancedCameraController : MonoBehaviour
    {
        [Header("Runtime Value")]
        public Vector3 pivotPoint; // ���Χ��ת��������㣬�۽�ʱ��ͨ�����Ƹ������λ��
        public float distance; // ��������ĵ�ĵ�ǰ����
        public float xRotation;
        public float yRotation;

        [Header("Speed Settings")]
        public float rotateSpeed = 5.0f; // �����ת���ٶ�
        public float moveSpeed = 5.0f; // ����ƶ����ٶ�
        public float zoomSpeed = 5.0f; // ������ŵ��ٶ�

        [Header("SmoothSpeed Settings")]
        public float rotateSmoothSpeed = 5.0f;//�����תƽ���ٶ�
        public float moveSmoothSpeed = 5.0f;//����ƶ�ƽ���ٶ�

        [Header("Constraints Value Settings")]
        public float lockHeight = 0.0f;//����۽������߶�
        public float minDistance = 2.0f; // ��������ĵ����С����
        public float maxDistance = 50.0f; // ��������ĵ��������

        void Start()
        {
            // ��ʼ����������ĵ�ľ���
            distance = Vector3.Distance(transform.position, pivotPoint);
            // ��ʼ����ת�Ƕ�
            Vector3 angles = transform.eulerAngles;
            xRotation = angles.y;
            yRotation = angles.x;
        }

        void Update()
        {

            // �����ֿ��ƾ���
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (!EventSystem.current.IsPointerOverGameObject()) 
            {
                distance -= scroll * zoomSpeed;
                distance = Mathf.Clamp(distance, minDistance, maxDistance);
            } 

            // �������ƶ�
            if (Input.GetMouseButton(2) && !EventSystem.current.IsPointerOverGameObject())
            {
                float xMove = Input.GetAxis("Mouse X") * moveSpeed;
                float zMove = Input.GetAxis("Mouse Y") * moveSpeed;
                pivotPoint -= transform.right * xMove;
                pivotPoint -= transform.up * zMove;
                pivotPoint.y = Mathf.Clamp(pivotPoint.y, lockHeight, lockHeight);
            }

            // ����Ҽ���ת
            if (Input.GetMouseButton(1)&&!EventSystem.current.IsPointerOverGameObject())
            {
                xRotation += Input.GetAxis("Mouse X") * rotateSpeed;
                yRotation -= Input.GetAxis("Mouse Y") * rotateSpeed;
                yRotation = Mathf.Clamp(yRotation, 0f, 90f); // ��ֹ��ֱ������ת����90��
            }

            // Ӧ����ת
            Quaternion rotation = Quaternion.Euler(yRotation, xRotation, 0);
            Vector3 targetPosition = pivotPoint - rotation * Vector3.forward * distance;
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSmoothSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSmoothSpeed * Time.deltaTime);
        }
    }
}