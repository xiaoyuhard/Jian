using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorItem : MonoBehaviour
{
    public GameObject doorCollider;
    public GameObject doorAniL;
    public GameObject doorAniR;
    private bool _hasPlayed = false;     // 标记是否已播放
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

        switch (transform.name)
        {
            case "Shanshi":
                GameManager.Instance.SetStepDetection(true);
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

    }
    private void OnEnable()
    {
        ResDoor();

        _hasPlayed = false;
    }

    public void ResDoor()
    {
        doorCollider.SetActive(true);
        //doorAniL.GetComponent<Animation>().enabled = false;
        //doorAniR.GetComponent<Animation>().enabled = false;
        //doorAniL.GetComponent<Animation>()["双开门右"].time = 0;

        //doorAniL.GetComponent<Animation>().Play();

        //doorAniR.GetComponent<Animation>().Rewind();
        ////doorAniL.GetComponent<Animation>().Stop();
        //Animation legacyAnimation = doorAniR.GetComponent<Animation>();
        ////doorAniR.GetComponent<Animation>().Stop(); // 停止所有动画
        ////AnimationState animState = doorAniR.GetComponent<Animation>()["双开门左"];
        ////animState.time = 0f; // 时间设为0
        ////animState.enabled = true; // 启用状态以进行采样
        ////doorAniR.GetComponent<Animation>().Sample(); // 应用当前状态
        ////animState.enabled = false; // 禁用防止自动播放

        //legacyAnimation["双开门左"].time = 0.01f;
        ////legacyAnimation.Sample(); // 强制更新到第 0 帧
        ////legacyAnimation.Play();
        ////legacyAnimation.Stop();
        //legacyAnimation.Stop();
        if (_hasPlayed )
        {
            StartCoroutine(ConUp());
        }

    }
    IEnumerator ConUp()
    {
        doorAniR.GetComponent<Animation>().Play();
        doorAniL.GetComponent<Animation>().Play();
        yield return new WaitForSeconds(0.1f);//点击后播放动画  1
        doorAniR.GetComponent<Animation>().Stop();
        doorAniL.GetComponent<Animation>().Stop();
        _hasPlayed = false;

    }

}
