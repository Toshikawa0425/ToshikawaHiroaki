using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaMove : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField]
    private bool onGround = true;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Direction direction;

    private bool running = false;

    [SerializeField]
    private float nowSpeed;
    [SerializeField]
    private float runSpeed;

    [SerializeField]
    private float _angle;

    [SerializeField]
    private Vector3 moveDirection;

    [SerializeField]
    private FootStep footStep;

    [SerializeField]
    private float footStepFlame = 0;

    [SerializeField]
    private ForwardChecker forwardChecker;

    public enum Direction
    {
        Forward,
        Back,
        Right,
        Left
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //SetAnimDirection(direction);
    }

    private void OnEnable()
    {
        SetAnimDirection(direction);
    }

    public void StopMove()
    {
        if (running)
        {
            anim.SetBool("Run", false);
            nowSpeed = 0;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            running = false;
        }
    }

    public Direction GetDirection()
    {
        return direction;
    }

    public void MoveChara(Vector3 _direction,float speed)
    {
        MoveDirection(_direction);
        Vector3 moveDirection = _direction.normalized; 

        //ï˚å¸ÇÃì¸óÕÇ™Ç†ÇÈÇ©Ç«Ç§Ç©
        if (moveDirection != Vector3.zero)
        {
            running = true;
            anim.SetBool("Run", true);


            //à⁄ìÆ
            Vector3 _velo = MoveVelocity(moveDirection,speed);
            _velo = AdjustVelocityToSlope(_velo);
            _velo = forwardChecker.ForwardCheck(_velo);
            rb.velocity = _velo;
            SetAnimDirection(direction);
        }
        else
        {
            footStepFlame = 0;
            StopMove();
        }

        
    }




    public void MoveDirection(Vector3 _direction)
    {
        if (_direction != Vector3.zero)
        {
            //ÉJÉÅÉâÇÃê≥ñ éÊìæ
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1));


            _angle = Vector3.SignedAngle(cameraForward, _direction.normalized, Vector3.up);



            if (Mathf.Abs(_angle) < 20f)
            {
                direction = Direction.Forward;
            }
            else if (Mathf.Abs(_angle) > 160f)
            {
                direction = Direction.Back;
            }
            else
            {
                if (_angle >= 20f)
                {
                    direction = Direction.Right;
                }
                else if (_angle <= -20f)
                {
                    direction = Direction.Left;
                }
            }
        }
    }

    public void SetAnimDirection(Direction _dir)
    {
        if(_dir != direction)
        {
            direction = _dir;
        }
        switch (_dir)
        {
            case Direction.Forward:
                anim.SetBool("Forward", true);
                anim.SetBool("Back", false);
                anim.SetBool("Right", false);
                anim.SetBool("Left", false);
                break;

            case Direction.Back:
                anim.SetBool("Forward", false);
                anim.SetBool("Back", true);
                anim.SetBool("Right", false);
                anim.SetBool("Left", false);
                break;

            case Direction.Right:
                anim.SetBool("Forward", false);
                anim.SetBool("Back", false);
                anim.SetBool("Right", true);
                anim.SetBool("Left", false);
                break;

            case Direction.Left:
                anim.SetBool("Forward", false);
                anim.SetBool("Back", false);
                anim.SetBool("Right", false);
                anim.SetBool("Left", true);
                break;
        }
    }

    //à⁄ìÆë¨ìx
    private Vector3 MoveVelocity(Vector3 moveDirection, float speed )
    {
        //Vector3 _velo = new Vector3(0, 0, 0);
        float targetSpeed = runSpeed * speed * transform.lossyScale.y;

        if (nowSpeed < targetSpeed)
        {
            nowSpeed += Time.fixedDeltaTime * runSpeed * 2;
        }
        else
        {
            nowSpeed = targetSpeed;
        }

        anim.SetFloat("WalkSpeed", nowSpeed * speed / runSpeed);

        if (footStep != null)
        {
            footStepFlame += nowSpeed * speed / runSpeed;


            if (footStepFlame >= 20)
            {
                footStep.SetFootStep();
                footStepFlame = 0;
            }
        }

        float _speed = nowSpeed;
        return moveDirection * _speed;
    }

    private Vector3 AdjustVelocityToSlope(Vector3 _velocity)
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 0.5f))
        {
            onGround = true;
            Quaternion slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            Vector3 adjustVelocity = slopeRotation * _velocity;

            if (adjustVelocity.y < 0)
            {
                return adjustVelocity;
            }
        }
        return _velocity;
    }

    
}
