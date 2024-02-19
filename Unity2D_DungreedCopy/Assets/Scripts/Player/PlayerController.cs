using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState { Idle = 0, Walk, Jump, Die }   // YS: 플레이어 상태 
public class PlayerController : MonoBehaviour
{
    static public PlayerController instance;

    [Header("보스를 만났는지 여부")]
    public bool     playerMeetsBoss;
    [Header("보스가 죽었는지 여부")]
    public bool     isBossDie = false;
    [Header("보스가 완전히 죽었는지")]
    public bool     bossOpentheStele = false;

    public bool     onUI;   // 플레이어의 움직임을 제한하기 위한 bool값

    [Header("방향")]
    public float    lastMoveDirX;
    public Vector3  mousePos;

    [Header("피격")]
    [SerializeField]
    public bool    isHurt;
    [SerializeField]
    private float   hurtRoutineDuration = 3f;
    [SerializeField]
    private float   blinkDuration = 0.5f;
    private Color   halfA = new Color(1,1,1,0.5f);
    private Color   fullA = new Color(1,1,1,1);
    public bool     isDie;
    [SerializeField]
    private KeyCode jumpKey = KeyCode.Space;
    [SerializeField]
    private KeyCode dashKey = KeyCode.Mouse1;

    public PlayerState      playerState;

    [Header("현재 맵 이름")]
    public string           curSceneName;        
    public string           curDungeonName;        

    private Movement2D              movement;
    private Animator                ani;
    public  SpriteRenderer          spriteRenderer;
    private PlayerStats             playerStats;
    private BoxCollider2D           boxCollider2D;
    
    private DungeonPortalController dungeonPortalController;

    [SerializeField]
    private UIManager               UIManager;
    
    private void Awake()
    {
        if(instance == null)
        {
            // YS : 씬 변경시에도 플레이어 파괴되지 않도록
            DontDestroyOnLoad(gameObject);

            ChangeState(PlayerState.Idle);
            movement        = GetComponent<Movement2D>();
            ani             = GetComponent<Animator>();
            spriteRenderer  = GetComponent<SpriteRenderer>();
            playerStats     = GetComponent<PlayerStats>();
            boxCollider2D   = GetComponent<BoxCollider2D>();

            dungeonPortalController = FindObjectOfType<DungeonPortalController>();

            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        isDie = false;

        // 최초 시작지 이름을 마을로 설정
        curSceneName = "Village";
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        ChangeAnimation();

        if(!isDie && !dungeonPortalController.isCollideToPlayer && !onUI)
        {
            boxCollider2D.offset    = new Vector2(0, -0.1f);
            boxCollider2D.size      = new Vector2(0.8f, 1.1f);
            UpdateMove();
            UpdateJump();
            UpdateSight();
            UpdateDash();
        }
        else if(onUI)
        {
            movement.rigidbody.velocity = Vector2.zero;
        }
        else
        {
            boxCollider2D.offset    = new Vector2(0, 0);
            boxCollider2D.size      = new Vector2(1.2f,0.7f);
        }

        if (movement.isDashing) return;

        if(dungeonPortalController.isCollideToPlayer)
        {
            StartCoroutine("ChangePlayerAlpha");
            
            // YS: 플레이어 Ground에 빠지는 현상 및 이동하던 방향으로 계속 이동현상 수정 
            movement.rigidbody.velocity = new Vector2(0, 0);
            this.transform.position = new Vector2(transform.position.x, -6.334974f);
        }

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

            movement.rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        // YS: x축 입렵이 없을시 경사면에서 미끄러지는 현상 수정
        else
        {
            movement.rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }

        movement.MoveTo(x);
        movement.isWalk = true;
    }

    public void UpdateJump()
    {
        if (Input.GetKeyDown(jumpKey))
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
        else if(Input.GetKeyDown(KeyCode.C))
        {
            movement.DownJumpTo();
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
    public IEnumerator HurtRoutine()
    {
        yield return new WaitForSeconds(hurtRoutineDuration);
        isHurt = false;
    }
    public IEnumerator BlinkPlayer()
    {
        while(isHurt)
        {
            yield return new WaitForSeconds(blinkDuration);
            spriteRenderer.color = halfA;
            yield return new WaitForSeconds(blinkDuration);
            spriteRenderer.color = fullA;
        }
    }

    private IEnumerator ChangePlayerAlpha()
    {
        yield return new WaitForSeconds(0.8f);
        Color color = spriteRenderer.color;
        color.a = 0;
        spriteRenderer.color = color;
        StartCoroutine("BackPlayerAlpha");
    }
    private IEnumerator BackPlayerAlpha()
    {
        yield return new WaitForSeconds(2.2f);
        Color color = spriteRenderer.color;
        color.a = 1;
        spriteRenderer.color = color;
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
        else if(collision.gameObject.tag == "ItemFairy" && playerStats.HP < playerStats.MaxHP)
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
        if (isDie)
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
