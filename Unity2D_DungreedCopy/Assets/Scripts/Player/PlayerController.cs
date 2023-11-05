using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {Idle = 0 , Walk, Jump, Die }   // YS: 플레이어 상태 
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private KeyCode         jumpKey = KeyCode.Space;

    public PlayerState     playerState;

    private Movement2D      movement;
    private SpriteRenderer  spriteRenderer;
    private Animator        ani;
    

    private void Awake()
    {
        movement        = GetComponent<Movement2D>();
        spriteRenderer  = GetComponent<SpriteRenderer>();
        ani             = GetComponent<Animator>();
    }

    private void Start()
    {
        ChangeState(PlayerState.Idle);
    }

    private void Update()
    {
        UpdateMove();
        UpdateJump();
        UpdateSight();
        ChangeAnimation();
    }

    //======================================================================================
    // YS: 플레이어 움직임
    //======================================================================================

    public void UpdateMove()
    {
        float x = Input.GetAxis("Horizontal");

        movement.MoveTo(x);
        movement.isWalk = true;
    }
    public void UpdateJump()
    {
        if(Input.GetKeyDown(jumpKey))
        {
            bool isJump = movement.JumpTo();
        }
        else if (Input.GetKey(jumpKey))
        {
            movement.isLongJump = true;
        }
        else if (Input.GetKeyUp(jumpKey))
        {
            movement.isLongJump = false;
        }
    }
    public void UpdateSight()
    {
        Vector2 mousPos = Input.mousePosition;
        Vector2 target  = Camera.main.ScreenToWorldPoint(mousPos);

        if (target.x < transform.position.x)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
    }

    //======================================================================================
    // YS: 플레이어 상태 변경
    //======================================================================================

    public void ChangeState(PlayerState newState)
    {
        playerState = newState;
    }

    public void ChangeAnimation()
    {
        // 걷는 상태
        if(movement.rigidbody.velocity.x != 0)
        {
            ChangeState(PlayerState.Walk);
            ani.SetFloat("MoveSpeed", movement.rigidbody.velocity.x);
            movement.StartCoroutine("DustEffect");
        }
        // 점프 상태
        if(movement.isJump == true)
        {
            ChangeState(PlayerState.Jump);
            ani.SetBool("IsJump", true);
        }
        // 죽는 상태
        if(Input.GetKeyDown(KeyCode.Q))
        {
            ChangeState(PlayerState.Die);
            ani.SetBool("IsDie", true);
        }
        // 기본 상태
        if(movement.isGrounded == true)
        {
            ChangeState(PlayerState.Idle);
            ani.SetFloat("MoveSpeed", movement.rigidbody.velocity.x);
            ani.SetBool("IsJump", false);
        }
    }
}
