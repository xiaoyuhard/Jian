using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjBlClick : MonoBehaviour
{
    public bool isClick = false;
    public bool isClickHigh = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetBlClick(bool ifCl,bool ifCl2)
    {
        isClick = ifCl;
        isClickHigh = ifCl2;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
