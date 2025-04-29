using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//显示实验过程中的图片
public class UI_ShowPicture : UIBase
{
    public Image img;
    public Button btnNext;
    public Button btnClose;

    int picIndex = 0;
    ExpStep curStep;

    private void OnEnable()
    {
        MessageCenter.Instance.Register(EventName.UI_ShowPicture, ShowStepPic);
    }

    private void OnDisable()
    {
        MessageCenter.Instance.Unregister(EventName.UI_ShowPicture, ShowStepPic);
    }

    // Start is called before the first frame update
    void Start()
    {
        btnNext.onClick.AddListener(OnClickNextPic);
        btnClose.onClick.AddListener(CloseUI);
    }

    void ShowStepPic(string msg)
    {
        picIndex = 0;
        curStep = ExpStepCtrl.Instance.CurExpStep;
        img.sprite = curStep.pictures[0];
        //img.SetNativeSize();
    }

    //显示下一张
    void OnClickNextPic()
    {
        picIndex++;

        if (picIndex < curStep.pictures.Length)
        {
            img.sprite = curStep.pictures[picIndex];
            //img.SetNativeSize();
        }
        else
        {
            CloseUI();
            MessageCenter.Instance.Send(EventName.Exp_NextStep);
        }
    }

    void CloseUI()
    {
        UIManager.Instance.CloseUI(UINameType.UI_ShowPicture);
    }

}
