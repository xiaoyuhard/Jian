using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdjustScrollViewSize : MonoBehaviour
{
    public ScrollRect scrollRect; // ScrollView �� ScrollRect ���
    public RectTransform content; // Content �� RectTransform
    public float maxHeight = 700f; // ScrollView �����߶�
    public float minHeight = 60f; // ScrollView ����С�߶�

    void Start()
    {
        // ��ʼʱ����һ�γߴ�

        StartCoroutine(UpdateScrollViewHeight());



    }
    // �� Content �仯ʱ���ô˷��������綯̬���/ɾ���������

    private IEnumerator UpdateScrollViewHeight()
    {
        yield return new WaitForSeconds(0.01f);
        // ���� Content ���ܸ߶ȣ�����������ͼ�ࣩ
        float contentHeight = content.rect.height;

        // ���� Content �߶ȵ��� ScrollView �ĸ߶�
        float targetHeight = Mathf.Clamp(contentHeight, minHeight, maxHeight);

        // ���� ScrollView �ĸ߶�
        scrollRect.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            targetHeight
        );
    }

}
