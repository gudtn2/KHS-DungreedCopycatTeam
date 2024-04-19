using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterG1 : Test_Monster
{
    public static event System.Action<GameObject> EnemyDieEvent; // ���� ���� �� �߻��ϴ� �̺�Ʈ
    public enum State
    {
        None,
        Idle,
        Wander,
        Chase,
        Attack,
        Die,
    }
    public State monState;

    //#region Bullet
    //[SerializeField]
    //private GameObject prefabBullet;
    //private PoolManager bulletPool;
    //#endregion
    [SerializeField]
    private float   chaseRadius;
    [SerializeField]
    private float   attackRadius;
    private float   rayDis = 0.95f;
    private float   dis;
    private bool    isMove = false;
    private float   dirX;
    private float   moveX;
    private bool    isAttacking;



    private PoolManager pool;

    public override void InitValueSetting()
    {
        base.SetupEffectPools();
        monData.capsuleCollider2D.isTrigger = true;

        monData.maxHP = 20;
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

        InitValueSetting();

        monData.hpBar.UpdateHPBar(monData.curHP, monData.maxHP);

        // ó�� ������ ���� canvasHP ��Ȱ��ȭ
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

        monData.isGround = IsGrounded();

        if(!monData.isGround)
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
            CalculateDisToTargetAndselectState();


            moveX = dirX * monData.moveSpeed;
            
            yield return null;
        }
    } 

    private IEnumerator Attack()
    {
        isAttacking = true;
        monData.animator.SetBool("IsAttack", true);
        while(isAttacking)
        {
            monData.rigidbody2D.velocity = Vector2.zero;
            yield return null;
        }

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

            // �i���� �ִ� �Ÿ� ���� ������
            if (dis <= chaseRadius && dis >attackRadius)
            {
                ChangeState(State.Chase);
            }
            // ��� �Ÿ����� ����� => Idle
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
        if(!isAttacking)
        {
            if (PlayerController.instance.transform.position.x > transform.position.x)
            {
                monData.spriteRenderer.flipX = false;
                dirX = 1;
            }
            else
            {
                monData.spriteRenderer.flipX = true;
                dirX = -1;
            }
        }
    }
    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, rayDis, LayerMask.GetMask("Platform"));
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
