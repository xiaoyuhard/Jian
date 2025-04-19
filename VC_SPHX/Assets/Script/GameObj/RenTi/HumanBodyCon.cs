using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanBodyCon : MonoBehaviour
{
    public List<GameObject> bodyList;
    private Renderer objectRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetBodyObjHigh(int index,bool isHigh)
    {
        bodyList[index].gameObject.SetActive(isHigh);
    }

    // …Ë÷√Õ∏√˜∂»
    public void SetTransparency(float alpha)
    {
        foreach (var mat in objectRenderer.materials)
        {
            Color color = mat.color;
            color.a = alpha;
            mat.color = color;
        }
    }

}
