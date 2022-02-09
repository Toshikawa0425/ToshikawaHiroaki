using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTalk : EventBase
{
    public List<BattleTalkManager.BattleTalkInfo> talkList;
    public BattleTalkManager.TalkCondition condition;

    public override void StartEvent()
    {
        BattleTalkManager.Instance.SetTalkList(this, talkList);
    }
}
