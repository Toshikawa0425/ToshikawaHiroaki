using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talk : EventBase
{
    [SerializeField]
    private int startNum = 0;
    [SerializeField]
    private GameObject fukidashi;
    public List<TalkManager.TalkInfo> talkList;


    public override void StartEvent()
    {
        Debug.Log("startTalk");
        Fukidashi_Off();
        TalkManager.Instance.SetTalkList(this,talkList,startNum);
    }

    public void Fukidashi_On()
    {
        if(fukidashi != null)
        fukidashi.SetActive(true);
    }

    public void Fukidashi_Off()
    {
        if(fukidashi != null)
        fukidashi.SetActive(false);
    }

    public void SetStartNum(int _num)
    {
        startNum = _num;
    }

    public void OpenGamen(float _time)
    {
        DisplayManager.Instance.GamenOpen(_time);
    }

    public void CloseGamen(float _time)
    {
        DisplayManager.Instance.GamenClose(_time);
    }

    public void DisPlayImage(Sprite _sprite)
    {
        DisplayManager.Instance.DisplayImage(_sprite);
    }

    public void NotDisPlayImage()
    {
        DisplayManager.Instance.DisplayImage(null);
    }

    public void PlCanMove()
    {
        PlController_Field.Instance.CanMoveOn();
    }
}
