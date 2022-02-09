using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEvent : EventBase
{
    public string sceneName = "";
    public List<BattleManager.CardAndHP> enemyCharactors;
    public List<CardBase> enemyDeck = new List<CardBase>();
    public List<EventBase> afterBattleEv = new List<EventBase>();
    public int exp = 0;
    public List<BattleTalk> battleTalkList = new List<BattleTalk>();

    public override void StartEvent()
    {
        GameManager.Instance.SetBattle(sceneName, enemyCharactors,enemyDeck,exp);
    }
}
