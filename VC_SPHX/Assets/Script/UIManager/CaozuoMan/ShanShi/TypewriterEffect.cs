using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 字体滚动效果 走马灯
/// </summary>
public class TypewriterEffect : MonoBehaviour
{
    public Text marqueeText;     // 绑定的旧版Text组件
    public float scrollSpeed=0.1f; // 滚动速度（像素/秒）

    private RectTransform textRect;
    private RectTransform containerRect;
    private float textWidth;
    private float containerWidth;
    private Vector2 startPos;

    void Start()
    {
        textRect = marqueeText.GetComponent<RectTransform>();
        containerRect = transform.parent.GetComponent<RectTransform>(); // 父级容器
        startPos = textRect.anchoredPosition;

        // 强制更新布局以获取正确宽度
        Canvas.ForceUpdateCanvases();
        textWidth = marqueeText.preferredWidth;
        containerWidth = containerRect.rect.width;
    }

    void Update()
    {
        // 如果文本长度 <= 容器宽度，无需滚动
        if (textWidth <= containerWidth) return;

        // 向左移动文本
        textRect.anchoredPosition += Vector2.left * scrollSpeed * Time.deltaTime;

        // 当文本右端完全移出容器时，重置到右侧
        float currentTextRightEdge = textRect.anchoredPosition.x + textWidth;
        if (currentTextRightEdge < containerWidth)
        {
            textRect.anchoredPosition = new Vector2(containerWidth, startPos.y);
        }
    }
}
