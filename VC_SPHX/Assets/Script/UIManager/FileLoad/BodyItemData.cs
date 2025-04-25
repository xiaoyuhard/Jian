using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BodyItem", menuName = "Body/BodyItem")]
[System.Serializable]
public class BodyItemData : ScriptableObject
{
    public Dictionary<string, BodyItem> bodyItemData = new Dictionary<string, BodyItem>(); //��λ����

}

public class BodyItem
{
    public string bodyId;      // ����
    public string bodyExplanation; //��λ˵��

}
