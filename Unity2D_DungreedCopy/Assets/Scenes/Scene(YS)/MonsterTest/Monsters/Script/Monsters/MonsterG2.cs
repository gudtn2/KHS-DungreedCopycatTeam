 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterG2 : Test_Monster
{
    public static event System.Action<GameObject> EnemyDieEvent; // 적이 죽을 때 발생하는 이벤트
    public enum State
    {
        None,
        Idle,
        Dash,
        Attack,
        Die,
    }
    public State monState;

    //움직임관련
    [SerializeField]
    private float       groundRayDis;
    private float       gravity = -9.8f;
    private Vector3     vel     = Vector3.zero;
    private Vector3     seeDir  = Vector3.zero;
    private Color       colorDebugGround;


    // 거리
    [SerializeField]
    private float   findTargetDis;
    [SerializeField]
    private float   checkYDis;
    [SerializeField]
    private float   checkXDis;
    [SerializeField]
    private bool    findTarget;
    [SerializeField]
    private int     stateType = 0;

    // 공격 관련
    private GameObject      attackCollider;
    private BoxCollider2D   attackBoxCollider;

    private PoolManager     pool;

    public override void InitValueSetting()
    {
        base.SetupEffectPools();
        monData.capsuleCollider2D.isTrigger = true;

        monData.maxHP = 50;
        monData.moveSpeed = 3;
        monData.isDie = false;
        monData.isGround = false;
        monData.originColor = Color.white;
        monData.hitColor = Color.red;
        monData.curHP = monData.maxHP;
    }

    public void Setup(PoolManager newPool)
    {
        this.pool = newPool;

        base.Awake();

        attackCollider = transform.GetChild(1).gameObject;
        attackBoxCollider = attackCollider.GetComponent<BoxCollider2D>();
        attackBoxCollider.enabled = false;

        InitValueSetting();

        monData.hpBar.UpdateHPBar(monData.curHP, monData.maxHP);

        // 처음 생성된 적의 canvasHP 비활성화
        monData.canvasHP.SetActive(false);
    }
    private void OnEnable()
    {
        ChangeState(State.Idle);
    }
    private void OnDisable()
    {
        StopCoroutine(monState.ToString());
        monState = State.None;
    }
    private void FixedUpdate()
    {
        CalaulateDisToTarget();

        #region Die
        if (monData.curHP <= 0 && !monData.isDie)
        {
            monData.isDie = true;

            if (monData.isDie)
            {
                ChangeState(State.Die);
            }
        }
        #endregion

        // 바닥을 향해 레이 발사
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRayDis, LayerMask.GetMask("Platform"));
        Debug.DrawRay(transform.position, Vector2.down * groundRayDis, colorDebugGround);

        if(hit.collider != null)
        {
            monData.isGround = true;
            colorDebugGround = Color.green;
        }
        else
        {
            monData.isGround = false;
            colorDebugGround = Color.red;
        }
        
        if(!monData.isGround)
        {
            vel.y += gravity * Time.deltaTime;
        }
        else if(monData.isGround)
        {
            vel.y = 0;
        }

        transform.position += vel * Time.deltaTime;
        
    }
    #region 상태
    private IEnumerator Idle()
    {
        monData.animator.SetBool("IsMove", false);

        while (true)
        {
            vel.x = 0;

            switch (stateType)
            {
                case 0:
                    break;
                case 1:
                    ChangeState(State.Attack);
                    break;
                case 2:
                    ChangeState(State.Dash);
                    break;

            }

            yield return null;
        }
    }
    private void CalaulateDisToTarget()
    {
        PlayerController player = PlayerController.instance;

        if(player != null)
        {
            Vector2 targetPos = player.transform.position;
            float dis = Vector2.Distance(targetPos, transform.position);

            if(dis <= findTargetDis)
            {
                findTarget = true;
            }
            else
            {
                stateType = 0;
            }
        }

        if(findTarget)
        {
            stateType = 1;

            Vector2 targetPos = player.transform.position;

            // y 범위 안에 있으면?
            if (targetPos.y <= transform.position.y + checkYDis && targetPos.y >= transform.position.y - checkYDis)
            {
                // x축 범위 안
                if(targetPos.x <= transform.position.x + checkXDis && targetPos.y >= transform.position.x - checkXDis)
                {
                    // Type2 상태 패턴
                    stateType = 2;
                }
                // x축 범위 밖
                else
                {
                    // Type1 상태 패턴
                    stateType = 1;
                }
            }
            else
            {
                // Type1 상태 패턴
                stateType = 1;
            }
        }


    }

    private IEnumerator Attack()
    {
        while(true)
        {
            vel.x = 0;

            yield return null;
        }
    }
    private IEnumerator Die()
    {
        ActivateDieEffect(transform);

        if (EnemyDieEvent != null)
        {
            EnemyDieEvent(gameObject);
        }
        // 던전 내 킬 카운트 상승
        PlayerDungeonData.instance.countKill++;
        // exp 플레이어에게 추가 => 차후 변수부분으로 정수값 수정
        PlayerDungeonData.instance.totalEXP += 100;

        // 해당 몬스터 비활성화
        pool.DeactivePoolItem(this.gameObject);

        yield return null;
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 무기 공격시 무기의 정보 받아와 상호작용
        if (collision.gameObject.tag == "PlayerAttack")
        {
            WeponInfo weapon = collision.gameObject.GetComponent<WeponInfo>();

            TakeAttack(weapon.curATK, weapon.textColor);
        }

        // 대시 공격시 PlayerStats에서 정보 받아와 상호작용
        else if (PlayerController.instance.movement.isDashing && collision.gameObject.tag == "Player")
        {
            PlayerStats player = PlayerStats.instance;

            TakeAttack(player.DashATK, Color.blue);
        }
    }

    private void UpdateSight()
    {
        bool isRight = PlayerController.instance.transform.position.x >= transform.position.x;
        monData.spriteRenderer.flipX = !isRight;

        seeDir = isRight ? Vector3.right : Vector3.left;
    }

    public void ChangeState(State newState)
    {
        if (monState == newState) return;

        // 이전에 재생하던 상태 종료 
        StopCoroutine(monState.ToString());

        // 상태 변경
        monState = newState;

        // 새로운 상태 재생
        StartCoroutine(monState.ToString());
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, findTargetDis);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position,new Vector3(50,checkYDis*2));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position,new Vector3(checkXDis*2,checkYDis*2));
    }
}
