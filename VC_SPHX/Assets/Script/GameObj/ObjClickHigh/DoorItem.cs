using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ײ���� ���Ŷ���
public class DoorItem : MonoBehaviour
{
    public GameObject doorCollider;
    public GameObject doorAniL;
    public GameObject doorAniR;
    private bool _hasPlayed = false;     // ����Ƿ��Ѳ���
    public bool triggerNext = true;

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
        if (_hasPlayed) return;
        doorCollider.SetActive(false);
        doorAniL.GetComponent<Animation>().enabled = true;
        doorAniR.GetComponent<Animation>().enabled = true;
        doorAniL.GetComponent<Animation>().Play();
        doorAniR.GetComponent<Animation>().Play();
        _hasPlayed = true;

        //���ź󴥷���һ��
        if (triggerNext)
            MessageCenter.Instance.Send(EventName.Exp_NextStep);

        switch (transform.name)
        {
            case "Shanshi":
                GameManager.Instance.SetStepDetection(true);
                gameObject.SetActive(false);
                break;
            case "Ӫ���ҽ���":
                GameManager.Instance.SetStepDetection(true);
                break;
            case "����ҽ���":
                GameManager.Instance.SetStepDetection(true);
                break;
            case "Qiping":
                GameManager.Instance.SetStepDetection(true);
                break;
            case "Gaowen":
                //GameManager.Instance.SetStepDetection(true);
                break;
            default:
                break;
        }
    }


    private void OnDisable()
    {
        //doorAniL.GetComponent<Animation>()["˫������"].time = 0;
        //doorAniL.GetComponent<Animation>().Play();
        //doorAniR.GetComponent<Animation>().Play();
        //doorAniL.GetComponent<Animation>().Stop();
        //doorAniR.GetComponent<Animation>().Stop();
        //doorAniR.GetComponent<Animation>()["˫������"].time = 0;
        //doorCollider.SetActive(true);

    }
    private void OnEnable()
    {
        ResDoor();

        _hasPlayed = false;
    }

    public void ResDoor()
    {
        doorCollider.SetActive(true);

        if (_hasPlayed )
        {
            StartCoroutine(ConUp());
        }

    }
    IEnumerator ConUp()
    {
        doorAniR.GetComponent<Animation>().Play();
        doorAniL.GetComponent<Animation>().Play();
        yield return new WaitForSeconds(0.1f);//����󲥷Ŷ���  1
        doorAniR.GetComponent<Animation>().Stop();
        doorAniL.GetComponent<Animation>().Stop();
        _hasPlayed = false;

    }

}
