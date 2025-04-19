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
    [SerializeField] private RectTransform content;      // ScrollView的Content
    [SerializeField] private GameObject togglePrefab;       // Toggle预制体
    [SerializeField] private ScrollRect scrollRect;     // ScrollView组件

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

    //根据调用显示对应的数量
    public void GetFromPool(int count)
    {

        if (pools.Count < count)
            ExpandPool(5); // 池不足时自动扩容
        for (int i = 0; i < count; i++)
        {
            pools[i].SetActive(true);
        }

    }

    //对象池中不够的情况下进行再补充创建
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

    //切换关闭所有
    public void ClosePool()
    {
        foreach (var item in pools)
        {
            item.SetActive(false);
        }
    }


    ///一打开显示
    public void ShowPool()
    {
        for (int i = 0; i < pools.Count; i++)
        {
            pools[i].SetActive(true);
        }
    }
}
