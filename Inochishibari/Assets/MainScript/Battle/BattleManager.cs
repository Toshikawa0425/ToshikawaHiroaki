using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleManager : SingletonMonoBehaviour<BattleManager>
{
    [SerializeField]
    private BattleIntervalEvent intervalEvent;
    [SerializeField]
    private Talk winTalk;
    [SerializeField]
    private Talk loseTalk;
    [SerializeField]
    private GameObject nextButton;
    [SerializeField]
    private GameObject explainObj;
    [SerializeField]
    private GameObject explainObj_Cam;

    public CardAndHP battleChara_PL;
    public CardAndHP battleChara_EN;




    [SerializeField]
    private Animator sceneAnim;

    [SerializeField]
    private Animator battleCardAnim_PL;
    [SerializeField]
    private Animator battleCardAnim_EN;

    public BattlePlayer_PL bp_PL;
    public BattlePlayer_EN bp_EN;

    private int damageByEnemy;
    private int damageByPlayer;

    private GameObject nowCam;
    [SerializeField]
    private BattleCameras cameras;
    [SerializeField]
    private CameraFocusPoses focusPoses;

    public List<CardBase> baseDeck;

    public List<CardBase> baseDeck_Enemy;

    public Phase nowPhase = Phase.Start;
    public int turnNum = 1;
    public int battleNum = 5;

    [SerializeField]
    private Card_Object firstTefuda_PL;
    [SerializeField]
    private Card_Object firstTefuda_EN;


    public List<CardAndHP> plCardAndHP;
    public List<CardAndHP> enCardAndHP;

    [SerializeField]
    private Card_Object battleCard_PL;
    [SerializeField]
    private Card_Object battleCard_EN;

    [SerializeField]
    private Card_Object wazaCardObj_PL;
    [SerializeField]
    private Card_Object wazaCardObj_EN;

    public CardBase wazaCard_PL;
    public CardBase wazaCard_EN;

    private bool cardNoMach_PL = false;
    private bool cardNoMach_EN = false;



    public List<BattleTalk> talkEventList = new List<BattleTalk>();
    [SerializeField]
    private Talk chainCheck;

    public bool isEvent = false;

    public bool canClick = false;

    public BaseCards baseCards;

    public AttackerSide attackerSide;

    private AttackerSide targetSelectSide;

    private int camNum = 1;
    private bool canMoveCam = false;

    private Judge battleJudge;
    private bool isChain = false;
    [SerializeField]
    private int chainDamage = 0;

    public BattleSE battleSE;
    public CardSE cardSE;
    public float seVol = 0.4f;
    [SerializeField]
    private AudioClip gemSE;

    [System.Serializable]
    public class BattleSE
    {
        public AudioClip damage;
        public AudioClip KO;
    }

    [System.Serializable]
    public class CardSE
    {
        public AudioClip open;
        public AudioClip play;
    }

    [System.Serializable]
    public class CardAndHP
    {
        public CardBase charaCard;
        public int maxHP;
        public int nowHP;
        public bool isDead = false;
        public Zyoutai zyoutai = Zyoutai.None;
        public int zyoutaiTurn = 0;
        public LevelStates levelStates;

        [System.Serializable]
        public class LevelStates
        {
            public int Level = 1;
            public int EXP = 0;
            public int totalEXP = 0;
        }
    }

    public enum Zyoutai
    {
        None,
        Doku
    }


    [System.Serializable]
    public class BaseCards
    {
        public CardBase Da;
        public CardBase Ten;
        public CardBase Tou;
        public CardBase Syu;
    }

    [System.Serializable]
    public class BattleCameras
    {
        public GameObject mainCam;
        public GameObject enemyCharaCam;
        public GameObject tefudaCamera;
        public GameObject battleCam;
    }

    [System.Serializable]
    public class CameraFocusPoses
    {
        public Transform mainCam;
        public Transform enemyCam;
        public Transform tefudaCam;
    }

    public enum AttackerSide
    {
        Player,
        Enemy
    }


    public enum Phase
    {
        Start,

        Draw,
        PlusGem,

        SelectAttacker,
        SelectBlocker,


        SelectTefuda_PL,
        SelectTefuda_EN,

        
        
        
        Check,
        Open,
        Judge,

        TargetSelect_PL,
        Action_PL,
        TargetSelect_EN,
        Action_EN,

        DeadCheck,

        ChainCheck,
        ResetCards,

        Finish
    }

    public enum Judge
    {
        Win,
        Lose,
        Draw,

        BothLose
    }



    private void Update()
    {
        if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.N))
        {
            BattleEnd_Win();
        }
        if (canMoveCam)
        {
            float _ver = InputSetting.Instance.Vertical_Once;

            if (_ver > 0)
            {
                if (camNum > 0)
                {
                    camNum--;
                    CamChange(camNum);
                }
            }
            else if (_ver < 0)
            {

                if (camNum < 2)
                {
                    camNum++;
                    CamChange(camNum);
                }
            }
        }
    }

    public void InitState()
    {
        turnNum = 1;
        CamChange(1);
        attackerSide = AttackerSide.Player;

        bp_PL.ReadyBattle(plCardAndHP);
        bp_EN.ReadyBattle(enCardAndHP);

        //ActiveNextButton();
        Invoke("NextPhase", 2.0f);
        //NextPhase();
    }



    public void SetPlayerAndEnemyCharas(List<CardAndHP> _plCharas, List<CardAndHP> _enCharas)
    {
        plCardAndHP = _plCharas;
        enCardAndHP = _enCharas;



        InitState();
    }

    public void CanCamMove(bool _trigger)
    {
        canMoveCam = _trigger;
        explainObj_Cam.SetActive(_trigger);
    }

    public void CamChange(int _num)
    {

        if(nowCam != null)
        {
            nowCam.SetActive(false);
        }

        camNum = _num;

        switch (camNum)
        {
            case 0:
                cameras.enemyCharaCam.SetActive(true);
                nowCam = cameras.enemyCharaCam;
                break;

            case 1:
                cameras.mainCam.SetActive(true);
                nowCam = cameras.mainCam;
                break;

            case 2:
                cameras.tefudaCamera.SetActive(true);
                nowCam = cameras.tefudaCamera;
                break;

            case 3:
                cameras.battleCam.SetActive(true);
                nowCam = cameras.battleCam;
                break;
        }
    }

    public void ActiveNextButton()
    {
        explainObj.SetActive(false);
        nextButton.SetActive(true);
        ButtonManager.Instance.SetLastButton(nextButton);
    }

    private void InactiveNextButton()
    {
        ButtonManager.Instance.ResetLastButton();
        nextButton.SetActive(false);
    }

    public void Phase_PlusGem()
    {
        BattleLog.Instance.ResetLog();
        CanCamMove(true);
        //CamChange(cameras.mainCam);
        BattleCameraController.Instance.SetDepthOfField(focusPoses.mainCam);

        StartCoroutine(PlayGemSE());

        bp_PL.GemPlus(1);
        bp_EN.GemPlus(1);

        ActiveNextButton();
        //NextPhase();
    }

    public IEnumerator PlayGemSE()
    {
        yield return new WaitForSeconds(1.0f);
        AudioPlayer_SE.Instance.PlaySE(gemSE, 0.2f);
    }

    public void Phase_DrawCard()
    {
        bp_PL.DrawCard(1,true);
        bp_EN.DrawCard(1,true);

        ActiveNextButton();
        //NextPhase();
    }

    public void Phase_AttackerSelect()
    {
        
        switch (attackerSide)
        {
            case AttackerSide.Player:
                //ButtonManager.Instance.SetLastButton(firstChara_PL);

                bp_PL.FirstSelectPartyChara();

                explainObj.SetActive(true);

                break;

            case AttackerSide.Enemy:
                bp_EN.ChoiseFromParty_EN();
                break;
        }
        
    }

    public void Phase_BlockerSelect()
    {
        switch (attackerSide)
        {
            case AttackerSide.Player:
                BattleCameraController.Instance.SetDepthOfField(focusPoses.enemyCam);

                bp_EN.FirstSelectPartyChara();

                explainObj.SetActive(true);
                break;

            case AttackerSide.Enemy:
                bp_EN.ChoiseFromParty_PL();
                break;
        }
        
    }

    public void SetFirstTefuda(Card_Object _card)
    {
        firstTefuda_PL = _card;
    }

    public void Phase_SelectTefuda_Player()
    {
        //CamChange(cameras.tefudaCamera);
        BattleCameraController.Instance.SetDepthOfField(focusPoses.tefudaCam);
        bp_PL.CheckEnable_Tefuda();

        //ButtonManager.Instance.SetLastButton(firstTefuda_PL);
        Debug.Log("ŽèŽD‘I‘ð");
        firstTefuda_PL.SelectThisCard();
    }

    public void Phase_SelectTefuda_Enemy()
    {
        //CamChange(cameras.mainCam);
        BattleCameraController.Instance.SetDepthOfField(focusPoses.mainCam);

        bp_EN.ChoiseFromTefuda();
    }

    public void Phase_Check()
    {
        //CamChange(cameras.battleCam);
        canMoveCam = false;
        CamChange(3);

        BattleCameraController.Instance.SetDepthOfField(focusPoses.mainCam);
        ActiveNextButton();
    }

    public void Phase_Open()
    {
        PlaySE_CardOpen();
        BattleJudge();
        wazaCardObj_EN.RotateCard();

        //ButtonManager.Instance.SetLastButton(wazaCardObj_PL.gameObject);

        wazaCardObj_PL.SelectThisCard();
    }

    public void PlaySE_CardPlay()
    {
        AudioPlayer_SE.Instance.PlaySE(cardSE.play, seVol);
    }

    public void PlaySE_CardOpen()
    {
        AudioPlayer_SE.Instance.PlaySE(cardSE.open, seVol);
    }

    public void Phase_Action_PL()
    {
        if(battleJudge == Judge.Lose)
        {
            NextPhase();
            return;
        }

        canMoveCam = false;
        CamChange(1);

        BattleAction(true);
        //ActiveNextButton();
    }

    public void Phase_Action_EN()
    {
        if (battleJudge == Judge.Win)
        {
            NextPhase();
            return;
        }

        canMoveCam = false;
        CamChange(1);

        BattleAction(false);
    }

    public void Phase_TargetSelect(AttackerSide _attackerSide)
    {
        targetSelectSide = _attackerSide;
        if (_attackerSide == AttackerSide.Player)
        {
            if (battleJudge != Judge.Lose)
            {
                switch (wazaCard_PL.itemAndSkillStates.target)
                {
                    case CardBase.ItemAndSkillStates.Target.Player_Anyone:
                        CamChange(1);
                        canMoveCam = false;
                        bp_PL.FirstSelectPartyChara();
                        break;

                    case CardBase.ItemAndSkillStates.Target.Enemy_Anyone:
                        CamChange(1);
                        canMoveCam = false;
                        bp_EN.FirstSelectPartyChara();
                        break;

                    case CardBase.ItemAndSkillStates.Target.User:
                        bp_PL.StatesSet(damageByPlayer, wazaCard_PL,-1);
                        NextPhase();
                        break;

                    case CardBase.ItemAndSkillStates.Target.BP_Both:
                        NextPhase();
                        break;

                    default:
                        bp_EN.StatesSet(damageByPlayer, wazaCard_PL);
                        NextPhase();
                        break;
                }
            }
            else
            {
                NextPhase();
            }
        }
        else
        {
            if (battleJudge != Judge.Win)
            {
                switch (wazaCard_EN.itemAndSkillStates.target)
                {
                    case CardBase.ItemAndSkillStates.Target.Player_Anyone:
                        bp_EN.ChoiseFromParty_EN();
                        break;

                    case CardBase.ItemAndSkillStates.Target.Enemy_Anyone:
                        bp_EN.ChoiseFromParty_PL();
                        break;

                    case CardBase.ItemAndSkillStates.Target.User:
                        bp_EN.StatesSet(damageByEnemy, wazaCard_EN, -1);
                        NextPhase();
                        break;

                    case CardBase.ItemAndSkillStates.Target.BP_Both:
                        NextPhase();
                        break;

                    default:
                        bp_PL.StatesSet(damageByEnemy, wazaCard_EN);
                        NextPhase();
                        break;
                }
            }
            else
            {
                NextPhase();
            }
        }
    }

    public void Phase_ChainCheck()
    {
        if(bp_PL.BattleCharaDeadCheck() || bp_EN.BattleCharaDeadCheck())
        {
            isChain = false;
            chainDamage = 0;
            NextPhase();
            return;
        }

        if(bp_PL.tefudaNum < 1 || bp_EN.tefudaNum < 1)
        {
            isChain = false;
            chainDamage = 0;
            NextPhase();
            return;
        }

        CamChange(1);

        if (attackerSide == AttackerSide.Player)
        {
            chainCheck.StartEvent();
        }
        else
        {
            bp_EN.ChainCheck();
        }
    }


    public void Phase_ResetCards()
    {

        wazaCardObj_PL.ResetCard();
        wazaCardObj_EN.ResetCard();

        wazaCardObj_EN.RotateCard();
        wazaCardObj_PL.RotateCard();

        if (!isChain)
        {
            bp_PL.ResetBattleChara();
            bp_EN.ResetBattleChara();
        }
        bp_PL.SortingTefuda();
        bp_EN.SortingTefuda();

        ActiveNextButton();

        //CamChange(cameras.mainCam);
        CamChange(1);
        canMoveCam = true;

        turnNum++;
    }

    public void BattleEnd_Win()
    {
        ButtonManager.Instance.ResetLastButton();
        canMoveCam = false;
        CamChange(1);
        canMoveCam = false;
        bp_PL.ZyoutaiReset();
        DeckManager.Instance.SetPlayerDeck(bp_PL.GetMainDeck());
        Debug.Log("Ÿ—˜II");

        winTalk.StartEvent();
    }

    public void BattleEnd_Lose()
    {
        CamChange(1);
        canMoveCam = false;
        loseTalk.StartEvent();
    }

    public void LoadNextScene()
    {
        GameManager.Instance.FinishBattleScene_WIN(bp_PL.partyCharas);
    }

    public void LoadLoseScene()
    {
        GameManager.Instance.FinishBattleScene_LOSE();
    }



    public void SelectBattleChara_Player(CardAndHP _chara)
    {
        //battleCard_PL.SetCard(_chara.charaCard,_chara.nowHP);
        battleChara_PL = _chara;
        ActiveNextButton();
    }


    public void SelectBattleChara_Enemy(CardAndHP _chara)
    {
        //battleCard_EN.SetCard(_chara.charaCard, _chara.nowHP);
        battleChara_EN = _chara;
        ActiveNextButton();
    }

    public void SelectTefuda_Player(CardBase _card, bool _noMach)
    {
        wazaCardObj_PL.SetCard(_card);
        wazaCardObj_PL.SetEnable(!_noMach);
        wazaCard_PL = _card;
        cardNoMach_PL = _noMach;
        NextPhase();
    }

    public void SelectTefuda_Enemy(CardBase _card, bool _noMach)
    {
        wazaCardObj_EN.SetCard(_card);
        wazaCardObj_EN.SetEnable(!_noMach);
        wazaCard_EN = _card;
        cardNoMach_EN = _noMach;
        NextPhase();
    }

    public void OpenCard_PL()
    {
        AudioPlayer_SE.Instance.PlaySE(cardSE.open, 0.2f);
        Debug.Log("Open");
        wazaCardObj_PL.RotateCard();
        switch (battleJudge)
        {
            case Judge.Win:
                BattleLog.Instance.WriteLog("ƒLƒ~‚ÌŸ‚¿");
                break;

            case Judge.Lose:
                BattleLog.Instance.WriteLog("‘ŠŽè‚ÌŸ‚¿");
                break;

            case Judge.Draw:
                BattleLog.Instance.WriteLog("‚ ‚¢‚±");
                break;

            case Judge.BothLose:
                BattleLog.Instance.WriteLog("Ÿ•‰‚È‚µ");
                break;
        }

        ActiveNextButton();
    }

    public void BattleJudge()
    {

        if (cardNoMach_PL != true && cardNoMach_EN != true)
        {
            CardBase.WazaType _plType = wazaCard_PL.wazaType;
            CardBase.WazaType _enType = wazaCard_EN.wazaType;

            if(_plType == CardBase.WazaType.Syu || _enType == CardBase.WazaType.Syu)
            {
                battleJudge = Judge.Draw;
            }
            else if (_plType == _enType)
            {
                
                battleJudge = Judge.Draw;
            }
            else
            {
                switch (_plType)
                {
                    case CardBase.WazaType.Da:
                        switch (_enType)
                        {
                            case CardBase.WazaType.Ten:
                                

                                battleJudge = Judge.Lose;


                                break;

                            case CardBase.WazaType.Tou:
                                

                                battleJudge = Judge.Win;



                                break;
                        }
                        break;

                    case CardBase.WazaType.Ten:
                        switch (_enType)
                        {
                            case CardBase.WazaType.Tou:

                                battleJudge = Judge.Lose;
                                break;

                            case CardBase.WazaType.Da:

                                battleJudge = Judge.Win;

                                break;
                        }
                        break;

                    case CardBase.WazaType.Tou:
                        switch (_enType)
                        {
                            case CardBase.WazaType.Da:

                                battleJudge = Judge.Lose;

                                break;

                            case CardBase.WazaType.Ten:

                                battleJudge = Judge.Win;

                                break;
                        }
                        break;
                }
            }
        }
        else
        {
            if(cardNoMach_PL == true && cardNoMach_EN == true)
            {
                battleJudge = Judge.BothLose;
            }
            else
            {
                if(cardNoMach_PL == true)
                {
                    battleJudge = Judge.Lose;
                }
                else
                {
                    battleJudge = Judge.Win;
                }
            }
        }

        if (battleJudge != Judge.BothLose)
        {
            DamageCalFromJudge();
        }
    }


    private void DamageCalFromJudge()
    {
        CardBase.WazaType _plType = wazaCard_PL.wazaType;
        CardBase.WazaType _enType = wazaCard_EN.wazaType;

        damageByEnemy = wazaCard_EN.itemAndSkillStates.damagePoint;
        damageByPlayer = wazaCard_PL.itemAndSkillStates.damagePoint;


        switch (battleJudge)
        {
            case Judge.Draw:
                damageByEnemy = 0;
                damageByPlayer = 0;

                if (_plType == battleChara_PL.charaCard.wazaType)
                {
                        damageByPlayer++;
                    
                }
                if (_enType == battleChara_EN.charaCard.wazaType)
                {
                        damageByEnemy++;
                }
                break;

            case Judge.Win:
                damageByEnemy = 0;
                

                if (_plType == battleChara_PL.charaCard.wazaType)
                {
                    damageByPlayer++;
                }
                break;

            case Judge.Lose:
                damageByPlayer = 0;

                if (_enType == battleChara_EN.charaCard.wazaType)
                {
                    damageByEnemy++;
                }
                break;

        }

        if(wazaCard_PL.cardType == CardBase.CardType.Item)
        {
            damageByPlayer = wazaCard_PL.itemAndSkillStates.damagePoint;
        }

        if(wazaCard_EN.cardType == CardBase.CardType.Item)
        {
            damageByEnemy = wazaCard_EN.itemAndSkillStates.damagePoint;
        }

        damageByEnemy += chainDamage;
        damageByPlayer += chainDamage;
    }

    public void TargetSelect(AttackerSide _targetSide, int _charaNum)
    {
        if(_targetSide == AttackerSide.Enemy)
        {
            if (targetSelectSide == AttackerSide.Enemy)
            {
                bp_EN.StatesSet(damageByEnemy, wazaCard_EN, _charaNum);
            }
            else
            {
                bp_EN.StatesSet(damageByPlayer, wazaCard_PL, _charaNum);
            }
        }
        else
        {
            if(targetSelectSide == AttackerSide.Player)
            {
                bp_PL.StatesSet(damageByPlayer, wazaCard_PL, _charaNum);
            }
            else
            {
                bp_PL.StatesSet(damageByEnemy, wazaCard_EN, _charaNum);
            }
            
        }
        

        NextPhase();
    }



    

    public void BattleAction(bool plSide)
    {
        if (plSide)
        {
            bp_PL.GemUse(wazaCard_PL.itemAndSkillStates.cost);
            
            switch (wazaCard_PL.itemAndSkillStates.target)
            {
                case CardBase.ItemAndSkillStates.Target.Target:
                    battleCardAnim_PL.SetTrigger("NormalAttack");
                    CamChange(3);
                    bp_EN.PlayDamageAnim();
                    break;

                case CardBase.ItemAndSkillStates.Target.User:
                case CardBase.ItemAndSkillStates.Target.Player_Anyone:
                    //battleCardAnim_PL.SetTrigger("NormalAttack");
                    CamChange(1);
                    bp_PL.PlayHealAnim();
                    break;

                case CardBase.ItemAndSkillStates.Target.BP_Both:
                    CamChange(1);
                    bp_PL.DrawCard(wazaCard_PL.itemAndSkillStates.damagePoint);
                    bp_EN.DrawCard(wazaCard_PL.itemAndSkillStates.damagePoint);
                    ActiveNextButton();
                    break;

                default:
                    battleCardAnim_PL.SetTrigger("NormalAttack");
                    CamChange(1);
                    bp_EN.PlayDamageAnim();
                    break;
            }
        }
        else
        {
            bp_EN.GemUse(wazaCard_EN.itemAndSkillStates.cost);
            //battleCardAnim_EN.SetTrigger("NormalAttack");
            switch (wazaCard_EN.itemAndSkillStates.target)
            {
                case CardBase.ItemAndSkillStates.Target.Target:
                    battleCardAnim_EN.SetTrigger("NormalAttack");
                    CamChange(3);
                    bp_PL.PlayDamageAnim();
                    break;

                case CardBase.ItemAndSkillStates.Target.User:
                case CardBase.ItemAndSkillStates.Target.Player_Anyone:
                    CamChange(1);
                    bp_EN.PlayHealAnim();
                    break;

                case CardBase.ItemAndSkillStates.Target.BP_Both:
                    CamChange(1);
                    bp_PL.DrawCard(wazaCard_EN.itemAndSkillStates.damagePoint);
                    bp_EN.DrawCard(wazaCard_EN.itemAndSkillStates.damagePoint);
                    ActiveNextButton();
                    break;

                default:
                    battleCardAnim_EN.SetTrigger("NormalAttack");
                    CamChange(1);
                    bp_PL.PlayDamageAnim();
                    break;
            }
        }

    }

    public void ChainCheck(bool _chain)
    {
        isChain = _chain;
        if (isChain)
        {
            chainDamage++;
        }
        else
        {
            chainDamage = 0;
        }
        NextPhase();
    }

    public void SideChange()
    {
        switch (attackerSide)
        {
            case AttackerSide.Player:
                attackerSide = AttackerSide.Enemy;
                break;

            case AttackerSide.Enemy:
                attackerSide = AttackerSide.Player;
                break;
        }
    }

    public void RepetePhase()
    {
        switch (nowPhase)
        {
            case Phase.SelectAttacker:
                Phase_AttackerSelect();
                break;
            case Phase.SelectBlocker:
                Phase_BlockerSelect();
                break;
            case Phase.SelectTefuda_PL:
                Phase_SelectTefuda_Player();
                break;

            case Phase.SelectTefuda_EN:
                Phase_SelectTefuda_Enemy();
                break;

            case Phase.TargetSelect_PL:
                Phase_TargetSelect(AttackerSide.Player);
                break;
        }
    }

    public void NextPhase()
    {
        if (nextButton.activeInHierarchy)
        {
            InactiveNextButton();
        }

        explainObj.SetActive(false);

        switch (nowPhase)
        {
            case Phase.Start:
                if (BattleIntervalEvent.Instance.GetFlagOfEvent(Phase.PlusGem))
                {
                    intervalEvent.PlayEvent();
                    return;
                }

                nowPhase = Phase.PlusGem;
                Phase_PlusGem();
                break;

            case Phase.PlusGem:
                if (isChain) 
                {
                    if (BattleIntervalEvent.Instance.GetFlagOfEvent(Phase.SelectTefuda_PL))
                    {
                        intervalEvent.PlayEvent();
                        return;
                    }

                    nowPhase = Phase.SelectTefuda_PL;
                    Phase_SelectTefuda_Player();
                }
                else
                {
                    if (BattleIntervalEvent.Instance.GetFlagOfEvent(Phase.SelectAttacker))
                    {
                        intervalEvent.PlayEvent();
                        return;
                    }

                    nowPhase = Phase.SelectAttacker;
                    Phase_AttackerSelect();
                }
                
                break;

            case Phase.SelectAttacker:
                if (BattleIntervalEvent.Instance.GetFlagOfEvent(Phase.SelectBlocker))
                {
                    intervalEvent.PlayEvent();
                    return;
                }

                nowPhase = Phase.SelectBlocker;
                Phase_BlockerSelect();
                break;
            case Phase.SelectBlocker:
                if (BattleIntervalEvent.Instance.GetFlagOfEvent(Phase.SelectTefuda_PL))
                {
                    intervalEvent.PlayEvent();
                    return;
                }

                nowPhase = Phase.SelectTefuda_PL;
                Phase_SelectTefuda_Player();
                break;

            case Phase.SelectTefuda_PL:
                if (BattleIntervalEvent.Instance.GetFlagOfEvent(Phase.SelectTefuda_EN))
                {
                    intervalEvent.PlayEvent();
                    return;
                }

                nowPhase = Phase.SelectTefuda_EN;
                Phase_SelectTefuda_Enemy();
                break;

            case Phase.SelectTefuda_EN:
                if (BattleIntervalEvent.Instance.GetFlagOfEvent(Phase.Check))
                {
                    intervalEvent.PlayEvent();
                    return;
                }

                nowPhase = Phase.Check;
                Phase_Check();
                break;

            case Phase.Check:
                if (BattleIntervalEvent.Instance.GetFlagOfEvent(Phase.Open))
                {
                    intervalEvent.PlayEvent();
                    return;
                }

                nowPhase = Phase.Open;
                Phase_Open();
                break;

            case Phase.Open:
                

                if (battleJudge != Judge.BothLose)
                {
                    if(attackerSide == AttackerSide.Player)
                    {
                        if (BattleIntervalEvent.Instance.GetFlagOfEvent(Phase.TargetSelect_PL))
                        {
                            intervalEvent.PlayEvent();
                            return;
                        }

                        nowPhase = Phase.TargetSelect_PL;
                        Phase_TargetSelect(AttackerSide.Player);
                    }
                    else
                    {
                        if (BattleIntervalEvent.Instance.GetFlagOfEvent(Phase.TargetSelect_EN))
                        {
                            intervalEvent.PlayEvent();
                            return;
                        }

                        nowPhase = Phase.TargetSelect_EN;
                        Phase_TargetSelect(AttackerSide.Enemy);
                    }
                }
                else
                {
                    if (BattleIntervalEvent.Instance.GetFlagOfEvent(Phase.DeadCheck))
                    {
                        intervalEvent.PlayEvent();
                        return;
                    }
                    nowPhase = Phase.DeadCheck;
                    DeadCheck();
                }
                break;

            case Phase.TargetSelect_PL:

                
                if (BattleIntervalEvent.Instance.GetFlagOfEvent(Phase.Action_PL))
                {
                    intervalEvent.PlayEvent();
                    return;
                }

                nowPhase = Phase.Action_PL;
                Phase_Action_PL();
                break;

            case Phase.Action_PL:
                

                if (attackerSide == AttackerSide.Enemy || bp_EN.BattleCharaDeadCheck())
                {
                    if (BattleIntervalEvent.Instance.GetFlagOfEvent(Phase.DeadCheck))
                    {
                        intervalEvent.PlayEvent();
                        return;
                    }

                    nowPhase = Phase.DeadCheck;
                    DeadCheck();
                }
                else
                {
                    
                    if (BattleIntervalEvent.Instance.GetFlagOfEvent(Phase.TargetSelect_EN))
                    {
                        intervalEvent.PlayEvent();
                        return;
                    }
                    nowPhase = Phase.TargetSelect_EN;
                    Phase_TargetSelect(AttackerSide.Enemy);
                }

                break;

            case Phase.TargetSelect_EN:
                if (BattleIntervalEvent.Instance.GetFlagOfEvent(Phase.Action_EN))
                {
                    intervalEvent.PlayEvent();
                    return;
                }

                nowPhase = Phase.Action_EN;
                Phase_Action_EN();
                break;

            case Phase.Action_EN:
                

                if (attackerSide == AttackerSide.Player || bp_PL.BattleCharaDeadCheck())
                {
                    if (BattleIntervalEvent.Instance.GetFlagOfEvent(Phase.DeadCheck))
                    {
                        intervalEvent.PlayEvent();
                        return;
                    }

                    nowPhase = Phase.DeadCheck;
                    DeadCheck();
                }
                else
                {
                    if (BattleIntervalEvent.Instance.GetFlagOfEvent(Phase.TargetSelect_PL))
                    {
                        intervalEvent.PlayEvent();
                        return;
                    }
                    nowPhase = Phase.TargetSelect_PL;
                    Phase_TargetSelect(AttackerSide.Player);
                }
                break;


            case Phase.DeadCheck:
                

                if (BattleIntervalEvent.Instance.GetFlagOfEvent(Phase.ChainCheck))
                {
                    intervalEvent.PlayEvent();
                    return;
                }
                SideChange();

                nowPhase = Phase.ChainCheck;
                isChain = false;
                Phase_ChainCheck();
                
                break;

            case Phase.ChainCheck:
                if (BattleIntervalEvent.Instance.GetFlagOfEvent(Phase.ResetCards))
                {
                    intervalEvent.PlayEvent();
                    return;
                }
                nowPhase = Phase.ResetCards;
                Phase_ResetCards();
                break;

            case Phase.ResetCards:
                if (isChain)
                {
                    if (BattleIntervalEvent.Instance.GetFlagOfEvent(Phase.PlusGem))
                    {
                        intervalEvent.PlayEvent();
                        return;
                    }
                    nowPhase = Phase.PlusGem;
                    Phase_PlusGem();

                }
                else
                {
                    if (BattleIntervalEvent.Instance.GetFlagOfEvent(Phase.Draw))
                    {
                        intervalEvent.PlayEvent();
                        return;
                    }

                    nowPhase = Phase.Draw;
                    Phase_DrawCard();
                }

                
                break;

            case Phase.Draw:
                if (BattleIntervalEvent.Instance.GetFlagOfEvent(Phase.PlusGem))
                {
                    
                    intervalEvent.PlayEvent();
                    return;
                }

                nowPhase = Phase.PlusGem;
                Phase_PlusGem();
                break;

        }
    }

    public void DeadCheck()
    {
        bp_PL.ZyoutaiDamage();
        bp_EN.ZyoutaiDamage();

        bool plCheck = bp_PL.DeadCheck();
        bool enCheck = bp_EN.DeadCheck();

        if (plCheck)
        {
            Debug.Log("ƒQ[ƒ€‚É”s–k");
            BattleEnd_Lose();
            return;
        }
        if (enCheck)
        {
            Debug.Log("ƒQ[ƒ€‚ÉŸ—˜");
            BattleEnd_Win();
            return;
        }

        NextPhase();
    }

    public void Quit()
    {
        /*
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif
        */

        sceneAnim.SetTrigger("Close");

        ButtonManager.Instance.ResetLastButton();
        DisplayManager.Instance.GamenClose(1.0f);
        Invoke("LoadTitle", 2.0f);
    }

    public void LoadTitle()
    {
        TableSceneManager.Instance.TableCamActive();
        SceneEventManager.Instance.ChangeScene("TitleDemo", 0);
    }

}
