using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnterExitEvent : MonoBehaviour
{
    [SerializeField]
    private UnityEvent enterEvent;
    [SerializeField]
    private UnityEvent exitEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (enterEvent.GetPersistentEventCount() > 0)
        {
            enterEvent.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (exitEvent.GetPersistentEventCount() > 0)
        {
            exitEvent.Invoke();
        }
    }
}
