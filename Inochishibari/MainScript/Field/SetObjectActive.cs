using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObjectActive : InOutEvent
{
    [SerializeField]
    private ObjInfo[] positiveEv;
    [SerializeField]
    private ObjInfo[] negativeEv;

    [System.Serializable]
    private class ObjInfo
    {
        public GameObject targetObj;
        public bool states;
    }

    public override void PlayEvent_Positive()
    {
        if(positiveEv.Length > 0)
        foreach(ObjInfo inf in positiveEv)
        {
            inf.targetObj.SetActive(inf.states);
        }
    }

    public override void PlayEvent_Negative()
    {
        if(negativeEv.Length > 0)
        foreach(ObjInfo inf in negativeEv)
        {
            inf.targetObj.SetActive(inf.states);
        }
    }
}
