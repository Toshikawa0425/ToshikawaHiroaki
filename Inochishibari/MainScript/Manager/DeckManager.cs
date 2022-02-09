using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : SingletonMonoBehaviour<DeckManager>
{
    public List<CardBase> playerCardDeck = new List<CardBase>();
    [SerializeField]
    private List<CardBase> enemyCardDeck = new List<CardBase>();

    public List<CardBase> addDeck_Sub = new List<CardBase>();
    public List<CardBase> itemDeck_Sub = new List<CardBase>();
    public List<CardBase> skillDeck_Sub = new List<CardBase>();

    /*
    public List<CardBase> addDeck_Main = new List<CardBase>();
    public List<CardBase> itemDeck_Main = new List<CardBase>();
    public List<CardBase> skillDeck_Main = new List<CardBase>();
    */


    public List<CardBase> GetPlayerDeck()
    {
        return playerCardDeck;
    }

    public void SetDeckAll(List<CardBase> _main, List<CardBase> _skill, List<CardBase> _item, List<CardBase> _add)
    {
        SetPlayerDeck(_main);
        SetSkill(_skill);
        SetItem(_item);
        SetAdd(_add);
    }

    public void SetPlayerDeck(List<CardBase> _deck)
    {
        playerCardDeck = _deck;
    }

    public void SetSkill(List<CardBase> _deck)
    {
        skillDeck_Sub = _deck;
    }

    public void SetItem(List<CardBase> _deck)
    {
        itemDeck_Sub = _deck;
    }

    public void SetAdd(List<CardBase> _deck)
    {
        addDeck_Sub = _deck;
    }
    /*
    public void SetPlayerDeck()
    {
        playerCardDeck = new List<CardBase>();

        if (skillDeck_Main.Count > 0)
        {
            foreach (CardBase _skill in skillDeck_Main)
            {
                playerCardDeck.Add(_skill);
            }
        }

        if (addDeck_Main.Count > 0)
        {
            foreach (CardBase _add in addDeck_Main)
            {
                playerCardDeck.Add(_add);
            }
        }


        if (itemDeck_Main.Count > 0)
        {
            foreach (CardBase _item in itemDeck_Main)
            {
                playerCardDeck.Add(_item);
            }
        }
    }
    */

    public List<CardBase> GetEnemyDeck()
    {
        return enemyCardDeck;
    }

    public void SetEnemyDeck(List<CardBase> _deck)
    {
        enemyCardDeck = _deck;
    }
    


    public void GetCard(CardBase _card)
    {
        Debug.Log("ÉJÅ[Éhì¸éË");
        switch (_card.cardType)
        {
            case CardBase.CardType.Skill:
                skillDeck_Sub.Add(_card);
                break;
            case CardBase.CardType.Item:
                itemDeck_Sub.Add(_card);
                break;

            case CardBase.CardType.Add:
                addDeck_Sub.Add(_card);
                break;
        }
    }
}
