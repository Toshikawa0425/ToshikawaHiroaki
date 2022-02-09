using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerCharasEvent : EventBase
{
    [SerializeField]
    private List<PlayerCharactor> plCharas;
    [SerializeField]
    private List<int> HPs;
    [SerializeField]
    private List<int> Levels;

    public override void StartEvent()
    {
        GameManager.Instance.SetPlayerCharas(plCharas, HPs,Levels);
    }
}
