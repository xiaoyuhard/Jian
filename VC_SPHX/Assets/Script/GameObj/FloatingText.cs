using OfficeOpenXml.Packaging;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    [Header("绑定设置")]
    public Transform targetModel;  // 需要跟随的3D模型
    public Text uiText;           // UI文字组件
    public GameObject uiObj; //UI文字和背景

    [Header("位置设置")]
    public float verticalOffset = 0.05f; // 基础垂直偏移
    public float scaleFactor = 0.1f;    // 尺寸影响系数

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


        // 实例化 Text 并挂载到当前物体
        //textInstance = Instantiate(textPrefab, transform);
        uiObj.transform.localPosition = Vector3.zero;

        // 计算物体尺寸（无 MeshRenderer 时使用 Collider 或手动尺寸）
        Vector3 objectSize = CalculateObjectSize();

        // 设置 Text 的本地偏移位置
        Vector3 finalOffset = new Vector3(
            offsetRatio.x * objectSize.x,
            offsetRatio.y * objectSize.y,
            offsetRatio.z * objectSize.z
        );
        uiObj.transform.localPosition = finalOffset;

        // 可选：根据物体大小调整 Text 字体
        Text textComponent = uiText.GetComponent<Text>();
        //textComponent.fontSize = (int)(objectSize.magnitude * 10); // 根据实际需求调整
        //uiText.fontSize = 8;

    }


    // 示例：提取第4-6个字符（索引从0开始）
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

        //// 直接设置3D位置
        //transform.position = targetPosition;
        // 计算物体的世界坐标，并附加偏移（例如高度偏移）
        //Vector3 worldPosition = targetModel.position + new Vector3(0, 1, 0);
        //uiText.transform.position = worldPosition;

        //// 始终面向相机
        //transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
        //                mainCamera.transform.rotation * Vector3.up);
    }

    public GameObject textPrefab; // 拖入 Text 预制体
    public Vector3 offsetRatio = new Vector3(0, 1, 0); // 偏移比例（基于物体高度）

    //private GameObject textInstance;


    Vector3 CalculateObjectSize()
    {
        // 方法 1：若有 Collider，使用 Collider 的尺寸
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

        // 方法 2：若手动设置尺寸，通过脚本参数传入
        return Vector3.one; // 默认返回单位尺寸
    }

    void LateUpdate()
    {
        // 使Text的正面始终朝向相机
        uiObj.transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
        // 可选：锁定X和Z轴旋转，保持水平
        // transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}
