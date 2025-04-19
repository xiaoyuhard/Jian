using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaboratoryUI : MonoSingletonBase<LaboratoryUI>
{
    [SerializeField] private Button[] _labButtons;

    /// <summary>
    /// ���÷ǵ�ǰʵ���Ұ�ť
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
    /// ��������ʵ���Ұ�ť
    /// </summary>
    public void EnableAllButtons()
    {
        foreach (Button btn in _labButtons)
        {
            btn.interactable = true;
        }
    }
}
// ʵ���Ұ�ť���
public class LabButton : MonoSingletonBase<LabButton>
{
    public int LabID; // ��Inspector������

    public void OnClick()
    {
        JoinGameManager.Instance.SelectLaboratory(LabID);
    }
}
