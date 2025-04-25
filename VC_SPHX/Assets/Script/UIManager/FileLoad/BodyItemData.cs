using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BodyItem", menuName = "Body/BodyItem")]
[System.Serializable]
public class BodyItemData : ScriptableObject
{
    public Dictionary<string, BodyItem> bodyItemData = new Dictionary<string, BodyItem>(); //部位数据

}

public class BodyItem
{
    public string bodyId;      // 名字
    public string bodyExplanation; //部位说明

}
