using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBGMEvent : EventBase
{
    [SerializeField]
    private bool isNullBGM;
    [SerializeField]
    private AudioClip bgm;
    [SerializeField]
    private float outTime;
    [SerializeField]
    private float inTime;
    [SerializeField]
    private float targetVolume;

    public override void StartEvent()
    {
        if (isNullBGM)
        {
            bgm = null;
        }
        AudioPlayer_BGM.Instance.PlayBGM(bgm, outTime, inTime, targetVolume);
    }
}
