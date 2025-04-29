using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �������Ч�� �����
/// </summary>
public class TypewriterEffect : MonoBehaviour
{
    public Text marqueeText;     // �󶨵ľɰ�Text���
    public float scrollSpeed=0.1f; // �����ٶȣ�����/�룩

    private RectTransform textRect;
    private RectTransform containerRect;
    private float textWidth;
    private float containerWidth;
    private Vector2 startPos;

    void Start()
    {
        textRect = marqueeText.GetComponent<RectTransform>();
        containerRect = transform.parent.GetComponent<RectTransform>(); // ��������
        startPos = textRect.anchoredPosition;

        // ǿ�Ƹ��²����Ի�ȡ��ȷ���
        Canvas.ForceUpdateCanvases();
        textWidth = marqueeText.preferredWidth;
        containerWidth = containerRect.rect.width;
    }

    void Update()
    {
        // ����ı����� <= ������ȣ��������
        if (textWidth <= containerWidth) return;

        // �����ƶ��ı�
        textRect.anchoredPosition += Vector2.left * scrollSpeed * Time.deltaTime;

        // ���ı��Ҷ���ȫ�Ƴ�����ʱ�����õ��Ҳ�
        float currentTextRightEdge = textRect.anchoredPosition.x + textWidth;
        if (currentTextRightEdge < containerWidth)
        {
            textRect.anchoredPosition = new Vector2(containerWidth, startPos.y);
        }
    }
}
