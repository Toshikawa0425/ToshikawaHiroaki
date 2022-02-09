using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkRange : MonoBehaviour
{
    [SerializeField]
    private Talk targetTalk;

    private void Awake()
    {
        targetTalk = null;
    }

    private void Update()
    {
        if (targetTalk != null && PlController_Field.Instance.canMove)
        {
            if (InputSetting.Instance.Accept)
            {
                Debug.Log("inputAccept");
                targetTalk.StartEvent();
                targetTalk = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Talk>())
            if (PlController_Field.Instance.canMove)
        {
            Debug.Log("intalk");
            targetTalk = other.GetComponent<Talk>();
            targetTalk.Fukidashi_On();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<Talk>())
        if(targetTalk == other.GetComponent<Talk>())
        {
            targetTalk.Fukidashi_Off();
            targetTalk = null;
        }
    }
}
