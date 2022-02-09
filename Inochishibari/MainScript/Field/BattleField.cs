using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleField : MonoBehaviour
{
    /*
    public string BattleSceneName = "";
    public float encountPercent = 0.05f;

    public List<EnemyInfo> enemyList = new List<EnemyInfo>();

    public int rateSum = 0;

    private void Awake()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
    }

    private void Start()
    {
        RateSumCalcurate();
    }

    public void SetBattleField()
    {
        EnemyEncountManager.Instance.SetBattleField(this);
    }

    [System.Serializable]
    public class EnemyInfo
    {
        public EnemyCharactor enemy;
        public int appearnceRate;
    }

    private void SortEnemyList()
    {
        enemyList.Sort((a, b) => (b.appearnceRate - a.appearnceRate));
    }

    public void AddEnemy(EnemyCharactor _enemy, int _rate)
    {
        EnemyInfo _enemyInfo = new EnemyInfo();
        _enemyInfo.enemy = _enemy;
        _enemyInfo.appearnceRate = _rate;

        RateSumCalcurate();
    }

    private void RateSumCalcurate()
    {
        SortEnemyList();
        rateSum = 0;
        foreach(EnemyInfo info in enemyList)
        {
            rateSum += info.appearnceRate;
        }
    }
    */
}
