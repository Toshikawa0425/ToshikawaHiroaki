using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlController_Field : SingletonMonoBehaviour<PlController_Field>
{
    public Transform parentTransform;
    public CharaMove charaMove;
    [SerializeField]
    private MoveCharaAnim anim_Pl1;
    [SerializeField]
    private MoveCharaAnim anim_Pl2;
    [SerializeField]
    private MoveCharaAnim anim_Pl3;

    [SerializeField]
    private FollowManager followManager;

    [SerializeField]
    private GameObject follower1;
    [SerializeField]
    private GameObject follower2;

    public MemberController memberController_1;
    public MemberController memberController_2;

    public bool isDashing = false;
    
    [SerializeField]
    private Transform cameraTransform;

    private Vector3 currentInputDirection = Vector3.zero;
    private Vector3 currentMoveDirection = Vector3.zero;

    [SerializeField]
    private GameObject encountFukidashi;
    [SerializeField]
    private GameObject encountCamera;

    //private Rigidbody rb;
    //public Direction direction;
    /*
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float nowSpeed;
    */
    public bool canMove = true;

    private FootStep footStep;

    [SerializeField]
    private bool forwardCheck = true;
    /*
    [SerializeField]
    private bool sprint = false;
    */
    [SerializeField]
    private Transform followPoses;

    private float walkFlame = 0;

    private new void  Awake()
    {
        //rb = GetComponent<Rigidbody>();
        charaMove = GetComponent<CharaMove>();
        footStep = GetComponent<FootStep>();
        follower1.SetActive(false);
        follower2.SetActive(false);
    }

    /*
    private void Start()
    {
        SetCharactors(GameManager.Instance.GetPlCharas());
    }
    */

    private void OnEnable()
    {
        encountFukidashi.SetActive(false);
        encountCamera.SetActive(false);
    }

    public void SetScale(Vector3 _scale)
    {
        transform.localScale = _scale;
        memberController_1.transform.localScale = _scale;
        memberController_2.transform.localScale = _scale;
    }

    public void SetCharactors(List<PlayerCharactor> _charas, CharaMove.Direction _direction)
    {
        //footStep.ResetFootSteps();
        for(int i = 0; i < _charas.Count; i++)
        {
            switch (i)
            {
                case 0:
                    anim_Pl1.SetAnims(_charas[0].Sprites_Idle,_charas[0].Sprites_Run);
                    charaMove.SetAnimDirection(_direction);
                    break;
                case 1:
                    anim_Pl2.SetAnims(_charas[1].Sprites_Idle, _charas[1].Sprites_Run);
                    Vector3 _pos1 = Vector3.zero;
                    switch (_direction)
                    {
                        case CharaMove.Direction.Forward:
                            _pos1 = transform.position - transform.forward*2;
                            break;
                        case CharaMove.Direction.Back:
                            _pos1 = transform.position + transform.forward*2;
                            break;
                        case CharaMove.Direction.Left:
                            _pos1 = transform.position + transform.right*2;
                            break;
                        case CharaMove.Direction.Right:
                            _pos1 = transform.position - transform.right*2;
                            break;
                    }
                    follower1.transform.position = _pos1;
                    follower1.SetActive(true);
                    follower1.GetComponent<CharaMove>().SetAnimDirection(_direction);

                    memberController_1.UpdateTargetPos(_pos1);

                    followManager.SetMember(memberController_1);
                    break;
                case 2:
                    anim_Pl3.SetAnims(_charas[2].Sprites_Idle, _charas[2].Sprites_Run);
                    Vector3 _pos2 = Vector3.zero ;
                    switch (_direction)
                    {
                        case CharaMove.Direction.Forward:
                            _pos2 = transform.position - transform.forward * 4;
                            break;
                        case CharaMove.Direction.Back:
                            _pos2 = transform.position + transform.forward * 4;
                            break;
                        case CharaMove.Direction.Left:
                            _pos2 = transform.position + transform.right * 4;
                            break;
                        case CharaMove.Direction.Right:
                            _pos2 = transform.position - transform.right * 4;
                            break;
                    }
                    follower2.transform.position = _pos2;
                    follower2.SetActive(true);
                    follower2.GetComponent<CharaMove>().SetAnimDirection(_direction);

                    memberController_2.UpdateTargetPos(_pos2);

                    followManager.SetMember(memberController_2);
                    break;
                default:
                    break;
            }

        }

        followManager.InitState(transform.position);
    }

    private void Update()
    {
        if (canMove)
        {
            Dash();
        }
    }


    private void FixedUpdate()
    {
        if (canMove)
        {
            MoveChara();
        }
    }

    public void CanMoveOff()
    {
        charaMove.StopMove();
        canMove = false;
    }

    public void CanMoveOn()
    {
        charaMove.StopMove();
        AllowFollow();
        canMove = true;
    }

    private void Dash()
    {
        isDashing = InputSetting.Instance.Dash;
    }

    private void MoveChara()
    {
        Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1));

        Vector3 inputDirection = new Vector3(InputSetting.Instance.Horizontal, 0, InputSetting.Instance.Vertical);

        Vector3 _direction = Vector3.zero;

        if (inputDirection != currentInputDirection)
        {
            currentInputDirection = inputDirection;
            _direction = Camera.main.transform.right * InputSetting.Instance.Horizontal + camForward * InputSetting.Instance.Vertical;
            currentMoveDirection = _direction;
        }
        else
        {
            
            _direction = currentMoveDirection;
        }

        followManager.UpdatePositions(transform.position);
        float speed = isDashing ? 1.3f : 1.0f;
        charaMove.MoveChara(_direction, speed);
            

        /*
        if (_direction != Vector3.zero)
        {
            walkFlame += speed;

            if (walkFlame >= 20)
            {
                EnemyEncountManager.Instance.StepCount();
                walkFlame = 0;
            }
        }
        */
    }

    public void NotFollow(int memberNum = 0)
    {
        switch (memberNum)
        {
            default:
                if(follower1.activeInHierarchy)
                memberController_1.CanMoveOff();
                if(follower2.activeInHierarchy)
                memberController_2.CanMoveOff();
                break;

            case 1:
                memberController_1.CanMoveOff();
                break;

            case 2:
                memberController_2.CanMoveOff();
                break;
        }
        
    }
    
    public void AllowFollow(int memberNum = 0)
    {
        switch (memberNum)
        {
            default:
                if(follower1.activeInHierarchy)
                memberController_1.CanMoveOn();
                if(follower2.activeInHierarchy)
                memberController_2.CanMoveOn();
                break;

            case 1:
                memberController_1.CanMoveOn();
                break;

            case 2:
                memberController_2.CanMoveOn();
                break;
        }
        
    }

    public void Encount(string _sceneName)
    {
        StartCoroutine(EncountCoroutine(_sceneName));
    }

    private IEnumerator EncountCoroutine(string _sceneName)
    {
        encountFukidashi.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        CameraController.Instance.SetBlendTime(0.2f);
        encountCamera.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        SceneEventManager.Instance.LoadScene(_sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive, 0, true);
    }
}
