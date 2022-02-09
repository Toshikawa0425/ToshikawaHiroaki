using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCameraForward : MonoBehaviour
{
    private int flame = 0;
    private CharaMove charaMove;

    private void Start()
    {
        charaMove = GetComponent<CharaMove>();
    }
    private void Update()
    {
        if (flame > 5)
        {
            transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
        }
        else
        {
            flame++;
        }
    }
}
