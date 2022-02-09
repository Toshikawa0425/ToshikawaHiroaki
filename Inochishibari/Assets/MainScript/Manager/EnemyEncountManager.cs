using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEncountManager : SingletonMonoBehaviour<EnemyEncountManager>
{
    /*
    [SerializeField]
    private BattleField nowBattleField = null;
    [SerializeField]
    private float encountProbability = 0;
    [SerializeField]
    private int stepCount = 0;
    [SerializeField]
    private int encountStep = 10;
    [SerializeField]
    private string battleSceneName = "";

    private int rateSum = 0;


    private void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
    }


    public void SetBattleField(BattleField _bf)
    {
        nowBattleField = _bf;
        encountProbability = nowBattleField.encountPercent;
        battleSceneName = nowBattleField.BattleSceneName;
    }

    public void ResetStep()
    {
        stepCount = 0;
    }

    public void StepCount()
    {
        if(nowBattleField == null)
        {
            return;
        }

        stepCount++;

        if(stepCount >= encountStep)
        {
            CheckEncount();
            ResetStep();
        }
    }

    private void CheckEncount()
    {
        Debug.Log("checkEncount");
        if(nowBattleField == null)
        {
            return;
        }


        float _num = Random.Range(0, 1.0f);

        if (_num >= encountProbability)
        {
            encountProbability += 0.05f;
            return;
        }

        encountProbability = nowBattleField.encountPercent;
        PlController_Field.Instance.CanMoveOff();
        BattleStart();
    }

    private void BattleStart()
    {

        int _charaNum = Random.Range(1,4);

        int _rateNum = nowBattleField.rateSum;


        do
        {
            int _random_rate = Random.Range(0, _rateNum);

            foreach(BattleField.EnemyInfo info in nowBattleField.enemyList)
            {
                if(_random_rate <= info.appearnceRate)
                {
                    _enemyCardList.Add(info.enemy.charaCard);
                    break;
                }
                else
                {
                    _random_rate -= info.appearnceRate;
                    continue;
                }
            }
        }
        while (_enemyCardList.Count < _charaNum);

        //GameManager.Instance.EncountBattle(_enemyCardList,battleSceneName);
    }
    */
}
