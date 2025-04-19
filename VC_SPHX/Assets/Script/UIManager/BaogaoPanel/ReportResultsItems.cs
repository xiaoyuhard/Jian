using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class ReportResultsItems : MonoBehaviour
{
    public string id;
    public Text titleT;
    public Text serialT1;
    public Text serialT2;

    public Text operationT1;
    public Text operationT2;

    public Text markT1;
    public Text markT2;

    private void Awake()
    {
        titleT = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        serialT1 = transform.GetChild(4).GetChild(0).GetComponent<Text>();
        serialT2 = transform.GetChild(5).GetChild(0).GetComponent<Text>();
        operationT1 = transform.GetChild(6).GetChild(0).GetComponent<Text>();
        operationT2 = transform.GetChild(7).GetChild(0).GetComponent<Text>();
        markT1 = transform.GetChild(8).GetChild(0).GetComponent<Text>();
        markT2 = transform.GetChild(9).GetChild(0).GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpadteShowUI(ReportData data)
    {
        titleT.text = data.titleT;
        serialT1.text = !string.IsNullOrEmpty(data.serialT1) ? data.serialT1 : " ";
        serialT2.text = !string.IsNullOrEmpty(data.serialT2) ? data.serialT2 : " ";
        operationT1.text = !string.IsNullOrEmpty(data.operationT1) ? data.operationT1 : " ";
        operationT2.text = !string.IsNullOrEmpty(data.operationT2) ? data.operationT2 : " ";
        markT1.text = !string.IsNullOrEmpty(data.markT1) ? data.markT1 : " ";
        markT2.text = !string.IsNullOrEmpty(data.markT2) ? data.markT2 : " ";
    }


}


