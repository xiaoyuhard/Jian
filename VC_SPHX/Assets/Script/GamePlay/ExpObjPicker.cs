using System;
using System.Collections;
using System.Collections.Generic;
using RTS;
using UnityEngine;
using UnityEngine.EventSystems;

//实验物体 点击触发
public class ExpObjPicker : MonoSingletonBase<ExpObjPicker>
{
    public LayerMask layer = -1;
    /// <summary>
    /// 点击到场景中的物体时触发
    /// </summary>
    public Action<RaycastHit> OnHitObj;
    
    Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var clickUI = EventSystem.current.IsPointerOverGameObject();

            if (!clickUI)
            {
                //print("IsPointerOverGameObject " + clickUI);

                var ray = mainCam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit, 10, layer))
                {
                    Debug.Log("[ExpObjPicker] hit " + hit.transform, hit.transform);
                    OnHitObj?.Invoke(hit);
                }
            }
        }
    }


}
