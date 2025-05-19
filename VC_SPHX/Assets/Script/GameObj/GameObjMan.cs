using RTS;
using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjMan : MonoSingletonBase<GameObjMan>
{
    public List<GameObject> objects;
    public GameObject objPlayer;

    private void OnEnable()
    {
        //UpObjPosCon();
    }

    public void Start()
    {
        //objPlayer.transform.position = objects[0].transform.position;
        //UpObjPosCon();
    }

    public void SetPosition(int index)
    {
        objPlayer.transform.eulerAngles = Vector3.zero;
        objPlayer.transform.position = objects[index].transform.position;
    }

    /// <summary>
    /// 关闭玩家控制
    /// </summary>
    public void CLoseFirst()
    {
        objPlayer.GetComponent<FirstPersonController>().enabled = false;
        FirstPersonController.Instance.Close_isRightMouseDown();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// 打开玩家控制
    /// </summary>
    public void OpenFirst()
    {
        objPlayer.GetComponent<FirstPersonController>().enabled = true;
    }

    /// <summary>
    /// 打开某个实验的控制器
    /// </summary>
    /// <param name="index"></param>
    public void OpenObjCon(int index)
    {
        Debug.Log(index);

        objects[index].SetActive(true);
        objIndex = index;
    }

    public void CloseObjCon(int index)
    {
        objects[index].SetActive(false);
        objects[objIndex].SetActive(false);
        Debug.Log(objIndex);
    }
    int objIndex = 1;
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

}
