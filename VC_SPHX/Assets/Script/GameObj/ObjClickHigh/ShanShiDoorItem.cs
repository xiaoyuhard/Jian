using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ��ʳ�ſ���ײ���ż��������õ���ʼ״̬
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

        if (transform.name == "Ӫ��ʦ")
        {
            //GameManager.Instance.SetStepDetection(true);
            ChooseFoodAllInformCon.Instance.EnableInform();

        }
        if (transform.name == "����һ���"||transform.name== "����̨")
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
