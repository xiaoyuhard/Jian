using RTS;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Windows;

/// <summary>
/// 人体控制
/// </summary>
public class RenTiCon : MonoSingletonBase<RenTiCon>
{
    public List<Transform> bodyManModelList;
    public List<Transform> bodyWomanModelList;

    public List<GameObject> timeList;
    public Transform man;
    public Transform woman;

    public GameObject bodyModel;

    public GameObject posObj;

    public Material material;

    private void OnEnable()
    {
        //StartCoroutine(ConUp());
        if (posObj != null)
            posObj.SetActive(true);
        if (bodyModel != null)
            bodyModel.SetActive(true);
        ResetTimeline();
    }

    public void ResetTimeline()
    {
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
            if (child.GetComponent<MeshRenderer>() != null)
            {
                child.AddComponent<MaterialSwitcher>();
            }

        }
        foreach (Transform child in woman.GetComponentsInChildren<Transform>())
        {
            bodyWomanModelList.Add(child);
            if (child.GetComponent<MeshRenderer>() != null)
            {
                child.AddComponent<MaterialSwitcher>();
            }
        }

    }
    Transform obj;

    public string ShowBodyWomanModel(string bodyName)
    {
        if (bodyName == "")
        {
            //obj.gameObject.layer = LayerMask.NameToLayer("Default");
            //foreach (Transform body in obj.GetComponentsInChildren<Transform>())
            //{
            //    body.gameObject.layer = LayerMask.NameToLayer("Default");
            //}
            if (obj.GetComponent<MeshRenderer>() != null)
            {
                obj.GetComponent<MaterialSwitcher>().RestoreOriginalMaterials();
            }
            foreach (var item in obj.GetComponentsInChildren<MaterialSwitcher>())
            {
                item.GetComponent<MaterialSwitcher>().RestoreOriginalMaterials();

            }

            return "";
        }
        if (obj != null)
        {
            //obj.gameObject.layer = LayerMask.NameToLayer("Default");
            //foreach (Transform body in obj.GetComponentsInChildren<Transform>())
            //{
            //    body.gameObject.layer = LayerMask.NameToLayer("Default");

            //}
            if (obj.GetComponent<MeshRenderer>() != null)
            {
                obj.GetComponent<MaterialSwitcher>().RestoreOriginalMaterials();
            }
            foreach (var item in obj.GetComponentsInChildren<MaterialSwitcher>())
            {
                item.GetComponent<MaterialSwitcher>().RestoreOriginalMaterials();

            }
        }
        string bodyItemName = "";
        foreach (Transform body in bodyWomanModelList)
        {
            //var outline = body.GetComponent<Outline>();
            //outline.enabled = false;
            bodyItemName = GetMiddleChars(body.name);
            if (bodyItemName == bodyName)
            {
                obj = body;
                //body.gameObject.layer = LayerMask.NameToLayer("Perspective");
                if (obj.GetComponent<MeshRenderer>() != null)
                {
                    obj.GetComponent<MaterialSwitcher>().ApplyTemporaryMaterial(material);
                }
                foreach (var item in obj.GetComponentsInChildren<MaterialSwitcher>())
                {
                    item.GetComponent<MaterialSwitcher>().ApplyTemporaryMaterial(material);

                }

                //outline.enabled = true;
                //outline.OutlineColor = Color.yellow;
            }
        }
        if (obj.GetComponentsInChildren<Transform>() != null)
        {
            if (obj.GetComponent<MeshRenderer>() != null)
            {
                obj.GetComponent<MaterialSwitcher>().ApplyTemporaryMaterial(material);
            }
            foreach (var item in obj.GetComponentsInChildren<MaterialSwitcher>())
            {
                item.GetComponent<MaterialSwitcher>().ApplyTemporaryMaterial(material);

            }
        }
        return obj.name;

    }
    // 示例：提取第3到最后字符（索引从0开始）
    string GetMiddleChars(string objName)
    {
        if (objName.Contains("男性_") || objName.Contains("女性_"))
        {
            //string pattern = @"^[^_]+_[^_]+_(.*)";
            string pattern = @"^[^_]+_(.*)";
            Match match = Regex.Match(objName, pattern);

            //if (match.Success)
            {
                return match.Groups[1].Value; // 返回捕获的剩余部分
            }
            //Debug.Log(objName + "safs    " + objName.Substring(8));
            //return objName.Substring(8);
        }
        return objName;
    }

    public string ShowBodyManModel(string bodyName)
    {
        if (bodyName == "")
        {
            //obj.gameObject.layer = LayerMask.NameToLayer("Default");
            //foreach (Transform body in obj.GetComponentsInChildren<Transform>())
            //{
            //    body.gameObject.layer = LayerMask.NameToLayer("Default");

            //}

            if (obj.GetComponent<MeshRenderer>() != null)
            {
                obj.GetComponent<MaterialSwitcher>().RestoreOriginalMaterials();
            }
            foreach (var item in obj.GetComponentsInChildren<MaterialSwitcher>())
            {
                item.GetComponent<MaterialSwitcher>().RestoreOriginalMaterials();

            }
            return "";
        }
        if (obj != null)
        {
            //obj.gameObject.layer = LayerMask.NameToLayer("Default");
            //foreach (Transform body in obj.GetComponentsInChildren<Transform>())
            //{
            //    body.gameObject.layer = LayerMask.NameToLayer("Default");

            //}
            if (obj.GetComponent<MeshRenderer>() != null)
            {
                obj.GetComponent<MaterialSwitcher>().RestoreOriginalMaterials();
            }
            foreach (var item in obj.GetComponentsInChildren<MaterialSwitcher>())
            {
                item.GetComponent<MaterialSwitcher>().RestoreOriginalMaterials();

            }
        }
        string bodyItemName = "";

        foreach (Transform body in bodyManModelList)
        {
            //var outline = body.GetComponent<Outline>();
            //outline.enabled = false;
            bodyItemName = GetMiddleChars(body.name);

            if (bodyItemName == bodyName)
            {
                obj = body;
                //body.gameObject.layer = LayerMask.NameToLayer("Perspective");
                if (obj.GetComponent<MeshRenderer>() != null)
                {
                    obj.GetComponent<MaterialSwitcher>().ApplyTemporaryMaterial(material);
                }
                foreach (var item in obj.GetComponentsInChildren<MaterialSwitcher>())
                {
                    item.GetComponent<MaterialSwitcher>().ApplyTemporaryMaterial(material);

                }

                //outline.enabled = true;
                //outline.OutlineColor = Color.yellow;
            }
        }
        if (obj.GetComponentsInChildren<Transform>() != null)
        {

            //foreach (Transform body in obj.GetComponentsInChildren<Transform>())
            //{
            //    body.gameObject.layer = LayerMask.NameToLayer("Perspective");

            //}
            if (obj.GetComponent<MeshRenderer>() != null)
            {
                obj.GetComponent<MaterialSwitcher>().ApplyTemporaryMaterial(material);
            }
            foreach (var item in obj.GetComponentsInChildren<MaterialSwitcher>())
            {
                item.GetComponent<MaterialSwitcher>().ApplyTemporaryMaterial(material);

            }
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
        if (bodyModel != null)
            bodyModel.SetActive(false);

    }


    bool isTriOnce = true;
    private void OnTriggerEnter(Collider other)
    {
        //if (!isTriOnce) return;
        //posObj.SetActive(false);
        //isTriOnce = false;


    }

    public void OpenUI()
    {
        UIManager.Instance.OpenUICaoZuo("BodyConUI");
        posObj.SetActive(false);
        isTriOnce = false;
    }

    private void OnTriggerExit(Collider other)
    {
        UIManager.Instance.CloseUICaoZuo("BodyConUI");
        posObj.SetActive(true);
        isTriOnce = true;
        ResetTimeline();

    }
}
