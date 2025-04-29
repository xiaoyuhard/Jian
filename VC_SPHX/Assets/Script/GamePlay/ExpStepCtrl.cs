using System.Collections;
using System.Collections.Generic;
using RTS;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class ExpStepCtrl : MonoSingletonBase<ExpStepCtrl>
{
    public ExpStepFragment[] stepFragments;
    //����Ư������
    public GameObject floatText;
    //����ģʽ
    public bool m_testMode = false;
    //����index���������
    public int mStepIndex = 0;

    GameObject lastTriggerObj;
    bool lastTriggerObjSelfActive;
    GameObject lastLightObj;
    ExpStep mCachedExpStep;
    TextMeshPro lastText;
    bool isPlayingAnim = false;

    public int StepIndex => mStepIndex;
    /// <summary>
    /// ��ǰ���е�ʵ�鲽��
    /// </summary>
    public ExpStep CurExpStep => mCachedExpStep;

    // Start is called before the first frame update
    void Start()
    {
        m_testMode = GameData.Instance.IsTestMode;
        CheckExpObj();
    }

    // Update is called once per frame
    void Update()
    {
        //���޲�����
        if (Application.isEditor || Debug.isDebugBuild)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (isPlayingAnim)
                    SkipAnim();
            }
        }
    }

    //���ʵ�������Ƿ���ȷ
    void CheckExpObj()
    {
        for (int i = 0; i < stepFragments.Length; i++)
        {
            var frag = stepFragments[i];

            if (frag == null)
            {
                Debug.LogError($"������Ϊ�գ���������Ƿ���ȷ�� Index:{i}");
                continue;
            }

            var fragObj = frag.gameObject;
            var step = frag.stepFragment;
            var triggerObj = step.triggerObj;
            var lightObj = step.lightObj;
            var type = step.type;

            if (triggerObj == null)
            {
                Debug.LogError($"������ triggerObj Ϊ�գ���������Ƿ���ȷ�� Index:{i}");
            }
            else
            {
                step.triggerObjSelfActive = triggerObj.activeSelf;
            }

            if (lightObj == null)
            {
                lightObj = triggerObj;
                step.lightObj = triggerObj;
                //Debug.LogError($"������ lightObj Ϊ�գ���������Ƿ���ȷ�� Index:{i}");
                Debug.Log($"������ lightObj Ϊ�գ��Զ�����Ϊ triggerObj������Ƿ���ȷ�� Index:{i}");
            }

            if (type == ExpActionType.ClickPlayAnim)
            {
                if (step.director == null)
                    Debug.LogError($"������Ϊ {type}���� director Ϊ�գ���������Ƿ���ȷ�� Index:{i}������{fragObj.name}", fragObj);
            }
            else if (type == ExpActionType.ClickShowImage)
            {
                if (step.pictures.Length == 0)
                    Debug.LogError($"������Ϊ {type}���� pictures Ϊ�գ���������Ƿ���ȷ�� Index:{i}");
            }

            if (lightObj)
                InitLightObj(lightObj);
        }
    }

    /// <summary>
    /// ��ʼִ��ʵ�鲽�� 
    /// </summary>
    public void StartExp()
    {
        NextStep();
    }

    /// <summary>
    /// ִ��ʵ����һ��
    /// </summary>
    public void NextStep()
    {
        if (mStepIndex >= stepFragments.Length)
        {
            print("=======ʵ�������cur mStepIndex: " + mStepIndex);
            UIManager.Instance.OpenUI(UINameType.UI_CaozuoManager);
            UIManager.Instance.CloseUICaoZuo(UINameType.UI_ProTipsMan);
            GameObjMan.Instance.CLoseFirst();
            return;
        }

        //��������ǰ����������һ��
        if (isPlayingAnim)
            return;

        print("cur mStepIndex: " + mStepIndex);

        var curStep = stepFragments[mStepIndex].stepFragment;
        var stepType = curStep.type;
        var triggerObj = curStep.triggerObj;
        var lightObj = curStep.lightObj;

        if (triggerObj == null)
        {
            Debug.LogError($"{stepFragments[mStepIndex].name} triggerObj Ϊ�գ�����ִ����һ������������Ƿ���ȷ������", stepFragments[mStepIndex].gameObject);
            mStepIndex++;
            NextStep();
            return;
        }
        else
        {
            Debug.Log($"�������壺{triggerObj}", triggerObj);
        }

        SetLastStepObjOff();
        StalkProcedureManager.Instance.UpdateUIInf(mStepIndex);

        lastTriggerObj = triggerObj;
        lastTriggerObjSelfActive = curStep.triggerObjSelfActive;
        lastLightObj = lightObj;

        //��ʾ�������壬��������
        triggerObj.SetActive(true);
        triggerObj.layer = LayerMask.NameToLayer("ClickModel");

        lightObj.SetActive(true);
        SetLightObj(lightObj, true);

        if (stepType == ExpActionType.TriggerObject)
        {

        }
        else if (stepType == ExpActionType.ClickOpenCabinet)
        {
            ExpObjPicker.Instance.OnHitObj = OnClickGuizi;
        }
        else if (stepType == ExpActionType.ClickPlayAnim)
        {
            //�������������Ϊ ������� ����
            mCachedExpStep = curStep;
            ExpObjPicker.Instance.OnHitObj = OnClickAnimObj;
            SpawnFloatText(triggerObj, curStep.name);
        }
        else if (stepType == ExpActionType.ClickShowImage)
        {
            //�������������Ϊ ������� ����
            mCachedExpStep = curStep;
            ExpObjPicker.Instance.OnHitObj = OnClickPicObj;
            SpawnFloatText(triggerObj, curStep.name);
        }

        mStepIndex++;
    }

    //������������Ư��
    void SpawnFloatText(GameObject triggerObj, string name)
    {
        var textObj = Instantiate(floatText);
        var col = triggerObj.GetComponent<BoxCollider>();
        Vector3 textPos = triggerObj.transform.position;
        textPos.y += col.size.y;
        textPos.y += 0.1f;
        textObj.transform.position = textPos;
        textObj.transform.parent = triggerObj.transform;

        lastText = textObj.GetComponentInChildren<TextMeshPro>();
        lastText.text = name.Trim();
    }

    //��ʼ��������
    void InitLightObj(GameObject obj)
    {
        //������Ч��
        var outline = obj.GetOrAddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineWidth = 5.6f;
        //ColorUtility.TryParseHtmlString("#FFDB00", out var col);
        //outline.OutlineColor = col;
        outline.OutlineColor = GameHelper.HexToColor("FFDB00");
        outline.enabled = false;

        //�����ײ�У�Ϊ�˵��
        obj.GetOrAddComponent<AddBoxCollider>();
        //obj.GetOrAddComponent<InteractableObject>();

        //ȷ�����Ե��
        var boxCol = obj.GetOrAddComponent<BoxCollider>();
        boxCol.enabled = false;
    }
    //���ø�������״̬
    void SetLightObj(GameObject obj, bool showLight)
    {
        //������Ч��
        var outline = obj.GetOrAddComponent<Outline>();

        if (m_testMode)
            outline.enabled = false;
        else
            outline.enabled = showLight;

        //ȷ�����Ե��
        var boxCol = obj.GetOrAddComponent<BoxCollider>();
        boxCol.enabled = showLight;
    }
    void SetLightOff(GameObject obj)
    {
        var outline = obj.GetComponent<Outline>();

        if (outline)
            outline.enabled = false;
    }

    //������һ����ʱ����һ����Ҫȡ��
    void SetLastStepObjOff()
    {
        if (lastTriggerObj != null)
        {
            lastTriggerObj.SetActive(lastTriggerObjSelfActive);
        }

        if (lastLightObj != null)
        {
            SetLightObj(lastLightObj, false);
        }
    }

    //������Ӵ򿪽���
    void OnClickGuizi(RaycastHit hit)
    {
        if (hit.transform.tag == "Guizi")
        {
            UIManager.Instance.OpenUICaoZuo(UINameType.UI_GenyishiMan);
            ExpObjPicker.Instance.OnHitObj -= OnClickGuizi;
        }
    }

    //�����������
    void OnClickAnimObj(RaycastHit hit)
    {
        var curStep = mCachedExpStep;

        if (hit.transform.gameObject == curStep.triggerObj)
        {
            var director = curStep.director;
            director.gameObject.SetActive(true);
            director.Play();
            //�����ȡ������
            SetLightOff(curStep.lightObj);

            print($"���Ŷ��� {director}��ʱ����{director.duration}");

            StartCoroutine(nameof(PlayAnim), director);
            ExpObjPicker.Instance.OnHitObj -= OnClickAnimObj;
            isPlayingAnim = true;

            if (lastText)
            {
                Destroy(lastText.gameObject);
                lastText = null;
            }

        }
    }

    IEnumerator PlayAnim(PlayableDirector director)
    {
        yield return new WaitForSeconds((float)director.duration);

        //�������꣬���Խ�����һ��
        print("�����������ˣ�������һ��");
        AnimEndOver();
        NextStep();
    }

    //����������һЩ����
    void AnimEndOver()
    {
        var director = mCachedExpStep.director;
        director.Stop();
        //director.time = 0;
        director.Evaluate();
        director.gameObject.SetActive(false);

        isPlayingAnim = false;
    }

    //��������������������һ��
    void SkipAnim()
    {
        print("����������������һ��");
        StopCoroutine(nameof(PlayAnim));
        AnimEndOver();
        NextStep();
    }

    //�����ʾͼƬ
    void OnClickPicObj(RaycastHit hit)
    {
        var curStep = mCachedExpStep;

        if (hit.transform.gameObject == curStep.triggerObj)
        {
            UIManager.Instance.OpenUI(UINameType.UI_ShowPicture);
            MessageCenter.Instance.Send(EventName.UI_ShowPicture);
            ExpObjPicker.Instance.OnHitObj -= OnClickPicObj;

            if (lastText)
            {
                Destroy(lastText.gameObject);
                lastText = null;
            }
        }
    }

}