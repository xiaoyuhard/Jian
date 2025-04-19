using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XiangqiExamineUI : UICaoZuoBase
{
    public Button recordBtn;
    public Button selectBtn;
    public GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        recordBtn.onClick.AddListener(Record);
        selectBtn.onClick.AddListener(Select);
    }

    //记录信息并通知负责人 的操作
    private void Select()
    {
        UIManager.Instance.CloseUICaoZuo("XiangqiExamineUI");
        obj.SetActive(false);
        GameManager.Instance.stepDetection = true;

    }

    //选择药品 告知 进行操作
    private void Record()
    {
        UIManager.Instance.CloseUICaoZuo("XiangqiExamineUI");
        obj.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        obj.SetActive(true);
    }
    private void OnDisable()
    {
        obj.SetActive(false);
    }
}
