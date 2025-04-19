using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QitiShiCon : MonoBehaviour
{
    public GameObject guiZi;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag != "Player") return;
        //LabSystemManager.Instance.HighlightObject(anObj);
        guiZi.SetActive(true);
        LabSystemManager.Instance.HighlightObject(guiZi);

    }
}
