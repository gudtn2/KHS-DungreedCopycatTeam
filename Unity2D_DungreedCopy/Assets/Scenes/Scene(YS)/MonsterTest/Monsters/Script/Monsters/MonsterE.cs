using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterE : Test_Monster
{
    public static event System.Action<GameObject> EnemyDieEvent; // ���� ���� �� �߻��ϴ� �̺�Ʈ
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

        // ó�� ������ ���� canvasHP ��Ȱ��ȭ
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
        // �÷��̾� ������ �ٶ󺸵���
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
                // Wander Ƚ�� ����
                wanderCount++;

                //3�� ������ ��� Attack
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
        // ���� �� ų ī��Ʈ ���
        PlayerDungeonData.instance.countKill++;
        // exp �÷��̾�� �߰� => ���� �����κ����� ������ ����
        PlayerDungeonData.instance.totalEXP += 100;

        // �ش� ���� ��Ȱ��ȭ
        pool.DeactivePoolItem(this.gameObject);

        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� ���ݽ� ������ ���� �޾ƿ� ��ȣ�ۿ�
        if (collision.gameObject.tag == "PlayerAttack")
        {
            WeponInfo weapon = collision.gameObject.GetComponent<WeponInfo>();

            TakeAttack(weapon.curATK, weapon.textColor);
        }

        // ��� ���ݽ� PlayerStats���� ���� �޾ƿ� ��ȣ�ۿ�
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

        // ������ ����ϴ� ���� ���� 
        StopCoroutine(monState.ToString());

        // ���� ����
        monState = newState;

        // ���ο� ���� ���
        StartCoroutine(monState.ToString());
    }
}
