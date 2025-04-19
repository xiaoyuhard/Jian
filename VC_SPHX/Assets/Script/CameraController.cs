using UnityEngine;
using UnityEngine.EventSystems;

namespace RTS 
{
    public class AdvancedCameraController : MonoBehaviour
    {
        [Header("Runtime Value")]
        public Vector3 pivotPoint; // 相机围绕转动的坐标点，聚焦时可通过控制该坐标的位置
        public float distance; // 相机与中心点的当前距离
        public float xRotation;
        public float yRotation;

        [Header("Speed Settings")]
        public float rotateSpeed = 5.0f; // 相机旋转的速度
        public float moveSpeed = 5.0f; // 相机移动的速度
        public float zoomSpeed = 5.0f; // 相机缩放的速度

        [Header("SmoothSpeed Settings")]
        public float rotateSmoothSpeed = 5.0f;//相机旋转平滑速度
        public float moveSmoothSpeed = 5.0f;//相机移动平滑速度

        [Header("Constraints Value Settings")]
        public float lockHeight = 0.0f;//相机聚焦锁定高度
        public float minDistance = 2.0f; // 相机与中心点的最小距离
        public float maxDistance = 50.0f; // 相机与中心点的最大距离

        void Start()
        {
            // 初始化相机与中心点的距离
            distance = Vector3.Distance(transform.position, pivotPoint);
            // 初始化旋转角度
            Vector3 angles = transform.eulerAngles;
            xRotation = angles.y;
            yRotation = angles.x;
        }

        void Update()
        {

            // 鼠标滚轮控制距离
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (!EventSystem.current.IsPointerOverGameObject()) 
            {
                distance -= scroll * zoomSpeed;
                distance = Mathf.Clamp(distance, minDistance, maxDistance);
            } 

            // 鼠标左键移动
            if (Input.GetMouseButton(2) && !EventSystem.current.IsPointerOverGameObject())
            {
                float xMove = Input.GetAxis("Mouse X") * moveSpeed;
                float zMove = Input.GetAxis("Mouse Y") * moveSpeed;
                pivotPoint -= transform.right * xMove;
                pivotPoint -= transform.up * zMove;
                pivotPoint.y = Mathf.Clamp(pivotPoint.y, lockHeight, lockHeight);
            }

            // 鼠标右键旋转
            if (Input.GetMouseButton(1)&&!EventSystem.current.IsPointerOverGameObject())
            {
                xRotation += Input.GetAxis("Mouse X") * rotateSpeed;
                yRotation -= Input.GetAxis("Mouse Y") * rotateSpeed;
                yRotation = Mathf.Clamp(yRotation, 0f, 90f); // 防止垂直方向旋转超过90度
            }

            // 应用旋转
            Quaternion rotation = Quaternion.Euler(yRotation, xRotation, 0);
            Vector3 targetPosition = pivotPoint - rotation * Vector3.forward * distance;
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSmoothSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSmoothSpeed * Time.deltaTime);
        }
    }
}