using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonstterData
{
    public float curHP;         // 현재 체력
    public float maxHP;         // 최대 체력
    public float moveSpeed;     // 움직임 속도
    public float jumpForce;     // 점프 힘
    
    public bool  isDie;         // 죽었는지?          => DieEffect 실행 위함
    public bool  isGround;      // 땅인지?            => Trigger로 몬스터의 Collider을 붙일것이기에 땅과 맡닿아있는지 확인/ 점프
    public bool  canAttack;     // 공격이 가능한지?   => Trigger로 몬스터의 Collider을 붙일것이기에 땅과 맡닿아있는지 확인/ 점프
    
    public Color originColor;   // 기본 컬러
    public Color hitColor;      // 피격시 컬러

    public SpriteRenderer       spriteRenderer;
    public CapsuleCollider2D    capsuleCollider2D;
    public Rigidbody2D          rigidbody2D;
    public Animator             animator;
}

[System.Serializable]
public class EffectData
{
    public GameObject  prefabDieEffect;    // 죽음시 나타나는 이펙트
    public GameObject  prefabDamageTest;   // 타격시 텍스트 이펙트
}

public abstract class Test_Monster : MonoBehaviour
{
    #region 데이터 클래스
    public MonstterData monData;
    public EffectData   monEffectData;
    #endregion

    #region 이펙트 Pools
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

    // 각각의 Monster 클래스에서 변수값을 셋팅할 수 있게
    public abstract void InitValueSetting();

    // 각각의 Monster가 생성될 Clone들을 불러올 수 있게
    protected virtual void SetupEffectPools()
    {
        dieEffectPool   = new PoolManager(monEffectData.prefabDieEffect);
        damageTextPool  = new PoolManager(monEffectData.prefabDamageTest);
    }

    // 클론을 실질적으로 생성해주는 메서드
    protected virtual void ActivateEffect(Transform transform, PoolManager pool)
    {
        GameObject prefab = pool.ActivePoolItem();
        prefab.transform.position = transform.position;
        prefab.transform.rotation = transform.rotation;
        prefab.GetComponent<EffectPool>().Setup(pool);
    }
}
