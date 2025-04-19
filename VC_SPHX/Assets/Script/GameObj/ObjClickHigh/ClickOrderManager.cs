using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOrderManager : MonoBehaviour
{
    public static ClickOrderManager Instance;

    [SerializeField] private List<InteractableObject> clickOrder = new();
    private int _currentIndex;

    private void Awake()
    {
        Instance = this;
        DisableAllColliders();
        EnableNextCollider();
    }

    public void ReportObjectClicked(InteractableObject obj)
    {
        if (obj != clickOrder[_currentIndex]) return;

        obj.OnCorrectClick.Invoke();
        _currentIndex++;
        //EnableNextCollider();
    }

    private void EnableNextCollider()
    {
        if (_currentIndex < clickOrder.Count)
        {
            clickOrder[_currentIndex].GetComponent<Collider>().enabled = true;
        }
    }

    private void DisableAllColliders()
    {
        foreach (var obj in clickOrder)
        {
            obj.GetComponent<Collider>().enabled = false;
        }
    }
}
