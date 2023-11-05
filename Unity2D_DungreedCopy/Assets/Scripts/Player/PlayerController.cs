using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {Idle = 0 , Walk, Jump, Die }   // YS: 플레이어 상태 
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private KeyCode         jumpKey = KeyCode.Space;

    public PlayerState     playerState;

    [Header("먼지 이펙트")]
    [SerializeField]
    private GameObject      effectDust;
    [SerializeField]
    private Transform       parent;
    [SerializeField]
    private float           delayTime;
    [SerializeField]
    private bool            isSpawning = false; // 생성 중인지 여부


    [Header("방향")]
    [SerializeField]
    public float            lastMoveDir;

    private Movement2D      movement;
    private Animator        ani;
    private PoolManager     poolManager;
    

    private void Awake()
    {
        movement        = GetComponent<Movement2D>();
        ani             = GetComponent<Animator>();
        poolManager     = new PoolManager(effectDust);
    }

    private void OnApplicationQuit()
    {
        poolManager.DestroyObjcts();
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

        if(!isSpawning )
        {
            StartCoroutine("UpdateDustEffect");
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
            lastMoveDir = Mathf.Sign(x);
        }

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
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public IEnumerator UpdateDustEffect()
    {
        isSpawning = true;
        while(movement.rigidbody.velocity.x != 0 && movement.rigidbody.velocity.y == 0)
        {
            GameObject dustEffect = poolManager.ActivePoolItem();
            dustEffect.transform.position = parent.position;
            dustEffect.transform.SetParent(parent);
            dustEffect.GetComponent<PlayerDustEffect>().Setup(poolManager);
            yield return new WaitForSeconds(delayTime);
        }
        isSpawning = false;
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
        if(movement.isGrounded == true && movement.rigidbody.velocity.x ==0)
        {
            ChangeState(PlayerState.Idle);
            ani.SetFloat("MoveSpeed", movement.rigidbody.velocity.x);
        }
        if(movement.isGrounded == true)
        {
            ani.SetBool("IsJump", false);
        }
    }
}
