using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardBase", menuName ="CreateCard")]
public class CardBase : ScriptableObject
{
    [Header("Add,Item,Skill,Chara")]
    public string CardID = "00000000";
    public CardType cardType;
    public string cardName;
    public WazaType wazaType;
    public Sprite cardImage;
    public Zokusei cardZokusei;

    public CharaStates charaStates;
    public ItemAndSkillStates itemAndSkillStates;

    public enum CardType
    {
        Chara = 0,
        Skill = 1,
        Item = 2,
        Base = -1,
        Dangeon = -2,
        Add = 3
    }

    public enum WazaType
    {
        Da,
        Ten,
        Tou,
        Syu
    }

    public enum Zokusei
    {
        Normal,
        Fire,
        Water,
        Nature,
        Thunder,
        Ground,
        Wind,
        Ice
    }

    [System.Serializable]
    public class CharaStates
    {
        public int defaultHP;
    }

    [System.Serializable]
    public class ItemAndSkillStates
    {
        public bool isImportant;
        public int cost;
        public bool isPrivateCard;
        public string infoText;

        public int damagePoint;

        //public List<CardEffect> cardEffect = new List<CardEffect>();
        public List<CardBase> userCard = new List<CardBase>();

        public Target target;
        public EffectType effectType;

        public enum Target
        {
            Target,
            User,
            Enemy_Anyone,
            Enemy_All,
            Player_Anyone,
            Player_All,
            BP_Enemy,
            BP_Player,
            BP_Both
        }

        public enum EffectType
        {
            Damage,
            Heal,
            Doku,
            Mahi,
            Hyoketsu,
            Draw
        }

        [System.Serializable]
        public class CardEffect
        {
            public Target target;
            public EffectType effectType;
        }
    }
}
