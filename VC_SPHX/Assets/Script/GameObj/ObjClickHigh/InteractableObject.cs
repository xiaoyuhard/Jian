using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    public UnityEvent OnCorrectClick;

    private void OnMouseDown()
    {
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (Camera.main == null) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.transform, hit.transform);

            if (hit.transform.name == transform.name)
            {
                //ClickOrderManager.Instance.ReportObjectClicked(this);
                GameManager.Instance.SetStepDetection(true);
                transform.GetComponent<Collider>().enabled = false;
                //transform.Find("IconWhiteExclamation").gameObject.SetActive(false);
                //MessageCenter.Instance.Send(EventName.Exp_NextStep);
            }
        }
    }
}
