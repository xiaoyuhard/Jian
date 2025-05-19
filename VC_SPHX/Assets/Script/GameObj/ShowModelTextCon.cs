using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowModelTextCon : MonoBehaviour
{
    public List<GameObject> modelList;
    public GameObject canText;
    public Transform targetModel;  // ��Ҫ�����3Dģ��

    // Start is called before the first frame update
    void Start()
    {
        foreach (var model in modelList)
        {
            //GameObject obj = Instantiate(targetModel.gameObject, model.transform);
            //obj.SetActive(true);
            //obj.transform.localPosition = Vector3.zero;
            SpawnFloatText(model.transform, GetMiddleChars(targetModel.gameObject));
            //model.AddComponent<HoverUIHandler>();
        }

    }
    //������������Ư��
    void SpawnFloatText(Transform triggerObj, string name)
    {
        var textObj = Instantiate(targetModel, triggerObj);
        textObj.gameObject.SetActive(false);
        Transform parObj = textObj.parent;
        var col = triggerObj.GetComponent<BoxCollider>();
        textObj.GetComponent<TextMeshPro>().text = GetMiddleChars(parObj.gameObject);
        Vector3 textPos = triggerObj.transform.position;
        textPos.y += col.size.y;
        textPos.y += 0.2f;
        textObj.transform.position = textPos;
        textObj.transform.parent = triggerObj.transform;

    }

    // ʾ������ȡ��4-6���ַ���������0��ʼ��
    string GetMiddleChars(GameObject obj)
    {

        return obj.name.Substring(3);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
