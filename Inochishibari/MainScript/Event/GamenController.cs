using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GamenController : MonoBehaviour
{
    [SerializeField]
    private UnityEvent afterOpenEvent;
    [SerializeField]
    private UnityEvent afterCloseEvent;

    public void OpenGamen(float _time)
    {
        DisplayManager.Instance.GamenOpen(_time);

        if(afterOpenEvent.GetPersistentEventCount() > 0)
        {
            DisplayManager.Instance.SetAfterEvent(afterOpenEvent);
        }
    }

    public void CloseGamen(float _time)
    {
        DisplayManager.Instance.GamenClose(_time);

        if(afterCloseEvent.GetPersistentEventCount() > 0)
        {
            DisplayManager.Instance.SetAfterEvent(afterCloseEvent);
        }
    }
}
