using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharaAnim : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprites_Idle sprites_Idle;
    [SerializeField]
    private Sprites_Run sprites_Run;

    private int runNum = 0;
    private int maxNum = 0;

    [SerializeField]
    private PlayerCharactor playerCharactor = null;

    public enum Direction
    {
        Forward,
        Back,
        Right,
        Left
    }

    [System.Serializable]
    public class Sprites_Idle
    {
        public Sprite forward;
        public Sprite back;
        public Sprite right;
        public Sprite left;
    }

    [System.Serializable]
    public class Sprites_Run
    {
        public Sprite[] forward;
        public Sprite[] back;
        public Sprite[] right;
        public Sprite[] left;
    }

    private void Start()
    {
        if(playerCharactor != null)
        {
            SetAnims(playerCharactor.Sprites_Idle, playerCharactor.Sprites_Run);
        }
    }

    public void SetAnimFromPlayerChara(PlayerCharactor _chara)
    {
        Debug.Log("アニメーションチェンジ");
        sprites_Idle = _chara.Sprites_Idle;
        sprites_Run = _chara.Sprites_Run;
    }

    public void SetAnims(Sprites_Idle _idle, Sprites_Run _run)
    {
        sprites_Idle = _idle;
        sprites_Run = _run;
    }

    public void Anim_Idle(Direction _direction)
    {
        runNum = 0;
        switch (_direction)
        {
            case Direction.Forward:
                spriteRenderer.sprite = sprites_Idle.forward;
                break;

            case Direction.Back:
                spriteRenderer.sprite = sprites_Idle.back;
                break;

            case Direction.Right:
                spriteRenderer.sprite = sprites_Idle.right;
                break;

            case Direction.Left:
                spriteRenderer.sprite = sprites_Idle.left;
                break;
        }
    }

    public void Anim_Run(Direction _direction)
    {
        runNum++;
        switch (_direction)
        {
            case Direction.Forward:
                if (sprites_Run.forward.Length > 0)
                {
                    maxNum = sprites_Run.forward.Length;

                    runNum %= maxNum;

                    spriteRenderer.sprite = sprites_Run.forward[runNum];
                }
                break;

            case Direction.Back:
                if (sprites_Run.back.Length > 0)
                {
                    maxNum = sprites_Run.back.Length;
                    runNum %= maxNum;

                    spriteRenderer.sprite = sprites_Run.back[runNum];
                }
                break;

            case Direction.Right:
                if (sprites_Run.right.Length > 0)
                {
                    maxNum = sprites_Run.right.Length;
                    runNum %= maxNum;

                    spriteRenderer.sprite = sprites_Run.right[runNum];
                }
                break;

            case Direction.Left:
                if (sprites_Run.left.Length > 0)
                {
                    maxNum = sprites_Run.left.Length;
                    runNum %= maxNum;

                    spriteRenderer.sprite = sprites_Run.left[runNum];
                }
                break;
        }
    }

    public void ContinueAnim()
    {
        return;
    }
}
