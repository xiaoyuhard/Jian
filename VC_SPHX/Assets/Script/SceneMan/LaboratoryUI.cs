using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaboratoryUI : MonoSingletonBase<LaboratoryUI>
{
    [SerializeField] private Button[] _labButtons;

    /// <summary>
    /// 禁用非当前实验室按钮
    /// </summary>
    public void DisableOtherButtons(int allowedLabID)
    {
        foreach (Button btn in _labButtons)
        {
            int btnLabID = btn.GetComponent<LabButton>().LabID;
            btn.interactable = (btnLabID == allowedLabID);
        }
    }

    /// <summary>
    /// 启用所有实验室按钮
    /// </summary>
    public void EnableAllButtons()
    {
        foreach (Button btn in _labButtons)
        {
            btn.interactable = true;
        }
    }
}
// 实验室按钮组件
public class LabButton : MonoSingletonBase<LabButton>
{
    public int LabID; // 在Inspector中设置

    public void OnClick()
    {
        JoinGameManager.Instance.SelectLaboratory(LabID);
    }
}
