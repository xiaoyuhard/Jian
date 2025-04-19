using UnityEngine;

public class DragRotate : MonoBehaviour
{
    public bool isEnable = true;
    [SerializeField, Header("��ת����"),Range(0.1f, 5f)] private float sensitivity = 2f;
    [SerializeField, Header("��ת����"), Range(0f, 85f)] private float maxPitchAngle = 65f;
    [SerializeField, Header("��������"), Range(0.1f, 10f)] private float smoothSpeed = 10f;

    private Vector2 _mouseReference;
    private Vector2 _mouseDelta;
    private Vector3 _rotation;
    private bool _isDragging;
    private bool invertVertical = false;


    void Start()
    {
        // ��ʼ����ǰ��ת�Ƕ�
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
        // �����׼�����������
        Vector2 currentPos = Input.mousePosition;
        _mouseDelta = (currentPos - _mouseReference) * (sensitivity * 0.01f);
        _mouseReference = currentPos;

        // Ӧ�ô�ֱ��ת����
        float verticalModifier = invertVertical ? -1f : 1f;

        // ���������ת��
        _rotation.y -= _mouseDelta.x;       // ˮƽ�ƶ���ӦY����ת

        //Լ���Ƕ�ֵ
        if (_rotation.y > 360) 
        {
            _rotation.y -= 360;
        }
        if (_rotation.y < 0) 
        {
            _rotation.y += 360;
        }

        //�޶�ģ�ͱ���ʱ������ת�ķ���
        if (_rotation.y > 90 && _rotation.y < 270)
        {
            invertVertical = false;
        }
        else 
        {
            invertVertical = true;
        }
        _rotation.x -= _mouseDelta.y * verticalModifier; // ��ֱ�ƶ���ӦX����ת

        // ���Ƹ����Ƕ�
        _rotation.x = Mathf.Clamp(_rotation.x, -maxPitchAngle, maxPitchAngle);
    }

    void ApplyRotation()
    {
        if (!_isDragging) return;

        // ʹ����Ԫ��������Z����ת
        Quaternion target = Quaternion.Euler(_rotation.x, _rotation.y, 0f);

        // ƽ����ֵ��ת
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