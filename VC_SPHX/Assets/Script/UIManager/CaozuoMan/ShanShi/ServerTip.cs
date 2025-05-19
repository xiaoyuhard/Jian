using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerTip : UICaoZuoBase
{
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        StartCoroutine(CloseObj());
    }

    IEnumerator CloseObj()
    {
        yield return new WaitForSeconds(2f);
        UIManager.Instance.CloseUICaoZuo(UINameType.UI_ServerTip);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
