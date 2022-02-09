using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using TMPro;

public class BattlePlayer_Base : MonoBehaviour
{
    public string playerName = "";
    [SerializeField]
    private protected bool deckKotei = false;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private protected List<CardBase> defaultDeck = new List<CardBase>();
    [SerializeField]
    private protected List<CardBase> cardDeck;
    [SerializeField]
    private protected List<CardBase> usedCard;

    [SerializeField]
    private TextMeshPro damageTMP_battle;
    [SerializeField]
    private List<TextMeshPro> damageTMP = new List<TextMeshPro>();

    public GemGenerator gemGenerator;


    [SerializeField]
    private Transform battleCardParent;
    [SerializeField]
    private List<Transform> partyCardParents = new List<Transform>();


    public List<BattleManager.CardAndHP> partyCharas = new List<BattleManager.CardAndHP>();

    [SerializeField]
    private protected List<Card_Object> partyCharaCardObjs = new List<Card_Object>();

    public int tefudaNum = 0;
    [SerializeField]
    private protected int deckNum;

    private protected int maxDeckNum;

    [SerializeField]
    private protected int battleCharaNum;


    [SerializeField]
    private protected List<CardBase> tefuda = new List<CardBase>();

    [SerializeField]
    private protected List<Card_Object> tefudaCardObjs = new List<Card_Object>();
    [SerializeField]
    private protected Transform deckParentTransform;

    private List<string> logList = new List<string>();


    private int deadNum = 0;
    private int targetNum = 0;

    private bool drawing = false;


    //①攻撃キャラと防御キャラを選択（攻撃側）
    //②手札から使用するカードを選択（両者）
    //③オープン＆効果の処理
    //④攻守交代
    //⑤攻撃側は、”縛る”かどうかを選択（縛る場合、②へ。）
    //⑥ドロー（攻撃側）そして①へ。

    public void SetCharas(List<BattleManager.CardAndHP> cardAndHPs)
    {
        partyCharas = cardAndHPs;
        for(int i = 0; i < 3; i++)
        {
            if (i < cardAndHPs.Count)
            {
                partyCharaCardObjs[i].gameObject.SetActive(true);
                partyCharaCardObjs[i].SetCard(partyCharas[i].charaCard, partyCharas[i].nowHP);
            }
            else
            {
                partyCardParents[i].gameObject.SetActive(false);
            }
        }
    }

    public void ShuffleDeck()
    {
        cardDeck = cardDeck.OrderBy(a => Guid.NewGuid()).ToList();
    }

