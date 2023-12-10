using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement2D : MonoBehaviour
{

    [Header("MoveX,Jump")]
    [SerializeField]
    private float           moveSpeed = 3.0f;
    [SerializeField]        
    private float           jumpForce = 8.0f;
    [SerializeField]        
    private float           lowGravity = 1.0f;  // 점프키를 오래 누르고 있을때 적용되는 낮은 중력
    [SerializeField]        
    private float           highGravity = 1.5f; // 일반적으로 적용되는 점프 
    [HideInInspector]       
    public bool             isJump = false;     // Jump상태 채크
    [HideInInspector]       
    public bool             isWalk = false;     // Walk상태 채크

    [Header("DoubleJump")]
    public bool             haveDoubleJump;
    [SerializeField]
    private int             haveDoubleJump_MaxJumpCount = 2;
    [SerializeField]
    private int             normalState_MaxJumpCount = 1;
    [SerializeField]
    private int             curJumpCount;

    [Header("Checking Ground")]
    [SerializeField]
    private LayerMask       collisionLayer;
    [HideInInspector]
    public bool             isGrounded;
    private Vector3         footPos;
    
    [Header("Dash")]
    [SerializeField]
    public bool             isDashing = false;
    public float            dashDis = 3.0f;
    [SerializeField]
    private float           dashSpeed = 20.0f;
    public float            ghostDelay;
    [SerializeField]
    private float           ghostDelaySeconds = 1.0f;
    [SerializeField]
    private GameObject      dashPrefab;
    [SerializeField]
    public Vector3          dashDir;
    private PoolManager     dashPoolManager;
    
    [Header("Dash Count")]
    public int              maxDashCount = 3;
    public int              curDashCount;
    public float            dashCountChargeDelayTime = 5.0f;

    [Header("DustEffect")]
    private PoolManager     dustPoolManager;
    [SerializeField]
    private GameObject      dustPrefab;
    [SerializeField]
    private bool            isSpawningDust = false;
    
    [Header("JumpEffect")]
    private PoolManager     jumpDustPoolManager;
    [SerializeField]
    private GameObject      jumpDustPrefab;

    [Header("DoubleJumpEffect")]
    private PoolManager     doubleJumpDustPoolManager;
    [SerializeField]
    private GameObject      doubleJumpDustPrefab;


    public bool             isLongJump { set; get; } = false;

    [HideInInspector]
    public Rigidbody2D              rigidbody;
    private BoxCollider2D           boxCollider2D;
    private PlayerStats             playerStats;


    private void Awake()
    {
        rigidbody           = GetComponent<Rigidbody2D>();
        boxCollider2D       = GetComponent<BoxCollider2D>();
        playerStats         = GetComponent<PlayerStats>();

        dashPoolManager             = new PoolManager(dashPrefab);
        dustPoolManager             = new PoolManager(dustPrefab);
        jumpDustPoolManager         = new PoolManager(jumpDustPrefab);
        doubleJumpDustPoolManager   = new PoolManager(doubleJumpDustPrefab);
    }
    private void Start()
    {
        ghostDelaySeconds   = ghostDelay;

        // YS: Dash변수 초기화
        curDashCount        = maxDashCount;
    }
    
    private void OnApplicationQuit()
    {
        dashPoolManager.DestroyObjcts();
        dustPoolManager.DestroyObjcts();
        jumpDustPoolManager.DestroyObjcts();
        doubleJumpDustPoolManager.DestroyObjcts();
    }
    private void FixedUpdate()
    {

        GroundCheckAndJumpType();

        // DashEffect Active
        if (isDashing)
        {
            ActiveDashEffect();
        }
        // DustEffect Active
        if (!isSpawningDust)
        {
            StartCoroutine("ActiveDustEffect");
        }
    }
    public void MoveTo(float x)
    {
        rigidbody.velocity = new Vector2(x * moveSpeed, rigidbody.velocity.y);
    }

    public bool JumpTo()
    {
        if (curJumpCount > 0)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
            curJumpCount--;
            isJump = true;
            isWalk = false;

            if(haveDoubleJump == true && curJumpCount < 1 && Input.GetKeyDown(KeyCode.Space))
            {
                ActiveDoubleJumpDustEffect();
            }
            return true;
            
        }
        return false;
    }

    private void GroundCheckAndJumpType()
    {
        Bounds bounds = boxCollider2D.bounds;

        footPos = new Vector3(bounds.center.x, bounds.min.y);

        isGrounded = Physics2D.OverlapCircle(footPos, 0.02f, collisionLayer);

        if (isGrounded == true && rigidbody.velocity.y <= 0)
        {
            isJump = false;

            if (haveDoubleJump == true)
            {
                curJumpCount = haveDoubleJump_MaxJumpCount;
            }
            else if (haveDoubleJump == false)
            {
                curJumpCount = normalState_MaxJumpCount;
            }
        }

        if (isLongJump && rigidbody.velocity.y > 0)
        {
            rigidbody.gravityScale = lowGravity;
        }
        else
        {
            rigidbody.gravityScale = highGravity;
        }
    }
    public void PlayDash()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        dashDir = mousePos - transform.position;
        Vector3 moveTarget = transform.position + Vector3.ClampMagnitude(dashDir, dashDis);
        if(playerStats.DC > 0)
        {
            StartCoroutine(DashTo(moveTarget));
            playerStats.UseDC();
        }
    }
    private IEnumerator DashTo(Vector3 moveTarget)
    {
        isDashing = true;
        curDashCount--;

        float dis = Vector3.Distance(transform.position, moveTarget);
        float step = (dashSpeed / dis) * Time.fixedDeltaTime;
        float t = 0f;

        Vector3 startingPos = transform.position;

        while (t <= 1.0f)
        {
            t += step;
            rigidbody.MovePosition(Vector3.Lerp(startingPos, moveTarget, t));
            yield return new WaitForFixedUpdate();
        }
        playerStats.timer = 0;
        isDashing = false;
    }
    //=====================================================================
    // YS: Player Effect Active
    //=====================================================================
    private IEnumerator ActiveDustEffect()
    {
        isSpawningDust = true;
        while (rigidbody.velocity.x != 0 && rigidbody.velocity.y == 0)
        {
            GameObject dustEffect = dustPoolManager.ActivePoolItem();
            dustEffect.transform.position = transform.position + new Vector3(0,-0.25f,0);
            dustEffect.transform.rotation = transform.rotation;
            dustEffect.GetComponent<EffectPool>().Setup(dustPoolManager);
            yield return new WaitForSeconds(0.3f);
        }
        isSpawningDust = false;
    }
    public void ActiveJumpDustEffect()
    {
        GameObject jumpDustEffect = jumpDustPoolManager.ActivePoolItem();
        jumpDustEffect.transform.position = transform.position + new Vector3(0, -0.25f, 0);
        jumpDustEffect.transform.rotation = transform.rotation;
        jumpDustEffect.GetComponent<EffectPool>().Setup(jumpDustPoolManager);
    }
    public void ActiveDoubleJumpDustEffect()
    {
        GameObject doubleJumpDustEffect = doubleJumpDustPoolManager.ActivePoolItem();
        doubleJumpDustEffect.transform.position = transform.position + new Vector3(0, -0.25f, 0);
        doubleJumpDustEffect.transform.rotation = transform.rotation;
        doubleJumpDustEffect.GetComponent<EffectPool>().Setup(doubleJumpDustPoolManager);
    }
    private void ActiveDashEffect()
    {
        if (ghostDelaySeconds > 0)
        {
            ghostDelaySeconds -= Time.deltaTime;
        }
        else
        {
            GameObject ghostEffect = dashPoolManager.ActivePoolItem();
            ghostEffect.transform.position = transform.position;
            ghostEffect.transform.rotation = transform.rotation;
            ghostEffect.GetComponent<EffectPool>().Setup(dashPoolManager);
            Sprite curSprite = GetComponent<SpriteRenderer>().sprite;
            ghostEffect.GetComponent<SpriteRenderer>().sprite = curSprite;
            ghostDelaySeconds = ghostDelay;
        }
    }
    //=====================================================================
    // YS: Player Giamos
    //=====================================================================
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(footPos, 0.02f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dashDis);


    }
}
