using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickManager : MonoSingletonBase<ClickManager>
{
    [Header("ȫ������")]
    public float fadeAlpha = 0.2f;
    public LayerMask interactableLayer;

    public List<HighlightController> _allControllers = new List<HighlightController>();
    private HighlightController _currentHighlight;

    public Button btn1;
    public Button btn2;
    public Button btn3;
    public Button btn4;
    public Button btn5;
    
    

    void Start()
    {
        btn1.onClick.AddListener(ClickObj1);
        btn2.onClick.AddListener(ClickObj2);
        btn3.onClick.AddListener(ClickObj3);
        btn4.onClick.AddListener(ClickObj4);
        btn5.onClick.AddListener(ClickObj5);
    }

    private void OnEnable()
    {
        InitializeAllControllers();
    }

    void InitializeAllControllers()
    {
        _allControllers.Clear();
        var controllers = FindObjectsOfType<HighlightController>();
        _allControllers.AddRange(controllers);
    }

    public void ClickObj1()
    {
        var controller = _allControllers[0].GetComponent<HighlightController>();
        HandleSelection(controller);
    }
    public void ClickObj2()
    {
        var controller = _allControllers[1].GetComponent<HighlightController>();
        HandleSelection(controller);
    }
    public void ClickObj3()
    {
        var controller = _allControllers[2].GetComponent<HighlightController>();
        HandleSelection(controller);
    }
    public void ClickObj4()
    {
        var controller = _allControllers[3].GetComponent<HighlightController>();
        HandleSelection(controller);
    }
    public void ClickObj5()
    {
        var controller = _allControllers[4].GetComponent<HighlightController>();
        HandleSelection(controller);
    }


    void HandleSelection(HighlightController selected)
    {
        // ������������
        ResetAllObjects();

        // ����ǰѡ������
        _currentHighlight = selected;

        // ��ȡ���������
        Transform rootParent = selected.GetRootParent();
        List<Transform> siblings = selected.GetSiblings();

        // ���ø�����͸��
        foreach (var controller in _allControllers)
        {
            bool isSameRoot = controller.GetRootParent() == rootParent;
            bool isSibling = siblings.Contains(controller.transform);

            if (controller == selected)
            {
                controller.SetHighlight(true);
            }
            else if (isSameRoot && isSibling)
            {
                SetObjectTransparency(controller, fadeAlpha);
            }
            else
            {
                SetObjectTransparency(controller, fadeAlpha);
            }
        }
    }

    void ResetAllObjects()
    {
        foreach (var controller in _allControllers)
        {
            controller.SetHighlight(false);
            ResetObjectTransparency(controller);
        }
    }

    void SetObjectTransparency(HighlightController controller, float alpha)
    {
        foreach (var mat in controller.GetComponent<Renderer>().materials)
        {
            Color color = mat.color;
            color.a = alpha;
            mat.color = color;

            // ����͸����Ⱦģʽ
            mat.SetFloat("_Mode", 2);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.renderQueue = 3000;
        }
    }

    void ResetObjectTransparency(HighlightController controller)
    {
        foreach (var mat in controller.GetComponent<Renderer>().materials)
        {
            Color color = mat.color;
            color.a = 1f;
            mat.color = color;

            // �ָ���͸��ģʽ
            mat.SetFloat("_Mode", 0);
            mat.DisableKeyword("_ALPHABLEND_ON");
            mat.renderQueue = -1;
        }
    }
}
