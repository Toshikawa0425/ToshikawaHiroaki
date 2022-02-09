using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRange : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<EventBase>())
        other.GetComponent<EventBase>().StartEvent();
    }
}
