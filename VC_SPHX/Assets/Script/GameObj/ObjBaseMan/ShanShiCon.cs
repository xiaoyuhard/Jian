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
        // �ȴ�������������
        yield return new WaitUntil(() => GameManager.Instance.stepDetection); // �� conditionMet == true ʱ���� ������
        posList[0].SetActive(true);
        GameManager.Instance.stepDetection = false;
        yield return new WaitUntil(() => GameManager.Instance.stepDetection); // �� conditionMet == true ʱ���� ������̨
        posList[1].SetActive(true);
        GameManager.Instance.stepDetection = false;
        yield return new WaitUntil(() => GameManager.Instance.stepDetection); // �� conditionMet == true ʱ���� ����
        posList[2].SetActive(true);
        GameManager.Instance.stepDetection = false;
        yield return new WaitUntil(() => GameManager.Instance.stepDetection); // �� conditionMet == true ʱ���� һ���
        posList[3].SetActive(true);
        GameManager.Instance.stepDetection = false;
        yield return new WaitUntil(() => GameManager.Instance.stepDetection); // �� conditionMet == true ʱ���� �����
        posList[4].SetActive(true);
        ChooseFoodCon.SetActive(true);

    }

    private void OnTriggerEnter(Collider other)
    {
        SetIsConUpBl();
        DoorClickCon.Instance.CloseHighlightAll();

    }
}
