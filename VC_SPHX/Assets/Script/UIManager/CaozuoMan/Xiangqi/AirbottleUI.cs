using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirbottleUI : UICaoZuoBase
{
    public RawImage rawImage;
    private void OnEnable()
    {
        StartCoroutine(WaitRawIamgePlay());

    }

    IEnumerator WaitRawIamgePlay()
    {
        yield return new WaitForSeconds(1);
        GameManager.Instance.stepDetection = true;
        UIManager.Instance.CloseUICaoZuo("AirbottleUI");

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
