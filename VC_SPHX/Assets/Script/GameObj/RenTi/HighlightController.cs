using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class HighlightController : MonoBehaviour
{
    [Header("材质设置")]
    public Material highlightMaterial; // 高亮材质（需带发光效果）

    // 自动获取的关联组件
    private Renderer _renderer;
    private Material[] _originalMaterials;
    private List<Material> _materialInstances = new List<Material>();

    // 层级关系
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
        // 查找根父物体
        _rootParent = transform;
        while (_rootParent.parent != null)
        {
            _rootParent = _rootParent.parent;
        }

        // 缓存所有同级子物体（包括自身）
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
