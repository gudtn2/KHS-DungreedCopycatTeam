using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterC : Test_Monster
{
    public static event Action<GameObject> EnemyDieEvent; // ���� ���� �� �߻��ϴ� �̺�Ʈ
    public enum State
    {
        None,
        Idle,
        Chase,
        ChaseAttack,
        Die,
    }
    public State monState;

    [Header("Chase ����")]
    [SerializeField]
    private float checkPlayerRadius;

    private PoolManager pool;

    public override void InitValueSetting()
    {
        base.SetupEffectPools();
        monData.capsuleCollider2D.isTrigger = true;

        monData.maxHP = 20;
        monData.moveSpeed = 5;
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
        while(true)
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, checkPlayerRadius, Vector2.right, 0, LayerMask.GetMask("Player"));
            
            // => ��� �Լ� 
            yield return null;
        }
    }

    private void CalculateDistanceToTarget()
    {
        // Idle => �� �Լ����� �Ÿ� �˻� 
        // chaseRadius�� ǥ���� Circle ���� ������ Chase
        // attackRadius�� ǥ���� Circle ���� ������ ChaseAttack
    }

    private IEnumerator Die()
    {
        ActivateDieEffect(transform);

        if(EnemyDieEvent != null)
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
