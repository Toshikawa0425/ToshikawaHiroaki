using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveEvent : EventBase
{
    public List<MoveInfo> moveEvents;
    public UnityEvent endEvents;
    private int eventNum = 0;

    [System.Serializable]
    public class MoveInfo
    {
        public CharaType charaType;
        public CharaMove chara;
        public Transform[] movePoses;
        public float[] speed;
    }

    public enum CharaType
    {
        Party,
        Player,
        Follow1,
        Follow2,
        Other
    }

    public override void StartEvent()
    {
        EventManager.Instance.StartMoveEvent(this);
    }
}
