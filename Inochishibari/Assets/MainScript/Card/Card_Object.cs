using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Card_Object : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private CardBase thisCard;

    [SerializeField]
    private SpriteRenderer cardSR;
    [SerializeField]
    private SpriteRenderer zokuseiSR;

    [SerializeField]
    private GameObject cardBaseObj;
    [SerializeField]
    private GameObject charaCardObj;
    [SerializeField]
    private GameObject itemAndSkillCardObj;

    [SerializeField]
    private TextMeshPro pointTMPro;
    [SerializeField]
    private CharaCard charaCard_Param;
    [SerializeField]
    private ItemSkillCard itemSkillCard_Param;

    [SerializeField]
    private WazaTypeImages wazaTypeImages;
    [SerializeField]
    private ZokuseiImages zokuseiImages;
    [SerializeField]
    private TargetTypeImages targetTypeImages;
    [SerializeField]
    private EffectTypeImages effectTypeImage;
    


    [SerializeField]
    private Button thisButton;

    [SerializeField]
    private bool startButtonOff = false;

    [SerializeField]
    private GameObject highlightObj;

    [System.Serializable]
    private class CharaCard
    {
        public SpriteRenderer wazaTypeSR;
        //public TextMeshPro hpTMPro;
        public GameObject dokuMark;
    }

    [System.Serializable]
    private class ItemSkillCard
    {
        public GameObject costObj;
        public SpriteRenderer wazaTypeSR;
        public SpriteRenderer targetTypeSR;
        public SpriteRenderer effectTypeSR;
        public TextMeshPro cardNameTMPro;
        //public TextMeshPro damageTMPro;
        public TextMeshPro costTMPro;
        public GameObject notEnableObj;
    }

    [System.Serializable]
    public class WazaTypeImages
    {
        public Sprite Da;
        public Sprite Ten;
        public Sprite Tou;
        public Sprite Syu;
    }

    [System.Serializable]
    public class TargetTypeImages
    {
        public Sprite AllForENParty;
        public Sprite AllForMyParty;

        public Sprite ChoiseFromENParty;
        public Sprite ChoiseFromMyParty;

        public Sprite BothPlayer;
    }

    [System.Serializable]
    public class EffectTypeImages
    {
        public Sprite Heal;
        public Sprite Poison;
        public Sprite Draw;
    }

    [System.Serializable]
    public class ZokuseiImages
    {
        public Sprite Normal;
        public Sprite Fire;
        public Sprite Water;
        public Sprite Nature;
        public Sprite Thunder;
        public Sprite Wind;
        public Sprite Ice;
        public Sprite Ground;
    }

    private void OnEnable()
    {
        if (startButtonOff)
        {
            ButtonOff();
        }
    }

    public void SetCard(CardBase _card, int _HP = 0)
    {
        cardSR.sprite = _card.cardImage;
        cardBaseObj.SetActive(true);

        switch (_card.cardType)
        {
            case CardBase.CardType.Base:
            case CardBase.CardType.Add:
                pointTMPro.gameObject.SetActive(false);
                zokuseiSR.sprite = null;
                charaCardObj.SetActive(false);
                itemAndSkillCardObj.SetActive(false);
                break;

            case CardBase.CardType.Dangeon:
                pointTMPro.gameObject.SetActive(false);
                break;


            case CardBase.CardType.Chara:
                charaCardObj.SetActive(true);
                itemAndSkillCardObj.SetActive(false);


                charaCard_Param.wazaTypeSR.sprite = GetWazaTypeSprite(_card.wazaType);
                pointTMPro.gameObject.SetActive(true);
                pointTMPro.text = _HP.ToString();
                SetZokuseiImage(_card.cardZokusei);
                break;

            


            default:
                charaCardObj.SetActive(false);
                itemAndSkillCardObj.SetActive(true);
                pointTMPro.gameObject.SetActive(true);
                pointTMPro.text = _card.itemAndSkillStates.damagePoint.ToString();

                if(_card.itemAndSkillStates.cost > 0)
                {
                    itemSkillCard_Param.costObj.SetActive(true);
                    itemSkillCard_Param.costTMPro.text = _card.itemAndSkillStates.cost.ToString();
                }
                else
                {
                    itemSkillCard_Param.costObj.SetActive(false);
                }

                
                itemSkillCard_Param.wazaTypeSR.sprite = GetWazaTypeSprite(_card.wazaType);

                itemSkillCard_Param.cardNameTMPro.text = _card.cardName;
                //itemSkillCard_Param.cardInfoTMPro.text = _card.itemAndSkillStates.infoText;

                if (_card.itemAndSkillStates.target != CardBase.ItemAndSkillStates.Target.Target)
                {
                    switch (_card.itemAndSkillStates.target)
                    {
                        case CardBase.ItemAndSkillStates.Target.Enemy_All:
                            itemSkillCard_Param.targetTypeSR.sprite = targetTypeImages.AllForENParty;
                            break;

                        case CardBase.ItemAndSkillStates.Target.Player_All:
                            itemSkillCard_Param.targetTypeSR.sprite = targetTypeImages.AllForMyParty;
                            break;

                        case CardBase.ItemAndSkillStates.Target.Enemy_Anyone:
                            itemSkillCard_Param.targetTypeSR.sprite = targetTypeImages.ChoiseFromENParty;
                            break;

                        case CardBase.ItemAndSkillStates.Target.Player_Anyone:
                            itemSkillCard_Param.targetTypeSR.sprite = targetTypeImages.ChoiseFromMyParty;
                            break;

                        case CardBase.ItemAndSkillStates.Target.BP_Both:
                            itemSkillCard_Param.targetTypeSR.sprite = targetTypeImages.BothPlayer;
                            break;

                        default:
                            itemSkillCard_Param.targetTypeSR.sprite = null;
                            break;
                    }
                }
                else
                {
                    itemSkillCard_Param.targetTypeSR.sprite = null;
                }

                if (_card.itemAndSkillStates.effectType != CardBase.ItemAndSkillStates.EffectType.Damage)
                {
                    switch (_card.itemAndSkillStates.effectType)
                    {
                        case CardBase.ItemAndSkillStates.EffectType.Heal:
                            itemSkillCard_Param.effectTypeSR.sprite = effectTypeImage.Heal;
                            break;

                        case CardBase.ItemAndSkillStates.EffectType.Doku:
                            itemSkillCard_Param.effectTypeSR.sprite = effectTypeImage.Poison;
                            break;

                        case CardBase.ItemAndSkillStates.EffectType.Draw:
                            itemSkillCard_Param.effectTypeSR.sprite = effectTypeImage.Draw;
                            break;

                        default:
                            itemSkillCard_Param.effectTypeSR.sprite = null;
                            break;
                    }
                }
                else
                {
                    itemSkillCard_Param.effectTypeSR.sprite = null;
                }



                SetZokuseiImage(_card.cardZokusei);
                break;
        }

        thisCard = _card;
    }

    public void ResetCard()
    {
        zokuseiSR.sprite = null;
        charaCardObj.SetActive(false);
        itemAndSkillCardObj.SetActive(false);
        cardBaseObj.SetActive(false);
        thisCard = null;
    }

    public void StatesUpDate(BattleManager.CardAndHP _states)
    {
        pointTMPro.text = _states.nowHP.ToString();
        SetZyoutaiMark(_states.zyoutai);

        if (_states.isDead)
        {
            RotateCard();
        }
    }

    public void ButtonOff()
    {
        thisButton.enabled = false;
    }

    public void ButtonOn()
    {
        thisButton.enabled = true;
    }

    public void RotateCard()
    {
        animator.SetTrigger("Rotate");
    }

    public void SetEnable(bool _flag)
    {
        itemSkillCard_Param.notEnableObj.SetActive(!_flag);
    }

    public void SelectThisCard()
    {
        ButtonManager.Instance.SetLastButton(cardBaseObj);
    }


    private Sprite GetWazaTypeSprite(CardBase.WazaType wazaType)
    {
        switch (wazaType)
        {
            case CardBase.WazaType.Da:
                return wazaTypeImages.Da;

            case CardBase.WazaType.Ten:
                return wazaTypeImages.Ten;

            case CardBase.WazaType.Tou:
                return wazaTypeImages.Tou;

            case CardBase.WazaType.Syu:
                return wazaTypeImages.Syu;

            default:
                Debug.LogError("‹Zƒ^ƒCƒv‚ª‚ ‚è‚Ü‚¹‚ñ");
                return null;
        }
    }

    private void SetZokuseiImage(CardBase.Zokusei zokusei)
    {
        switch (zokusei)
        {
            case CardBase.Zokusei.Normal:
                zokuseiSR.sprite = zokuseiImages.Normal;
                break;
            case CardBase.Zokusei.Fire:
                zokuseiSR.sprite = zokuseiImages.Fire;
                break;
            case CardBase.Zokusei.Water:
                zokuseiSR.sprite = zokuseiImages.Water;
                break;
            case CardBase.Zokusei.Nature:
                zokuseiSR.sprite = zokuseiImages.Nature;
                break;
            case CardBase.Zokusei.Thunder:
                zokuseiSR.sprite = zokuseiImages.Thunder;
                break;
            case CardBase.Zokusei.Wind:
                zokuseiSR.sprite = zokuseiImages.Wind;
                break;
            case CardBase.Zokusei.Ground:
                zokuseiSR.sprite = zokuseiImages.Ground;
                break;
            case CardBase.Zokusei.Ice:
                zokuseiSR.sprite = zokuseiImages.Ice;
                break;

            default:
                break;
        }
    }

    public void SetZyoutaiMark(BattleManager.Zyoutai _zyoutai)
    {
        switch (_zyoutai)
        {
            default:
                charaCard_Param.dokuMark.SetActive(false);
                break;

            case BattleManager.Zyoutai.Doku:
                charaCard_Param.dokuMark.SetActive(true);
                break;
        }
    }

    public void Highlight_On()
    {
        highlightObj.SetActive(true);
    }

    public void Highlight_Off()
    {
        highlightObj.SetActive(false);
    }
}
