using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InOutEvent : MonoBehaviour
{
    [SerializeField]
    private float searchAngle = 60;

    [SerializeField]
    protected Vector3 inPos;
    [SerializeField]
    protected Vector3 outPos;

    protected Vector3 pathDirection;
    protected Vector3 forward;

    [SerializeField]
    private UnityEvent positiveEvents;
    [SerializeField]
    private UnityEvent negativeEvents;




    /*
    [SerializeField]
    private GameObject ball1;
    [SerializeField]
    private GameObject ball2;
    */

    private void Start()
    {
        forward = Vector3.Scale(transform.forward, new Vector3(1, 0, 1));
    }

    public void SetInPos(Vector3 _pos)
    {
        inPos = _pos;
        //ball1.transform.position = _pos;
    }

    public void SetOutPos(Vector3 _pos)
    {
        outPos = _pos;
        //ball2.transform.position = outPos;
        pathDirection = Vector3.Scale((outPos - inPos), new Vector3(1, 0, 1));
        float angle = Vector3.Angle(forward, pathDirection);

        Debug.DrawLine(inPos, outPos, Color.red, 5.0f);


        if (angle <= searchAngle)
        {
            PlayEvent_Positive();
        }
        else if (angle >= (180 - searchAngle))
        {
            PlayEvent_Negative();
        }
        else
        {
            Debug.Log(angle);
            return;
        }
    }

    public virtual void PlayEvent_Positive()
    {
        positiveEvents.Invoke();
        return;
    }

    public virtual void PlayEvent_Negative()
    {
        negativeEvents.Invoke();
    }
}
