using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XiangqiCuiquyiTipUI : UICaoZuoBase
{
    public Button btn;
    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(ConfirmClick);
    }

    private void ConfirmClick()
    {
        UIManager.Instance.CloseUICaoZuo("XiangqiCuiquyiTipUI");
        GameManager.Instance.stepDetection = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
