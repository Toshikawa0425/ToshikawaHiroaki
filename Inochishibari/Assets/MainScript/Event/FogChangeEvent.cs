using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogChangeEvent : InOutEvent
{
    [SerializeField]
    private Color positiveFogColor;
    [SerializeField]
    private float positiveChangeTime;

    [SerializeField]
    private Color negativeFogColor;
    [SerializeField]
    private float negativeChangeTime;

    public override void PlayEvent_Positive()
    {
        FogManager.Instance.ChangeFogColor(positiveFogColor, positiveChangeTime);
    }

    public override void PlayEvent_Negative()
    {
        FogManager.Instance.ChangeFogColor(negativeFogColor, negativeChangeTime);
    }
}
