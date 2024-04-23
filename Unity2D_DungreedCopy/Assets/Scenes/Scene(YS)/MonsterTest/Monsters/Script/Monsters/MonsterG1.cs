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

    //움직임관련
    [SerializeField]
    private float       jumpForce;
    private float       gravity = -9.8f;
    private Vector3     vel     = Vector3.zero;
    private Vector3     seeDir  = Vector3.zero;
    private bool        Jumping;
    private Color       colorDebugGround;

    // 거리
    [SerializeField]
    private float chaseDis;
    [SerializeField]
    private float attackDis;

    // 공격 관련
    private GameObject      attackCollider;
    private BoxCollider2D   attackBoxCollider;
    private bool            isAttacking;

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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.95f, LayerMask.GetMask("Platform"));
        Debug.DrawRay(transform.position, Vector2.down * 0.95f, colorDebugGround);

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
        else if(monData.isGround && !Jumping)
        {
            vel.y = 0;
        }
        else if (monData.isGround)
        {
            Jumping = false;
        }
        transform.position += vel * Time.deltaTime;
        
    }

    private IEnumerator Idle()
    {
        monData.animator.SetBool("IsMove", false);

        while (true)
        {
            vel.x = 0;

            // Idle 인 경우 행동
            CalculateDisToTargetAndselectState();

            yield return null;
        }
    }

    private IEnumerator Chase()
    {
        monData.animator.SetBool("IsMove", true);
        while(true)
        {
            // 타겟을 바라보도록
            UpdateSight();

            CheckWall();

            CheckPosY();

            // 거리에 따른 상태 변화
            CalculateDisToTargetAndselectState();

            //transform.Translate(seeDir * monData.moveSpeed * Time.deltaTime);

            yield return null;
        }
    }

    private void CheckPosY()
    {
        if(PlayerController.instance.transform.position.y > transform.position.y +1)
        {
            if(monData.isGround && !Jumping)
            {
                vel.x = seeDir.x * monData.moveSpeed;
                Jump();
            }
        }
    }
    
    private void CheckWall()
    {
        // Raycast 발사를 위한 시작점과 방향 설정
        Vector3 rayOrigin = transform.position; // 캐릭터에서 시작
        Vector3 rayDir = seeDir;                // 레이 방향 = 바라보는 방향

        // Raycast 발사
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDir, 1f, LayerMask.GetMask("Platform"));

        // 디버그를 위한 Ray 그리기
        Debug.DrawRay(rayOrigin, rayDir * 1f, Color.red);

        // Platform 충돌 확인
        if (hit.collider != null)
        {
            vel.x = 0;
            if (monData.isGround && !Jumping)
            {
                Jump();
            }
            //else if (!monData.isGround) return;
        }
        else
        {
            // 타겟의 방향으로 이동
            vel.x = seeDir.x * monData.moveSpeed;
        }
    }

    private void Jump()
    {
        vel.y = jumpForce;
        Jumping = true;
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        monData.animator.SetBool("IsAttack", true);
        while (isAttacking)
        {
            vel.x = 0;

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
            float dis = Vector2.Distance(target, transform.position);

            // 쫒을수 있는 거리 내에 있으면
            if (dis <= chaseDis&& dis > attackDis)
            {
                ChangeState(State.Chase);
            }
            // 모든 거리에서 벗어나면 => Idle
            else if (dis > chaseDis)
            {
                ChangeState(State.Idle);
            }
            else if (dis <= attackDis)
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
}
