using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//ÊµÑé½áÊø
public class UI_ExpEnd : UIBase
{
    public Button btnClose;

    // Start is called before the first frame update
    void Start()
    {
        btnClose.onClick.AddListener(CloseUI);
    }

    void CloseUI()
    {
        //UIManager.Instance.CloseUI(UINameType.UI_ShowPicture);
        SceneMgr.LoadScene(GameScene.Main);
    }

}
