using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUpManager : DangeonAddSceneBase
{
    [SerializeField]
    private int earnedEXP;

    [SerializeField]
    private List<Card_Object> partyChara_Obj = new List<Card_Object>();

    [SerializeField]
    private GameObject nextButton;

    [SerializeField]
    private GameObject partyCharaButton;

    [SerializeField]
    private TextMeshPro[] levelTMPro;
    [SerializeField]
    private TextMeshPro[] expTMPro;
    [SerializeField]
    private TextMeshPro[] hpTMPro;

    [SerializeField]
    private TextMeshPro[] level_Next_TMPro;
    [SerializeField]
    private TextMeshPro[] exp_Plus_TMPro;
    [SerializeField]
    private TextMeshPro[] hp_Next_TMPro;

    private int selectedCharaNum;
    [SerializeField]
    private List<BattleManager.CardAndHP.LevelStates> nowStates = new List<BattleManager.CardAndHP.LevelStates>();

    private List<BattleManager.CardAndHP> partyList = new List<BattleManager.CardAndHP>();

    private List<List<CardBase>> getSkillCardList = new List<List<CardBase>>();

    [SerializeField]
    private Card_Object getSkillCard_Obj;
    [SerializeField]
    private Talk getSkillTalk;

    private bool[] isCaled = { false, false, false };

    private void Start()
    {

        InitState();
    }

    public override void InitState()
    {
        base.InitState();
        earnedEXP = GameManager.Instance.earnedEXP;
        partyList = GameManager.Instance.playerCharaCards;

        for(int i=0; i< 3; i++)
        {
            getSkillCardList.Add(new List<CardBase>());
            partyChara_Obj[i].SetCard(partyList[i].charaCard, partyList[i].nowHP);
            int _lv = partyList[i].levelStates.Level;
            int _exp = partyList[i].levelStates.EXP;
            int _hp = partyList[i].maxHP;

            levelTMPro[i].text = "LV: " + _lv;
            expTMPro[i].text = "EXP: " + _exp;
            hpTMPro[i].text = "HP: " + _hp;

            level_Next_TMPro[i].text = "";
            exp_Plus_TMPro[i].text = "";
            hp_Next_TMPro[i].text = "";

            BattleManager.CardAndHP selectChara = partyList[i];
            nowStates[i].Level = selectChara.levelStates.Level;
            nowStates[i].EXP = selectChara.levelStates.EXP;
            nowStates[i].totalEXP = selectChara.levelStates.totalEXP;
        }
    }


    public void SelectingChara(int _num)
    {
        selectedCharaNum = _num;

        partyChara_Obj[_num].Highlight_On();

        if (isCaled[_num] == false)
        {
            CalEXP();
        }
        else
        {
            NextTextUpdate(_num);
        }

    }

    public void DeselectingChara(int _num)
    {
        partyChara_Obj[_num].Highlight_Off();

        NextTextReset(_num);
    }

    public void SelectedChara()
    {
        ButtonManager.Instance.ResetLastButton();
        GameManager.Instance.earnedEXP = 0;
        GameManager.Instance.SetLevelAndEXP(selectedCharaNum,nowStates[selectedCharaNum]);

        foreach (CardBase _skill in getSkillCardList[selectedCharaNum])
        {
            DeckManager.Instance.GetCard(_skill);
        }

        //partyChara_Obj[i].SetCard(GameManager.Instance.playerCharaCards[i].charaCard, GameManager.Instance.playerCharaCards[i].nowHP);
        int _lv = nowStates[selectedCharaNum].Level;
        int _exp = nowStates[selectedCharaNum].EXP;
        int _hp = GameManager.Instance.playerCharaCards[selectedCharaNum].maxHP;

        levelTMPro[selectedCharaNum].text = "LV: " + _lv;
        expTMPro[selectedCharaNum].text = "EXP: " + _exp;
        hpTMPro[selectedCharaNum].text = "HP: " + _hp;

        level_Next_TMPro[selectedCharaNum].text = "";
        exp_Plus_TMPro[selectedCharaNum].text = "";
        hp_Next_TMPro[selectedCharaNum].text = "";

        partyChara_Obj[selectedCharaNum].SetCard(GameManager.Instance.playerCharaCards[selectedCharaNum].charaCard, GameManager.Instance.playerCharaCards[selectedCharaNum].maxHP); ;


        if (getSkillCardList[selectedCharaNum].Count == 0)
        {
            SelectNextButton();
        }
        else
        {
            getSkillCard_Obj.SetCard(getSkillCardList[selectedCharaNum][0]);
            getSkillTalk.StartEvent();
        }
    }

    public void SelectNextButton()
    {
        ButtonManager.Instance.SetLastButton(nextButton);
    }


    private void CalEXP()
    {
        int _total = earnedEXP;
        while (_total > 0)
        {
            _total--;
            nowStates[selectedCharaNum].EXP++;

            if (nowStates[selectedCharaNum].EXP == nowStates[selectedCharaNum].Level)
            {
                LevelUp();
            }
        }
        nowStates[selectedCharaNum].totalEXP += earnedEXP;
        isCaled[selectedCharaNum] = true;

        NextTextUpdate(selectedCharaNum);
    }

    private void LevelUp()
    {
        nowStates[selectedCharaNum].EXP = 0;
        nowStates[selectedCharaNum].Level++;

        List<PlayerCharactor.GetSkillLevel> _skills =GameManager.Instance.playerCharas[selectedCharaNum].getSkillLevel;

        for (int i = 0; i < _skills.Count; i++)
        {
            int _lv = _skills[i].level;
            if(nowStates[selectedCharaNum].Level == _lv)
            {
                CardBase _skill = _skills[i].skill;
                getSkillCardList[selectedCharaNum].Add(_skill);
                break;
            }

            if(_lv > nowStates[selectedCharaNum].Level)
            {
                break;
            }
        }
    }

    private void NextTextUpdate(int _num)
    {
        level_Next_TMPro[_num].text = "(-> " + nowStates[_num].Level + ")";
        exp_Plus_TMPro[_num].text = "(+" + earnedEXP + ")";
        int _max = partyList[_num].charaCard.charaStates.defaultHP + Mathf.FloorToInt(nowStates[_num].Level / 2.0f);
        hp_Next_TMPro[_num].text = "(-> " + _max + ")";
    }

    public void NextTextReset(int _num)
    {
        level_Next_TMPro[_num].text = "";
        exp_Plus_TMPro[_num].text = "";
        hp_Next_TMPro[_num].text = "";
    }

    public override void SelectFirstCard()
    {
        ButtonManager.Instance.SetLastButton(partyCharaButton);
    }

    public override void UnLoadScene()
    {
        SceneEventManager.Instance.UnloadScene(-1);
        DangeonManager.Instance.FirstSelect();
    }
}
