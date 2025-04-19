using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseFoodItem : MonoBehaviour
{

    public string foodKindName;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        UIManager.Instance.OpenUICaoZuo("ChooseShanShiUI");
        MessageCenter.Instance.Send("SendChooseItemToShanShiUI", foodKindName); //°±»ùËá³¬ÉùÍÑÆø»ú
        //Debug.Log(transform.name + "   " + foodKindName);
        GameObjMan.Instance.CLoseFirst();


    }

    private void OnTriggerExit(Collider other)
    {


    }
}
