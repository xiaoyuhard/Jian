using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XiangqiRongyeTipUI : UICaoZuoBase
{
    public Button btn;
    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(ConfirmClick);
    }

    private void ConfirmClick()
    {
        UIManager.Instance.CloseUICaoZuo("XiangqiRongyeTipUI");
        GameManager.Instance.stepDetection = true;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
