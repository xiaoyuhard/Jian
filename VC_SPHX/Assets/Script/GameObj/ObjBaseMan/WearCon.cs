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
        //guizi.GetComponent<Outline>().enabled = true; //柜子高亮
    }
    bool isTriggerCabint = false;
    //点击柜子 打开换装界面 同时发送哪个操作的tag
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

    //接收到从操作传过来到底是哪个操作的tag 
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


    // 柜子点击处理（通用）
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
