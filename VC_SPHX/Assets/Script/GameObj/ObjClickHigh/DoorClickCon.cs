using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//控制门开关
public class DoorClickCon : MonoSingletonBase<DoorClickCon>
{
    void Start()
    {
        //MessageCenter.Instance.Register("SendDoorClick", CloseDoorHigh);
        MessageCenter.Instance.Register("OpenDoor", OpenDoorHigh);
        //MessageCenter.Instance.Register("OpenDoorKaoHe", OpenDoorHighKaoHe);
        //MessageCenter.Instance.Register("SendGengToDoor", GengToDoor);
        //MessageCenter.Instance.Register("SendGengToDoorKaoHe", GengToDoorKaoHe);
    }

    public void SetHighlight(int index)
    {
        switch (index)
        {
            case 0:
                Doors[index].SetActive(true);
                break;
            case 1:
                Doors[index].SetActive(true);
                break;
            case 2:
                Doors[10].SetActive(true);
                break;
            case 3:
                Doors[index].SetActive(true);
                break;
            case 8:
                Doors[index].SetActive(true);
                break;
            case 9:
                Doors[index].SetActive(true);
                break;
            case 11:
                Doors[index].SetActive(true);
                break;
            default:
                break;
        }
        //Doors[index].GetComponent<Outline>().enabled = true;
    }

    public void OpenDoorHigh(int index)
    {
        Doors[index].SetActive(true);

    }
    public void CloseHighlightAll()
    {
        foreach (GameObject item in Doors)
        {
            item.SetActive(false);

            //item.GetComponent<Outline>().enabled = false;
        }
    }
    public void CloseDoorHigh(int index)
    {
        Doors[index].SetActive(false);

    }


    public List<GameObject> Doors;

    ////鼠标点击到门 接受到消息 判断如果是实验室 就进入
    //public void CloseDoorHigh(string doorName)
    //{
    //    GameObject obj = Doors.Find(go => go.tag == doorName);

    //    //if (obj.activeSelf)
    //    {
    //        //obj.GetComponent<Outline>().enabled = false;
    //        //obj.SetActive(false);

    //        switch (doorName)
    //        {
    //            case "WearPos":
    //                LabSystemManager.Instance.OnDressingRoomDoorClicked();
    //                break;
    //            case "AnjisuanPos":
    //                LabSystemManager.Instance.OnLabDoorClicked(1);
    //                AnjisuanCon.Instance.JoinAnjisuan();
    //                break;
    //            case "Xiangqi":
    //                LabSystemManager.Instance.OnLabDoorClicked(2);

    //                break;
    //            case "Zhongjinshu":
    //                LabSystemManager.Instance.OnLabDoorClicked(3);

    //                break;
    //            case "Shachongji":
    //                LabSystemManager.Instance.OnLabDoorClicked(4);

    //                break;
    //            case "Didingfa":
    //                LabSystemManager.Instance.OnLabDoorClicked(5);

    //                break;
    //            case "Suoshi":
    //                LabSystemManager.Instance.OnLabDoorClicked(6);

    //                break;
    //            case "Danbaizhi":
    //                LabSystemManager.Instance.OnLabDoorClicked(7);

    //                break;
    //            case "Shanshi":
    //                LabSystemManager.Instance.OnLabDoorClicked(8);

    //                break;
    //            case "Yingyang":
    //                LabSystemManager.Instance.OnLabDoorClicked(9);

    //                break;
    //            case "Shuzi":
    //                LabSystemManager.Instance.OnLabDoorClicked(10);

    //                break;

    //            default:
    //                break;
    //        }
    //    }
    //}

    public void OpenDoorHigh(string doorName)
    {
        GameObject obj = Doors.Find(go => go.tag == doorName);
        obj.SetActive(true);
        obj.GetComponent<Outline>().enabled = true;
    }


    // Update is called once per frame
    void Update()
    {

    }
    private void OnEnable()
    {
        foreach (GameObject item in Doors)
        {
            //item.GetComponent<Outline>().enabled = false;
            //item.SetActive(false);
            item.SetActive(false);
        }

    }

    public void UpdateDoorHighlights(bool state)
    {
        foreach (GameObject item in Doors)
        {
            //item.GetComponent<Outline>().enabled = state;
            //item.SetActive(state);
            item.SetActive(state);

        }
    }

    public void ResDoorItem()
    {
        foreach (GameObject item in Doors)
        {
            if (item.activeSelf)
            {
                item.GetComponent<DoorItem>().ResDoor();
            }
            //item.SetActive(false);
        }
        StartCoroutine(ConUp());

    }

    IEnumerator ConUp()
    {
        yield return new WaitForSeconds(0.11f);//点击后播放动画  1
        foreach (GameObject item in Doors)
        {
            item.SetActive(false);
        }
    }
}
