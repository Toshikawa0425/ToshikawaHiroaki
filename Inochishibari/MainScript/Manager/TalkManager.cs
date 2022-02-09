using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class TalkManager : SingletonMonoBehaviour<TalkManager>
{
    [SerializeField]
    private AudioClip talkSE;
    [SerializeField]
    private WindowPosition windowPos_UP;

    [SerializeField]
    private Talk talkScript = null;

    [SerializeField]
    private GameObject talkObj;

    [SerializeField]
    private GameObject fukidashi;
    [SerializeField]
    private GameObject fukidashiTale;
    [SerializeField]
    private GameObject fukidashiTale_Under;
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

    public List<TalkInfo> talkList = new List<TalkInfo>();

    [SerializeField]
    private TalkInfo nowTalk;

    private int talkListNum;
    [SerializeField]
    private int nowNum = 0;

    public bool talking = false;
    private bool activeWindow = false;
    private bool waitForInput = false;

    const string COLOR_RED = "<color=#ff4c4c>";


    [System.Serializable]
    private class WindowPosition
    {
        public Vector2 FukidashiPos_UP;
        public Vector2 FukidashiPos_Down;
        public Vector2 FukidashiPos_Middle;

        public Vector2 NamePos_UP;
        public Vector2 NamePos_Down;
        public Vector2 NamePos_Middle;

        public Vector2 ChoicePos_UP;
        public Vector2 ChoicePos_Down;
        public Vector2 ChoicePos_Middle;
    }


    [System.Serializable]
    public class TalkInfo
    {
        public Transform talkerTransform;
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
        public bool skip = false;
        public bool autoNext = false;
        public bool waitForNext = false;
        public bool nowWindow = false;

        public UnityEvent answerEvent_A;
        public UnityEvent answerEvent_B;

        public int nextNum = -1;
        public UnityEvent endEvent;
    }

    public enum WindowSide
    {
        UP,
        DOWN,
        MIDDLE,
        AboveTalker,
        AbovePlayer,
        UnderTalker,
        UnderPlayer
    }
    


    private void Update()
    {
        if (talking)
        {
            
            if (waitForInput)
            {
                if (InputSetting.Instance.Accept || nowTalk.autoNext)
                {
                    NextTalk();
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (talking)
        {
            if (activeWindow)
            {
                SetWindowPos();
            }
        }
    }

    public void SetTalkList(Talk _talkScript ,List<TalkInfo> _talkList,int _startNum)
    {
        Debug.Log("SETTALKLIST");

            talkScript = _talkScript;
            talkList = _talkList;
            talkListNum = talkList.Count;
            nowNum = _startNum;
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

    private void StartTalk()
    {
        Debug.Log("STARTTALK");
        if (PlController_Field.Instance)
        {
            PlController_Field.Instance.CanMoveOff();
        }
        waitForInput = false;
        talking = true;
        
        talkObj.SetActive(true);
        StartCoroutine(TalkTextDisPlay());
    }

    private void SetWindowPos()
    {

        switch (nowTalk.windowSide)
        {
            case WindowSide.AboveTalker:
                Vector2 _pos = 
                    RectTransformUtility.WorldToScreenPoint(Camera.main, nowTalk.talkerTransform.position) 
                    + new Vector2(-960, -540) 
                    + new Vector2(-100, 350);

                fukidashiTale_Under.SetActive(false);
                fukidashiTale.SetActive(true);
                fukidashi.GetComponent<RectTransform>().anchoredPosition = _pos;
                nameBack.GetComponent<RectTransform>().anchoredPosition = _pos + new Vector2(-250, -160);
                choiceObj.GetComponent<RectTransform>().anchoredPosition = _pos + new Vector2(340,-210);
                break;

            case WindowSide.UnderTalker:
                Vector2 _pos_1 =
                    RectTransformUtility.WorldToScreenPoint(Camera.main, nowTalk.talkerTransform.position)
                    + new Vector2(-960, -540)
                    + new Vector2(-100, 0);

                fukidashiTale.SetActive(false);
                fukidashiTale_Under.SetActive(true);
                fukidashi.GetComponent<RectTransform>().anchoredPosition = _pos_1;
                nameBack.GetComponent<RectTransform>().anchoredPosition = _pos_1 + new Vector2(-250, 160);
                choiceObj.GetComponent<RectTransform>().anchoredPosition = _pos_1 + new Vector2(340, 210);
                break;

            case WindowSide.AbovePlayer:
                Vector2 _posPl =
                    RectTransformUtility.WorldToScreenPoint(Camera.main, PlController_Field.Instance.transform.position)
                    + new Vector2(-960, -540)
                    + new Vector2(-100, 350);

                fukidashiTale_Under.SetActive(false);
                fukidashiTale.SetActive(true);
                fukidashi.GetComponent<RectTransform>().anchoredPosition = _posPl;
                
                nameBack.GetComponent<RectTransform>().anchoredPosition = _posPl + new Vector2(-250, -160);
                choiceObj.GetComponent<RectTransform>().anchoredPosition = _posPl + new Vector2(340, -210);
                break;

            case WindowSide.UnderPlayer:
                Vector2 _posPl_1 =
                    RectTransformUtility.WorldToScreenPoint(Camera.main, PlController_Field.Instance.transform.position)
                    + new Vector2(-960, -540)
                    + new Vector2(-100, -175);

                fukidashiTale.SetActive(false);
                fukidashiTale_Under.SetActive(true);
                fukidashi.GetComponent<RectTransform>().anchoredPosition = _posPl_1;
                nameBack.GetComponent<RectTransform>().anchoredPosition = _posPl_1 + new Vector2(-250,160);
                choiceObj.GetComponent<RectTransform>().anchoredPosition = _posPl_1 + new Vector2(340,210);
                break;

            

            case WindowSide.UP:
                fukidashiTale.SetActive(false);
                fukidashiTale_Under.SetActive(false);
                fukidashi.GetComponent<RectTransform>().anchoredPosition = windowPos_UP.FukidashiPos_UP;
                
                nameBack.GetComponent<RectTransform>().anchoredPosition = windowPos_UP.NamePos_UP;
                choiceObj.GetComponent<RectTransform>().anchoredPosition = windowPos_UP.ChoicePos_UP;
                break;

            case WindowSide.MIDDLE:
                fukidashiTale.SetActive(false);
                fukidashiTale_Under.SetActive(false);
                fukidashi.GetComponent<RectTransform>().anchoredPosition = windowPos_UP.FukidashiPos_Middle;

                nameBack.GetComponent<RectTransform>().anchoredPosition = windowPos_UP.NamePos_Middle;
                choiceObj.GetComponent<RectTransform>().anchoredPosition = windowPos_UP.ChoicePos_Middle;
                break;

            case WindowSide.DOWN:
                fukidashiTale.SetActive(false);
                fukidashiTale_Under.SetActive(false);
                fukidashi.GetComponent<RectTransform>().anchoredPosition = windowPos_UP.FukidashiPos_Down;
                
                nameBack.GetComponent<RectTransform>().anchoredPosition = windowPos_UP.NamePos_Down;
                choiceObj.GetComponent<RectTransform>().anchoredPosition = windowPos_UP.ChoicePos_Down;
                break;
        }
    }

    public void NextTalk()
    {
        
            activeWindow = false;
            arrow.SetActive(false);
            waitForInput = false;

            if (nowTalk.choice)
            {
                OpenChoiceWindow();
                return;
            }


            if (nowNum == talkListNum - 1 || nowTalk.end)
            {
                EndTalk();
            }
            else
            {
                    if (!nowTalk.skip)
                    {
                        nowNum++;
                    }
                    else
                    {
                        nowNum = nowTalk.nextNum;
                    }
                    nowTalk = talkList[nowNum];
                    StartCoroutine(TalkTextDisPlay());
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

        if (nowTalk.answerEvent_A.GetPersistentEventCount() > 0)
        {
            nowTalk.answerEvent_A.Invoke();
        }

        if (nowNum == talkListNum - 1)
        {
            EndTalk();
        }
        else
        {
            nowNum = nowTalk.answer_A;
            nowTalk = talkList[nowNum];
            StartCoroutine(TalkTextDisPlay());
        }
       
    }

    public void ChoiceB()
    {
        choiceObj.SetActive(false);
        if (nowTalk.answerEvent_B.GetPersistentEventCount() > 0)
        {
            nowTalk.answerEvent_B.Invoke();
        }
        if (nowNum == talkListNum - 1)
        {
            EndTalk();
        }
        else
        {
            nowNum = nowTalk.answer_B;
            nowTalk = talkList[nowNum];
            StartCoroutine(TalkTextDisPlay());
        }    
            
    }

    private void EndTalk()
    {
        TalkInfo _talk = nowTalk;
        nowDialogueTM.text = "";
        name_TM.text = "";
        iconImage.sprite = null;


        talking = false;
        talkObj.SetActive(false);

        

        if(nowTalk.nextNum != -1 && talkScript.gameObject.activeInHierarchy)
        {
            talkScript.SetStartNum(nowTalk.nextNum);
        }
        nowTalk = null;
        talkScript = null;

        if (_talk.endEvent.GetPersistentEventCount() > 0)
        {
            _talk.endEvent.Invoke();
        }
        else
        {
            if(PlController_Field.Instance)
            PlController_Field.Instance.CanMoveOn();
        }

        

    }




    private IEnumerator TalkTextDisPlay()
    {
        //SetWindowPos();
        activeWindow = true;
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

        if(nowTalk.getItem != null)
        {
            Inventry.Instance.GetItem(nowTalk.getItem, nowTalk.getItemNum);
        }

        string _disPlayText = "";
        int _textLength = nowTalk.dialogue.Length;
        int _textNum = 0;
        int _speed = nowTalk.talkSpeedFlame;
        string _dialogue = nowTalk.dialogue;

        int _nowFlame;

        if(nowTalk.startEvent.GetPersistentEventCount() > 0)
        {
            nowTalk.startEvent.Invoke();
            yield return null;
        }

        while (_textNum < _textLength)
        {
            _nowFlame = 0;
            while(_nowFlame < _speed)
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
            AudioPlayer_SE.Instance.PlaySE(talkSE, 0.2f);
            nowDialogueTM.text = _disPlayText;
            yield return null;
        }

        if (nowTalk.autoNext)
        {
            NextTalk();
            yield break;
        }
        if (nowTalk.waitForNext)
        {
            yield break;
        }

        Debug.Log("“ü—Í‘Ò‚¿");
            waitForInput = true;
            arrow.SetActive(true);
        
    }

    private bool GetSpecialText(string _text)
    {
        return _text == "*";
    }
}
