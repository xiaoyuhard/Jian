using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class RenTiCon : MonoSingletonBase<RenTiCon>
{
    public List<Transform> bodyManModelList;
    public List<Transform> bodyWomanModelList;

    public List<GameObject> timeList;
    public Transform man;
    public Transform woman;

    public GameObject bodyModel;

    public GameObject posObj;

    private void OnEnable()
    {
        StartCoroutine(ConUp());
        posObj.SetActive(true);
        bodyModel.SetActive(true);
        foreach (var item in timeList)
        {
            //item.GetComponent<PlayableDirector>().enabled = false;
            item.GetComponent<PlayableDirector>().time = 0.1f;
            item.GetComponent<PlayableDirector>().Evaluate(); // 强制应用第0帧状态

            item.SetActive(true);
            item.GetComponent<PlayableDirector>().Stop();

        }
    }


    IEnumerator ConUp()
    {

        yield return new WaitUntil(() => GameManager.Instance.BackStepDetection());
        GameManager.Instance.SetStepDetection(false);

    }

    //点击后显示对应的模型 播放动画
    public void ClickBtnShowBody(int index)
    {
        timeList[index].SetActive(true);
        timeList[index].GetComponent<PlayableDirector>().Play();

    }


    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in man.GetComponentsInChildren<Transform>())
        {
            bodyManModelList.Add(child);
        }
        foreach (Transform child in woman.GetComponentsInChildren<Transform>())
        {
            bodyWomanModelList.Add(child);
        }

    }
    Transform obj;

    public string ShowBodyWomanModel(string bodyName)
    {
        if (bodyName == "")
        {
            obj.gameObject.layer = LayerMask.NameToLayer("Default");
            foreach (Transform body in obj.GetComponentsInChildren<Transform>())
            {
                body.gameObject.layer = LayerMask.NameToLayer("Default");

            }
            return "";
        }
        if (obj != null)
        {
            obj.gameObject.layer = LayerMask.NameToLayer("Default");
            foreach (Transform body in obj.GetComponentsInChildren<Transform>())
            {
                body.gameObject.layer = LayerMask.NameToLayer("Default");

            }
        }
        foreach (Transform body in bodyWomanModelList)
        {
            //var outline = body.GetComponent<Outline>();
            //outline.enabled = false;
            if (body.name == bodyName)
            {
                obj = body;
                body.gameObject.layer = LayerMask.NameToLayer("Perspective");

                //outline.enabled = true;
                //outline.OutlineColor = Color.yellow;
            }
        }
        if (obj.GetComponentsInChildren<Transform>() != null)

            foreach (Transform body in obj.GetComponentsInChildren<Transform>())
            {
                body.gameObject.layer = LayerMask.NameToLayer("Perspective");

            }
        return obj.name;

    }
    public string ShowBodyManModel(string bodyName)
    {
        if (bodyName == "")
        {
            obj.gameObject.layer = LayerMask.NameToLayer("Default");
            foreach (Transform body in obj.GetComponentsInChildren<Transform>())
            {
                body.gameObject.layer = LayerMask.NameToLayer("Default");

            }
            return "";
        }
        if (obj != null)
        {
            obj.gameObject.layer = LayerMask.NameToLayer("Default");
            foreach (Transform body in obj.GetComponentsInChildren<Transform>())
            {
                body.gameObject.layer = LayerMask.NameToLayer("Default");

            }
        }

        foreach (Transform body in bodyManModelList)
        {
            //var outline = body.GetComponent<Outline>();
            //outline.enabled = false;
            if (body.name == bodyName)
            {
                obj = body;
                body.gameObject.layer = LayerMask.NameToLayer("Perspective");

                //outline.enabled = true;
                //outline.OutlineColor = Color.yellow;
            }
        }
        if (obj.GetComponentsInChildren<Transform>() != null)
            foreach (Transform body in obj.GetComponentsInChildren<Transform>())
            {
                body.gameObject.layer = LayerMask.NameToLayer("Perspective");

            }
        return obj.name;
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void OnDisable()
    {
       
        if (obj != null)
        {
            obj.gameObject.layer = LayerMask.NameToLayer("Default");
            foreach (Transform body in obj.GetComponentsInChildren<Transform>())
            {
                body.gameObject.layer = LayerMask.NameToLayer("Default");

            }
        }
        obj = null;
        isTriOnce = true;
        bodyModel.SetActive(false);

    }


    bool isTriOnce = true;
    private void OnTriggerEnter(Collider other)
    {
        if (!isTriOnce) return;

        UIManager.Instance.OpenUICaoZuo("BodyConUI");
        posObj.SetActive(false);
        isTriOnce = false;
    }
}
