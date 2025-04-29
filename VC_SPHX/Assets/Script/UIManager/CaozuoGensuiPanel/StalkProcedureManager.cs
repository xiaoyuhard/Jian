using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StalkProcedureManager : UICaoZuoBase
{
    // 静态实例
    private static StalkProcedureManager _instance;
    // 全局访问点
    public static StalkProcedureManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // 如果实例不存在，尝试在场景中查找
                _instance = FindObjectOfType<StalkProcedureManager>();

                // 如果仍然不存在，创建一个新的GameObject并挂载脚本
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("GameManager");
                    _instance = singletonObject.AddComponent<StalkProcedureManager>();
                    DontDestroyOnLoad(singletonObject); // 跨场景持久化
                }
            }
            return _instance;
        }
    }


    public Text displayText; // UI显示组件
    private int index = 0;   //要使用步骤为第几条下标

    // 初始化时检查重复实例
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject); // 销毁重复实例
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject); // 确保持久化
        MessageCenter.Instance.Register("SendTiShiUIName", TiShiUIName);
        //UIManager.Instance.CloseUICaoZuo(UINameType.UI_ProTipsMan);
    }

    void Start()
    {
        //StartCoroutine(LoadAndSplitText());
        //MessageCenter.Instance.Register("SendTiShiUIName", TiShiUIName);
    }
    List<EquipmentItemData> list = new List<EquipmentItemData>();
    public void AddIndex()
    {
        index++;
    }

    public void UpdateUIInf(int index)
    {
        if (list != null)
        {
            if (index < list.Count)
                displayText.text = list[index].parent;
            else
                Debug.LogError($"index越界！index:{index} list.Count:{list.Count}");
        }
        else
        {
            Debug.LogError($"list 为 null...");
        }
    }

    public void TiShiUIName(string labName)
    {
        list = DataManager.Instance.GetItemById(labName);

        UpdateUIInf(0);
    }


    private void OnEnable()
    {
        index = 0;
        MessageCenter.Instance.Register("SendTiShiUIName", TiShiUIName);

    }


}
