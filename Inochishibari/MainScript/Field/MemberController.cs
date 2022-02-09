using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemberController : MonoBehaviour
{
    public CharaMove charaMove;
    private Rigidbody rb;
    [SerializeField]
    private Transform followTransform;

    [SerializeField]
    private Vector3 targetPos;

    /*
    [SerializeField]
    private PlController_Field player;
    */

    [SerializeField]
    private Animator anim;
    /*
    [SerializeField]
    private Transform cameraTransform;
    */

    //private CharaMove.Direction direction;


    public bool canMove = true;

    /*
    [SerializeField]
    private float nowSpeed;
    [SerializeField]
    private float runSpeed;
    */

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        charaMove = GetComponent<CharaMove>();
    }



    private void FixedUpdate()
    {
        if (canMove)
            MoveChara();
    }


    public void UpdateTargetPos(Vector3 _pos)
    {
        targetPos = _pos;
    }


    private void MoveChara()
    {
        float dis_toFollow = Vector3.Distance(transform.position, followTransform.position);
        float dis_toTarget = Vector3.Distance(transform.position, targetPos);

        if(dis_toFollow > 20)
        {
            Debug.Log("èuä‘à⁄ìÆ");
            transform.position = targetPos;
            return;
        }

        if(dis_toTarget <= 0.25f)
        {
            dis_toTarget = 0;
        }

        Vector3 _direction =Vector3.Scale(targetPos - transform.position, new Vector3(1, 0, 1)) * dis_toTarget;

        float speed = 1.0f;

        if (PlController_Field.Instance.isDashing)
        {
            speed = 1.3f;
        }

        charaMove.MoveChara(_direction, speed) ;

    }

    public void CanMoveOn()
    {
        charaMove.StopMove();
        canMove = true;
    }

    public void CanMoveOff()
    {
        charaMove.StopMove();
        canMove = false;
    }

}
