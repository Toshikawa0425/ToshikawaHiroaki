using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayer_EN : BattlePlayer_Base
{
    [SerializeField]
    private bool choiseCharaKotei = false;
    [SerializeField]
    private int choiseCharaNum;
    [SerializeField]
    private Strongs strongs;

    [SerializeField]
    private int chainKakuritsu = 30;

    [SerializeField]
    private Talk isChainTalk;
    [SerializeField]
    private Talk isNotChainTalk;



    private bool canChain = false;
    public enum Strongs
    {
        Normal,
        Win,
        Lose,
        Draw
    }

    private void Awake()
    {
        playerName = "‘ŠŽè";
    }
    public override void SelectBattleChara(int _num)
    {
        if (_num == -1)
        {
            _num = battleCharaNum;
        }
        ButtonManager.Instance.ResetLastButton();

        if (partyCharas[_num].isDead)
        {
            BattleManager.Instance.RepetePhase();
            return;
        }

        

        if (BattleManager.Instance.nowPhase == BattleManager.Phase.SelectAttacker || BattleManager.Instance.nowPhase == BattleManager.Phase.SelectBlocker)
        {
            BattleManager.Instance.SelectBattleChara_Enemy(partyCharas[_num]);
            base.SelectBattleChara(_num);
        }
        else
        {
            SelectThisAsTarget(_num);
        }
    }

    public override void SelectThisAsTarget(int _num)
    {
        BattleManager.Instance.TargetSelect(BattleManager.AttackerSide.Enemy, _num);
    }

    public override void SetDeck(bool newDeck = false)
    {
        if (newDeck)
        {
            cardDeck = DeckManager.Instance.GetEnemyDeck();

            for (int i = 0; i < BattleManager.Instance.baseDeck_Enemy.Count; i++)
            {
                cardDeck.Add(BattleManager.Instance.baseDeck_Enemy[i]);
            }
        }
        else
        {
            cardDeck = usedCard;
        }

        base.SetDeck();
    }

    public override void SelectTefuda(int _num)
    {
        bool _noMach = false;
        if (_num == -1)
        {
            _num = battleCharaNum;
        }
        CardBase _card = tefuda[_num];

        switch (strongs)
        {
            default:
                break;
            case Strongs.Lose:
                switch (BattleManager.Instance.wazaCard_PL.wazaType)
                {
                    case CardBase.WazaType.Da:
                        _card = BattleManager.Instance.baseCards.Tou;
                        break;
                    case CardBase.WazaType.Ten:
                        _card = BattleManager.Instance.baseCards.Da;
                        break;
                    case CardBase.WazaType.Tou:
                        _card = BattleManager.Instance.baseCards.Ten;
                        break;
                }
                break;
            case Strongs.Draw:
                switch (BattleManager.Instance.wazaCard_PL.wazaType)
                {
                    case CardBase.WazaType.Da:
                        _card = BattleManager.Instance.baseCards.Da;
                        break;
                    case CardBase.WazaType.Ten:
                        _card = BattleManager.Instance.baseCards.Ten;
                        break;
                    case CardBase.WazaType.Tou:
                        _card = BattleManager.Instance.baseCards.Tou;
                        break;
                }
                break;
        }

        if ((_card.itemAndSkillStates.userCard.Count != 0 && !_card.itemAndSkillStates.userCard.Contains(partyCharas[battleCharaNum].charaCard)) || _card.itemAndSkillStates.cost > gemGenerator.gemNum)
        {
            _noMach = true;
        }


        BattleManager.Instance.SelectTefuda_Enemy(_card,_noMach);
        base.SelectTefuda(_num);
    }

    public void ChoiseFromTefuda()
    {
        List<int> canUseTefudaNum = new List<int>();
        int _selectNum = 0;


        for (int i = 0; i< tefudaNum; i++)
        {
            CardBase _card = tefuda[i];

            if ((_card.itemAndSkillStates.userCard.Count != 0 && !_card.itemAndSkillStates.userCard.Contains(partyCharas[battleCharaNum].charaCard)) || _card.itemAndSkillStates.cost > gemGenerator.gemNum)
            {
                continue;
            }
            else
            {
                canUseTefudaNum.Add(i);
            }
        }

        if(canUseTefudaNum.Count == 0)
        {
            canChain = false;
            _selectNum = Random.Range(0, tefudaNum);
        }
        else
        {
            canChain = true;
            int _n = Random.Range(0, canUseTefudaNum.Count);

            _selectNum = canUseTefudaNum[_n];
        }

        SelectTefuda(_selectNum);
    }

    public void ChoiseFromParty_EN()
    {
        List<int> canSelectNum = new List<int>();
        int _selectNum = 0;


        for (int i = 0; i < partyCharas.Count; i++)
        {
            bool _isDead = partyCharas[i].isDead;

            if (_isDead)
            {
                continue;
            }
            else
            {
                canSelectNum.Add(i);
            }
        }

        int _num = Random.Range(0, canSelectNum.Count);

        _selectNum = canSelectNum[_num];
        SelectBattleChara(_selectNum);
    }

    public void ChoiseFromParty_PL()
    {
        List<BattleManager.CardAndHP> _plParty = BattleManager.Instance.bp_PL.partyCharas;
        List<int> canSelectNum = new List<int>();
        int _selectNum = 0;


        for (int i = 0; i < _plParty.Count; i++)
        {
            bool _isDead = _plParty[i].isDead;

            if (_isDead)
            {
                continue;
            }
            else
            {
                canSelectNum.Add(i);
            }
        }

        int _num = Random.Range(0, canSelectNum.Count);

        _selectNum = canSelectNum[_num];

        if (choiseCharaKotei)
        {
            _selectNum = choiseCharaNum;
        }

        BattleManager.Instance.bp_PL.SelectBattleChara(_selectNum);
    }

    public void SetStrong(int _strongs)
    {
        switch (_strongs)
        {
            default:
                strongs = Strongs.Normal;
                break;
            case 1:
                strongs = Strongs.Win;
                break;
            case 2:
                strongs = Strongs.Lose;
                break;
            case 3:
                strongs = Strongs.Draw;
                break;
        }
    }

    public void ChainCheck()
    {
        if (canChain)
        {
            int _c = Random.Range(0, 100);

            if(_c < chainKakuritsu)
            {
                isChainTalk.StartEvent();
            }
            else
            {
                isNotChainTalk.StartEvent();
            }
        }
        else
        {
            isNotChainTalk.StartEvent();
        }
    }
}
