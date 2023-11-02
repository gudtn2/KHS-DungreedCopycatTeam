using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private KeyCode         jumpKey = KeyCode.Space;
    
    private Movement2D      movement;
    private SpriteRenderer  spriteRenderer; 

    private void Awake()
    {
        movement        = GetComponent<Movement2D>();
        spriteRenderer  = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        UpdateMove();
        UpdateJump();
        UpdateSight();
    }

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

            if(isJump == true)
            {
                // 점프 성공시 To Do
            }
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

        if (mousPos.x < transform.position.x)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
    }
}
