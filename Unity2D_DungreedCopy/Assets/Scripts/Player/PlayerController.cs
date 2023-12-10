using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState { Idle = 0, Walk, Jump, Die }   // YS: 플레이어 상태 
public class PlayerController : MonoBehaviour
{
    [Header("방향")]
    public float    lastMoveDirX;
    public Vector3  mousePos;

    [Header("피격")]
    [SerializeField]
    private bool    isHurt;
    [SerializeField]
    private float   hurtRoutineDuration = 3f;
    [SerializeField]
    private float   blinkDuration = 0.5f;
    private Color   halfA = new Color(1,1,1,0.5f);
    private Color   fullA = new Color(1,1,1,1);

    [SerializeField]
    private KeyCode jumpKey = KeyCode.Space;
    [SerializeField]
    private KeyCode dashKey = KeyCode.Mouse1;

    public PlayerState playerState;

    private Movement2D      movement;
    private Animator        ani;
    private SpriteRenderer  spriteRenderer;
    private PlayerStats     playerStats;

    [SerializeField]
    private PlayerStatsUIManager    UIManager;
    
    private void Awake()
    {
        movement        = GetComponent<Movement2D>();
        ani             = GetComponent<Animator>();
        spriteRenderer  = GetComponent<SpriteRenderer>();
        playerStats     = GetComponent<PlayerStats>();
    }

    private void Start()
    {
        ChangeState(PlayerState.Idle);
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        UpdateMove();
        UpdateJump();
        UpdateSight();
        ChangeAnimation();
        UpdateDash();

        if (movement.isDashing) return;

        
    }


    //======================================================================================
    // YS: 플레이어 움직임
    //======================================================================================

    public void UpdateMove()
    {
        float x = Input.GetAxis("Horizontal");

        if (x != 0)
        {
            lastMoveDirX = Mathf.Sign(x);
        }

        movement.MoveTo(x);
        movement.isWalk = true;
    }

    public void UpdateJump()
    {
        if (Input.GetKeyDown(jumpKey))
        {
            bool isJump = movement.JumpTo();
            if(movement.isGrounded == true)
            {
                movement.ActiveJumpDustEffect();
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
        if (mousePos.x < transform.position.x)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void UpdateDash()
    {
        if (Input.GetKeyDown(dashKey) && movement.isDashing == false)
        {
            movement.PlayDash();
        }
    }
    //======================================================================================
    // YS: 플레이어 움직임 제외한 기능
    //======================================================================================
    public void TakeDamage(float mon_Att)
    {
        bool isDie = playerStats.DecreaseHP(mon_Att);

        if (isDie == true)
        {
            Debug.Log("GameOver");
        }
        else
        {
            if(!isHurt)
            {
                isHurt = true;
            }
        }
    }
    private IEnumerator HurtRoutine()
    {
        yield return new WaitForSeconds(hurtRoutineDuration);
        isHurt = false;
    }
    private IEnumerator BlinkPlayer()
    {
        while(isHurt)
        {
            yield return new WaitForSeconds(blinkDuration);
            spriteRenderer.color = halfA;
            yield return new WaitForSeconds(blinkDuration);
            spriteRenderer.color = fullA;
        }
    }
    //======================================================================================
    // YS: 플레이어 Collider
    //======================================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Monster" && !isHurt)
        {
            TakeDamage(20f);
            StartCoroutine(HurtRoutine());
            StartCoroutine(BlinkPlayer());
        }
        else if(collision.gameObject.tag == "ItemFairy" && playerStats.HP<playerStats.MaxHP)
        {
            collision.GetComponent<ItemBase>().Use(this.gameObject);
        }
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
        if (movement.rigidbody.velocity.x != 0)
        {
            ChangeState(PlayerState.Walk);
            ani.SetFloat("MoveSpeed", movement.rigidbody.velocity.x);
        }
        // 점프 상태
        if (movement.isJump == true)
        {
            ChangeState(PlayerState.Jump);
            ani.SetBool("IsJump", true);
        }
        // 죽는 상태
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeState(PlayerState.Die);
            ani.SetBool("IsDie", true);
        }
        // 기본 상태
        if (movement.isGrounded == true && movement.rigidbody.velocity.x == 0)
        {
            ChangeState(PlayerState.Idle);
            ani.SetFloat("MoveSpeed", movement.rigidbody.velocity.x);
        }
        if (movement.isGrounded == true)
        {
            ani.SetBool("IsJump", false);
        }
    }
}
