using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �������Ч�� �����
/// </summary>
public class TypewriterEffect : MonoBehaviour
{
    public Text marqueeText;     // �󶨵ľɰ�Text���
    public float scrollSpeed = 0.1f; // �����ٶȣ�����/�룩

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
        containerRect = transform.parent.GetComponent<RectTransform>(); // ��������

        // ǿ�Ƹ��²����Ի�ȡ��ȷ���
        Canvas.ForceUpdateCanvases();
        containerWidth = containerRect.rect.width;
        rectBl = true;
    }
    bool rectBl = false;

    void Update()
    {
        if (!rectBl) return;
        // ����ı����� <= ������ȣ��������
        if (textWidth <= containerWidth) return;
        // �����ƶ��ı�
        textRect.anchoredPosition += Vector2.left * 10f * Time.deltaTime;

        // ���ı��Ҷ���ȫ�Ƴ�����ʱ�����õ��Ҳ�
        float currentTextRightEdge = textRect.anchoredPosition.x + textWidth;
        if (currentTextRightEdge < containerWidth)
        {
            textRect.anchoredPosition = new Vector2(containerWidth, startPos.y);
        }
    }
}
