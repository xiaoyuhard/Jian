using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProcedureItem", menuName = "Data/ProcedureItem")]
public class ProcedureData : ScriptableObject
{
    public string id;      // 实验名字
    public string procedure;   //第几步骤

}
