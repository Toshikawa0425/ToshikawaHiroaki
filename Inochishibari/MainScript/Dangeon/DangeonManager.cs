using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangeonManager : SingletonMonoBehaviour<DangeonManager>
{
    [SerializeField]
    private int lineUnitNum = 5;
    [SerializeField]
    private Transform[] lineTransforms;
    [SerializeField]
    private PlayBGMEvent bgmEV;
    public PointPos nowPos;

    [SerializeField]
    private PointPos bossPos;

    [SerializeField]
    private bool canPose = false;

    private Animator pieceAnim;

    [SerializeField]
    private DangeonPoint nowCard;
    [SerializeField]
    private List<DangeonPoint> nextCardList = new List<DangeonPoint>();

    public EventCardBases eventCardBases;

    [SerializeField]
    private string deckMakeSceneName = "DeckMakeScene";

    [SerializeField]
    private GameObject[] cameras;

    private List<CardBase> getCardList = new List<CardBase>();

    [SerializeField]
    private EndDangeonParam endDangeonParam;

    [SerializeField]
    private AudioClip SE_CardOpen;
    [SerializeField]
    private AudioClip SE_CardSet;

    [SerializeField]
    private GameObject tutorial_E;
    [SerializeField]
    private GameObject tutorial_DeckMake;
    [SerializeField]
    private GameObject tutorial_Rest;
    [SerializeField]
    private GameObject tutorial_GetCard;
    [SerializeField]
    private GameObject tutorial_LevelUp;

    [System.Serializable]
    private class EndDangeonParam
    {
        public string endSceneName = "";
        public bool endSceneIsChange = false;
        public int endEvNum = 0;
    }
    

    [System.Serializable]
    public class PointPos
    {
        public int cardNum;
        public int lineNum;
    }

    [System.Serializable]
    public class EventCardBases
    {
        public CardBase NoEvent;
        public CardBase Battle;
        public CardBase Rest;
        public CardBase GetCard;
        public CardBase LevelUp;
        public CardBase Boss;
        public CardBase Start;
        public CardBase End;
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.B) && Input.GetKey(KeyCode.S))
        {
            SetPoint(bossPos.cardNum, bossPos.lineNum, true);
        }
    }


    private void FixedUpdate()
    {
        if (canPose)
        {
            tutorial_E.SetActive(true);
            if (InputSetting.Instance.Check)
            {
                
                Debug.Log("OpenDeckMakeScene");
                OpenDeckMakeScene();
                canPose = false;
            }
            
        }
        else
        {
            tutorial_E.SetActive(false);
        }
    }


    public void OpenGetCardScene()
    {
        tutorial_GetCard.SetActive(true);
        Debug.Log("OpenDeck");
        ButtonOff();
        SceneEventManager.Instance.AddScene("GetCardScene", true);
    }

    public void OpenRestScene()
    {
        tutorial_Rest.SetActive(true);
        Debug.Log("Rest");
        ButtonOff();
        SceneEventManager.Instance.AddScene("RestScene", true);
    }

    public void OpenLVUPScene()
    {
        tutorial_LevelUp.SetActive(true);
        Debug.Log("Rest");
        ButtonOff();
        SceneEventManager.Instance.AddScene("LevelUpScene", true);
    }

    public void EndDangeon()
    {
        DisplayManager.Instance.GamenClose(1.0f);
        if (endDangeonParam.endSceneIsChange)
        {
            SceneEventManager.Instance.ChangeScene(endDangeonParam.endSceneName, endDangeonParam.endEvNum);
        }
        else
        {
            SceneEventManager.Instance.LoadScene(endDangeonParam.endSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single, endDangeonParam.endEvNum);
        }
    }

    public void InitState()
    {
        bgmEV.StartEvent();
        if (GameManager.Instance.currentDangeonPointPos.cardNum != 0 && GameManager.Instance.currentDangeonPointPos.lineNum != 0)
        {
            nowPos = GameManager.Instance.currentDangeonPointPos;
        }
        FirstSelect();
    }

    public void FirstSelect()
    {
        TableSceneManager.Instance.TableCamNoActive();
        DangeonPoint _point = lineTransforms[nowPos.lineNum].GetChild(nowPos.cardNum).GetComponent<DangeonPoint>();
        cameras[nowPos.lineNum].SetActive(true);

        _point.cardNum = nowPos.cardNum;
        _point.SetCard();
        //_point.ButtonOn();
        _point.RotateCard();

        SetPoint(nowPos.cardNum, nowPos.lineNum, false);
    }

    private void OpenDeckMakeScene()
    {
        tutorial_DeckMake.SetActive(true);
        Debug.Log("OpenDeck");
        ButtonOff();
        SceneEventManager.Instance.AddScene(deckMakeSceneName, true);
    }

    public void SetPoint(int _cardNum, int _lineNum, bool _playEv)
    {
        canPose = false;
        nowPos.cardNum = _cardNum;
        nowPos.lineNum = _lineNum;

        DangeonPoint _card = lineTransforms[nowPos.lineNum].GetChild(nowPos.cardNum).GetComponent<DangeonPoint>();
        ButtonManager.Instance.ResetLastButton();
        if (nowCard != null)
        {
            nowCard.RotateCard();
        }
        //現在のカード更新
        nowCard = _card;
        //Vector3 _pos = nowCard.transform.position;
        //playerPiece.transform.position = new Vector3(_pos.x,playerPiece.transform.position.y,_pos.z);

        //nowCard.SetCard();

        if (nextCardList.Count > 0)
        {
            //選ばれたカード以外を裏返す
            for (int i = 0; i < nextCardList.Count; i++)
            {
                if (nextCardList[i].gameObject.activeInHierarchy)
                {
                    nextCardList[i].ButtonOff();
                    if (nextCardList[i] != nowCard)
                    {
                        nextCardList[i].RotateCard();
                    }
                }
            }

            //nextCardListを初期化
            nextCardList = new List<DangeonPoint>();
        }

        if (_playEv)
        {
            switch (nowCard.eventType)
            {
                default:
                    nowCard.PlayEvent();
                    break;

                case DangeonPoint.EventType.NoEvent:
                    OpenNextCards();
                    break;
            }
        }
        else
        {
            OpenNextCards();
        }
    }

    public void SelectCard(int _num)
    {
        
        if (nowPos.lineNum >= 0 && nowPos.lineNum + 1 < cameras.Length)
        {
            cameras[nowPos.lineNum].SetActive(false);
            cameras[nowPos.lineNum + 1].SetActive(true);
        }

        SetPoint(_num, nowPos.lineNum + 1, true) ;

        PlaySE_Set();
    }

    public void OpenNextCards()
    {
        tutorial_DeckMake.SetActive(false);
        tutorial_GetCard.SetActive(false);
        tutorial_LevelUp.SetActive(false);
        tutorial_Rest.SetActive(false);

        if (nowPos.lineNum + 1 == lineTransforms.Length)
        {
            canPose = true;
            return;
        }
        //次のラインを取得
        Transform _nextLine = lineTransforms[nowPos.lineNum + 1];


        //次の候補カードをオープン
        for (int j = -1; j <= 1; j++)
        {
            if ((nowPos.cardNum + j >= 0) && (nowPos.cardNum + j < lineUnitNum))
            {
                DangeonPoint _next = _nextLine.GetChild(nowPos.cardNum + j).GetComponent<DangeonPoint>();

                if (_next.gameObject.activeInHierarchy)
                {
                    _next.cardNum = nowPos.cardNum + j;
                    _next.SetCard();
                    _next.ButtonOn();
                    _next.RotateCard();
                    PlaySE_Open();
                }
                nextCardList.Add(_next);
            }
        }

        SelectFromNextCardList();
    }

    public void ButtonOff()
    {
        for (int i = 0; i < nextCardList.Count; i++)
        {
            if (nextCardList[i].gameObject.activeInHierarchy)
            {
                nextCardList[i].ButtonOff();
            }
        }
    }

    public void ButtonOn()
    {
        for (int i = 0; i < nextCardList.Count; i++)
        {
            if (nextCardList[i].gameObject.activeInHierarchy)
            {
                nextCardList[i].ButtonOn();
            }
        }
    }

    public void SelectFromNextCardList()
    {
        tutorial_DeckMake.SetActive(false);
        canPose = true;
        if (nextCardList[1].gameObject.activeInHierarchy)
        {
            nextCardList[1].SelectThisCard();
            return;
        }

        if (nextCardList[0].gameObject.activeInHierarchy)
        {
            nextCardList[0].SelectThisCard();
            return;
        }

        if (nextCardList[2].gameObject.activeInHierarchy)
        {
            nextCardList[2].SelectThisCard();
            return;
        }
    }

    public void BattleStart()
    {
        GameManager.Instance.ReadyBattle();
    }

    public void SetGetCardList(List<CardBase> _cardList)
    {
        getCardList = _cardList;
    }

    public List<CardBase> GetCardList()
    {
        return getCardList;
    }

    public void PlaySE_Open()
    {
        AudioPlayer_SE.Instance.PlaySE(SE_CardOpen, 0.2f);
    }

    public void PlaySE_Set()
    {
        AudioPlayer_SE.Instance.PlaySE(SE_CardSet, 0.2f);
    }
}
