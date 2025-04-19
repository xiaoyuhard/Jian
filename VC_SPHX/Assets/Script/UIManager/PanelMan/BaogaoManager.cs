using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaogaoManager : UIBase
{
    [Header("UI References")]
    [SerializeField] private RectTransform content;      // ScrollViewµÄContent
    [SerializeField] private ScrollRect scrollRect;     // ScrollView×é¼þ

    public GameObject prefab;

    public List<GameObject> reportObj = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject obj = Instantiate(prefab, content.transform);
            obj.SetActive(true);
            obj.AddComponent<ReportResultsItems>();
            reportObj.Add(obj);
        }
        SetReportItemUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetReportItemUI()
    {
        for (int i = 0; i < reportObj.Count; i++)
        {
            reportObj[i].GetComponent<ReportResultsItems>().UpadteShowUI(BaoGaoDataCon.Instance.reportDatas[i]);

        }
    }
    private void OnEnable()
    {
        SetReportItemUI();
    }

}
