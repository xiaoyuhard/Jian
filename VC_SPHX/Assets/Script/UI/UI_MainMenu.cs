using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class UI_MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.DonotCloseUI(UINameType.UI_HomeManager);
        UIManager.Instance.OpenUI(UINameType.UI_HomeManager);
        //UIManager.Instance.OpenUI(UINameType.UI_ZhishiManager);
        UIManager.Instance.OpenUI(UINameType.UI_CaozuoManager);

        GameData.Instance.CurrentExperiment = Experiment.Unknown;
        GameData.Instance.CurExpSubIndex = -1;
        GameData.Instance.IsTestMode = false;

        print("SystemInfo.supportsMultisampledTextures " + SystemInfo.supportsMultisampledTextures);

        //if (SystemInfo.supportsMultisampledTextures)
        //{
        //    QualitySettings.antiAliasing = 4; // 启用 4x MSAA
        //}
        //else
        //{
        //    // 降级为后处理抗锯齿
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
