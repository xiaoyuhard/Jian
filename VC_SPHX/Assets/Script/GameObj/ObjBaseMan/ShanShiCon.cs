using RTS;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShanShiCon : MonoSingletonBase<ShanShiCon>
{
    public List<GameObject> posList;


    public GameObject ChooseFoodCon;

    public void CloseObj()
    {
        foreach (GameObject go in posList)
        {
            go.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ConUp());

    }
    bool isConUpBl = false;
    // Update is called once per frame
    void Update()
    {
 
    }
    public void SetIsConUpBl()
    {
        isConUpBl = true;
    }

    IEnumerator ConUp()
    {
        // 等待条件满足后继续
        yield return new WaitUntil(() => GameManager.Instance.stepDetection); // 当 conditionMet == true 时继续 进入门
        posList[0].SetActive(true);
        GameManager.Instance.stepDetection = false;
        yield return new WaitUntil(() => GameManager.Instance.stepDetection); // 当 conditionMet == true 时继续 到服务台
        posList[1].SetActive(true);
        GameManager.Instance.stepDetection = false;
        yield return new WaitUntil(() => GameManager.Instance.stepDetection); // 当 conditionMet == true 时继续 过门
        posList[2].SetActive(true);
        GameManager.Instance.stepDetection = false;
        yield return new WaitUntil(() => GameManager.Instance.stepDetection); // 当 conditionMet == true 时继续 一体机
        posList[3].SetActive(true);
        GameManager.Instance.stepDetection = false;
        yield return new WaitUntil(() => GameManager.Instance.stepDetection); // 当 conditionMet == true 时继续 体检室
        posList[4].SetActive(true);
        ChooseFoodCon.SetActive(true);

    }

    private void OnTriggerEnter(Collider other)
    {
        SetIsConUpBl();
        DoorClickCon.Instance.CloseHighlightAll();

    }
}
