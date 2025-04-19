using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TogIntroduceItem;

public class DynamicScrollView : MonoSingletonBase<DynamicScrollView>
{
    [Header("UI References")]
    [SerializeField] private RectTransform content;      // ScrollView��Content
    [SerializeField] private GameObject togglePrefab;       // ToggleԤ����
    [SerializeField] private ScrollRect scrollRect;     // ScrollView���

    [SerializeField] private List<GameObject> pools;
    //public void CreateToggle(int count,string type)
    //{
    //    for (int i = 0; i < count; i++)
    //    {
    //        GameObject obj = Instantiate(togglePrefab,content.transform);
    //        obj.SetActive(false);
    //        pools.Add(obj);
    //        //ToggleButton togBtn = new ToggleButton();
    //        //togBtn.toggle= obj.GetComponent<Toggle>();
    //        //togBtn.label = obj.transform.GetChild(1).GetComponent<Text>();
    //        //togBtn.background = obj.transform.GetChild(0).GetComponent<Image>();
    //        //togBtn.toggle.group = content.GetComponent<ToggleGroup>();

    //    }
    //    TogIntroduceItem.Instance.AddTog(count);

    //}

    //���ݵ�����ʾ��Ӧ������
    public void GetFromPool(int count)
    {

        if (pools.Count < count)
            ExpandPool(5); // �ز���ʱ�Զ�����
        for (int i = 0; i < count; i++)
        {
            pools[i].SetActive(true);
        }

    }

    //������в���������½����ٲ��䴴��
    private void ExpandPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(togglePrefab, content.transform);
            obj.SetActive(false);
            pools.Add(obj);

        }
        TogIntroduceItem.Instance.AddTog(count);

    }

    //�л��ر�����
    public void ClosePool()
    {
        foreach (var item in pools)
        {
            item.SetActive(false);
        }
    }


    ///һ����ʾ
    public void ShowPool()
    {
        for (int i = 0; i < pools.Count; i++)
        {
            pools[i].SetActive(true);
        }
    }
}
