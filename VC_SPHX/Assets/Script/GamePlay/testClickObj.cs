using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class testClickObj : MonoBehaviour
{
    public GameObject floatText;
    public GameObject obj;
    public string objName;

    // Start is called before the first frame update
    void Start()
    {
        SpawnFloatText(obj, objName);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnFloatText(GameObject triggerObj, string name)
    {
        if (!GameData.Instance.IsTestMode)
        {
            var textObj = Instantiate(floatText);
            var col = triggerObj.GetComponent<BoxCollider>();
            var center = triggerObj.transform.TransformPoint(col.center);
            Vector3 textPos = center;
            textPos.y += col.size.y * triggerObj.transform.localScale.y * 0.5f;
            textPos.y += 0.2f;
            textObj.transform.position = textPos;
            //textObj.transform.parent = triggerObj.transform;

            var lastText = textObj.GetComponentInChildren<TextMeshPro>();
            lastText.text = name.Trim();
        }
    }
}
