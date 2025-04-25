using RTS;
using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class GameObjMan : MonoSingletonBase<GameObjMan>
{

    public List<GameObject> objects;
    public GameObject objPlayer;
    public GameObject ObjModel;


    public void Start()
    {
        objPlayer.transform.position = objects[0].transform.position;

    }


    public void SetPosition(int index)
    {

        objPlayer.transform.eulerAngles = Vector3.zero;

        objPlayer.transform.position = objects[index].transform.position;

    }

    /// <summary>
    /// �ر���ҿ���
    /// </summary>
    public void CLoseFirst()
    {
        objPlayer.GetComponent<FirstPersonController>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// ����ҿ���
    /// </summary>
    public void OpenFirst()
    {
        objPlayer.GetComponent<FirstPersonController>().enabled = true;

    }

    public void OpenObjCon(int index)
    {
        objects[index].SetActive(true);
    }

    public void UpObjPosCon()
    {
        SetPosition(0);
        for (int i = 2; i < objects.Count; i++)
        {
            objects[i].SetActive(false);

        }
        //foreach (var item in objects)
        //{
        //    item.SetActive(false);
        //}
        foreach (var item in objects)
        {
            //item.SetActive(true);
        }


    }

    private void OnEnable()
    {
        UpObjPosCon();
    }

}
