using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YiTiJiUI : UICaoZuoBase
{
    public Button randomBtn;
    public Button oneselfBtn;
    public GameObject selectUIObj;
    public Dropdown informDrop;
    public InputField inFBirthday;
    public Dropdown inFGender;
    public InputField inFHeight;
    public InputField inFWeight;
    public Dropdown workDrop;
    public Button verifyBtn;
    public GameObject informUIObj;

    private void OnEnable()
    {
        selectUIObj.SetActive(true);
        informUIObj.SetActive(false);
        informDrop.value = 0;
        inFBirthday.text = "";
        inFHeight.text = "";
        inFWeight.text = "";
        workDrop.value = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        randomBtn.onClick.AddListener(RandomInform);
        oneselfBtn.onClick.AddListener(OneselfInform);
        verifyBtn.onClick.AddListener(verifylfInform);

    }

    private void verifylfInform()
    {
        informUIObj.SetActive(false);
        GameManager.Instance.stepDetection = true;
        UIManager.Instance.CloseUICaoZuo("YiTiJiUI");
    }

    private void OneselfInform()
    {
        informUIObj.SetActive(true);
        selectUIObj.SetActive(false);

    }

    private void RandomInform()
    {
        selectUIObj.SetActive(false);
        GameManager.Instance.stepDetection = true;
        UIManager.Instance.CloseUICaoZuo("YiTiJiUI");

    }



    // Update is called once per frame
    void Update()
    {

    }
}
