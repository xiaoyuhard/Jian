using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExpClickObj : MonoBehaviour
{
    public string objName;

    Outline m_Outline;
    GameObject textObj;

    // Start is called before the first frame update
    void Start()
    {
        m_Outline = GetComponent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseEnter()
    {
        //Debug.Log("OnMouseEnter " + name);

        if (this.enabled == false)
            return;

        m_Outline.enabled = true;
        ShowText(true);
    }

    private void OnMouseExit()
    {
        //Debug.Log("OnMouseExit " + name);
        if (this.enabled == false)
            return;
        m_Outline.enabled = false;
        ShowText(false);
    }

    private void OnMouseDown()
    {
        //Debug.Log("OnMouseDown " + name);
        if (this.enabled == false)
            return;
        if (textObj)
        {
            Destroy(textObj);
            textObj = null;
            this.enabled = false;
        }
    }

    void ShowText(bool show)
    {
        if (GameData.Instance.IsTestMode)
            return;

        if (show)
        {
            if (textObj == null)
                SpawnFloatText(gameObject, objName);

            textObj.SetActive(true);
        }
        else
        {
            if (textObj)
                textObj.SetActive(false);
        }
    }

    //生成物体名字漂字
    void SpawnFloatText(GameObject triggerObj, string name)
    {
        if (GameData.Instance.IsTestMode)
            return;

        textObj = Instantiate(ExpStepCtrl.Instance.floatText);
        var col = triggerObj.GetComponent<BoxCollider>();
        var center = triggerObj.transform.TransformPoint(col.center);
        Vector3 textPos = center;
        textPos.y += col.size.y * triggerObj.transform.localScale.y * 0.5f;
        textPos.y += 0.2f;
        textObj.transform.position = textPos;
        //textObj.transform.parent = triggerObj.transform;
        textObj.transform.SetParent(triggerObj.transform);
        textObj.SetActive(true);

        var lastText = textObj.GetComponentInChildren<TextMeshPro>();
        lastText.text = name.Trim();
    }

}
