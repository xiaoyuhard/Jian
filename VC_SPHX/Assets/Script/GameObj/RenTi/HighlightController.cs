using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class HighlightController : MonoBehaviour
{
    [Header("��������")]
    public Material highlightMaterial; // �������ʣ��������Ч����

    // �Զ���ȡ�Ĺ������
    private Renderer _renderer;
    private Material[] _originalMaterials;
    private List<Material> _materialInstances = new List<Material>();

    // �㼶��ϵ
    private Transform _rootParent;
    private List<Transform> _siblingChildren = new List<Transform>();

    void Start()
    {
        InitializeComponents();
        CacheHierarchyInfo();
        CreateMaterialInstances();
    }

    void InitializeComponents()
    {
        _renderer = GetComponent<Renderer>();
        _originalMaterials = _renderer.sharedMaterials;
    }

    void CacheHierarchyInfo()
    {
        // ���Ҹ�������
        _rootParent = transform;
        while (_rootParent.parent != null)
        {
            _rootParent = _rootParent.parent;
        }

        // ��������ͬ�������壨��������
        _siblingChildren = new List<Transform>(_rootParent.GetComponentsInChildren<Transform>(true));
    }

    void CreateMaterialInstances()
    {
        foreach (var mat in _originalMaterials)
        {
            Material instanceMat = new Material(mat);
            _materialInstances.Add(instanceMat);
        }
        _renderer.materials = _materialInstances.ToArray();
    }

    public void SetHighlight(bool isHighlighted)
    {
        if (isHighlighted)
        {
            ApplyHighlightMaterials();
        }
        else
        {
            RestoreOriginalMaterials();
        }
    }

    void ApplyHighlightMaterials()
    {
        Material[] newMats = new Material[_materialInstances.Count];
        for (int i = 0; i < newMats.Length; i++)
        {
            newMats[i] = highlightMaterial;
        }
        _renderer.materials = newMats;
    }

    void RestoreOriginalMaterials()
    {
        _renderer.materials = _materialInstances.ToArray();
    }

    public Transform GetRootParent() => _rootParent;
    public List<Transform> GetSiblings() => _siblingChildren;
}
