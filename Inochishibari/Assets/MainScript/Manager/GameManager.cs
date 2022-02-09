using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    private GameObject playerObject;
    private GameObject playerObjInstance;
    public List<PlayerCharactor> playerCharas;
    [SerializeField]
    private List<int> playerNowHPs = new List<int>();

    [SerializeField]
    private List<CardBase> cardDeck = new List<CardBase>();

    public List<BattleManager.CardAndHP> playerCharaCards = new List<BattleManager.CardAndHP>();
    [SerializeField]
    private List<BattleManager.CardAndHP> enemyCharaCards = new List<BattleManager.CardAndHP>();
    public GameObject sceneObjs;
    [SerializeField]
    private string nowBattleScene = "";

    

    [SerializeField]
    private List<BattleTalk> battleTalkList = new List<BattleTalk>();

    [SerializeField]
    private string currentDangeonScene;
    public DangeonManager.PointPos currentDangeonPointPos = new DangeonManager.PointPos();

    public int earnedEXP = 0;
    public int getEXP = 0;

    public void SetLevelAndEXP(int _num, BattleManager.CardAndHP.LevelStates _states)
    {
        int _currentLevel = playerCharaCards[_num].levelStates.Level;
        playerCharaCards[_num].levelStates = _states;
        playerCharaCards[_num].maxHP = playerCharaCards[_num].charaCard.charaStates.defaultHP + Mathf.FloorToInt(_states.Level / 2.0f);

        if (_currentLevel != _states.Level)
        {
            playerCharaCards[_num].nowHP = playerCharaCards[_num].maxHP;
        }
    }

    public List<PlayerCharactor> GetPlCharas()
    {
        return playerCharas;
    }

    public List<CardBase> GetDeck()
    {
        return cardDeck;
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
    }


    public void SavePlayerDate(int posNum)
    {
        //SaveDateManager.Instance.PlayerDateSave(playerCharas, playerNowHPs, SceneManager.GetActiveScene().name, posNum);
    }

    public void LoadPlayerDate()
    {
        playerCharas = new List<PlayerCharactor>();
        playerNowHPs = new List<int>();

        SaveDateManager.PlayerDate _date = SaveDateManager.Instance.LoadPlayerDate();

        if (_date != null)
        {
            playerCharas = _date.playerList;
            playerNowHPs = _date.playerHPs;

            SceneEventManager.Instance.LoadScene(_date.currentSceneName, LoadSceneMode.Single, _date.posNum);
        }
        else
        {
            Debug.Log("プレイヤーデータなし");
            return;
        }
    }

    public void SetPlayerObject(Transform _setPosition, CharaMove.Direction _direction, Vector3 _scale, bool _canMove)
    {
        if (!PlayerObj.Instance)
        {
            playerObjInstance = Instantiate(playerObject);
        }
        else
        {
            playerObjInstance = PlayerObj.Instance.gameObject;
        }
        //_pl.transform.
        playerObjInstance.transform.GetChild(0).position = _setPosition.position;
        PlController_Field.Instance.SetCharactors(playerCharas, _direction);
        PlController_Field.Instance.SetScale(_scale);

        if (_canMove == true)
        {
            PlController_Field.Instance.CanMoveOn();
        }
        else
        {
            PlController_Field.Instance.CanMoveOff();
        }
    }

    public void SetPlayerCharas(List<PlayerCharactor> _playerCharas, List<int> _HPs,List<int> _levels)
    {
        playerCharas = _playerCharas;
        playerCharaCards = new List<BattleManager.CardAndHP>();
        for(int i = 0; i< _playerCharas.Count; i++)
        {
            int _max = _playerCharas[i].charaCard.charaStates.defaultHP + Mathf.FloorToInt(_levels[i] / 2.0f);
            if (_HPs[i] > _max)
            {
                _HPs[i] = _max;
            }

            BattleManager.CardAndHP _cah = new BattleManager.CardAndHP();
            _cah.charaCard = _playerCharas[i].charaCard;
            _cah.nowHP = _HPs[i];
            _cah.maxHP = _max;

            playerCharaCards.Add(_cah);
        }

        //playerNowHPs = _HPs;
    }

    public void NotActivePlayerObj()
    {
        if (playerObjInstance == null)
            return;

        playerObjInstance.SetActive(false);
    }

    public void ActivePlayerObj()
    {
        if (playerObjInstance == null)
            return;
        playerObjInstance.SetActive(true);
    }

    public void SetBattleSceneName(string _name)
    {
        nowBattleScene = _name;
    }

    /*
    public void EncountBattle(List<Card_Chara> _enChara, string _battleSceneName)
    {
        LoadBattleScene(_battleSceneName, _enChara, null);
    }
    */

    
    public void SetBattle(string _sceneName ,List<BattleManager.CardAndHP> _enemyCharas,List<CardBase> _enemyDeck, int _exp)
    {
        getEXP += _exp;
        currentDangeonScene = SceneManager.GetActiveScene().name;
        currentDangeonPointPos = DangeonManager.Instance.nowPos;
        //battleTalkList = _battleTalkList;
        enemyCharaCards = _enemyCharas;
        DeckManager.Instance.SetEnemyDeck(_enemyDeck);
        //EventManager.Instance.NextEv = _afterEvent;
        //PlController_Field.Instance.Encount(_sceneName);

        nowBattleScene = _sceneName;
    }

    public void ReadyBattle()
    {
        ButtonManager.Instance.ResetLastButton();
        DisplayManager.Instance.GamenClose(1.0f);
        Invoke("LoadBattleScene", 1f);
    }
    public void LoadBattleScene()
    {
        TableSceneManager.Instance.TableCamActive();
        SceneEventManager.Instance.ChangeScene(nowBattleScene, 0);
    }

    public void LoadDangeonScene()
    {
        TableSceneManager.Instance.TableCamActive();
        SceneEventManager.Instance.ChangeScene(currentDangeonScene, 0);
    }

    public void LoadTitleScene()
    {
        TableSceneManager.Instance.TableCamActive();
        SceneEventManager.Instance.ChangeScene("TitleDemo", 0);
    }

    public List<BattleManager.CardAndHP> GetPlayerParty()
    {
        return playerCharaCards;
    }

    public void SetStates_Player(List<BattleManager.CardAndHP> _plCards)
    {
        playerCharaCards = _plCards;
    }

    public void SetBattleCharas()
    {
        BattleManager.Instance.SetPlayerAndEnemyCharas(playerCharaCards, enemyCharaCards);
    }

    public void FinishBattleScene_WIN(List<BattleManager.CardAndHP> _plCards)
    {
        earnedEXP += getEXP;
        getEXP = 0;
        SetStates_Player(_plCards);
        ButtonManager.Instance.ResetLastButton();
        DisplayManager.Instance.GamenClose(1.0f);
        Invoke("LoadDangeonScene", 1f);
    }

    public void FinishBattleScene_LOSE()
    {
        currentDangeonPointPos.cardNum = 0;
        currentDangeonPointPos.lineNum = 0;
        earnedEXP = 0;
        getEXP = 0;
        ButtonManager.Instance.ResetLastButton();
        DisplayManager.Instance.GamenClose(1.0f);
        Invoke("LoadTitleScene", 1f);
    }

}
