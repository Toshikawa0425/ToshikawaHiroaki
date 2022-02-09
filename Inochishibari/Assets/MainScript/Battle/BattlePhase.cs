using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePhase : SingletonMonoBehaviour<BattlePhase>
{
    /*
    private BattleManager battleManager;
    private ReadyPhase readyPhase;

    [SerializeField]
    private GameObject battleButton;
    [SerializeField]
    private GameObject battleEndButton;


    public List<GameObject> plCharaObjs;
    public List<GameObject> enCharaObjs;

    [SerializeField]
    private List<BattleCharaController> plControllers = new List<BattleCharaController>();
    [SerializeField]
    private List<BattleCharaController> enControllers = new List<BattleCharaController>();

    private BattleCharaController attackerController;
    private BattleCharaController damagerController;

    public List<Vector3> plPoses = new List<Vector3>();
    public List<Vector3> enPoses = new List<Vector3>();

    [SerializeField]
    private Image plCardImage;
    [SerializeField]
    private Image enCardImage;

    [SerializeField]
    private Transform plFightPos;
    [SerializeField]
    private Transform enFightPos;

    [SerializeField]
    private List<Animator> plCharaAnims;
    [SerializeField]
    private List<Animator> enCharaAnims;

    [SerializeField]
    private List<GameObject> hearts_Pl;
    [SerializeField]
    private List<GameObject> hearts_En;


    [SerializeField]
    private List<BattleManager.StatesAndHP> plStates = new List<BattleManager.StatesAndHP>();
    [SerializeField]
    private List<BattleManager.StatesAndHP> enStates = new List<BattleManager.StatesAndHP>();

    private List<float> plHPs = new List<float>();
    private List<float> enHPs = new List<float>();

    [SerializeField]
    private int selectedChara_Pl = 0;
    [SerializeField]
    private int selectedChara_En = 0;

    [SerializeField]
    private BattleCard selectedCard_Pl;

    [SerializeField]
    private BattleCard selectedCard_En;


    private BattleCard.CardNum loseCards;
    private BattleCard.CardNum returnCards;

    private new void Awake()
    {
        battleManager = GetComponent<BattleManager>();
        readyPhase = GetComponent<ReadyPhase>();
        Random.InitState(System.DateTime.Now.Millisecond);
    }

    public void ResetParam()
    {
        selectedChara_En = -1;
        returnCards = new BattleCard.CardNum();
        if (loseCards != null)
        {
            returnCards = loseCards;
        }
        loseCards = new BattleCard.CardNum();
    }

    public BattleCard.CardNum ReturnCards(BattleCard.CardNum _cardNum)
    {
        _cardNum.Da = _cardNum.Da - loseCards.Da + returnCards.Da;
        _cardNum.Ten = _cardNum.Ten - loseCards.Ten + returnCards.Ten;
        _cardNum.Tou = _cardNum.Tou - loseCards.Tou + returnCards.Tou;

        return _cardNum;
    }

    public void SetCharas(List<BattleManager.StatesAndHP> _plChara,List<BattleManager.StatesAndHP> _enChara)
    {
        plStates = _plChara;
        enStates = _enChara;

        for(int i = 0; i < 3; i++)
        {
            if(i < plStates.Count)
            {
                plCharaObjs[i].SetActive(true);
                plPoses.Add(plCharaObjs[i].transform.position);
                plCharaAnims[i].runtimeAnimatorController = _plChara[i].states.battleAnim;
                readyPhase.SetActiveButton(i, true);
            }
            else
            {
                plCharaObjs[i].SetActive(false);
                readyPhase.SetActiveButton(i, false);
            }


            if(i < enStates.Count)
            {
                enCharaObjs[i].SetActive(true);
                enPoses.Add(enCharaObjs[i].transform.position);
                enCharaAnims[i].runtimeAnimatorController = _enChara[i].states.battleAnim;
            }
            else
            {
                enCharaObjs[i].SetActive(false);
            }
        }

        StatesUpdate_Pl();
        StatesUpdate_En();
    }

    public void PlayerSelect_Card(BattleCard _card)
    {
        selectedCard_Pl = _card;
        plCardImage.gameObject.SetActive(true);
        plCardImage.sprite = _card.cardImage;

        battleButton.SetActive(true);
        ButtonManager.Instance.SetLastButton(battleButton);
    }

    public void PlayerSelect_Chara(int _charaNum)
    {
        if(plStates[_charaNum].nowHP <= 0)
        {
            return;
        }
        else
        {
            ButtonManager.Instance.ResetLastButton();
            selectedChara_Pl = _charaNum;
            plCharaObjs[selectedChara_Pl].transform.position = plFightPos.position;

            

            //hearts_Pl.SetActive(true);

            StatesUpdate_Pl();

            readyPhase.SelectPhase_BattleChara();
        }
    }

    public void EnemySelect()
    {
        selectedChara_En += 1;
        selectedChara_En %= enStates.Count;

        if(enStates[selectedChara_En].nowHP <= 0)
        {
            EnemySelect();
            return;
        }


        enCharaObjs[selectedChara_En].transform.position = enFightPos.position;
        StatesUpdate_En();

        BattleCard.CardNum _cardNum = enStates[selectedChara_En].states.battleStates.cardNum;

        float _max = _cardNum.Da + _cardNum.Ten + _cardNum.Tou;


        float p_Da = 0 +( 100 *  (float)(_cardNum.Da / _max));
        float p_Ten = p_Da + (100 * (float)(_cardNum.Ten / _max)) ;
        float p_Tou = p_Ten + (100 * (float)(_cardNum.Tou / _max));

        int _randomValue = Random.Range(0, 100);

        if (_randomValue < p_Da)
        {
            selectedCard_En = readyPhase.daCard;
        }
        else if(_randomValue < p_Ten)
        {
            selectedCard_En = readyPhase.tenCard;
        }
        else
        {
            selectedCard_En = readyPhase.touCard;
        }

        Debug.Log(p_Da + ":" + p_Ten + ":" + p_Tou + "::" + _randomValue);
    }

    public void StatesUpdate_Pl()
    {
        for (int n = 0; n < plStates.Count; n++)
        {
            for (int i = 0; i < 10; i++)
            {
                Transform _heart = hearts_Pl[n].transform.GetChild(i);
                if (i >= plStates[n].states.battleStates.maxHP)
                {
                    _heart.gameObject.SetActive(false);
                }
                else
                {
                    _heart.gameObject.SetActive(true);
                    if (i >= plStates[n].nowHP)
                    {
                        _heart.GetChild(0).gameObject.SetActive(false);
                    }
                    else
                    {
                        _heart.GetChild(0).gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    public void StatesUpdate_En()
    {
        for (int n = 0; n < enStates.Count; n++)
        {
            for (int i = 0; i < 10; i++)
            {
                Transform _heart = hearts_En[n].transform.GetChild(i);
                if (i >= enStates[n].states.battleStates.maxHP)
                {
                    _heart.gameObject.SetActive(false);
                }
                else
                {
                    _heart.gameObject.SetActive(true);
                    if (i >= enStates[n].nowHP)
                    {
                        _heart.GetChild(0).gameObject.SetActive(false);
                    }
                    else
                    {
                        _heart.GetChild(0).gameObject.SetActive(true);
                    }
                }
            }
        }
    }


    public void BattleStart()
    {
        ButtonManager.Instance.ResetLastButton();
        battleButton.SetActive(false);

        enCardImage.sprite = selectedCard_En.cardImage;
        enCardImage.gameObject.SetActive(true);

        BattleJudge();
    }

    private void BattleJudge()
    {
        if(selectedCard_Pl.cardType == selectedCard_En.cardType)
        {
            DamageCalcurate_Draw(selectedCard_Pl, selectedCard_En);
            Debug.Log("あいこ！");
        }
        else if(selectedCard_Pl.cardType == BattleCard.CardType.Syu)
        {
            DamageCalcurate_Mamori(plStates[selectedChara_Pl], selectedCard_En);
            attackerController = enControllers[selectedChara_En];
            damagerController = plControllers[selectedChara_Pl];

            Attack();
        }
        else
        {
            switch (selectedCard_Pl.cardType)
            {
                case BattleCard.CardType.Da:
                    if(selectedCard_En.cardType == BattleCard.CardType.Ten)
                    {
                        DamageCalcurate_Win(enStates[selectedChara_En], plStates[selectedChara_Pl], selectedCard_En);
                        SetLoseCards();

                        attackerController = enControllers[selectedChara_En];
                        damagerController = plControllers[selectedChara_Pl];
                        Debug.Log("敵の勝ち！");
                    }
                    else
                    {
                        DamageCalcurate_Win(plStates[selectedChara_Pl], enStates[selectedChara_En], selectedCard_Pl);

                        attackerController = plControllers[selectedChara_Pl];
                        damagerController = enControllers[selectedChara_En];
                        Debug.Log("プレイヤーの勝ち！");
                    }
                    break;

                case BattleCard.CardType.Ten:
                    if(selectedCard_En.cardType == BattleCard.CardType.Tou)
                    {
                        DamageCalcurate_Win(enStates[selectedChara_En], plStates[selectedChara_Pl], selectedCard_En);
                        SetLoseCards();

                        attackerController = enControllers[selectedChara_En];
                        damagerController = plControllers[selectedChara_Pl];
                        Debug.Log("敵の勝ち！");
                    }
                    else
                    {
                        DamageCalcurate_Win(plStates[selectedChara_Pl], enStates[selectedChara_En], selectedCard_Pl);

                        attackerController = plControllers[selectedChara_Pl];
                        damagerController = enControllers[selectedChara_En];
                        Debug.Log("プレイヤーの勝ち！");
                    }
                    break;

                case BattleCard.CardType.Tou:
                    if (selectedCard_En.cardType == BattleCard.CardType.Da)
                    {
                        DamageCalcurate_Win(enStates[selectedChara_En], plStates[selectedChara_Pl], selectedCard_En);
                        SetLoseCards();

                        attackerController = enControllers[selectedChara_En];
                        damagerController = plControllers[selectedChara_Pl];
                        Debug.Log("敵の勝ち！");
                    }
                    else
                    {
                        DamageCalcurate_Win(plStates[selectedChara_Pl], enStates[selectedChara_En], selectedCard_Pl);

                        attackerController = plControllers[selectedChara_Pl];
                        damagerController = enControllers[selectedChara_En];
                        Debug.Log("プレイヤーの勝ち！");
                    }
                    break;
            }

            Attack();
        }
    }

    private void SetLoseCards()
    {
        switch (selectedCard_Pl.cardType)
        {
            case BattleCard.CardType.Da:
                loseCards.Da++;
                break;

            case BattleCard.CardType.Ten:
                loseCards.Ten++;
                break;

            case BattleCard.CardType.Tou:
                loseCards.Tou++;
                break;
        }
    }

    private void DamageCalcurate_Win(BattleManager.StatesAndHP _attacker, BattleManager.StatesAndHP _victim, BattleCard _card)
    {

        CharaStates.Zokusei _atkZokusei = _attacker.states.battleStates.zokusei;
        CharaStates.Zokusei _vctZokusei = _victim.states.battleStates.zokusei;

        _victim.nowHP -= _card.damage * DamageMagnification(_atkZokusei, _vctZokusei);
    }

    private void DamageCalcurate_Draw(BattleCard _plCard, BattleCard _enCard)
    {
        BattleManager.StatesAndHP _pl = plStates[selectedChara_Pl];
        BattleManager.StatesAndHP _en = enStates[selectedChara_En];

        CharaStates.Zokusei _plZokusei = _pl.states.battleStates.zokusei;
        CharaStates.Zokusei _enZokusei = _en.states.battleStates.zokusei;

        int _damageToPl = _enCard.damage * (DamageMagnification(_enZokusei,_plZokusei) - 1);
        int _damageToEn = _plCard.damage * (DamageMagnification(_plZokusei,_enZokusei) - 1);

        _pl.nowHP -= _damageToPl;
        _en.nowHP -= _damageToEn;

        if(_damageToPl != _damageToEn)
        {
            if(_damageToPl > 0)
            {
                attackerController = enControllers[selectedChara_En];
                damagerController = plControllers[selectedChara_Pl];
            }
            else
            {
                attackerController = plControllers[selectedChara_Pl];
                damagerController = enControllers[selectedChara_En];
            }

            Attack();
        }
        else
        {
            AttackEnd();
        }
    }

    private void DamageCalcurate_Mamori(BattleManager.StatesAndHP _victim, BattleCard _atkCard)
    {
        _victim.nowHP -= _atkCard.damage;
    }

    private int DamageMagnification(CharaStates.Zokusei _atk, CharaStates.Zokusei _vct)
    {
        int _damage = 1;

        if(_atk != _vct)
        {
            switch (_atk)
            {
                case CharaStates.Zokusei.Fire:
                    if (_vct == CharaStates.Zokusei.Nature || _vct == CharaStates.Zokusei.Ice || _vct == CharaStates.Zokusei.Thunder)
                    {
                        _damage = 2;
                    }
                    break;

                case CharaStates.Zokusei.Water:
                    if (_vct == CharaStates.Zokusei.Fire || _vct == CharaStates.Zokusei.Ground || _vct == CharaStates.Zokusei.Wind)
                    {
                        _damage = 2;
                    }
                    break;

                case CharaStates.Zokusei.Nature:
                    if (_vct == CharaStates.Zokusei.Water || _vct == CharaStates.Zokusei.Ground || _vct == CharaStates.Zokusei.Wind)
                    {
                        _damage = 2;
                    }
                    break;

                case CharaStates.Zokusei.Thunder:
                    if (_vct == CharaStates.Zokusei.Water || _vct == CharaStates.Zokusei.Nature || _vct == CharaStates.Zokusei.Ground)
                    {
                        _damage = 2;
                    }
                    break;

                case CharaStates.Zokusei.Ground:
                    if (_vct == CharaStates.Zokusei.Fire || _vct == CharaStates.Zokusei.Wind || _vct == CharaStates.Zokusei.Ice)
                    {
                        _damage = 2;
                    }
                    break;

                case CharaStates.Zokusei.Wind:
                    if (_vct == CharaStates.Zokusei.Fire || _vct == CharaStates.Zokusei.Thunder || _vct == CharaStates.Zokusei.Ice)
                    {
                        _damage = 2;
                    }
                    break;

                case CharaStates.Zokusei.Ice:
                    if (_vct == CharaStates.Zokusei.Water || _vct == CharaStates.Zokusei.Nature || _vct == CharaStates.Zokusei.Thunder)
                    {
                        _damage = 2;
                    }
                    break;
            }
        }

        return _damage;
        
    }

    private void Attack()
    {
        attackerController.Attack();
    }

    public void AttackHit()
    {
        StatesUpdate_Pl();
        StatesUpdate_En();

        damagerController.Damage();
    }

    public void AttackEnd()
    {
        battleEndButton.SetActive(true);
        ButtonManager.Instance.ResetLastButton();
        ButtonManager.Instance.SetLastButton(battleEndButton);
    }


    public void BattleEnd()
    {
        //hearts_Pl.SetActive(false);
        //hearts_En.SetActive(false);

        if (StatesCheck())
        {

            plCardImage.sprite = null;
            enCardImage.sprite = null;

            plCardImage.gameObject.SetActive(false);
            enCardImage.gameObject.SetActive(false);

            plCharaObjs[selectedChara_Pl].transform.position = plPoses[selectedChara_Pl];
            enCharaObjs[selectedChara_En].transform.position = enPoses[selectedChara_En];

            battleEndButton.SetActive(false);
            ButtonManager.Instance.ResetLastButton();

            //battleManager.PhaseEnd_BattlePhase();
        }

    }

    private bool StatesCheck()
    {
        int _plNum = 0;
        for(int i = 0; i < plStates.Count; i++)
        {
            if(plStates[i].nowHP > 0)
            {
                continue;
            }
            else
            {
                _plNum++;
            }
        }

        if(_plNum == plStates.Count)
        {
            battleManager.BattleEnd_Lose();
            return false;
        }

        int _enNum = 0;

        for (int i = 0; i < enStates.Count; i++)
        {
            if (enStates[i].nowHP > 0)
            {
                continue;
            }
            else
            {
                _enNum++;
            }
        }

        if (_enNum == enStates.Count)
        {
            List<int> _hp = new List<int>();
            for(int i = 0; i< plStates.Count; i++)
            {
                if(plStates[i].nowHP <= 0)
                {
                    plStates[i].nowHP = 1;
                }

                _hp.Add(plStates[i].nowHP);
            }
            battleManager.BattleEnd_Win(_hp);
            return false;
        }

        return true;
    }
    */
}
