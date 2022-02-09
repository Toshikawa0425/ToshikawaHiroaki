using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySE : EventBase
{
    [SerializeField]
    private AudioClip se;
    [SerializeField]
    private float volume;

    public override void StartEvent()
    {
        AudioPlayer_SE.Instance.PlaySE(se, volume);
    }
}
