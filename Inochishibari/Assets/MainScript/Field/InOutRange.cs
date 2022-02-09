using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InOutRange : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<InOutEvent>().SetInPos(transform.position);
    }

    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<InOutEvent>().SetOutPos(transform.position);
    }
}
