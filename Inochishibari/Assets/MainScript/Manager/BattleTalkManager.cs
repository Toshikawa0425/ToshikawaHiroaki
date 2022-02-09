using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class BattleTalkManager : SingletonMonoBehaviour<BattleTalkManager>
{
    [SerializeField]
    private WindowPosition windowPos_UP;

    [SerializeField]
    private BattleTalk talkScript = null;

    [SerializeField]
    private GameObject talkObj;

    [SerializeField]
    private GameObject fukidashi;
    [SerializeField]
    private GameObject fukidashiTale;
    [SerializeField]
    private GameObject arrow;
    [SerializeField]
    private GameObject choiceObj;
    [SerializeField]
    private Button choiceButton_A;
    [SerializeField]
    private TextMeshProUGUI choiceA_TM;
    [SerializeField]
    private TextMeshProUGUI choiceB_TM;

    [SerializeField]
    private TextMeshProUGUI dialogue_TM_iconOn;
    [SerializeField]
    private TextMeshProUGUI dialogue_TM_iconOff;

    [SerializeField]
    private TextMeshProUGUI name_TM;

    private TextMeshProUGUI nowDialogueTM;

    [SerializeField]
    private GameObject nameBack;

    [SerializeField]
    private GameObject iconBack;
    [SerializeField]
    private Image iconImage;

    private RectTransform RT_Fukidashi;
    private RectTransform RT_Tale;
    private RectTransform RT_Name;

    public List<BattleTalkInfo> talkList = new List<BattleTalkInfo>();

    [SerializeField]
    private BattleTalkInfo nowTalk;

    private int talkListNum;
    [SerializeField]
    private int nowNum = 0;

    public bool talking = false;
    private bool waitForInput = false;

    const string COLOR_RED = "<color=#ff4c4c>";



    [System.Serializable]
    public class TalkCondition
    {
        public int turn;
    }


    [System.Serializable]
    private class WindowPosition
    {
        public Vector2 FukidashiPos_Right;
        public Vector2 FukidashiPos_Left;

        public Vector2 TalePos_Right;
        public Vector2 TalePos_Left;

        public Vector2 NamePos_Right;
        public Vector2 NamePos_Left;
    }


    [System.Serializable]
    public class BattleTalkInfo
    {
        public UnityEvent startEvent;
        public string talkerName = "";
        public WindowSide windowSide;
        [TextArea(1, 6)]
        public string dialogue = "";
        public Sprite talkerIcon;
        public int talkSpeedFlame = 2;
        public bool choice;
        public string TextA = "";
        public string TextB = "";
        public int answer_A;
        public int answer_B;

        public CardBase getItem = null;
        public int getItemNum = 0;
        public bool end = false;

        public int nextNum = -1;
        public UnityEvent endEvent;
    }

    public enum WindowSide
    {
        Right,
        Left
    }

    private void Start()
    {
        RT_Fukidashi = fukidashi.GetComponent<RectTransform>();
        RT_Tale = fukidashiTale.GetComponent<RectTransform>();
        RT_Name = nameBack.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (talking)
        {
            if (waitForInput)
            {
                NextTalk();
            }
        }
    }

    public void SetTalkList(BattleTalk _talkScript, List<BattleTalkInfo> _talkList)
    {
        if (!talking)
        {
            talkScript = _talkScript;
            talkList = _talkList;
            talkListNum = talkList.Count;
            nowNum = 0;
            nowTalk = talkList[nowNum];

            if (nowTalk.talkerIcon == null)
            {
                nowDialogueTM = dialogue_TM_iconOff;
            }
            else
            {
                nowDialogueTM = dialogue_TM_iconOn;
            }

            StartTalk();
        }
    }

    private void StartTalk()
    {
        /*
        if (GameObject.Find("Player"))
            PlController_Field.Instance.canMove = false;
        */
        waitForInput = false;
        talking = true;

        talkObj.SetActive(true);
        StartCoroutine(TalkTextDisPlay());
    }

    private void SetWindowPos()
    {
        Debug.Log("set");
        if (nowTalk.windowSide == WindowSide.Right)
        {
            RT_Fukidashi.anchoredPosition = windowPos_UP.FukidashiPos_Right;
            RT_Name.anchoredPosition = windowPos_UP.NamePos_Right;
            RT_Tale.anchoredPosition = windowPos_UP.TalePos_Right;
            RT_Tale.localScale = new Vector3(1, 1, 0);
        }
        else
        {
            RT_Fukidashi.anchoredPosition = windowPos_UP.FukidashiPos_Left;
            RT_Name.anchoredPosition = windowPos_UP.NamePos_Left;
            RT_Tale.anchoredPosition = windowPos_UP.TalePos_Left;
            RT_Tale.localScale = new Vector3(-1, 1, 0);
        }
    }

    private void NextTalk()
    {
        if (InputSetting.Instance.Accept)
        {
            arrow.SetActive(false);
            waitForInput = false;


            if (nowNum == talkListNum - 1 || nowTalk.end)
            {
                EndTalk();
            }
            else
            {
                if (!nowTalk.choice)
                {
                    nowNum++;
                    nowTalk = talkList[nowNum];
                    StartCoroutine(TalkTextDisPlay());
                }
                else
                {
                    OpenChoiceWindow();
                }
            }
        }
    }

    private void OpenChoiceWindow()
    {
        choiceObj.SetActive(true);
        choiceA_TM.text = nowTalk.TextA;
        choiceB_TM.text = nowTalk.TextB;
        choiceButton_A.Select();
    }

    public void ChoiceA()
    {
        choiceObj.SetActive(false);
        nowNum = nowTalk.answer_A;
        nowTalk = talkList[nowNum];
        StartCoroutine(TalkTextDisPlay());
    }

    public void ChoiceB()
    {
        choiceObj.SetActive(false);
        nowNum = nowTalk.answer_B;
        nowTalk = talkList[nowNum];
        StartCoroutine(TalkTextDisPlay());
    }

    private void EndTalk()
    {
        nowDialogueTM.text = "";
        name_TM.text = "";
        iconImage.sprite = null;



        talkObj.SetActive(false);

        if (nowTalk.endEvent.GetPersistentEventCount() > 0)
        {
            nowTalk.endEvent.Invoke();
        }

        nowTalk = null;
        talkScript = null;
        talking = false;

        BattleManager.Instance.isEvent = false;
    }




    private IEnumerator TalkTextDisPlay()
    {
        SetWindowPos();
        nowDialogueTM.text = "";

        if (nowTalk.talkerName != "")
        {
            nameBack.SetActive(true);
            name_TM.text = nowTalk.talkerName;
        }
        else
        {
            nameBack.SetActive(false);
        }

        if (nowTalk.talkerIcon != null)
        {
            iconBack.SetActive(true);
            iconImage.sprite = nowTalk.talkerIcon;
        }
        else
        {
            iconImage.sprite = null;
            iconBack.SetActive(false);
        }

        if (nowTalk.getItem != null)
        {
            Inventry.Instance.GetItem(nowTalk.getItem, nowTalk.getItemNum);
        }

        string _disPlayText = "";
        int _textLength = nowTalk.dialogue.Length;
        int _textNum = 0;
        int _speed = nowTalk.talkSpeedFlame;
        string _dialogue = nowTalk.dialogue;

        int _nowFlame;

        if (nowTalk.startEvent.GetPersistentEventCount() > 0)
        {
            nowTalk.startEvent.Invoke();
            yield return null;
        }

        while (_textNum < _textLength)
        {
            _nowFlame = 0;
            while (_nowFlame < _speed)
            {
                _nowFlame++;
                yield return null;
            }

            if (GetSpecialText(_dialogue[_textNum].ToString()))
            {
                if ((_dialogue[_textNum + 1].ToString() == "K"))
                {
                    _textNum += 4;
                    _disPlayText += "\n";
                }
                else if ((_dialogue[_textNum + 1].ToString() == "C"))
                {
                    if (_dialogue[_textNum + 2].ToString() == "R")
                    {
                        _disPlayText += COLOR_RED;
                    }

                    _textNum += 4;

                    while (_dialogue[_textNum].ToString() != "/")
                    {
                        _disPlayText += _dialogue[_textNum];
                        _textNum++;
                    }
                    _textNum += 2;
                    _disPlayText += "</color>";
                }
            }
            else
            {
                _disPlayText += _dialogue[_textNum];
                _textNum++;
            }

            nowDialogueTM.text = _disPlayText;
            yield return null;
        }

        waitForInput = true;
        arrow.SetActive(true);
    }

    private bool GetSpecialText(string _text)
    {
        return _text == "*";
    }
}
