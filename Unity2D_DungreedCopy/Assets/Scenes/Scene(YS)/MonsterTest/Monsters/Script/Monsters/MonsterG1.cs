using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterG1 : Test_Monster
{
    public static event System.Action<GameObject> EnemyDieEvent; // 적이 죽을 때 발생하는 이벤트
    public enum State
    {
        None,
        Idle,
        Chase,
        Attack,
        Die,
    }
    public State monState;

    [SerializeField]
    private float chaseRadius;
    [SerializeField]
    private float attackRadius;
    [SerializeField]
    private float maxWanderDis;
    private float rayDis = 0.95f;
    private float dis;
    
    // 공격 관련
    private GameObject      attackCollider;
    private BoxCollider2D   attackBoxCollider;

    #region 점프를 위한 벽 채크 변수
    private Vector3 seeDir;
    [SerializeField]
    private float seeRayDis;
    #endregion 


    private float moveX;
    private bool isAttacking;



    private PoolManager pool;

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

        //bulletPool = new PoolManager(prefabBullet);
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
        if (monData.curHP <= 0 && !monData.isDie)
        {
            monData.isDie = true;

            if (monData.isDie)
            {
                ChangeState(State.Die);
            }
        }

        monData.isGround = IsGrounded();

        if (!monData.isGround)
        {
            monData.rigidbody2D.velocity = new Vector3(0, -9.8f);
        }
        else
        {
            monData.rigidbody2D.velocity = new Vector3(moveX, 0);
        }
    }

    private IEnumerator Idle()
    {
        monData.animator.SetBool("IsMove", false);

        while (true)
        {
            CalculateDisToTargetAndselectState();

            moveX = 0;

            yield return null;
        }
    }
    private IEnumerator Chase()
    {
        monData.animator.SetBool("IsMove", true);
        while (true)
        {
            UpdateSight();

            moveX = seeDir.x * (monData.moveSpeed * 1.2f);

            CalculateDisToTargetAndselectState();
            yield return null;
        }
        moveX = 0;

    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        monData.animator.SetBool("IsAttack", true);
        while (isAttacking)
        {
            moveX = 0;

            // 왼쪽 바라보고 있을 때
            if(monData.spriteRenderer.flipX)
            {
                attackCollider.transform.localPosition = new Vector2(-1.5f, attackCollider.transform.localPosition.y);
            }
            // 오른쪽 바라보고 있을 때
            else
            {
                attackCollider.transform.localPosition = new Vector2(1.5f, attackCollider.transform.localPosition.y);
            }

            yield return null;
        }
    }
    public void EnableAttackCollider()
    {
        attackBoxCollider.enabled = true;
    }
    public void DisableAttackCollider()
    {
        attackBoxCollider.enabled = false;
    }

    public void CutAni()
    {
        isAttacking = false;
        monData.animator.SetBool("IsAttack", false);
        ChangeState(State.Idle);
    }

    private void CalculateDisToTargetAndselectState()
    {
        PlayerController player = PlayerController.instance;

        if (player != null)
        {
            Vector3 target = player.transform.position;
            dis = Vector2.Distance(target, transform.position);

            // 쫒을수 있는 거리 내에 있으면
            if (dis <= chaseRadius && dis > attackRadius)
            {
                ChangeState(State.Chase);
            }
            // 모든 거리에서 벗어나면 => Idle
            else if (dis > chaseRadius)
            {
                ChangeState(State.Idle);
            }
            else if (dis <= attackRadius)
            {
                ChangeState(State.Attack);
            }
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
        if (PlayerController.instance.transform.position.x > transform.position.x)
        {
            monData.spriteRenderer.flipX = false;
            seeDir = Vector3.right;
        }
        else
        {
            monData.spriteRenderer.flipX = true;
            seeDir = Vector3.left;
        }
    }
    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, rayDis, LayerMask.GetMask("Platform"));
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
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
