using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineEvent : EventBase
{
    [SerializeField]
    private PlayableDirector director;

    public override void StartEvent()
    {
        director.Play();
    }
}
