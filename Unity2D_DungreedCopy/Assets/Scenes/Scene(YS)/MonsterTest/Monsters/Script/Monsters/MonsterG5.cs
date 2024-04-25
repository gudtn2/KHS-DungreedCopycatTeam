using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterG5 : Test_Monster
{
    public static event System.Action<GameObject> EnemyDieEvent; // ���� ���� �� �߻��ϴ� �̺�Ʈ
    public enum State
    {
        None,
        Idle,
        Chase,
        Charge,
        Attack,
        Die,
    }
    public State monState;


    //�����Ӱ���
    [SerializeField]
    private float gravity = -9.8f;      // �߷� ���
    private Vector3 vel = Vector3.zero;
    private Vector3 seeDir = Vector3.zero;
    private bool Jumping;
    private Color colorDebugGround;
    private float groundRayDis;
    [SerializeField]
    private bool findTarget;
    // ������ ��� ����� ����
    private const float launchAngle = 45f;
    private bool        isAttack;

    // �Ÿ�
    [SerializeField]
    private float chaseDis;
    [SerializeField]
    private float attackDis;

    // ���� ����
    private const float attackCooldown = 3f; // ���� ��ٿ� �ð�
    private float lastAttackTime; // ������ ���� �ð�

    private PoolManager pool;

    public override void InitValueSetting()
    {
        base.SetupEffectPools();
        monData.capsuleCollider2D.isTrigger = true;

        monData.maxHP = 50;
        monData.moveSpeed = 4.5f;
        monData.jumpForce = 4;
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

        groundRayDis = monData.capsuleCollider2D.size.y * 0.5f;

        InitValueSetting();

        monData.hpBar.UpdateHPBar(monData.curHP, monData.maxHP);

        // ó�� ������ ���� canvasHP ��Ȱ��ȭ
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
        FindTarget();

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

        #region �ٴ��� �˻��ϴ� ray/ isGround ����
        // �ٴ��� ���� ���� �߻�
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRayDis, LayerMask.GetMask("Platform"));
        Debug.DrawRay(transform.position, Vector2.down * groundRayDis, colorDebugGround);

        if (hit.collider != null)
        {
            monData.isGround = true;
            colorDebugGround = Color.green;
        }
        else
        {
            monData.isGround = false;
            colorDebugGround = Color.red;
        }

        // isGround���¿� ���� y�� ��ȯ
        if (!monData.isGround)
        {
            vel.y += gravity   * Time.deltaTime;
        }
        else if (monData.isGround && !Jumping)
        {
            vel.y = 0;
        }
        else if (monData.isGround)
        {
            Jumping = false;
        }
        #endregion

        transform.position += vel * Time.deltaTime;

    }

    private IEnumerator Idle()
    {
        while (true)
        {
            vel.x = 0;

            if (findTarget)
            {
                yield return new WaitForSeconds(0.5f);
                ChangeState(State.Chase);
            }

            yield return null;
        }
    }

    private IEnumerator Chase()
    {
        monData.animator.SetBool("IsMove", true);
        while (true)
        {
            UpdateSight();

            vel.x = seeDir.x * monData.moveSpeed;

            // �Ÿ��� ���� ���� ��ȭ
            CalculateDisToTargetAndselectState();

            yield return null;
        }
        monData.animator.SetBool("IsMove", false);

    }

    private IEnumerator Charge()
    {
        monData.animator.SetBool("IsMove", false);
        vel.x = 0;

        yield return new WaitForSeconds(0.5f);
        ChangeState(State.Attack);
    }

    private IEnumerator Attack()
    {

        PlayerController player = PlayerController.instance;

        monData.animator.SetBool("IsJumpAttack", true);
        isAttack = true;
        Jump();
        while (true)
        {
            if(vel.y < 0)
            {
                monData.animator.SetBool("IsJumpAttack", false);
                monData.animator.SetBool("IsFall", true);
            }

            if( vel.y == 0)
            {
                // ���� �� ���� ��ٿ� ����
                lastAttackTime = Time.time;

                CutAni();
            }
            yield return null;
        }
    }
    private void Jump()
    {
        vel.y = monData.jumpForce;
        vel.x = seeDir.x * (monData.moveSpeed *2f);
        Jumping = true;
    }
    public void CutAni()
    {
        monData.animator.SetBool("IsFall", false);
        isAttack = false;
        ChangeState(State.Idle);
    }

    private void FindTarget()
    {
        PlayerController player = PlayerController.instance;

        if (player != null)
        {
            Vector3 target = player.transform.position;
            float dis = Vector2.Distance(target, transform.position);

            if (dis <= chaseDis)
            {
                findTarget = true;
            }
        }
    }

    private void CalculateDisToTargetAndselectState()
    {
        PlayerController player = PlayerController.instance;

        if (player != null)
        {
            Vector3 target = player.transform.position;
            float dis = Vector2.Distance(target, transform.position);

            if (dis <= attackDis)
            {
                if(Time.time >= lastAttackTime + attackCooldown)
                    ChangeState(State.Charge);
                else
                    ChangeState(State.Chase);

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

        if(isAttack && collision.gameObject.tag == "Player")
        {
            PlayerController.instance.TakeDamage(5);
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
        Gizmos.DrawWireSphere(transform.position, chaseDis);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDis);
    }
}
