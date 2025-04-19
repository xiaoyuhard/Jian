using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudInforUI : UICaoZuoBase
{
    public Button verifyBtn;
    public InputField studIDInF;
    public InputField semesterInF;
    public GameObject studInfUIObj;

    private void OnEnable()
    {
        studIDInF.text = "";
        semesterInF.text = "";
    }
    // Start is called before the first frame update
    void Start()
    {
        verifyBtn.onClick.AddListener(CloseStudInfUI);
    }

    private void CloseStudInfUI()
    {
        //studInfUIObj.SetActive(false);
        GameManager.Instance.stepDetection = true;
        UIManager.Instance.CloseUICaoZuo("StudInforUI");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
