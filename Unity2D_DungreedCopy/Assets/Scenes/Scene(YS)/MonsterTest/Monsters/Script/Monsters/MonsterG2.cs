 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterG2 : Test_Monster
{
    public static event System.Action<GameObject> EnemyDieEvent; // ���� ���� �� �߻��ϴ� �̺�Ʈ
    public enum State
    {
        None,
        Idle,
        Dash,
        Attack,
        Die,
    }
    public State monState;

    //�����Ӱ���
    [SerializeField]
    private float       groundRayDis;
    private float       gravity = -9.8f;
    private Vector3     vel     = Vector3.zero;
    private Vector3     seeDir  = Vector3.zero;
    private Color       colorDebugGround;


    // �Ÿ�
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

    // ���� ����
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

        // �ٴ��� ���� ���� �߻�
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
    #region ����
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

            // y ���� �ȿ� ������?
            if (targetPos.y <= transform.position.y + checkYDis && targetPos.y >= transform.position.y - checkYDis)
            {
                // x�� ���� ��
                if(targetPos.x <= transform.position.x + checkXDis && targetPos.y >= transform.position.x - checkXDis)
                {
                    // Type2 ���� ����
                    stateType = 2;
                }
                // x�� ���� ��
                else
                {
                    // Type1 ���� ����
                    stateType = 1;
                }
            }
            else
            {
                // Type1 ���� ����
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
        // ���� �� ų ī��Ʈ ���
        PlayerDungeonData.instance.countKill++;
        // exp �÷��̾�� �߰� => ���� �����κ����� ������ ����
        PlayerDungeonData.instance.totalEXP += 100;

        // �ش� ���� ��Ȱ��ȭ
        pool.DeactivePoolItem(this.gameObject);

        yield return null;
    }
    #endregion

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
        Gizmos.DrawWireSphere(transform.position, findTargetDis);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position,new Vector3(50,checkYDis*2));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position,new Vector3(checkXDis*2,checkYDis*2));
    }
}
