using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardChoiceManager : MonoBehaviour
{
    [SerializeField]
    private SceneType sceneType;
    [SerializeField]
    private List<Card_Object> cardList_Obj = new List<Card_Object>();
    [SerializeField]
    private List<CardBase> cardList_Base = new List<CardBase>();
    [SerializeField]
    private List<BattleManager.CardAndHP> partyCharas = new List<BattleManager.CardAndHP>();

    public enum SceneType
    {
        GetCard,
        Rest
    }

    private void Start()
    {
        switch (sceneType)
        {
            case SceneType.GetCard:
                cardList_Base = DangeonManager.Instance.GetCardList();

                for (int i = 0; i < 3; i++)
                {
                    cardList_Obj[i].SetCard(cardList_Base[i]);
                }
                break;

            case SceneType.Rest:
                partyCharas = GameManager.Instance.GetPlayerParty();

                for(int i = 0; i < partyCharas.Count; i++)
                {
                    cardList_Obj[i].SetCard(partyCharas[i].charaCard, partyCharas[i].nowHP);
                }
                break;
        }
        

        cardList_Obj[0].SelectThisCard();
    }

    public void SelectCard(int _num)
    {
        switch (sceneType)
        {
            case SceneType.GetCard:
                DeckManager.Instance.GetCard(cardList_Base[_num]);
                break;

            case SceneType.Rest:
                partyCharas[_num].nowHP = partyCharas[_num].maxHP;
                GameManager.Instance.SetStates_Player(partyCharas);
                break;
        }
        
        UnLoadScene();
    }

    private void CloseScene()
    {
        //DeckManager.Instance.SetDeckAll(mainDeck_Base, skills_Base, items_Base, adds_Base);
        //deckAnim.SetTrigger("Close");
    }

    public void UnLoadScene()
    {
        SceneEventManager.Instance.UnloadScene();
        DangeonManager.Instance.OpenNextCards();
    }
}
