using RTS;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ��ʳʵ��
/// </summary>
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

    }
    private void OnEnable()
    {
        StartCoroutine(ConUp());
        foreach (GameObject go in posList)
        {
            go.SetActive(true);
        }
        //ChooseFoodCon.SetActive(false);

        //CloseObj();
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
        yield return new WaitForSeconds(0.5f);
        CloseObj();
        ChooseFoodCon.SetActive(true);

        // �ȴ�������������
        yield return new WaitUntil(() => GameManager.Instance.stepDetection); // �� conditionMet == true ʱ���� ������
        posList[0].SetActive(true);
        GameManager.Instance.SetStepDetection(false);

        yield return new WaitUntil(() => GameManager.Instance.stepDetection); // �� conditionMet == true ʱ���� ������̨
        posList[0].SetActive(false);
        posList[1].SetActive(true);
        GameManager.Instance.SetStepDetection(false);

        yield return new WaitUntil(() => GameManager.Instance.stepDetection); // �� conditionMet == true ʱ���� ����
        posList[1].SetActive(false);

        posList[2].SetActive(true);
        GameManager.Instance.SetStepDetection(false);

        yield return new WaitUntil(() => GameManager.Instance.stepDetection); // �� conditionMet == true ʱ���� һ���
        posList[2].SetActive(false);

        posList[3].SetActive(true);
        GameManager.Instance.SetStepDetection(false);
        yield return new WaitUntil(() => GameManager.Instance.stepDetection); // �� conditionMet == true ʱ���� �����
        posList[3].SetActive(false);
        GameManager.Instance.SetStepDetection(false);
        ChooseFoodAllInformCon.Instance.SetChooseMealActive(true);

        posList[4].SetActive(true);

        yield return new WaitUntil(() => GameManager.Instance.stepDetection); // �� conditionMet == true ʱ���� �����
        GameManager.Instance.SetStepDetection(false);

        posList[5].SetActive(true);
        yield return new WaitUntil(() => GameManager.Instance.stepDetection); // �� conditionMet == true ʱ���� �����
        GameManager.Instance.SetStepDetection(false);
     

        //ChooseFoodAllInformCon.Instance.EnableInform();
    }

    public void OnDisable()
    {
        ChooseFoodCon.SetActive(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        SetIsConUpBl();
        DoorClickCon.Instance.CloseHighlightAll();

    }
}
