using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DangeonPoint : MonoBehaviour
{
    private bool cardSet = false;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameObject cardBaseObj;
    [SerializeField]
    private SpriteRenderer cardSR;

    public EventType eventType;
    [SerializeField]
    private Button thisButton;

    public int cardNum;
    [SerializeField]
    private Talk beforeEventTalk;

    [SerializeField]
    private BattleEvParam battleEvParam;
    [SerializeField]
    private GetCardEvParam getCardEvParam;
    [SerializeField]
    private EndParam endEvParam;


    public enum EventType
    {
        NoEvent,
        Battle,
        Rest,
        GetCard,
        LevelUp,
        Boss,
        Start,
        End
    }

    

    [System.Serializable]
    public class BattleEvParam
    {
        public int exp = 3;
        public string sceneName = "";
        public List<BattleManager.CardAndHP> enemyCharactors;
        public List<CardBase> enemyDeck = new List<CardBase>();
    }

    [System.Serializable]
    public class GetCardEvParam
    {
        public List<CardBase> cardList = new List<CardBase>();
    }

    [System.Serializable]
    public class EndParam
    {
        public string sceneName = "";
        public bool changeScene = false;
        public int evNum = 0;
    }

    public void SetCard()
    {
        if (cardSet)
        {
            return;
        }
        CardBase _card = new CardBase();
        switch (eventType)
        {
            default:
                _card = DangeonManager.Instance.eventCardBases.NoEvent;
                break;

            case EventType.Start:
                _card = DangeonManager.Instance.eventCardBases.Start;
                break;

            case EventType.Battle:
                _card = DangeonManager.Instance.eventCardBases.Battle;
                break;

            case EventType.Rest:
                _card = DangeonManager.Instance.eventCardBases.Rest;
                break;

            case EventType.GetCard:
                _card = DangeonManager.Instance.eventCardBases.GetCard;
                break;

            case EventType.LevelUp:
                _card = DangeonManager.Instance.eventCardBases.LevelUp;
                break;

            case EventType.Boss:
                _card = DangeonManager.Instance.eventCardBases.Boss;
                break;

            case EventType.End:
                _card = DangeonManager.Instance.eventCardBases.End;
                break;
        }

        cardSR.sprite = _card.cardImage;
        cardBaseObj.SetActive(true);

        cardSet = true;
    }

    public void RotateCard()
    {
        animator.SetTrigger("Rotate");
    }

    public void SelectThisCard()
    {
        ButtonManager.Instance.SetLastButton(this.gameObject);
    }

    public void ButtonOn()
    {
        thisButton.enabled = true;
    }

    public void ButtonOff()
    {
        thisButton.enabled = false;
    }

    public void Selected()
    {
        DangeonManager.Instance.SelectCard(cardNum);
    }

    public void PlayEvent()
    {
        //thisEvent.Invoke();
        switch (eventType)
        {
            case EventType.Boss:
            case EventType.Battle:
                ReadyBattle();
                break;

            case EventType.GetCard:
                ReadyGetCard();
                break;
            case EventType.LevelUp:
                beforeEventTalk.StartEvent();
                break;
            case EventType.Rest:
                beforeEventTalk.StartEvent();
                break;
            
            case EventType.End:
                ReadyEnd();
                break;
        }
    }

    private void ReadyBattle()
    {
        GameManager.Instance.SetBattle(battleEvParam.sceneName, battleEvParam.enemyCharactors, battleEvParam.enemyDeck,battleEvParam.exp);
        beforeEventTalk.StartEvent();
    }

    private void ReadyGetCard()
    {
        DangeonManager.Instance.SetGetCardList(getCardEvParam.cardList);
        beforeEventTalk.StartEvent();
    }

    private void ReadyEnd()
    {
        beforeEventTalk.StartEvent();
    }
}
