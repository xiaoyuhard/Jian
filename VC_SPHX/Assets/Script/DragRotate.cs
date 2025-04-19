using UnityEngine;

public class DragRotate : MonoBehaviour
{
    public bool isEnable = true;
    [SerializeField, Header("旋转设置"),Range(0.1f, 5f)] private float sensitivity = 2f;
    [SerializeField, Header("旋转限制"), Range(0f, 85f)] private float maxPitchAngle = 65f;
    [SerializeField, Header("线性设置"), Range(0.1f, 10f)] private float smoothSpeed = 10f;

    private Vector2 _mouseReference;
    private Vector2 _mouseDelta;
    private Vector3 _rotation;
    private bool _isDragging;
    private bool invertVertical = false;


    void Start()
    {
        // 初始化当前旋转角度
        _rotation = transform.eulerAngles;
    }

    void Update()
    {
        if (!isEnable) return;

        HandleMouseInput();
        ApplyRotation();
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartRotation();
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopRotation();
        }

        if (_isDragging)
        {
            CalculateDelta();
        }
    }

    void StartRotation()
    {
        _isDragging = true;
        _mouseReference = Input.mousePosition;
    }

    void CalculateDelta()
    {
        // 计算标准化的鼠标增量
        Vector2 currentPos = Input.mousePosition;
        _mouseDelta = (currentPos - _mouseReference) * (sensitivity * 0.01f);
        _mouseReference = currentPos;

        // 应用垂直反转设置
        float verticalModifier = invertVertical ? -1f : 1f;

        // 计算各轴旋转量
        _rotation.y -= _mouseDelta.x;       // 水平移动对应Y轴旋转

        //约束角度值
        if (_rotation.y > 360) 
        {
            _rotation.y -= 360;
        }
        if (_rotation.y < 0) 
        {
            _rotation.y += 360;
        }

        //限定模型背面时上下旋转的方向
        if (_rotation.y > 90 && _rotation.y < 270)
        {
            invertVertical = false;
        }
        else 
        {
            invertVertical = true;
        }
        _rotation.x -= _mouseDelta.y * verticalModifier; // 垂直移动对应X轴旋转

        // 限制俯仰角度
        _rotation.x = Mathf.Clamp(_rotation.x, -maxPitchAngle, maxPitchAngle);
    }

    void ApplyRotation()
    {
        if (!_isDragging) return;

        // 使用四元数创建无Z轴旋转
        Quaternion target = Quaternion.Euler(_rotation.x, _rotation.y, 0f);

        // 平滑插值旋转
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            target,
            Time.deltaTime * smoothSpeed
        );
    }

    void StopRotation()
    {
        _isDragging = false;
        Cursor.visible = true;
    }
}