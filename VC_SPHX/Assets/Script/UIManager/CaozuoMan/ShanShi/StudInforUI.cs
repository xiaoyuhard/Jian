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

    public Button geRenBtn;
    public Button qunTiBtn;
    private void OnEnable()
    {
        studIDInF.text = "";
        semesterInF.text = "";
    }
    // Start is called before the first frame update
    void Start()
    {
        //verifyBtn.onClick.AddListener(CloseStudInfUI);
        geRenBtn.onClick.AddListener(CloseStudInfUI);
        qunTiBtn.onClick.AddListener(CloseQunTiInfUI);
    }

    private void CloseStudInfUI()
    {
        //studInfUIObj.SetActive(false);
        GameManager.Instance.stepDetection = true;
        UIManager.Instance.CloseUICaoZuo("StudInforUI");
        GameObjMan.Instance.OpenFirst();

    }

    private void CloseQunTiInfUI()
    {
        FoodManager.Instance.LoadRecipeList();
        UIManager.Instance.CloseUICaoZuo("StudInforUI");
        UIManager.Instance.OpenUICaoZuo("GroupRegisterUI");

    }

    // Update is called once per frame
    void Update()
    {

    }
}
