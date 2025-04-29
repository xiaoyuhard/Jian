using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����ʵ������������ʾ��UI
/// </summary>
public class ComputerUI :UICaoZuoBase
{

    public static ComputerUI Instance { get; private set; }


    void Awake()
    {
        InitializeManager();
    }

    // ��ʽ��ʼ�����������༭�����ã�
    public void InitializeManager()
    {
        if (Instance == null)
        {
            Instance = this;

            // ȷ���༭���˳�ʱ����ʵ��
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
        }
    }

#if UNITY_EDITOR
    private void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange state)
    {
        if (state == UnityEditor.PlayModeStateChange.ExitingEditMode)
        {
            Instance = null;
        }
    }
#endif

    public List<GameObject> comList;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject go in comList)
        {
            Button btn = go.transform.GetChild(0).GetComponent<Button>();
            btn.onClick.AddListener(ClickCom);
        }
        //ShowComUI();
    }

    private void ClickCom()
    {
        GameManager.Instance.SetStepDetection(true);

        //ShowComUI();
    }

    int index = 0;
    public void ShowComUI()
    {
        comList[index].SetActive(true);
        index++;
        if (index > comList.Count)
        {
            UIManager.Instance.OpenUICaoZuo("ChromatographUI");

        }
    }

    public void ShowComUI(int index)
    {
        comList[index].SetActive(true);

    }

    public void CloseUI()
    {
        foreach(GameObject go in comList)
        {
            go.SetActive(false);
        }
    }

    private void OnDisable()
    {
        foreach (GameObject go in comList)
        {
            go.SetActive(false);
        }
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
