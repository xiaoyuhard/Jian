using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HumanBodyUI : MonoBehaviour
{
    public List<BodyTglData> tglListA = new List<BodyTglData>();
    public List<BodyTglData> tglListB = new List<BodyTglData>();
    public List<BodyTglData> tglListC = new List<BodyTglData>();
    public List<BodyTglData> tglListD = new List<BodyTglData>();

    Dictionary<string, List<BodyTglData>> tglCDic=new Dictionary<string, List<BodyTglData>>();

    public GameObject unfoldScrollObj;
    public Transform content;
    public Transform contentB;
    public Transform contentC;
    public Transform contentD;
    public GameObject prefabItem;

    public GameObject img1;
    public GameObject img2;
    public GameObject img3;


    public void OnEnable()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        int index = 0;
        foreach (var tgl in tglListA)
        {
            tgl.index = index;
            //tgl.parent = tgl.tgl.gameObject;
            index++;
            tgl.tgl.onValueChanged.AddListener((isOn) =>
            {
                if (isOn) OnSystemSelectedTest1(tgl, isOn);
            });

        }

    }
    #region
    // 系统被选中时
    void OnSystemSelectedTest(BodyTglData tglItem, bool isOn)
    {
        if (isOn)
        {
            switch (tglItem.choose)
            {
                case 1:

                    GameObject endocrineUnfoldScrollObj = Instantiate(unfoldScrollObj, content);
                    endocrineUnfoldScrollObj.SetActive(true);
                    endocrineUnfoldScrollObj.transform.SetSiblingIndex(tglItem.parent.transform.GetSiblingIndex() + 1);
                    //Debug.Log(tglItem.index + 2);
                    LayoutRebuilder.ForceRebuildLayoutImmediate(tglItem.parent.transform.parent.GetComponent<RectTransform>());

                    int indexEndocrine = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        GameObject toggleObj = Instantiate(prefabItem, endocrineUnfoldScrollObj.transform.GetChild(0).GetComponent<ScrollRect>().content);
                        toggleObj.SetActive(true);
                        BodyTglData toggle = toggleObj.GetComponent<BodyTglDataItem>().bodyTglData;
                        toggle.choose = tglItem.choose;
                        toggle.index = indexEndocrine;
                        toggle.parent = tglItem.parent;
                        indexEndocrine++;
                        toggle.tgl.group = endocrineUnfoldScrollObj.transform.GetChild(0).GetComponent<ScrollRect>().content.GetComponent<ToggleGroup>();

                        toggle.tgl.onValueChanged.AddListener((isOn) =>
                        {
                            if (isOn) OnSystemSelectedTest(toggle, isOn);
                        });
                    }

                    break;
                case 2:
                    GameObject endocnfoldScrollObj = Instantiate(unfoldScrollObj, content);
                    endocnfoldScrollObj.SetActive(true);
                    endocnfoldScrollObj.transform.SetSiblingIndex(tglItem.parent.transform.GetSiblingIndex() + 1);
                    LayoutRebuilder.ForceRebuildLayoutImmediate(tglItem.parent.transform.parent.GetComponent<RectTransform>());

                    int indexEcrine = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        GameObject toggleObj = Instantiate(prefabItem, endocnfoldScrollObj.transform.GetChild(0).GetComponent<ScrollRect>().content);
                        toggleObj.SetActive(true);
                        BodyTglData toggle = toggleObj.GetComponent<BodyTglDataItem>().bodyTglData;
                        toggle.choose = tglItem.choose;
                        toggle.index = indexEcrine;
                        toggle.parent = tglItem.parent;

                        indexEcrine++;

                        toggle.tgl.group = endocnfoldScrollObj.transform.GetChild(0).GetComponent<ScrollRect>().content.GetComponent<ToggleGroup>();
                        toggle.tgl.onValueChanged.AddListener((isOn) =>
                        {
                            if (isOn) OnSystemSelectedTest(toggle, isOn);
                        });
                    }
                    break;
                default:
                    break;
            }
        }
        if (!isOn)
        {
            GetTagsFromIndex(tglItem.parent.transform.GetSiblingIndex());
        }
    }
    // 获取从指定索引开始的Tags
    public void GetTagsFromIndex(int startIndex)
    {

        // 遍历子物体（从第二个开始）
        for (int i = startIndex; i < content.childCount; i++)
        {
            Transform child = content.GetChild(i);

            // 验证组件和Tag有效性
            if (child.TryGetComponent<Image>(out var image))
            {
                if (!string.IsNullOrEmpty(child.tag))
                {
                    Destroy(child.gameObject);
                }
                else
                {
                    return;
                }
            }
        }

    }

    // 系统被选中时
    void OnSystemSelected(BodyTglData tglItem, bool isOn)
    {
        if (isOn)
        {
            switch (tglItem.choose)
            {
                case 1:
                    List<EndocrineSystemData> endocrineData = BodyManager.Instance.GetEndocrItemById(tglItem.name);
                    if (endocrineData.Count < 0)
                    {
                        return;
                    }
                    GameObject endocrineUnfoldScrollObj = Instantiate(unfoldScrollObj, content);
                    endocrineUnfoldScrollObj.SetActive(true);
                    endocrineUnfoldScrollObj.transform.SetSiblingIndex(tglItem.tgl.transform.GetSiblingIndex() + 1);
                    int indexEndocrine = 0;
                    foreach (var item in endocrineData)
                    {
                        GameObject toggleObj = Instantiate(prefabItem, endocrineUnfoldScrollObj.GetComponent<ScrollRect>().content);
                        toggleObj.SetActive(true);
                        BodyTglData toggle = toggleObj.GetComponent<BodyTglDataItem>().bodyTglData;
                        toggle.Male = item.Male;
                        toggle.Female = item.Female;
                        toggle.choose = tglItem.choose;
                        toggle.index = indexEndocrine;
                        indexEndocrine++;

                        toggle.tgl.transform.GetChild(1).GetComponent<Text>().text = BackBodyName(item.Male, item.Female, "", "");
                        toggle.tgl.group = endocrineUnfoldScrollObj.transform.GetChild(0).GetComponent<ScrollRect>().content.GetComponent<ToggleGroup>();

                        indexEndocrine++;
                        toggle.tgl.onValueChanged.AddListener((isOn) =>
                        {
                            if (isOn) OnSystemSelected(toggle, isOn);
                        });

                    }
                    break;
                case 2:
                    List<BoneHierarchy> boneDatas = BodyManager.Instance.GetBoneItemById(tglItem.name);
                    if (boneDatas.Count < 0)
                    {
                        return;
                    }
                    GameObject boneUnfoldScrollObj = Instantiate(unfoldScrollObj, content);
                    boneUnfoldScrollObj.SetActive(true);
                    boneUnfoldScrollObj.transform.SetSiblingIndex(tglItem.tgl.transform.GetSiblingIndex() + 1);
                    int indexBone = 0;

                    foreach (var item in boneDatas)
                    {
                        GameObject toggleObj = Instantiate(prefabItem, boneUnfoldScrollObj.GetComponent<ScrollRect>().content);
                        toggleObj.SetActive(true);
                        BodyTglData toggle = toggleObj.GetComponent<BodyTglDataItem>().bodyTglData;
                        toggle.Level1 = tglItem.Level1;
                        toggle.Level2 = tglItem.Level2;
                        toggle.Level3 = tglItem.Level3;
                        toggle.Level4 = tglItem.Level4;
                        toggle.tgl.transform.GetChild(1).GetComponent<Text>().text = BackBodyName(item.Level1, item.Level2, item.Level3, item.Level4);

                        toggle.choose = tglItem.choose;
                        toggle.index = indexBone;
                        indexBone++;
                        toggle.tgl.group = boneUnfoldScrollObj.transform.GetChild(0).GetComponent<ScrollRect>().content.GetComponent<ToggleGroup>();

                        indexBone++;
                        toggle.tgl.onValueChanged.AddListener((isOn) =>
                        {
                            if (isOn) OnSystemSelected(toggle, isOn);
                        });

                    }
                    break;
                default:
                    break;
            }
        }
    }
    public string BackBodyName(string a, string b, string c, string d)
    {
        if (d != "")
        {
            return d;
        }
        if (c != "")
        {
            return c;
        }
        if (b != "")
        {
            return b;
        }
        return a;
    }
    #endregion

    void OnSystemSelectedTest1(BodyTglData tglItem, bool isOn)
    {
        if (isOn)
        {
            List<EndocrineSystemData> endocrineData = BodyManager.Instance.GetEndocrItemById(tglItem.name);

            if (endocrineData.Count < 0)
            {
                return;
            }

            img1.SetActive(true);
            img1.transform.SetSiblingIndex(tglItem.tgl.transform.GetSiblingIndex() + 1);

            foreach (var item in endocrineData)
            {
                GameObject toggleObj = Instantiate(unfoldScrollObj, contentB);
                toggleObj.SetActive(true);
                BodyTglData toggle = toggleObj.GetComponent<BodyTglDataItem>().bodyTglData;
                toggle.Male = item.Male;
                toggle.Female = item.Female;
                toggle.choose = tglItem.choose;

                toggle.tgl.transform.GetChild(1).GetComponent<Text>().text = BackBodyName(item.Male, item.Female, "", "");
                toggle.tgl.group = contentB.GetComponent<ToggleGroup>();
                toggle.tgl.onValueChanged.AddListener((isOn) =>
                {
                    if (isOn) OnSystemSelectedTest2(toggle, isOn);
                });


            }

        }

    }
    void OnSystemSelectedTest2(BodyTglData tglItem, bool isOn)
    {
        if (isOn)
        {
            int index = 0;
            img2.SetActive(true);
            img2.transform.SetSiblingIndex(img1.transform.GetSiblingIndex() + 1);
            foreach (var tgl in tglListC)
            {
                GameObject endocrineUnfoldScrollObj = Instantiate(unfoldScrollObj, contentC);
                tgl.tgl = endocrineUnfoldScrollObj.GetComponent<Toggle>();

                tgl.index = index;
                //tgl.parent = tgl.tgl.gameObject;
                index++;
                tgl.tgl.onValueChanged.AddListener((isOn) =>
                {
                    if (isOn) OnSystemSelectedTest3(tgl, isOn);
                });

            }
        }

    }
    void OnSystemSelectedTest3(BodyTglData tglItem, bool isOn)
    {
        if (isOn)
        {
            int index = 0;
            img3.SetActive(true);
            img3.transform.SetSiblingIndex(img2.transform.GetSiblingIndex() + 1);
            foreach (var tgl in tglListD)
            {
                GameObject endocrineUnfoldScrollObj = Instantiate(unfoldScrollObj, contentD);
                tgl.tgl = endocrineUnfoldScrollObj.GetComponent<Toggle>();

                tgl.index = index;
                //tgl.parent = tgl.tgl.gameObject;
                index++;
                //tgl.tgl.onValueChanged.AddListener((isOn) =>
                //{
                //    if (isOn) OnSystemSelectedTest1(tgl, isOn);
                //});

            }
        }

    }


    // Update is called once per frame
    void Update()
    {

    }
}


[System.Serializable]
public class BodyTglData
{
    public string Male;
    public string Female;
    public string name;
    public int choose;
    public int index = 0;
    public Toggle tgl;
    public string Level1;
    public string Level2;
    public string Level3;
    public string Level4;
    public GameObject parent;

}