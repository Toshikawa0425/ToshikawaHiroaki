using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineController : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector playableDirector;

    public void PlayTimeLine()
    {
        Debug.Log("StartTimeLine");
        playableDirector.Play();
    }
    public void Pause()
    {
        playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0);
    }

    public void Restart()
    {
        playableDirector.time = playableDirector.time;
        playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(1);
    }

    public void StartTalk(Talk _talk)
    {
        _talk.StartEvent();
    }

    public void NextTalk()
    {
        TalkManager.Instance.NextTalk();
    }

    public void SetDepthOfField(float _distance)
    {
        MainCameraSetting.Instance.SetDepthOfField(_distance);
    }

    public void PlayerOff()
    {
        PlayerObj.Instance.PlayerOff();
    }

    public void PlayerOn()
    {
        PlayerObj.Instance.PlayerOn();
    }

    public void OpenGameon(float _time)
    {
        DisplayManager.Instance.GamenOpen(_time);
    }

    public void CloseGamen(float _time)
    {
        DisplayManager.Instance.GamenClose(_time);
    }
}
