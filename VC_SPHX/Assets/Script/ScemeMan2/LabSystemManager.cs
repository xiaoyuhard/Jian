using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LabSystemManager : MonoBehaviour
{
    // 单例模式
    public static LabSystemManager Instance;

    private bool canEnterDressingRoom = false; // 控制更衣室进入权限

    private int currentLabIndex = -1; // 当前选择的实验室编号
    private bool isAssessmentMode = false; // 是否为考核模式
    public GameObject currentHighlight; // 当前高亮物体
    private string currentLabName = "";

    void Awake()
    {
        Instance = this;
        InitializeSystem();
    }

    // 系统初始化
    void InitializeSystem()
    {

        // 禁用所有实验室门高亮
        UpdateDoorHighlights(false);
        //dressingRoomDoor.SetHighlight(false);
        canEnterDressingRoom = false; // 初始化时禁止进入
    }

    // 实验室按钮点击 (在UI按钮事件中绑定)
    public void OnLabButtonClicked(int labIndex, string labName)
    {
        currentLabIndex = labIndex;
        currentLabName = labName;
        GameObjMan.Instance.OpenObjCon(1);
        //labsPanel.SetActive(false);
        //optionPanel.SetActive(true); // 显示考核/跟练选择
    }

    public int ReutrnCurrIndex()
    {
        return currentLabIndex;
    }

    /// <summary>
    /// 选择跟练模式
    /// </summary>
    public void SelectPracticeMode()
    {
        GameObjMan.Instance.UpObjPosCon();

        isAssessmentMode = false;
        //optionPanel.SetActive(false);
        // 跟练模式允许进入更衣室并高亮
        canEnterDressingRoom = true;
        DoorClickCon.Instance.SetHighlight(0); // 高亮更衣室门
        UIManager.Instance.OpenUICaoZuo(UINameType.UI_ProTipsMan);
        EnterCorridorGen();
        GameManager.Instance.tipStalkBl = true;

    }

    /// <summary>
    /// 选择考核模式
    /// </summary>
    public void SelectAssessmentMode()
    {
        if (currentLabIndex != 10 || currentLabIndex != 8 || currentLabIndex != 9)
        {
            DoorClickCon.Instance.SetHighlight(0); // 高亮更衣室门
        }
        GameObjMan.Instance.UpObjPosCon();
        isAssessmentMode = true;
        //optionPanel.SetActive(false);
        // 考核模式允许进入但不高亮
        canEnterDressingRoom = true;
        EnterCorridor();
        //dressingRoomDoor.SetHighlight(false); // 考核模式不高亮

    }

    void EnterCorridorGen()
    {
        //CaozuoSceneCon.Instance.EnterLab(0);
        MessageCenter.Instance.Send("SendTiShiUIName", currentLabName);


    }

    // 进入走廊
    void EnterCorridor()
    {
        //CaozuoSceneCon.Instance.EnterLab(0);

        // 移动玩家到走廊位置
        //PlayerController.Instance.TeleportTo(corridorPosition.position);
        if (currentLabIndex == 8 || currentLabIndex == 9)
        {

            DoorClickCon.Instance.SetHighlight(8);
            DoorClickCon.Instance.CloseDoorHigh(0);
            GameObjMan.Instance.SetPosition(10);
            GameObjMan.Instance.OpenObjCon(9);
            return;
        }
        if (currentLabIndex == 10)
        {
            GameObjMan.Instance.CloseObjCon(1);
            DoorClickCon.Instance.CloseDoorHigh(0);

            DoorClickCon.Instance.SetHighlight(9);
            GameObjMan.Instance.SetPosition(0);
            GameObjMan.Instance.OpenObjCon(11);
            return;
        }
        if (isAssessmentMode)
        {
            DoorClickCon.Instance.SetHighlight(currentLabIndex);
            DoorClickCon.Instance.SetHighlight(currentLabIndex);
            GameObjMan.Instance.OpenObjCon(currentLabIndex + 1);

        }
        // 高亮对应实验室门（仅跟练模式）
        if (!isAssessmentMode)
        {
            DoorClickCon.Instance.SetHighlight(currentLabIndex);
            GameObjMan.Instance.OpenObjCon(currentLabIndex + 1);

        }

    }

    // 点击更衣室门
    public void OnDressingRoomDoorClicked()
    {
        if (!canEnterDressingRoom) return;
        canEnterDressingRoom = false; // 进入后立即禁止再次进入
        //dressingRoomDoor.SetHighlight(false);
        DoorClickCon.Instance.CloseHighlightAll(); // 关闭更衣室门高亮

    }


    // 退出按钮（柜子UI）
    public void OnExitLockerClicked()
    {
        //lockerUIPanel.SetActive(false);
        EnterCorridor();
        //labDoors[currentLabIndex].SetHighlight(!isAssessmentMode);
    }

    // 实验室门点击
    public void OnLabDoorClicked(int doorIndex)
    {
        if (doorIndex != currentLabIndex) return;
        DoorClickCon.Instance.CloseHighlightAll(); // 高亮更衣室门

        //labDoors[doorIndex].SetHighlight(false);
        //CompleteLabProcess();
    }


    // 完成面板继续按钮
    public void OnContinueClicked()
    {
        //completePanel.SetActive(false);
        if (isAssessmentMode)
        {
            SelectAssessmentMode(); // 重新进入考核
        }
        else
        {
            //labsPanel.SetActive(true);
            currentLabIndex = -1;
            currentLabName = "";
        }
        // 重置进入权限
        canEnterDressingRoom = false;
    }

    // 完成面板取消按钮
    public void OnCancelClicked()
    {
        //completePanel.SetActive(false);
        //labsPanel.SetActive(true);
        currentLabIndex = -1;
        currentLabName = "";

        // 重置进入权限
        canEnterDressingRoom = false;
    }

    // 更新所有门的高亮状态
    void UpdateDoorHighlights(bool state)
    {
        //DoorClickCon.Instance.UpdateDoorHighlights(state); // 高亮更衣室门

        //foreach (var door in labDoors)
        //{
        //    door.SetHighlight(state);
        //}

    }

    // 通用方法：高亮物体
    public void HighlightObject(GameObject obj)
    {
        if (obj == null)
        {
            if (currentHighlight != null)
            {
                currentHighlight.GetComponent<Outline>().enabled = false; // 考核模式不显示高亮
                currentHighlight = null;
            }
            return;
        }
        if (currentHighlight != null && currentHighlight.transform.Find("Canvas(Clone)") != null)
        {
            currentHighlight.transform.Find("Canvas(Clone)").gameObject.SetActive(false);
        }
        if (currentHighlight != null)
        {
            // 取消之前的高亮
            currentHighlight.GetComponent<Outline>().enabled = false;
            if (currentHighlight.GetComponent<InteractableObject>() != null)
                currentHighlight.GetComponent<InteractableObject>().enabled = false;

        }
        //obj.SetActive(true);    
        var outline = obj.GetComponent<Outline>();
        if (obj.transform.Find("Canvas(Clone)") != null)
        {
            obj.transform.Find("Canvas(Clone)").gameObject.SetActive(true);
            //obj.transform.Find("IconWhiteExclamation").gameObject.SetActive(true);
        }

        //if (outline == null) outline = obj.AddComponent<Outline>();
        if (obj.GetComponent<InteractableObject>() != null)
            obj.GetComponent<InteractableObject>().enabled = true;
        obj.GetComponent<BoxCollider>().enabled = true;
        outline.enabled = !isAssessmentMode; // 考核模式不显示高亮
        outline.OutlineColor = Color.yellow;
        obj.SetActive(true);

        //outline.OutlineWidth = 5f;
        currentHighlight = obj;

    }
}