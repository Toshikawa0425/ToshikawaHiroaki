using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayer_PL : BattlePlayer_Base
{
    [SerializeField]
    private bool choiseTefudaKotei = false;
    [SerializeField]
    private int choiseTefudaNum;

    private void Awake()
    {
        playerName = "キミ";
    }
    public override void SetDeck(bool newDeck = false)
    {
        if (newDeck)
        {
            if (!deckKotei)
            {
                cardDeck = DeckManager.Instance.GetPlayerDeck();

                for (int i = 0; i < BattleManager.Instance.baseDeck.Count; i++)
                {
                    cardDeck.Add(BattleManager.Instance.baseDeck[i]);
                }
            }
        }
        else
        {
            cardDeck = usedCard;
        }

        base.SetDeck();
    }
    public override void SelectBattleChara(int _num)
    {
        if(_num == -1)
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
            BattleManager.Instance.SelectBattleChara_Player(partyCharas[_num]);
            base.SelectBattleChara(_num);
        }
        else
        {
            SelectThisAsTarget(_num);
        }
        
    }

    public override void SelectThisAsTarget(int _num)
    {
        BattleManager.Instance.TargetSelect(BattleManager.AttackerSide.Player, _num);
    }

    public override void SelectTefuda(int _num)
    {
        if(_num == -1)
        {
            _num = battleCharaNum;
        }

        //デモ用
        if (choiseTefudaKotei)
        {
            Debug.Log("手札固定");
            if(_num != choiseTefudaNum)
            {
                return;
            }
        }

            Debug.Log("手札選択");
        CardBase _card = tefuda[_num];
        bool _noMach = false;

        if((_card.itemAndSkillStates.userCard.Count != 0 && !_card.itemAndSkillStates.userCard.Contains(partyCharas[battleCharaNum].charaCard)) || _card.itemAndSkillStates.cost > gemGenerator.gemNum)
        {
            _noMach = true;
            //return;
        }

        

        ButtonManager.Instance.ResetLastButton();
        BattleManager.Instance.SelectTefuda_Player(_card,_noMach);

        base.SelectTefuda(_num);
    }

    public void ChoiseTefudaKotei(int _num)
    {
        choiseTefudaKotei = true;
        choiseTefudaNum = _num;
    }

}
