using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ʵ��������ײ������ʾ
public class ExpObjTrigger : MonoBehaviour
{
    /// <summary>
    /// �Ƿ�Ӧ�ô�����һ���裨��ʱ����Ҫ����������¹���Ҫ������Ŵ�����һ����
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
