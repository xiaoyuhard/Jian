using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 切换人体模型材质球
/// </summary>
public class MaterialSwitcher : MonoBehaviour
{
    [Header("基础配置")]
    public int targetMaterialIndices; // 要操作的材质索引


    [Header("运行时数据")]
    public List<MaterialInfo> runtimeMaterialInfos = new List<MaterialInfo>(); // 自动填充的贴图信息

    public List<string> targetColorProperties = new List<string> { "_Base_Color", "_Fresnel_Color_01"/*, "_Color", "_MainColor"*/ }; // 常见颜色属性名

    private MeshRenderer meshRenderer;
    private Material[] originalMaterials;
    private Dictionary<int, Material> cachedTempMaterials = new Dictionary<int, Material>();

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalMaterials = meshRenderer.materials.Clone() as Material[];

        // 自动或手动初始化材质信息
        InitializeMaterialInfos();
    }

    // 初始化材质贴图信息
    void InitializeMaterialInfos()
    {
        runtimeMaterialInfos.Clear();

        {
            targetMaterialIndices = meshRenderer.materials.Length;
            // 自动检测所有目标材质的贴图属性
            for (int j = 0; j < targetMaterialIndices; j++)
            {
                if (j >= originalMaterials.Length) continue;

                Material mat = originalMaterials[j];
                Shader shader = mat.shader;
                //foreach (var item in targetColorProperties)
                //{
                //    Debug.Log(mat.GetColor(item));

                //}
                //for (int i = 0; i < shader.GetPropertyCount(); i++)
                //{
                //    string propName = shader.GetPropertyName(i);
                //    if (shader.GetPropertyName(i) == null)
                //    {
                //        Debug.Log("wu 16546541265");
                //    }
                //    Debug.Log($"属性名: {propName}, 类型: {shader.GetPropertyType(i)}");
                //}
                for (int i = 0; i < shader.GetPropertyCount(); i++)
                {
                    if (shader.GetPropertyType(i) == ShaderPropertyType.Texture)
                    {
                        string propertyName = shader.GetPropertyName(i);
                        //bool isPaExist = false;
                        //foreach (var item in targetColorProperties)
                        //{
                        //    if (item == propertyName)
                        //    {
                        //        isPaExist = true;
                        //    }
                        //}
                        //Debug.Log(propertyName + "   texture name");
                        //if (isPaExist)
                        {
                            Texture tex = mat.GetTexture(propertyName);
                            //Debug.Log(propertyName + " add  texture name");

                            runtimeMaterialInfos.Add(new MaterialInfo
                            {
                                materialIndex = j,
                                texturePropertyName = propertyName,
                                originalTexture = tex,
                                //colorPropertyName = targetColorProperties[0],
                                //fresnelColorName = targetColorProperties[1],
                                //originalColor = mat.GetColor(targetColorProperties[0]),
                                //fresnelColor = mat.GetColor(targetColorProperties[1])
                            });
                        }

                    }

                }
                // 检测颜色属性
                foreach (string colorProp in targetColorProperties)
                {
                    if (mat.HasProperty(colorProp))
                    {
                        runtimeMaterialInfos.Add(new MaterialInfo
                        {
                            materialIndex = j,
                            colorPropertyName = colorProp,
                            originalColor = mat.GetColor(colorProp)

                        });
                    }
                }

            }

        }

    }

    // 切换到临时材质
    public void ApplyTemporaryMaterial(Material tempMaterialTemplate)
    {
        Material[] currentMats = meshRenderer.materials;

        foreach (var info in runtimeMaterialInfos)
        {
            int index = info.materialIndex;
            if (index >= currentMats.Length) continue;

            // 获取或创建缓存的临时材质
            if (!cachedTempMaterials.ContainsKey(index))
            {
                Material tempMat = new Material(tempMaterialTemplate);
                cachedTempMaterials[index] = tempMat;
            }

            Material tempMatInstance = cachedTempMaterials[index];

            // 注入所有相关贴图
            if (!string.IsNullOrEmpty(info.texturePropertyName) && tempMatInstance.HasProperty(info.texturePropertyName))
            {
                tempMatInstance.SetTexture(info.texturePropertyName, info.originalTexture);
            }
            // 注入颜色 
            if (!string.IsNullOrEmpty(info.colorPropertyName) && tempMatInstance.HasProperty(info.colorPropertyName))
            {
                tempMatInstance.SetColor(info.colorPropertyName, info.originalColor);
                tempMatInstance.SetColor(info.fresnelColorName, info.fresnelColor);
            }

            currentMats[index] = tempMatInstance;
        }

        meshRenderer.materials = currentMats;
    }

    // 还原到原始材质
    public void RestoreOriginalMaterials()
    {
        meshRenderer.materials = originalMaterials;
        ClearTempMaterials();
    }

    // 清理临时材质
    void ClearTempMaterials()
    {
        foreach (var mat in cachedTempMaterials.Values)
        {
            Destroy(mat);
        }
        cachedTempMaterials.Clear();
    }

    void OnDestroy()
    {
        ClearTempMaterials();
    }
}
[System.Serializable]
public class MaterialInfo
{
    public int materialIndex;          // 要替换的材质索引
    public string texturePropertyName; // 原材质的贴图属性名（如 "_MainTex"）
    public string colorPropertyName;   // 颜色属性名（如 "_Color"）
    public string fresnelColorName;

    public Texture originalTexture;    // 动态记录的原始贴图（无需手动配置）
    public Color originalColor;       // 原始颜色（运行时自动记录）
    public Color fresnelColor;
}