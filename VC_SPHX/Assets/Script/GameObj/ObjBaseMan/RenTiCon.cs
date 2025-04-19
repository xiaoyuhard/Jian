using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenTiCon : MonoSingletonBase<RenTiCon>
{
    public List<GameObject> bodyManModelList;
    public List<GameObject> bodyWomanModelList;
    // Start is called before the first frame update
    void Start()
    {

    }
    GameObject obj;

    public void ShowBodyWomanModel(string bodyName)
    {
        foreach (GameObject body in bodyWomanModelList)
        {
            var outline = body.GetComponent<Outline>();
            outline.enabled = false;
            if (body.name == bodyName)
            {
                obj = body;

                outline.enabled = true;
                outline.OutlineColor = Color.yellow;
            }
        }
        if (obj.GetComponentsInChildren<GameObject>() != null)

            foreach (GameObject body in obj.GetComponentsInChildren<GameObject>())
            {
                var outline = body.GetComponent<Outline>();
                outline.enabled = true;
                outline.OutlineColor = Color.yellow;
            }
    }
    public void ShowBodyManModel(string bodyName)
    {
        foreach (GameObject body in bodyManModelList)
        {
            var outline = body.GetComponent<Outline>();
            outline.enabled = false;
            if (body.name == bodyName)
            {
                obj = body;

                outline.enabled = true;
                outline.OutlineColor = Color.yellow;
            }
        }
        if (obj.GetComponentsInChildren<GameObject>() != null)
            foreach (GameObject body in obj.GetComponentsInChildren<GameObject>())
            {
                var outline = body.GetComponent<Outline>();
                outline.enabled = true;
                outline.OutlineColor = Color.yellow;
            }
    }
    // Update is called once per frame
    void Update()
    {

    }


    bool isTriOnce = true;
    private void OnTriggerEnter(Collider other)
    {
        if (!isTriOnce) return;

        UIManager.Instance.OpenUICaoZuo("BodyConUI");

        isTriOnce = false;
    }
}
