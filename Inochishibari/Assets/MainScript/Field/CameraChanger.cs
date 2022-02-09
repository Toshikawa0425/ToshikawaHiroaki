using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChanger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<CameraChangeArea>().SetInPos(transform.position);
    }

    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<CameraChangeArea>().SetOutPos(transform.position);
    }
}
