using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MakingDeckManager : DangeonAddSceneBase
{
    
    [SerializeField]
    private Animator subAnim;
    [SerializeField]
    private List<Card_Object> party_Obj;
    [SerializeField]
    private List<Card_Object> mainDeck_Obj;
    [SerializeField]
    private List<Card_Object> skillsDeck_Obj;
    [SerializeField]
    private List<Card_Object> itemsDeck_Obj;
    [SerializeField]
    private List<Card_Object> addsDeck_Obj;
    [SerializeField]
    private TextMeshPro earnedExpTMPro;
    [SerializeField]
    private TextMeshPro[] partyLevels;
    [SerializeField]
    private TextMeshPro[] partyEXPs;

    [SerializeField]
    private GameObject[] partyButtonObjs;

    [SerializeField]
    private GameObject[] mainButtonObjs;
    [SerializeField]
    private GameObject[] subButtonObjs;

    private Card_Object nextCard = null;

    int mainNum = 0;
    int skillNum = 0;
    int addNum = 0;
    int itemNum = 0;

    int selectingNum_Main = 0;
    int selectingNum_Sub = 0;

    private List<CardBase> party_Base = new List<CardBase>();

    private List<CardBase> mainDeck_Base= new List<CardBase>();
    private List<CardBase> skills_Base = new List<CardBase>();
    private List<CardBase> items_Base = new List<CardBase>();
    private List<CardBase> adds_Base = new List<CardBase>();


    [SerializeField]
    private Transform ItemAndSkillsParent;

    private bool selectParty = true;
    //bool deckSelecting = true;
    private WhichSelecting whichSelecting;

    [SerializeField]
    private int currentNum = 0;


    private enum WhichSelecting
    {
        Skill,
        Add,
        Item
    }

    private void Start()
    {
        InitState();
    }

    private void FixedUpdate()
    {
        if (InputSetting.Instance.Cancel)
        {
            CloseScene();
        }
    }

    
    public override void InitState()
    {
        base.InitState();
        earnedExpTMPro.text = "‚ ‚Â‚ß‚½EXP: " + GameManager.Instance.earnedEXP;
        SetPartyStates();
        mainDeck_Base = DeckManager.Instance.playerCardDeck;
        skills_Base = DeckManager.Instance.skillDeck_Sub;
        items_Base = DeckManager.Instance.itemDeck_Sub;
        adds_Base = DeckManager.Instance.addDeck_Sub;
        SortDecks();
    }

    private void SetPartyStates()
    {
        for(int i = 0; i< 3; i++)
        {
            BattleManager.CardAndHP _card = GameManager.Instance.playerCharaCards[i];
            int _lv = GameManager.Instance.playerCharaCards[i].levelStates.Level;
            int _exp = GameManager.Instance.playerCharaCards[i].levelStates.EXP;
            party_Obj[i].SetCard(_card.charaCard,_card.nowHP);

            partyLevels[i].text = "LV: " + _lv;
            partyEXPs[i].text = "EXP: " + _exp + "/" + _lv;
        }
    }

    public override void CloseScene()
    {
        DeckManager.Instance.SetDeckAll(mainDeck_Base, skills_Base, items_Base, adds_Base);
        base.CloseScene();
    }


    public void SelectedFromMain(int _num)
    {
        RemoveFromMain(_num);
        ButtonManager.Instance.SetLastButton(mainButtonObjs[_num]);
    }

    public void SelectedFromSub(int _num)
    {
        SetMainFromSub(_num);
        ButtonManager.Instance.SetLastButton(subButtonObjs[_num]);
    }

    private void SetMainFromSub(int _num)
    {
        CardBase _card = new CardBase();

        switch (whichSelecting)
        {
            case WhichSelecting.Skill:
                _card = skills_Base[_num];
                skills_Base.RemoveAt(_num); 
                break;

            case WhichSelecting.Item:
                _card = items_Base[_num];
                items_Base.RemoveAt(_num); 
                break;

            case WhichSelecting.Add:
                _card = adds_Base[_num];
                adds_Base.RemoveAt(_num); 
                break;
        }

        
        mainDeck_Base.Add(_card);

        SortDecks();
    }

    private void RemoveFromMain(int _num)
    {
        CardBase _card = mainDeck_Base[_num];

        switch (_card.cardType)
        {
            case CardBase.CardType.Item:
                items_Base.Add(_card);
                
                break;

            case CardBase.CardType.Skill:
                skills_Base.Add(_card);
                break;

            case CardBase.CardType.Add:
                adds_Base.Add(_card);
                break;
        }


        mainDeck_Base.RemoveAt(_num);
        SortDecks();
    }

    private void SortDecks()
    {
        mainDeck_Base.Sort((a, b) => (int.Parse(a.CardID) - int.Parse(b.CardID)));
        skills_Base.Sort((a, b) => (int.Parse(a.CardID) - int.Parse(b.CardID)));
        items_Base.Sort((a, b) => (int.Parse(a.CardID) - int.Parse(b.CardID)));
        adds_Base.Sort((a, b) => (int.Parse(a.CardID) - int.Parse(b.CardID)));
        SetCards();
    }

    public void SetCards()
    {
        mainNum = mainDeck_Base.Count;
        skillNum = skills_Base.Count;
        addNum = adds_Base.Count;
        itemNum = items_Base.Count;

        for(int i = 0; i < mainDeck_Obj.Count; i++)
        {
            if (i < mainNum)
            {
                mainDeck_Obj[i].SetCard(mainDeck_Base[i]);
            }
            else
            {
                mainDeck_Obj[i].ResetCard();
            }
        }

        for(int j = 0; j < skillsDeck_Obj.Count; j++)
        {
            if(j < skillNum)
            {
                skillsDeck_Obj[j].SetCard(skills_Base[j]);
            }
            else
            {
                skillsDeck_Obj[j].ResetCard();
            }
        }

        for (int j = 0; j < addsDeck_Obj.Count; j++)
        {
            if (j < addNum)
            {
                addsDeck_Obj[j].SetCard(adds_Base[j]);
            }
            else
            {
                addsDeck_Obj[j].ResetCard();
            }
        }

        for (int k = 0; k< itemsDeck_Obj.Count; k++)
        {
            if(k < itemNum)
            {
                itemsDeck_Obj[k].SetCard(items_Base[k]);
            }
            else
            {
                itemsDeck_Obj[k].ResetCard();
            }
        }

        
    }

    public void Selecting_Party(int _num)
    {
        party_Obj[_num].Highlight_On();
        MainAnimSet(false);
    }

    public void Deselecting_Party(int _num)
    {
        party_Obj[_num].Highlight_Off();
    }


    
    public void Selecting_MainDeck(int _num)
    {
        if (mainNum == 0)
        {
            if (selectParty)
            {
                if (skillNum + itemNum + addNum == 0)
                {
                    ButtonManager.Instance.SetLastButton(partyButtonObjs[0]);
                }
                else
                {
                    ButtonManager.Instance.SetLastButton(subButtonObjs[0]);
                }
            }
            else
            {
                ButtonManager.Instance.SetLastButton(partyButtonObjs[0]);
            }

            return;
        }

        if(_num >= mainNum)
        {
            ButtonManager.Instance.SetLastButton(mainButtonObjs[_num - 1]);
            return;
        }

        selectingNum_Main = _num;
        mainDeck_Obj[_num].Highlight_On();
        mainDeck_Obj[_num].gameObject.transform.localPosition = new Vector3(3.0f * _num, 2.0f, ((0.1f * _num) - 0.5f));

        MainAnimSet(true);
    }

    public void Deselecting_MainDeck(int _num)
    {
        mainDeck_Obj[_num].Highlight_Off();
        mainDeck_Obj[_num].gameObject.transform.localPosition = new Vector3(3.0f * _num, 0, 0.1f * _num);
    }

    public void Selecting_Sub(int _num)
    {
        if(skillNum == 0 && itemNum == 0 && addNum == 0)
        {
            ButtonManager.Instance.SetLastButton(mainButtonObjs[selectingNum_Main]);
            return;
        }
        List<Card_Object> _cards = new List<Card_Object>();

        switch (whichSelecting)
        {
            case WhichSelecting.Skill:
                if(skillNum == 0)
                {
                    SetWhichSelect(WhichSelecting.Item, 0);
                }

                if (_num == -1)
                {
                    ButtonManager.Instance.SetLastButton(subButtonObjs[0]);
                    return;
                }

                if (_num >= skillNum)
                {
                    SetWhichSelect(WhichSelecting.Item, 0);
                    return;
                }

                _cards = skillsDeck_Obj;
                break;

            case WhichSelecting.Item:
                if(itemNum == 0)
                {
                    if(addNum != 0)
                    {
                        SetWhichSelect(WhichSelecting.Add, 0);
                        return;
                    }
                    else
                    {
                        SetWhichSelect(WhichSelecting.Skill, skillNum - 1);
                        return;
                    }
                }

                if (_num == -1)
                {
                    SetWhichSelect(WhichSelecting.Skill,skillNum - 1);
                    return;
                }

                if (_num >= itemNum)
                {
                    SetWhichSelect(WhichSelecting.Add, 0);
                    return;
                    
                }

                _cards = itemsDeck_Obj;
                break;

            case WhichSelecting.Add:
                if(addNum == 0)
                {
                    SetWhichSelect(WhichSelecting.Item, itemNum - 1);
                    return;
                }

                if (_num == -1)
                {
                    SetWhichSelect(WhichSelecting.Item, itemNum - 1);
                    return;
                }

                if (_num >= addNum)
                {
                    ButtonManager.Instance.SetLastButton(subButtonObjs[_num - 1]);
                    return;
                    
                }
                _cards = addsDeck_Obj;
                break;
        }

        _cards[_num].Highlight_On();
        _cards[_num].gameObject.transform.localPosition = new Vector3(1.5f * _num, 3.0f, ((0.1f * _num) - 0.5f));
        AnimSet();
        MainAnimSet(true);
    }

    private void SetWhichSelect(WhichSelecting _select , int _num)
    {
        if(_num < 0)
        {
            _num = 0;
        }
        whichSelecting = _select;
        ButtonManager.Instance.SetLastButton(subButtonObjs[_num]);
    }

    public void Deselecting_Sub(int _num)
    {
        List<Card_Object> _cards = new List<Card_Object>();
        switch (whichSelecting)
        {
            case WhichSelecting.Skill:
                _cards = skillsDeck_Obj;
                break;

            case WhichSelecting.Item:
                _cards = itemsDeck_Obj;
                break;

            case WhichSelecting.Add:
                _cards = addsDeck_Obj;
                break;
        }

        _cards[_num].Highlight_Off();
        _cards[_num].gameObject.transform.localPosition = new Vector3(1.5f * _num, 0, 0.1f * _num);
    }

    
    /*
    private void ResetMoveCard(List<Card_Object> _cards)
    {
        for(int i = 0; i< _cards.Count; i++)
        {
            _cards[i].gameObject.transform.localPosition = new Vector3(1.5f * i, 0, (0.1f * i));
        }
    }

    private void SelectMove(List<Card_Object> _cards, int _target)
    {
        float _d = 4.5f;
        
        _cards[_target].gameObject.transform.localPosition = new Vector3(1.5f * _target, 1.5f, ((0.1f * _target) - 0.5f));
        float _oriPos = 1.5f * _target;
        for (int i = 0; i < _cards.Count; i++)
        {
            if (i != _target)
            {
                int _dis = i - _target;
                float _s = 0;

                for(int j = 1; j < Mathf.Abs (_dis); j++)
                {
                    _s += 1.0f / j;
                }

                float _x = _d + _s;

                if(_dis < 0)
                {
                    _x *= -1;
                }

                _cards[i].gameObject.transform.localPosition = new Vector3((_oriPos + _x), 0, (0.1f * i));

            }
            else
            {
                continue;
            }
        }
    }
    */

    private void AnimSet()
    {
        subAnim.SetBool("Skill", whichSelecting == WhichSelecting.Skill ? true : false);
        subAnim.SetBool("Item", whichSelecting == WhichSelecting.Item ? true : false);
        subAnim.SetBool("Add", whichSelecting == WhichSelecting.Add ? true : false);
    }

    private void MainAnimSet(bool _trigger)
    {
        selectParty = !_trigger;
        mainAnim.SetBool("Deck", _trigger);
    }

    public override void SelectFirstCard()
    {
        ButtonManager.Instance.SetLastButton(partyButtonObjs[0]);
    }
}
