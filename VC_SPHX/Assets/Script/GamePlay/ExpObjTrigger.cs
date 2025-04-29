using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//实验物体碰撞触发提示
public class ExpObjTrigger : MonoBehaviour
{
    /// <summary>
    /// 是否应该触发下一步骤（有时候不需要触发，如打开衣柜，需要穿戴玩才触发下一步）
    /// </summary>
    public bool shouldTriggerNext = true;

    BoxCollider boxCol;
    bool hasDone = false;

    // Start is called before the first frame update
    void Start()
    {
        boxCol = GetComponent<BoxCollider>();
        boxCol.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        print("OnTriggerEnter " + other.transform);

        if (hasDone)
            return;

        if (other.gameObject.tag == GameTag.Player)
        {
            hasDone = true;

            if (shouldTriggerNext)
                MessageCenter.Instance.Send(EventName.Exp_NextStep);
        }
    }
}
