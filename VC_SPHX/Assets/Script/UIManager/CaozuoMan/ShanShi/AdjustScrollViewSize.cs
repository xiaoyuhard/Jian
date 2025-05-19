using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdjustScrollViewSize : MonoBehaviour
{
    public ScrollRect scrollRect; // ScrollView 的 ScrollRect 组件
    public RectTransform content; // Content 的 RectTransform
    public float maxHeight = 700f; // ScrollView 的最大高度
    public float minHeight = 60f; // ScrollView 的最小高度

    void Start()
    {
        // 初始时更新一次尺寸

        StartCoroutine(UpdateScrollViewHeight());



    }
    // 当 Content 变化时调用此方法（例如动态添加/删除子物体后）

    private IEnumerator UpdateScrollViewHeight()
    {
        yield return new WaitForSeconds(0.01f);
        // 计算 Content 的总高度（包括子物体和间距）
        float contentHeight = content.rect.height;

        // 根据 Content 高度调整 ScrollView 的高度
        float targetHeight = Mathf.Clamp(contentHeight, minHeight, maxHeight);

        // 设置 ScrollView 的高度
        scrollRect.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            targetHeight
        );
    }

}
