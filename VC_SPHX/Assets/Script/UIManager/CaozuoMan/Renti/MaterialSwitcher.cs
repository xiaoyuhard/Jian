using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// �л�����ģ�Ͳ�����
/// </summary>
public class MaterialSwitcher : MonoBehaviour
{
    [Header("��������")]
    public int targetMaterialIndices; // Ҫ�����Ĳ�������


    [Header("����ʱ����")]
    public List<MaterialInfo> runtimeMaterialInfos = new List<MaterialInfo>(); // �Զ�������ͼ��Ϣ

    public List<string> targetColorProperties = new List<string> { "_Base_Color", "_Fresnel_Color_01"/*, "_Color", "_MainColor"*/ }; // ������ɫ������

    private MeshRenderer meshRenderer;
    private Material[] originalMaterials;
    private Dictionary<int, Material> cachedTempMaterials = new Dictionary<int, Material>();

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalMaterials = meshRenderer.materials.Clone() as Material[];

        // �Զ����ֶ���ʼ��������Ϣ
        InitializeMaterialInfos();
    }

    // ��ʼ��������ͼ��Ϣ
    void InitializeMaterialInfos()
    {
        runtimeMaterialInfos.Clear();

        {
            targetMaterialIndices = meshRenderer.materials.Length;
            // �Զ��������Ŀ����ʵ���ͼ����
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
                //    Debug.Log($"������: {propName}, ����: {shader.GetPropertyType(i)}");
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
                // �����ɫ����
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

    // �л�����ʱ����
    public void ApplyTemporaryMaterial(Material tempMaterialTemplate)
    {
        Material[] currentMats = meshRenderer.materials;

        foreach (var info in runtimeMaterialInfos)
        {
            int index = info.materialIndex;
            if (index >= currentMats.Length) continue;

            // ��ȡ�򴴽��������ʱ����
            if (!cachedTempMaterials.ContainsKey(index))
            {
                Material tempMat = new Material(tempMaterialTemplate);
                cachedTempMaterials[index] = tempMat;
            }

            Material tempMatInstance = cachedTempMaterials[index];

            // ע�����������ͼ
            if (!string.IsNullOrEmpty(info.texturePropertyName) && tempMatInstance.HasProperty(info.texturePropertyName))
            {
                tempMatInstance.SetTexture(info.texturePropertyName, info.originalTexture);
            }
            // ע����ɫ 
            if (!string.IsNullOrEmpty(info.colorPropertyName) && tempMatInstance.HasProperty(info.colorPropertyName))
            {
                tempMatInstance.SetColor(info.colorPropertyName, info.originalColor);
                tempMatInstance.SetColor(info.fresnelColorName, info.fresnelColor);
            }

            currentMats[index] = tempMatInstance;
        }

        meshRenderer.materials = currentMats;
    }

    // ��ԭ��ԭʼ����
    public void RestoreOriginalMaterials()
    {
        meshRenderer.materials = originalMaterials;
        ClearTempMaterials();
    }

    // ������ʱ����
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
    public int materialIndex;          // Ҫ�滻�Ĳ�������
    public string texturePropertyName; // ԭ���ʵ���ͼ���������� "_MainTex"��
    public string colorPropertyName;   // ��ɫ���������� "_Color"��
    public string fresnelColorName;

    public Texture originalTexture;    // ��̬��¼��ԭʼ��ͼ�������ֶ����ã�
    public Color originalColor;       // ԭʼ��ɫ������ʱ�Զ���¼��
    public Color fresnelColor;
}