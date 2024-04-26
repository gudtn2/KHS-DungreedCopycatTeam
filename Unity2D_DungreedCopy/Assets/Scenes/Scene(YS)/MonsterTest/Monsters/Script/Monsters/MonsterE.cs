using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterE : Test_Monster
{
    public static event System.Action<GameObject> EnemyDieEvent; // 적이 죽을 때 발생하는 이벤트
    public enum State
    {
        None,
        Idle,
        Wander,
        Attack,
        Die,
    }
    public State monState;

    #region Bullet
    [SerializeField]
    private GameObject prefabBullet;
    private PoolManager bulletPool;
    #endregion

    [SerializeField]
    private float radius;
    [SerializeField]
    private float range;
    [SerializeField]
    private float maxDis;
    private Vector2 point;
    private int     wanderCount = 0;


    private PoolManager pool;

    public override void InitValueSetting()
    {
        base.SetupEffectPools();
        monData.capsuleCollider2D.isTrigger = true;

        monData.maxHP = 20;
        monData.moveSpeed = 2;
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

        InitValueSetting();

        monData.hpBar.UpdateHPBar(monData.curHP, monData.maxHP);

        // 처음 생성된 적의 canvasHP 비활성화
        monData.canvasHP.SetActive(false);

        bulletPool = new PoolManager(prefabBullet);
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
        // 플레이어 방향을 바라보도록
        UpdateSight();

        if (monData.curHP <= 0 && !monData.isDie)
        {
            monData.isDie = true;

            if (monData.isDie)
            {
                ChangeState(State.Die);
            }
        }
    }

    private IEnumerator Idle()
    {
        SetNewDestination();
        ChangeState(State.Wander);

        while (true)
        {

            yield return null;
        }
    }

    private void SetNewDestination()
    {
        Vector2 randomPoint = Vector2.zero;
        bool pointIsValid = false;

        while (!pointIsValid)
        {
            randomPoint = new Vector2(Random.Range(-maxDis, maxDis), Random.Range(-maxDis, maxDis));
            
            RaycastHit2D hit = Physics2D.Raycast(point, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Platform"));
            
            if(hit.collider == null)
            {
                pointIsValid = true;
            }
        }
        point = randomPoint;
    }
    private IEnumerator Wander()
    {

        while(true)
        {
            transform.position = Vector2.MoveTowards(transform.position, point, monData.moveSpeed * Time.deltaTime);
            
            if(Vector2.Distance(transform.position,point) < range)
            {
                // Wander 횟수 증가
                wanderCount++;

                //3번 진행한 경우 Attack
                if(wanderCount >= 3)
                {
                    ChangeState(State.Attack);
                }
                else
                {
                    ChangeState(State.Idle);
                }
            }
            yield return null;
        }
    }

    private IEnumerator Attack()
    {
        monData.animator.SetBool("IsAttack", true);

        yield return null;
    }

    public void CreateBatBullet()
    {
        Vector3 target = PlayerController.instance.transform.position;
        Vector3 dir = (target - transform.position).normalized;

        ActivateBullet(dir);
    }
    public void CutAni()
    {
        monData.animator.SetBool("IsAttack", false);
        wanderCount = 0;
        ChangeState(State.Idle);
    }

    private void ActivateBullet(Vector3 dir)
    {
        GameObject bullet = bulletPool.ActivePoolItem();
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;
        bullet.GetComponent<BatBullet>().Setup(bulletPool, dir);
    }

    private IEnumerator Die()
    {
        ActivateDieEffect(transform);
        GiveCompensation(transform, 5);

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
        }
        else
        {
            monData.spriteRenderer.flipX = true;
        }
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
