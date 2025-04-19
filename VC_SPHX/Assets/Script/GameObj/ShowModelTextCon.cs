using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowModelTextCon : MonoBehaviour
{
    public List<GameObject> modelList;
    public GameObject canText;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var model in modelList)
        {
            GameObject obj = Instantiate(canText, model.transform);
            obj.SetActive(true);
            obj.transform.localPosition = Vector3.zero;

            model.AddComponent<HoverUIHandler>();

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
