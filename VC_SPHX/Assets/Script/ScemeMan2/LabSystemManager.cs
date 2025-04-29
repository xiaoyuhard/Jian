using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LabSystemManager : MonoBehaviour
{
    // ����ģʽ
    public static LabSystemManager Instance;

    private bool canEnterDressingRoom = false; // ���Ƹ����ҽ���Ȩ��

    private int currentLabIndex = -1; // ��ǰѡ���ʵ���ұ��
    private bool isAssessmentMode = false; // �Ƿ�Ϊ����ģʽ
    public GameObject currentHighlight; // ��ǰ��������
    private string currentLabName = "";

    void Awake()
    {
        Instance = this;
        InitializeSystem();
    }

    // ϵͳ��ʼ��
    void InitializeSystem()
    {

        // ��������ʵ�����Ÿ���
        UpdateDoorHighlights(false);
        //dressingRoomDoor.SetHighlight(false);
        canEnterDressingRoom = false; // ��ʼ��ʱ��ֹ����
    }

    // ʵ���Ұ�ť��� (��UI��ť�¼��а�)
    public void OnLabButtonClicked(int labIndex, string labName)
    {
        currentLabIndex = labIndex;
        currentLabName = labName;
        GameObjMan.Instance.OpenObjCon(1);
        //labsPanel.SetActive(false);
        //optionPanel.SetActive(true); // ��ʾ����/����ѡ��
    }

    public int ReutrnCurrIndex()
    {
        return currentLabIndex;
    }

    /// <summary>
    /// ѡ�����ģʽ
    /// </summary>
    public void SelectPracticeMode()
    {
        GameObjMan.Instance.UpObjPosCon();

        isAssessmentMode = false;
        //optionPanel.SetActive(false);
        // ����ģʽ�����������Ҳ�����
        canEnterDressingRoom = true;
        DoorClickCon.Instance.SetHighlight(0); // ������������
        UIManager.Instance.OpenUICaoZuo(UINameType.UI_ProTipsMan);
        EnterCorridorGen();
        GameManager.Instance.tipStalkBl = true;

    }

    /// <summary>
    /// ѡ�񿼺�ģʽ
    /// </summary>
    public void SelectAssessmentMode()
    {
        if (currentLabIndex != 10 || currentLabIndex != 8 || currentLabIndex != 9)
        {
            DoorClickCon.Instance.SetHighlight(0); // ������������
        }
        GameObjMan.Instance.UpObjPosCon();
        isAssessmentMode = true;
        //optionPanel.SetActive(false);
        // ����ģʽ������뵫������
        canEnterDressingRoom = true;
        EnterCorridor();
        //dressingRoomDoor.SetHighlight(false); // ����ģʽ������

    }

    void EnterCorridorGen()
    {
        //CaozuoSceneCon.Instance.EnterLab(0);
        MessageCenter.Instance.Send("SendTiShiUIName", currentLabName);


    }

    // ��������
    void EnterCorridor()
    {
        //CaozuoSceneCon.Instance.EnterLab(0);

        // �ƶ���ҵ�����λ��
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
        // ������Ӧʵ�����ţ�������ģʽ��
        if (!isAssessmentMode)
        {
            DoorClickCon.Instance.SetHighlight(currentLabIndex);
            GameObjMan.Instance.OpenObjCon(currentLabIndex + 1);

        }

    }

    // �����������
    public void OnDressingRoomDoorClicked()
    {
        if (!canEnterDressingRoom) return;
        canEnterDressingRoom = false; // �����������ֹ�ٴν���
        //dressingRoomDoor.SetHighlight(false);
        DoorClickCon.Instance.CloseHighlightAll(); // �رո������Ÿ���

    }


    // �˳���ť������UI��
    public void OnExitLockerClicked()
    {
        //lockerUIPanel.SetActive(false);
        EnterCorridor();
        //labDoors[currentLabIndex].SetHighlight(!isAssessmentMode);
    }

    // ʵ�����ŵ��
    public void OnLabDoorClicked(int doorIndex)
    {
        if (doorIndex != currentLabIndex) return;
        DoorClickCon.Instance.CloseHighlightAll(); // ������������

        //labDoors[doorIndex].SetHighlight(false);
        //CompleteLabProcess();
    }


    // �����������ť
    public void OnContinueClicked()
    {
        //completePanel.SetActive(false);
        if (isAssessmentMode)
        {
            SelectAssessmentMode(); // ���½��뿼��
        }
        else
        {
            //labsPanel.SetActive(true);
            currentLabIndex = -1;
            currentLabName = "";
        }
        // ���ý���Ȩ��
        canEnterDressingRoom = false;
    }

    // ������ȡ����ť
    public void OnCancelClicked()
    {
        //completePanel.SetActive(false);
        //labsPanel.SetActive(true);
        currentLabIndex = -1;
        currentLabName = "";

        // ���ý���Ȩ��
        canEnterDressingRoom = false;
    }

    // ���������ŵĸ���״̬
    void UpdateDoorHighlights(bool state)
    {
        //DoorClickCon.Instance.UpdateDoorHighlights(state); // ������������

        //foreach (var door in labDoors)
        //{
        //    door.SetHighlight(state);
        //}

    }

    // ͨ�÷�������������
    public void HighlightObject(GameObject obj)
    {
        if (obj == null)
        {
            if (currentHighlight != null)
            {
                currentHighlight.GetComponent<Outline>().enabled = false; // ����ģʽ����ʾ����
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
            // ȡ��֮ǰ�ĸ���
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
        outline.enabled = !isAssessmentMode; // ����ģʽ����ʾ����
        outline.OutlineColor = Color.yellow;
        obj.SetActive(true);

        //outline.OutlineWidth = 5f;
        currentHighlight = obj;

    }
}