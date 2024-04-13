using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonstterData
{
    public float curHP;         // ���� ü��
    public float maxHP;         // �ִ� ü��
    public float moveSpeed;     // ������ �ӵ�
    public float jumpForce;     // ���� ��
    
    public bool  isDie;         // �׾�����?          => DieEffect ���� ����
    public bool  isGround;      // ������?            => Trigger�� ������ Collider�� ���ϰ��̱⿡ ���� �ô���ִ��� Ȯ��/ ����
    public bool  canAttack;     // ������ ��������?   => Trigger�� ������ Collider�� ���ϰ��̱⿡ ���� �ô���ִ��� Ȯ��/ ����
    
    public Color originColor;   // �⺻ �÷�
    public Color hitColor;      // �ǰݽ� �÷�

    public SpriteRenderer       spriteRenderer;
    public CapsuleCollider2D    capsuleCollider2D;
    public Rigidbody2D          rigidbody2D;
    public Animator             animator;
}

[System.Serializable]
public class EffectData
{
    public GameObject  prefabDieEffect;    // ������ ��Ÿ���� ����Ʈ
    public GameObject  prefabDamageTest;   // Ÿ�ݽ� �ؽ�Ʈ ����Ʈ
}

public abstract class Test_Monster : MonoBehaviour
{
    #region ������ Ŭ����
    public MonstterData monData;
    public EffectData   monEffectData;
    #endregion

    #region ����Ʈ Pools
    protected PoolManager spawnEffectPool;  
    protected PoolManager dieEffectPool;
    protected PoolManager damageTextPool;
    #endregion

    protected void Awake()
    {
        monData.spriteRenderer = GetComponent<SpriteRenderer>();
        monData.capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        monData.rigidbody2D = GetComponent<Rigidbody2D>();
        monData.animator = GetComponent<Animator>();
    }

    public virtual void CheckGround()
    {
        RaycastHit2D hit = Physics2D.BoxCast(monData.capsuleCollider2D.bounds.center,
                                             monData.capsuleCollider2D.bounds.size,
                                             0, Vector2.down, 0.02f, LayerMask.GetMask("Platform"));
        if(hit.collider != null)
        {
            monData.isGround = true;
        }
        else
            monData.isGround = false;

        if (!monData.isGround)
        {
            monData.rigidbody2D.velocity = new Vector2(monData.rigidbody2D.velocity.x, monData.rigidbody2D.velocity.y);
            monData.rigidbody2D.gravityScale = 1;
        }
        else
        {
            monData.rigidbody2D.velocity = new Vector2(monData.rigidbody2D.velocity.x, 0);
            monData.rigidbody2D.gravityScale = 0;
        }
    }

    // ������ Monster Ŭ�������� �������� ������ �� �ְ�
    public abstract void InitValueSetting();

    // ������ Monster�� ������ Clone���� �ҷ��� �� �ְ�
    protected virtual void SetupEffectPools()
    {
        dieEffectPool   = new PoolManager(monEffectData.prefabDieEffect);
        damageTextPool  = new PoolManager(monEffectData.prefabDamageTest);
    }

    // Ŭ���� ���������� �������ִ� �޼���
    protected virtual void ActivateEffect(Transform transform, PoolManager pool)
    {
        GameObject prefab = pool.ActivePoolItem();
        prefab.transform.position = transform.position;
        prefab.transform.rotation = transform.rotation;
        prefab.GetComponent<EffectPool>().Setup(pool);
    }
}
