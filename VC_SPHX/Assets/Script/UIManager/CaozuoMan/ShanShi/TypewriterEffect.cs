using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 字体滚动效果 走马灯
/// </summary>
public class TypewriterEffect : MonoBehaviour
{
    public Text marqueeText;     // 绑定的旧版Text组件
    public float scrollSpeed = 0.1f; // 滚动速度（像素/秒）

    private RectTransform textRect;
    private RectTransform containerRect;
    private float textWidth;
    private float containerWidth;
    private Vector2 startPos;

    private void OnEnable()
    {
        StartCoroutine(CloseObj());

    }
    void Start()
    {
        StartCoroutine(CloseObj());

    }

    IEnumerator CloseObj()
    {
        yield return new WaitForSeconds(0.01f);
        textRect = marqueeText.GetComponent<RectTransform>();
        startPos = textRect.anchoredPosition;
        textWidth = marqueeText.preferredWidth;
        containerRect = transform.parent.GetComponent<RectTransform>(); // 父级容器

        // 强制更新布局以获取正确宽度
        Canvas.ForceUpdateCanvases();
        containerWidth = containerRect.rect.width;
        rectBl = true;
    }
    bool rectBl = false;

    void Update()
    {
        if (!rectBl) return;
        // 如果文本长度 <= 容器宽度，无需滚动
        if (textWidth <= containerWidth) return;
        // 向左移动文本
        textRect.anchoredPosition += Vector2.left * 10f * Time.deltaTime;

        // 当文本右端完全移出容器时，重置到右侧
        float currentTextRightEdge = textRect.anchoredPosition.x + textWidth;
        if (currentTextRightEdge < containerWidth)
        {
            textRect.anchoredPosition = new Vector2(containerWidth, startPos.y);
        }
    }
}
