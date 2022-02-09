using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateSaveEV : EventBase
{
    [SerializeField]
    private int posNum;

    public override void StartEvent()
    {
        GameManager.Instance.SavePlayerDate(posNum);
    }
}
