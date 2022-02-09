using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnActiveEvent : MonoBehaviour
{
    [SerializeField]
    private UnityEvent events;

    private void OnEnable()
    {
        events.Invoke();
    }
}
