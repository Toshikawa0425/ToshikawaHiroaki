using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScaleEvent : EventBase
{
    [SerializeField]
    private List<ChangeTargetAndValue> changeTargetAndValue = new List<ChangeTargetAndValue>();

    public enum Target
    {
        PlayerObj,
        Select
    }

    [System.Serializable]
    public class ChangeTargetAndValue
    {
        public Target target;
        public Transform targetTransform;
        public Vector3 scale;
    }

    public override void StartEvent()
    {
        foreach(ChangeTargetAndValue _i in changeTargetAndValue)
        {
            if(_i.target == Target.PlayerObj)
            {
                PlController_Field.Instance.SetScale(_i.scale);
            }
            else
            {
                _i.targetTransform.localScale = _i.scale;
            }
        }
    }
}
