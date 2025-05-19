using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//碰撞触发 开门动画
public class DoorItem : MonoBehaviour
{
    public GameObject doorCollider;
    public GameObject doorAniL;
    public GameObject doorAniR;
    private bool _hasPlayed = false;     // 标记是否已播放
    public bool triggerNext = true;

    // Start is called before the first frame update
    void Start()
    {
        if (doorAniL.GetComponent<Outline>() != null)
        {
            doorAniL.GetComponent<Outline>().enabled = true;

        }
        if (doorAniR.GetComponent<Outline>() != null)
        {
            doorAniR.GetComponent<Outline>().enabled = true;

        }
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
        if (doorAniL.GetComponent<Outline>() != null)
        {
            doorAniL.GetComponent<Outline>().enabled = false;

        }
        if (doorAniR.GetComponent<Outline>() != null)
        {
            doorAniR.GetComponent<Outline>().enabled = false;

        }

        //开门后触发下一步
        if (triggerNext)
            MessageCenter.Instance.Send(EventName.Exp_NextStep);

        switch (transform.name)
        {
            case "Shanshi":
                GameManager.Instance.SetStepDetection(true);
                if (doorAniL.GetComponent<Outline>() != null)
                {
                    doorAniL.GetComponent<Outline>().enabled = false;

                }
                if (doorAniR.GetComponent<Outline>() != null)
                {
                    doorAniR.GetComponent<Outline>().enabled = false;

                }
                gameObject.SetActive(false);

                break;
            case "营养室进口":
                GameManager.Instance.SetStepDetection(true);
                break;
            case "体检室进口":
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
        //doorAniL.GetComponent<Animation>()["双开门左"].time = 0;
        //doorAniL.GetComponent<Animation>().Play();
        //doorAniR.GetComponent<Animation>().Play();
        //doorAniL.GetComponent<Animation>().Stop();
        //doorAniR.GetComponent<Animation>().Stop();
        //doorAniR.GetComponent<Animation>()["双开门右"].time = 0;
        //doorCollider.SetActive(true);

        if (doorAniL.GetComponent<Outline>() != null)
        {
            doorAniL.GetComponent<Outline>().enabled = false;

        }
        if (doorAniR.GetComponent<Outline>() != null)
        {
            doorAniR.GetComponent<Outline>().enabled = false;

        }

    }
    private void OnEnable()
    {
        //StartCoroutine(OpenDoorHigh());

        //ResDoor();
        if (doorAniL.GetComponent<Outline>() != null)
        {
            doorAniL.GetComponent<Outline>().enabled = true;

        }
        if (doorAniR.GetComponent<Outline>() != null)
        {
            doorAniR.GetComponent<Outline>().enabled = true;

        }

        _hasPlayed = false;
    }
    IEnumerator OpenDoorHigh()
    {
        doorAniR.GetComponent<Animation>().Play();
        doorAniL.GetComponent<Animation>().Play();
        yield return new WaitForSeconds(0.05f);//点击后播放动画  1
        doorAniR.GetComponent<Animation>().Stop();
        doorAniL.GetComponent<Animation>().Stop();
        _hasPlayed = false;
        //if (doorAniL.GetComponent<Outline>() != null)
        //{
        //    doorAniL.GetComponent<Outline>().enabled = false;

        //}
        //if (doorAniR.GetComponent<Outline>() != null)
        //{
        //    doorAniR.GetComponent<Outline>().enabled = false;

        //}
        ////gameObject.SetActive(false);
        //Debug.Log(222);

    }
    public void ResDoor()
    {
        doorCollider.SetActive(true);

        CloseDoorHigh();

    }

    public void CloseDoorHigh()
    {
        if (_hasPlayed)
        {
        }
        if (gameObject.activeSelf)
        {
            //StartCoroutine(ConUp());

        }

    }
    IEnumerator ConUp()
    {
        doorAniR.GetComponent<Animation>().Play();
        doorAniL.GetComponent<Animation>().Play();
        yield return new WaitForSeconds(0.05f);//点击后播放动画  1
        doorAniR.GetComponent<Animation>().Stop();
        doorAniL.GetComponent<Animation>().Stop();
        _hasPlayed = false;
        if (doorAniL.GetComponent<Outline>() != null)
        {
            doorAniL.GetComponent<Outline>().enabled = false;

        }
        if (doorAniR.GetComponent<Outline>() != null)
        {
            doorAniR.GetComponent<Outline>().enabled = false;

        }
        gameObject.SetActive(false);
        //Debug.Log(222);

    }

}
