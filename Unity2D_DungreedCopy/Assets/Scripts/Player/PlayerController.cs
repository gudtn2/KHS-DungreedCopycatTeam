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
        ChangeStateAndAnimation(PlayerState.Idle);
    }

    private void Update()
    {
        UpdateMove();
        UpdateJump();
        UpdateSight();

        if(Input.GetKeyDown(KeyCode.Q))
        {
            ChangeStateAndAnimation(PlayerState.Die);
        }
    }

    //======================================================================================
    // YS: 플레이어 움직임
    //======================================================================================

    public void UpdateMove()
    {
        float x = Input.GetAxis("Horizontal");

        movement.MoveTo(x);
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
    public void ChangeStateAndAnimation(PlayerState newState)
    {
        playerState = newState;

        switch (newState)
        {
            case PlayerState.Idle:
                ani.SetBool("IsWalk", false);
                ani.SetBool("IsJump", false);
                break;
            case PlayerState.Walk:
                ani.SetBool("IsWalk", true);
                break;
            case PlayerState.Jump:
                ani.SetBool("IsJump", true);
                break;
            case PlayerState.Die:
                ani.SetBool("IsDie", true);
                break;

        }
    }
}
