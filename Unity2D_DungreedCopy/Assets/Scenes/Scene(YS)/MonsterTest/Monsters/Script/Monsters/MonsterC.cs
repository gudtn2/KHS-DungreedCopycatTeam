using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterC : Test_Monster
{
    private PoolManager pool;
    private GameObject canvasHP;
    private HPBar hpBar;

    private Vector3 footPos;
    private float   radious;

   

    public override void InitValueSetting()
    {
        base.SetupEffectPools();
        monData.capsuleCollider2D.isTrigger = true;

       canvasHP = transform.GetChild(0).gameObject;
        hpBar = canvasHP.GetComponentInChildren<HPBar>();

        monData.maxHP = 70;
        monData.moveSpeed = 7;
        monData.jumpForce = 7;
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

        footPos = new Vector2(transform.position.x, transform.position.y - 0.5f);
        radious = 0.5f;
    }

    private void Update()
    {
        CheckGround();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(footPos, radious);
    }
}
