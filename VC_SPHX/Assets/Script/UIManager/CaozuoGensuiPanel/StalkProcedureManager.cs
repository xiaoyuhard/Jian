using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StalkProcedureManager : UICaoZuoBase
{
    // ��̬ʵ��
    private static StalkProcedureManager _instance;
    // ȫ�ַ��ʵ�
    public static StalkProcedureManager Instance
    {
        get
        {
            if (_instance == null)
            {
                //// ���ʵ�������ڣ������ڳ����в���
                //_instance = FindObjectOfType<StalkProcedureManager>();

                //// �����Ȼ�����ڣ�����һ���µ�GameObject�����ؽű�
                //if (_instance == null)
                //{
                //    GameObject singletonObject = new GameObject("GameManager");
                //    _instance = singletonObject.AddComponent<StalkProcedureManager>();
                //    //DontDestroyOnLoad(singletonObject); // �糡���־û�
                //}
            }
            return _instance;
        }
    }


    public TextMeshProUGUI displayText; // UI��ʾ���
    public TextMeshProUGUI txtStepName;
    public TextMeshProUGUI txtStepNum;
    private int index = 0;   //Ҫʹ�ò���Ϊ�ڼ����±�

    // ��ʼ��ʱ����ظ�ʵ��
    private void Awake()
    {
        _instance = this;

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject); // �����ظ�ʵ��
            return;
        }

        //DontDestroyOnLoad(this.gameObject); // ȷ���־û�
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
            Debug.Log(index + " shul  " + list.Count);
            if (index < list.Count)
            {
                displayText.text = list[index].parent;
                txtStepNum.text = $"{index + 1}/{list.Count}";
                txtStepName.text = list[index].iconName;
            }
            else
                Debug.LogError($"indexԽ�磡index:{index} list.Count:{list.Count}");
        }
        else
        {
            Debug.LogError($"list Ϊ null...");
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
