using System.Collections;
using System.Collections.Generic;
using RTS;
using UnityEngine;

public class ExpInfo : MonoSingletonBase<ExpInfo>
{
    [TextArea(5,8)]
    public string expInfo;
    [TextArea(5, 8)]
    public string expInfo2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
