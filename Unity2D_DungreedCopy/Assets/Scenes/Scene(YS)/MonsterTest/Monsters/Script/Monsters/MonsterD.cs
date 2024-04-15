using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class MonsterD : Test_Monster
{
    public static event Action<GameObject> EnemyDieEvent; // ���� ���� �� �߻��ϴ� �̺�Ʈ

    public enum State
    {
        None,
        Idle,
        Attack,
        Die,
    }
    public State monState;

    #region Bullet
    [SerializeField]
    private GameObject prefabBullet;
    private PoolManager bulletPool;
    #endregion

    private PoolManager pool;
    public GameObject bulltPos;
    public override void InitValueSetting()
    {
        base.SetupEffectPools();

        monData.maxHP = 50;
        monData.moveSpeed = 5;
        monData.jumpForce = 5;
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

    private void FixedUpdate()
    {
        // �÷��̾ �ִ� ������ �ٶ󺸵���
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
        yield return new WaitForSeconds(5.0f);
        ChangeState(State.Attack);
    }
    private IEnumerator Attack()
    {
        int roundNum = 23;
        
        monData.animator.SetBool("IsAttack", true);

       for (int index = 0; index < roundNum; index++)
        {

            GameObject bullet = bulletPool.ActivePoolItem();
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector3 dirVec = new Vector3(Mathf.Cos(Mathf.PI * 2 * index / roundNum) * 1.8f, Mathf.Sin(Mathf.PI * 2 * index / roundNum) * 1.8f);
            bullet.transform.position = transform.position + dirVec;
            //rigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);

            rigid.velocity = bulltPos.transform.right * 5;
            Debug.Log(dirVec);
            if (index >= roundNum - 1)
            {
                yield return new WaitForSeconds(3f);

                monData.animator.SetBool("IsAttack", false);
                ChangeState(State.Idle);
            }
        }
    }
    private IEnumerator Die()
    {
        ActivateDieEffect(transform);

        // ���� �׾����� �̺�Ʈ�� �߻���Ŵ
        if (EnemyDieEvent != null)
        {
            EnemyDieEvent(gameObject);
        }

        PlayerDungeonData.instance.countKill++;
        PlayerDungeonData.instance.totalEXP += 100;

        pool.DeactivePoolItem(this.gameObject);
        yield return null;
    }

    private void ActivateBullet(Vector3 dir)
    {
        GameObject bullet = bulletPool.ActivePoolItem();
        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;
        bullet.GetComponent<BatBullet>().Setup(bulletPool, dir);
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
