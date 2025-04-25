using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShanShiDoorItem : MonoBehaviour
{
    public GameObject nurseObj;
    public string uiPanName;

    // Start is called before the first frame update
    void Start()
    {
        

    }
    private void OnEnable()
    {
        if (nurseObj == null) return;
        nurseObj.GetComponent<Animator>().Rebind();
        nurseObj.GetComponent<Animator>().Update(0);
        nurseObj.GetComponent<Animator>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        UIManager.Instance.OpenUICaoZuo(uiPanName);
        if (nurseObj == null) return;

        nurseObj.GetComponent<Animator>().enabled = true;
        if(transform.name== "ÓªÑøÊ¦")
        {
            GameManager.Instance.SetStepDetection(true);

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (nurseObj == null) return;

        nurseObj.GetComponent<Animator>().Rebind();
        nurseObj.GetComponent<Animator>().Update(0);
        nurseObj.GetComponent<Animator>().enabled = false;

    }
}
