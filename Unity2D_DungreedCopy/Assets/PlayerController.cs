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

    private void Awake()
    {
        movement        = GetComponent<Movement2D>();
        spriteRenderer  = GetComponent<SpriteRenderer>();
        ChangeState(PlayerState.Idle);
    }

    private void Update()
    {
        UpdateMove();
        UpdateJump();
        UpdateSight();
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
        Vector3 target = Camera.main.ScreenToWorldPoint(mousPos);

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
}
