using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : SingletonMonoBehaviour<EventManager>
{
    public List<EventBase> NextEv = new List<EventBase>();

    private int eventNum;
    private UnityEvent endEvents;

    public void AfterTalk()
    {
        if(NextEv != null)
        {
            //PlController_Field.Instance.canMove = false;

            if (NextEv.Count > 0)
            {
                foreach (EventBase _ev in NextEv)
                {
                    _ev.StartEvent();
                }
                NextEv = new List<EventBase>();
            }
            else
            {
                if (PlController_Field.Instance)
                    PlController_Field.Instance.canMove = true;
            }
            
        }
        else
        {
            if (PlController_Field.Instance)
                PlController_Field.Instance.canMove = true;
        }
    }

    public void StartMoveEvent(MoveEvent moveEventBase)
    {
        PlController_Field.Instance.CanMoveOff();
        List<MoveEvent.MoveInfo> moveEvents = moveEventBase.moveEvents;
        eventNum = moveEvents.Count;
        endEvents = moveEventBase.endEvents;

        for (int i = 0; i < eventNum; i++)
        {
            switch (moveEvents[i].charaType)
            {
                default:
                    break;
                case MoveEvent.CharaType.Player:
                    moveEvents[i].chara = PlController_Field.Instance.charaMove;
                    PlController_Field.Instance.NotFollow();
                    break;
                case MoveEvent.CharaType.Follow1:
                    moveEvents[i].chara = PlController_Field.Instance.memberController_1.charaMove;
                    PlController_Field.Instance.NotFollow(1);
                    break;
                case MoveEvent.CharaType.Follow2:
                    moveEvents[i].chara = PlController_Field.Instance.memberController_2.charaMove;
                    PlController_Field.Instance.NotFollow(2);
                    break;
            }
            StartCoroutine(CharaMovesCoroutine(moveEvents[i]));
        }
    }

    public void EndMove()
    {
        if (eventNum <= 0)
        {
            if (endEvents.GetPersistentEventCount() == 0)
            {
                PlController_Field.Instance.CanMoveOn();
                PlController_Field.Instance.AllowFollow();
            }
            else
            {
                endEvents.Invoke();
                endEvents = null;
                eventNum = 0;
            }
        }
    }


    private IEnumerator CharaMovesCoroutine(MoveEvent.MoveInfo _moveInfo)
    {
        CharaMove _chara = _moveInfo.chara;

        for (int i = 0; i < _moveInfo.movePoses.Length; i++)
        {
            Debug.Log(i);
            Vector3 _pos = _chara.transform.position;

            Vector3 _targetPos = _moveInfo.movePoses[i].position;
            Vector3 _dire = Vector3.Scale(_targetPos - _pos, new Vector3(1, 0, 1));
            float _dis = _dire.magnitude;
            float _speed = _moveInfo.speed[i];

            while (_dis >= 1.0f)
            {
                _chara.MoveChara(_dire,_speed);
                _pos = _chara.transform.position;
                _dire = Vector3.Scale(_targetPos - _pos, new Vector3(1, 0, 1));
                _dis = _dire.magnitude;
                yield return null;
            }
            yield return null;
        }

        _chara.StopMove();
        eventNum--;
        EndMove();
    }

}
