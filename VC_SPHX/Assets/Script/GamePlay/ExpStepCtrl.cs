using System.Collections;
using System.Collections.Generic;
using RTS;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class ExpStepCtrl : MonoSingletonBase<ExpStepCtrl>
{
    public ExpStepFragment[] stepFragments;
    //物体漂字引用
    public GameObject floatText;
    //考核模式
    public bool m_testMode = false;
    //步骤index，方便测试
    public int mStepIndex = 0;

    GameObject lastTriggerObj;
    bool lastTriggerObjSelfActive;
    GameObject lastLightObj;
    ExpStep mCachedExpStep;
    TextMeshPro lastText;
    bool isPlayingAnim = false;

    public int StepIndex => mStepIndex;
    /// <summary>
    /// 当前进行的实验步骤
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
        //仅限测试用
        if (Application.isEditor || Debug.isDebugBuild)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (isPlayingAnim)
                    SkipAnim();
            }
        }
    }

    //检查实验设置是否正确
    void CheckExpObj()
    {
        for (int i = 0; i < stepFragments.Length; i++)
        {
            var frag = stepFragments[i];

            if (frag == null)
            {
                Debug.LogError($"该设置为空！！！检查是否正确。 Index:{i}");
                continue;
            }

            var fragObj = frag.gameObject;
            var step = frag.stepFragment;
            var triggerObj = step.triggerObj;
            var lightObj = step.lightObj;
            var type = step.type;

            if (triggerObj == null)
            {
                Debug.LogError($"该设置 triggerObj 为空！！！检查是否正确。 Index:{i}");
            }
            else
            {
                step.triggerObjSelfActive = triggerObj.activeSelf;
            }

            if (lightObj == null)
            {
                lightObj = triggerObj;
                step.lightObj = triggerObj;
                //Debug.LogError($"该设置 lightObj 为空！！！检查是否正确。 Index:{i}");
                Debug.Log($"该设置 lightObj 为空！自动设置为 triggerObj，检查是否正确。 Index:{i}");
            }

            if (type == ExpActionType.ClickPlayAnim)
            {
                if (step.director == null)
                    Debug.LogError($"该设置为 {type}，但 director 为空！！！检查是否正确。 Index:{i}，对象：{fragObj.name}", fragObj);
            }
            else if (type == ExpActionType.ClickShowImage)
            {
                if (step.pictures.Length == 0)
                    Debug.LogError($"该设置为 {type}，但 pictures 为空！！！检查是否正确。 Index:{i}");
            }

            if (lightObj)
                InitLightObj(lightObj);
        }
    }

    /// <summary>
    /// 开始执行实验步骤 
    /// </summary>
    public void StartExp()
    {
        NextStep();
    }

    /// <summary>
    /// 执行实验下一步
    /// </summary>
    public void NextStep()
    {
        if (mStepIndex >= stepFragments.Length)
        {
            print("=======实验结束，cur mStepIndex: " + mStepIndex);
            UIManager.Instance.OpenUI(UINameType.UI_CaozuoManager);
            UIManager.Instance.CloseUICaoZuo(UINameType.UI_ProTipsMan);
            GameObjMan.Instance.CLoseFirst();
            return;
        }

        //动画结束前，不允许下一步
        if (isPlayingAnim)
            return;

        print("cur mStepIndex: " + mStepIndex);

        var curStep = stepFragments[mStepIndex].stepFragment;
        var stepType = curStep.type;
        var triggerObj = curStep.triggerObj;
        var lightObj = curStep.lightObj;

        if (triggerObj == null)
        {
            Debug.LogError($"{stepFragments[mStepIndex].name} triggerObj 为空，跳过执行下一步！检查配置是否正确！！！", stepFragments[mStepIndex].gameObject);
            mStepIndex++;
            NextStep();
            return;
        }
        else
        {
            Debug.Log($"触发物体：{triggerObj}", triggerObj);
        }

        SetLastStepObjOff();
        StalkProcedureManager.Instance.UpdateUIInf(mStepIndex);

        lastTriggerObj = triggerObj;
        lastTriggerObjSelfActive = curStep.triggerObjSelfActive;
        lastLightObj = lightObj;

        //显示触发物体，高亮物体
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
            //缓存这个步骤作为 点击动画 对象
            mCachedExpStep = curStep;
            ExpObjPicker.Instance.OnHitObj = OnClickAnimObj;
            SpawnFloatText(triggerObj, curStep.name);
        }
        else if (stepType == ExpActionType.ClickShowImage)
        {
            //缓存这个步骤作为 点击动画 对象
            mCachedExpStep = curStep;
            ExpObjPicker.Instance.OnHitObj = OnClickPicObj;
            SpawnFloatText(triggerObj, curStep.name);
        }

        mStepIndex++;
    }

    //生成物体名字漂字
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

    //初始高亮物体
    void InitLightObj(GameObject obj)
    {
        //添加描边效果
        var outline = obj.GetOrAddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineWidth = 5.6f;
        //ColorUtility.TryParseHtmlString("#FFDB00", out var col);
        //outline.OutlineColor = col;
        outline.OutlineColor = GameHelper.HexToColor("FFDB00");
        outline.enabled = false;

        //添加碰撞盒，为了点击
        obj.GetOrAddComponent<AddBoxCollider>();
        //obj.GetOrAddComponent<InteractableObject>();

        //确保可以点击
        var boxCol = obj.GetOrAddComponent<BoxCollider>();
        boxCol.enabled = false;
    }
    //设置高亮物体状态
    void SetLightObj(GameObject obj, bool showLight)
    {
        //添加描边效果
        var outline = obj.GetOrAddComponent<Outline>();

        if (m_testMode)
            outline.enabled = false;
        else
            outline.enabled = showLight;

        //确保可以点击
        var boxCol = obj.GetOrAddComponent<BoxCollider>();
        boxCol.enabled = showLight;
    }
    void SetLightOff(GameObject obj)
    {
        var outline = obj.GetComponent<Outline>();

        if (outline)
            outline.enabled = false;
    }

    //进行下一步的时候，上一步的要取消
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

    //点击柜子打开界面
    void OnClickGuizi(RaycastHit hit)
    {
        if (hit.transform.tag == "Guizi")
        {
            UIManager.Instance.OpenUICaoZuo(UINameType.UI_GenyishiMan);
            ExpObjPicker.Instance.OnHitObj -= OnClickGuizi;
        }
    }

    //点击动画物体
    void OnClickAnimObj(RaycastHit hit)
    {
        var curStep = mCachedExpStep;

        if (hit.transform.gameObject == curStep.triggerObj)
        {
            var director = curStep.director;
            director.gameObject.SetActive(true);
            director.Play();
            //点击后取消高亮
            SetLightOff(curStep.lightObj);

            print($"播放动画 {director}，时长：{director.duration}");

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

        //动画播完，可以进行下一步
        print("动画播放完了，进入下一步");
        AnimEndOver();
        NextStep();
    }

    //动画结束后，一些操作
    void AnimEndOver()
    {
        var director = mCachedExpStep.director;
        director.Stop();
        //director.time = 0;
        director.Evaluate();
        director.gameObject.SetActive(false);

        isPlayingAnim = false;
    }

    //快速跳过动画，进入下一步
    void SkipAnim()
    {
        print("跳过动画，进入下一步");
        StopCoroutine(nameof(PlayAnim));
        AnimEndOver();
        NextStep();
    }

    //点击显示图片
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