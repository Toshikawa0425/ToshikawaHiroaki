using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowManager : MonoBehaviour
{
    [SerializeField]
    private List<MemberController> memberControllerList = new List<MemberController>();
    [SerializeField]
    private int followerNum = 0;

    [SerializeField]
    private List<Vector3> memberPositions = new List<Vector3>();

    [SerializeField]
    private float separateDistance = 1.0f;


    public void SetMember(MemberController _member)
    {
        memberControllerList.Add(_member);
        followerNum++;
    }

    public void InitState(Vector3 _pos)
    {
        memberPositions.Add(_pos);
    }

    public void UpdatePositions(Vector3 leaderPos, bool _update = false)
    {
        float _dis = Vector3.Distance(leaderPos, memberPositions[0]);

        if (!_update)
        {
            if (_dis < separateDistance)
            {
                return;
            }
            else
            {
                if (memberPositions.Count <= followerNum)
                {
                    memberPositions.Add(Vector3.zero);
                }
            }
        }

        for(int i = memberPositions.Count - 1; i >= 1; i--)
        {
            memberPositions[i] = memberPositions[i - 1];
            memberControllerList[i - 1].UpdateTargetPos(memberPositions[i]);



            //cubes[i].position = memberPositions[i];
        }

        memberPositions[0] = leaderPos;
        //memberControllerList[0].UpdateTargetPos(memberPositions[0]);

        //cubes[0].position = memberPositions[0];
    }
}
