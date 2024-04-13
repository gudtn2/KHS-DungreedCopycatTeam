using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterA : Test_Monster
{
    private PoolManager pool;
    private GameObject  canvasHP;
    private HPBar       hpBar;
    private Vector3 footPos;
    private float   radious;

    public override void InitValueSetting()
    {
        base.SetupEffectPools();

        canvasHP = transform.GetChild(0).gameObject;
        hpBar = canvasHP.GetComponentInChildren<HPBar>();

        monData.maxHP           = 50;
        monData.moveSpeed       = 5;
        monData.jumpForce       = 5;
        monData.isDie           = false;
        monData.isGround        = false;
        monData.originColor     = Color.white;
        monData.hitColor        = Color.red;
        monData.curHP = monData.maxHP;
    }

    public void Setup(PoolManager newPool)
    {
        this.pool = newPool;

        base.Awake();

        InitValueSetting();

        footPos = new Vector2(transform.position.x, transform.position.y - 1f);
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
