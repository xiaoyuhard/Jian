using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 膳食门口碰撞开门及把门重置到初始状态
/// </summary>
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

        if (transform.name == "营养师")
        {
            //GameManager.Instance.SetStepDetection(true);
            ChooseFoodAllInformCon.Instance.EnableInform();

        }
        if (transform.name == "健康一体机"||transform.name== "服务台")
        {
            //GameManager.Instance.SetStepDetection(true);
            GameObjMan.Instance.CLoseFirst();

        }
        if (nurseObj == null) return;

        nurseObj.GetComponent<Animator>().enabled = true;

    }

    private void OnTriggerExit(Collider other)
    {
        if (nurseObj == null) return;

        nurseObj.GetComponent<Animator>().Rebind();
        nurseObj.GetComponent<Animator>().Update(0);
        nurseObj.GetComponent<Animator>().enabled = false;

    }
}