    public IEnumerator DrawCardCoroutine(int _num, bool _turnDraw)
    {
        if (_turnDraw && (deckNum == 0))
        {
            SetDeck(false);
            yield return null;

            ShuffleDeck();
        }

        while (_num > 0)
        {
            if (tefudaNum < tefudaCardObjs.Count && deckNum > 0)
            {
                drawing = true;
                animator.SetTrigger("Draw");
                BattleManager.Instance.PlaySE_CardPlay();
                deckParentTransform.GetChild(deckNum - 1).gameObject.SetActive(false);


                CardBase _card = cardDeck[deckNum - 1];

                yield return new WaitUntil(() => drawing == false);

                tefuda.Add(_card);

                tefudaCardObjs[tefudaNum].SetCard(_card);

                cardDeck.RemoveAt(deckNum - 1);

                deckNum--;
                tefudaNum++;
            }
            else
            {
                break;
            }

            _num--;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void Drawed()
    {
        drawing = false;
    }

    public virtual void SetDeck(bool newDeck = false)
    {
        

        deckNum = cardDeck.Count;
        maxDeckNum = cardDeck.Count;

        for (int i = 0; i < maxDeckNum; i++)
        {
            deckParentTransform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void ReadyBattle(List<BattleManager.CardAndHP> _party)
    {
        battleCardParent.gameObject.SetActive(false);
        SetCharas(_party);
        SetDeck(true);
        if (!deckKotei)
        {
            ShuffleDeck();
        }
        //３枚ドロー
        DrawCard(3);
    }

    

    public void DrawCard(int _num, bool _turnDraw = false)
    {
        SortingTefuda();
        BattleLog.Instance.WriteLog(playerName + "は" + _num + "枚ドロー");
        StartCoroutine(DrawCardCoroutine(_num, _turnDraw));
    }

    public void CheckEnable_Tefuda()
    {
        for (int i = 0; i < tefudaNum; i++)
        {
            CardBase _card = tefuda[i];

            if ((_card.itemAndSkillStates.userCard.Count != 0 && !_card.itemAndSkillStates.userCard.Contains(partyCharas[battleCharaNum].charaCard)) || _card.itemAndSkillStates.cost > gemGenerator.gemNum)
            {
                tefudaCardObjs[i].SetEnable(false);
            }
            else
            {
                tefudaCardObjs[i].SetEnable(true);
            }
        }
    }

    public virtual void SelectBattleChara(int _num) 
    {
        battleCardParent.gameObject.SetActive(true);
        battleCharaNum = _num;
        MoveCard(partyCharaCardObjs[_num].gameObject, battleCardParent);
        BattleManager.Instance.PlaySE_CardPlay();
        partyCardParents[_num].gameObject.SetActive(false);
    }

    public virtual void SelectThisAsTarget(int _num) { }

    public virtual void SelectTefuda(int _num)
    {
        tefudaCardObjs[_num].ResetCard();
        RemoveTefuda(_num);
    }

    public void RemoveTefuda(int _tefudaNum)
    {
        BattleManager.Instance.PlaySE_CardPlay();
        if (tefuda[_tefudaNum].cardType != CardBase.CardType.Item)
        {
            usedCard.Add(tefuda[_tefudaNum]);
        }
        tefudaNum--;
        tefuda.RemoveAt(_tefudaNum);
    }

    public void SortingTefuda()
    {
        for(int i = 0; i < 5; i++)
        {
            if(i < tefudaNum)
            {
                tefudaCardObjs[i].SetCard(tefuda[i]);

                
            }
            else
            {
                
                tefudaCardObjs[i].ResetCard();
            }

            tefudaCardObjs[i].SetEnable(true);
        }
    }

    public void SelectingCard(int _num)
    {
        if(_num == -1)
        {
            _num = battleCharaNum;
        }

        partyCharaCardObjs[_num].Highlight_On();
    }

    public void DeselectingCard(int _num)
    {
        if(_num == -1)
        {
            _num = battleCharaNum;
        }

        partyCharaCardObjs[_num].Highlight_Off();
    }

    public void ResetBattleChara()
    {
        //partyCharaCardObjs[battleCharaNum].SetCard(partyCharas[battleCharaNum].charaCard,partyCharas[battleCharaNum].nowHP);
        partyCardParents[battleCharaNum].gameObject.SetActive(true);
        MoveCard(partyCharaCardObjs[battleCharaNum].gameObject, partyCardParents[battleCharaNum]);
        battleCardParent.gameObject.SetActive(false);

        BattleManager.Instance.PlaySE_CardPlay();
    }

    public void SelectChain(bool _trigger)
    {
        //縛る場合、ドローはしない。
    }

    public void GemPlus(int _num)
    {
        gemGenerator.GenerateGems(_num);
    }

    public void GemUse(int _num)
    {
        gemGenerator.UseGems(_num);
    }

    public void MoveCard(GameObject _card, Transform _parent)
    {
        _card.transform.parent = _parent;
        _card.transform.localPosition = Vector3.zero;
    }

    public void StatesSet(int _damage, CardBase _wazaCard, int _targetNum = 0)
    {
        logList = new List<string>();

        int _totalDamage = 0;
        if (_targetNum == -1)
        {
            _targetNum = battleCharaNum;
        }
        switch (_wazaCard.itemAndSkillStates.target)
        {
            case CardBase.ItemAndSkillStates.Target.Target:

                DamageCal(partyCharas[battleCharaNum], battleCharaNum);

                if(_wazaCard.itemAndSkillStates.effectType == CardBase.ItemAndSkillStates.EffectType.Heal)
                {
                    logList.Add(partyCharas[battleCharaNum].charaCard.cardName + "を" + _totalDamage + "回復");
                }
                else
                {
                    logList.Add(partyCharas[battleCharaNum].charaCard.cardName + "に" + _totalDamage + "ダメージ");
                }
                
                targetNum = battleCharaNum;
                break;

            case CardBase.ItemAndSkillStates.Target.Enemy_All:
            case CardBase.ItemAndSkillStates.Target.Player_All:
                for (int i = 0; i < partyCharas.Count; i++)
                {
                    BattleManager.CardAndHP _partyChara = partyCharas[i];
                    DamageCal(_partyChara, i);

                    if (_wazaCard.itemAndSkillStates.effectType == CardBase.ItemAndSkillStates.EffectType.Heal)
                    {
                        logList.Add(partyCharas[i].charaCard.cardName + "を" + _totalDamage + "回復");
                    }
                    else
                    {
                        logList.Add(partyCharas[i].charaCard.cardName + "に" + _totalDamage + "ダメージ");
                    }
                }
                targetNum = -1;
                break;

            case CardBase.ItemAndSkillStates.Target.Enemy_Anyone:
            case CardBase.ItemAndSkillStates.Target.Player_Anyone:
            case CardBase.ItemAndSkillStates.Target.User:
                DamageCal(partyCharas[_targetNum], _targetNum);
                if (_wazaCard.itemAndSkillStates.effectType == CardBase.ItemAndSkillStates.EffectType.Heal)
                {
                    logList.Add(partyCharas[_targetNum].charaCard.cardName + "を" + _totalDamage + "回復");
                }
                else
                {
                    logList.Add(partyCharas[_targetNum].charaCard.cardName + "に" + _totalDamage + "ダメージ");
                }
                targetNum = _targetNum;
                break;


            default:
                break;
        }

        void DamageCal(BattleManager.CardAndHP _chara, int _num)
        {
            switch (_wazaCard.itemAndSkillStates.effectType)
            {
                default:
                    _chara.zyoutai = BattleManager.Zyoutai.None;
                    break;
                case CardBase.ItemAndSkillStates.EffectType.Doku:
                    _chara.zyoutai = BattleManager.Zyoutai.Doku;
                    logList.Add(_chara.charaCard.cardName + "は" + "毒を受けた");
                    _chara.zyoutaiTurn = 3;
                    break;
            }

            if (_wazaCard.itemAndSkillStates.effectType == CardBase.ItemAndSkillStates.EffectType.Heal)
            {
                _totalDamage = _damage;
            }
            else
            {
                _totalDamage = (_damage + ZokuseiDamage(_wazaCard.cardZokusei, _chara.charaCard.cardZokusei)) * -1;
            }


            if (_num == battleCharaNum)
            {
                damageTMP_battle.text = _totalDamage.ToString();
            }
            else
            {
                damageTMP[_num].text = _totalDamage.ToString();
            }

            _chara.nowHP += _totalDamage;

            _totalDamage = Mathf.Abs(_totalDamage);

            if (_chara.nowHP < 0)
            {
                _chara.nowHP = 0;
            }
            else if (_chara.nowHP > _chara.maxHP)
            {
                _chara.nowHP = _chara.maxHP;
            }
        }
    }
    

    

    private int ZokuseiDamage(CardBase.Zokusei wazaZokusei, CardBase.Zokusei charaZokusei)
    {
        int _damage = 0;

        switch (wazaZokusei)
        {
            default:
                break;

            case CardBase.Zokusei.Fire:
                switch (charaZokusei)
                {
                    default:
                        break;
                    case CardBase.Zokusei.Ice:
                        _damage = 1;
                        break;

                    case CardBase.Zokusei.Nature:
                        _damage = 1;
                        break;

                    case CardBase.Zokusei.Water:
                        _damage = -1;
                        break;

                    
                }
                break;

            case CardBase.Zokusei.Water:
                switch (charaZokusei)
                {
                    default:
                        break;

                    case CardBase.Zokusei.Fire:
                        _damage = 1;
                        break;

                    case CardBase.Zokusei.Ground:
                        _damage = -1;
                        break;
                }
                break;

            case CardBase.Zokusei.Nature:
                switch (charaZokusei)
                {
                    default:
                        break;

                    case CardBase.Zokusei.Ground:
                        _damage = 1;
                        break;

                    

                    case CardBase.Zokusei.Fire:
                        _damage = -1;
                        break;
                }
                break;

            case CardBase.Zokusei.Ground:
                switch (charaZokusei)
                {
                    default:
                        break;

                    case CardBase.Zokusei.Wind:
                        _damage = 1;
                        break;

                    case CardBase.Zokusei.Ice:
                        _damage = -1;
                        break;
                }
                break;

            case CardBase.Zokusei.Wind:
                switch (charaZokusei)
                {
                    default:
                        break;

                    case CardBase.Zokusei.Thunder:
                        _damage = 1;
                        break;

                    case CardBase.Zokusei.Nature:
                        _damage = -1;
                        break;
                }
                break;

            case CardBase.Zokusei.Thunder:
                switch (charaZokusei)
                {
                    default:
                        break;

                    case CardBase.Zokusei.Water:
                        _damage = 1;
                        break;

                    case CardBase.Zokusei.Ice:
                        _damage = -1;
                        break;
                }
                break;

            case CardBase.Zokusei.Ice:
                switch (charaZokusei)
                {
                    default:
                        break;

                    case CardBase.Zokusei.Nature:
                        _damage = 1;
                        break;

                    case CardBase.Zokusei.Water:
                        _damage = 1;
                        break;

                    case CardBase.Zokusei.Wind:
                        _damage = -1;
                        break;
                }
                break;
        }

        return _damage;
    }

    public void ZyoutaiDamage()
    {
        for (int i = 0; i < partyCharas.Count; i++)
        {
            if (!partyCharas[i].isDead)
            {
                if (partyCharas[i].zyoutai == BattleManager.Zyoutai.Doku)
                {
                    if (i == battleCharaNum)
                    {
                        damageTMP_battle.text = "-1";
                    }
                    else
                    {
                        damageTMP[i].text = "-1";
                    }

                    partyCharas[i].nowHP--;
                    if (partyCharas[i].nowHP < 0)
                    {
                        partyCharas[i].nowHP = 0;
                    }

                    partyCharas[i].zyoutaiTurn--;

                    logList.Add(partyCharas[i].charaCard.cardName + "に毒ダメージ");

                    if (partyCharas[i].zyoutaiTurn == 0)
                    {
                        partyCharas[i].zyoutai = BattleManager.Zyoutai.None;
                    }
                }
            }
        }

        Damage();
    }

    public void Damage()
    {
        for (int i = 0; i< partyCharas.Count; i++)
        {
            if (!partyCharas[i].isDead)
            {
                if (partyCharas[i].nowHP == 0)
                {
                    
                    partyCharas[i].isDead = true;
                    deadNum++;
                }
                partyCharaCardObjs[i].StatesUpDate(partyCharas[i]);
            }
        }

        foreach(string _l in logList)
        {
            BattleLog.Instance.WriteLog(_l);
        }

        logList = new List<string>();
    }

    public void ZyoutaiReset()
    {
        

        foreach(BattleManager.CardAndHP _chara in partyCharas)
        {
            if(_chara.nowHP <= 0)
            {
                _chara.nowHP = 1;
                _chara.isDead = false;
            }
            _chara.zyoutai = BattleManager.Zyoutai.None;
            _chara.zyoutaiTurn = 0;
        }
    }

    public List<CardBase> GetMainDeck()
    {
        for(int i = cardDeck.Count - 1; i >= 0; i--)
        {
            if(cardDeck[i].cardType == CardBase.CardType.Base)
            {
                cardDeck.RemoveAt(i);
            }
        }

        foreach(CardBase _tefuda in tefuda)
        {
            if(_tefuda.cardType != CardBase.CardType.Base)
            {
                cardDeck.Add(_tefuda);
            }
        }

        foreach(CardBase _used in usedCard)
        {
            if(_used.cardType != CardBase.CardType.Base)
            {
                cardDeck.Add(_used);
            }
        }

        return cardDeck;
    }

    public void PlayDamageAnim()
    {
        if(targetNum == -1)
        {
            animator.SetTrigger("AllDamage");
            return;
        }
        else if(targetNum == battleCharaNum)
        {
            animator.SetTrigger("NormalDamage");
            return;
        }

        string _animTrigger = "DamageTo" + targetNum.ToString();

        animator.SetTrigger(_animTrigger);
    }

    public void PlayHealAnim()
    {
        if (targetNum == -1)
        {
            animator.SetTrigger("AllHeal");
            return;
        }
        else if (targetNum == battleCharaNum)
        {
            animator.SetTrigger("NormalHeal");
            return;
        }

        string _animTrigger = "HealTo" + targetNum.ToString();

        animator.SetTrigger(_animTrigger);
    }

    public void PlaySE_Damage()
    {
        AudioPlayer_SE.Instance.PlaySE(BattleManager.Instance.battleSE.damage, 0.2f);
    }

    public void PlaySE_ALL()
    {
        AudioPlayer_SE.Instance.PlaySE(BattleManager.Instance.battleSE.KO, 0.2f);
    }

    public bool DeadCheck()
    {
        if(deadNum == partyCharas.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool BattleCharaDeadCheck()
    {
        if (partyCharas[battleCharaNum].isDead)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void NextPhaseReady()
    {
        BattleManager.Instance.ActiveNextButton();
    }

    public void FirstSelectPartyChara()
    {
        if (battleCardParent.gameObject.activeInHierarchy)
        {
            ButtonManager.Instance.SetLastButton(battleCardParent.gameObject);
        }
        else
        {
            ButtonManager.Instance.SetLastButton(partyCardParents[0].gameObject);
        }
    }
}
