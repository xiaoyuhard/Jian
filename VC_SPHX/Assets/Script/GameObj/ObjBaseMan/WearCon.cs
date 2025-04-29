using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WearCon : MonoSingletonBase<WearCon>
{
    public GameObject guizi;
    public int index;

    public string dorCaozuoTag;
    public void JoinWear()
    {
        //CaozuoSceneCon.Instance.EnterLab(index);
        //guizi.GetComponent<Outline>().enabled = true; //���Ӹ���
    }
    bool isTriggerCabint = false;
    //������� �򿪻�װ���� ͬʱ�����ĸ�������tag
    public void MouseCabinet(string str)
    {
        if (isTriggerCabint)
            UIManager.Instance.OpenUICaoZuo(UINameType.UI_GenyishiMan);

    }
    private void OnEnable()
    {
        isTriggerCabint = false;

    }


    public void ResetObj()
    {
        guizi.GetComponent<Outline>().enabled = false;
        //MessageCenter.Instance.Send("SendWearToGenTag", dorCaozuoTag);

    }

    //���յ��Ӳ����������������ĸ�������tag 
    // Start is called before the first frame update
    void Start()
    {
        MessageCenter.Instance.Register("SendCaozuoToWear", DoorCaozuoTag);
        MessageCenter.Instance.Register("SendMouseToCabinet", MouseCabinet);

    }


    private void DoorCaozuoTag(string obj)
    {
        dorCaozuoTag = obj;
    }


    // Update is called once per frame
    void Update()
    {

    }


    // ���ӵ������ͨ�ã�
    public void OnCabinetClicked(int index)
    {
        //GameManager.Instance.hasDressed = true;
        UIManager.Instance.OpenUI(UINameType.UI_GenyishiMan);

    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag != "Player") return;
        LabSystemManager.Instance.HighlightObject(guizi);
        isTriggerCabint = true;
    }

    public void OnTriggerExit(Collider other)
    {
        LabSystemManager.Instance.HighlightObject(null);
        isTriggerCabint = false;

    }



}
