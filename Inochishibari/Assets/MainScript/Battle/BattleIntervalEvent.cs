using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleIntervalEvent : SingletonMonoBehaviour<BattleIntervalEvent>
{
    [SerializeField]
    private List<IntervalEvent> eventList = new List<IntervalEvent>();
    public IntervalEvent nextEvent = null;

    private int evNum = 0;

    [System.Serializable]
    public class IntervalEvent
    {
        public string evName;
        public int turnNum;
        public UnityEvent intervalEv;
        public BattleManager.Phase playPhase;
    }

    private void Start()
    {
        if(eventList.Count > 0)
        {
            SetNextEvent();
        }
    }

    public bool GetFlagOfEvent(BattleManager.Phase _phase)
    {
        if(nextEvent == null)
        {
            Debug.Log("evNull");
            return false;
        }

        if(nextEvent.turnNum == BattleManager.Instance.turnNum && nextEvent.playPhase == _phase)
        {
            Debug.Log("evTrue" + _phase.ToString());
            return true;
        }
        else
        {
            Debug.Log("evFalse" + _phase.ToString());
            return false;
        }
    }

    public void PlayEvent()
    {
        Debug.Log("PlayEV");
        Debug.Log(nextEvent.evName);
        nextEvent.intervalEv.Invoke();
        evNum++;

        SetNextEvent();
    }

    public void SetNextEvent()
    {
        if (evNum < eventList.Count)
        {
            nextEvent = eventList[evNum];
        }
        else
        {
            nextEvent = null;
        }
    }
}
