using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashEffect : MonoBehaviour
{
    private PoolManager         poolManager;
    private float               moveSpeed = 3.0f;

    public void Setup(PoolManager poolManager)
    {
        this.poolManager = poolManager;
    }
    private void Update()
    {
        EffectMove();
    }

    private void EffectMove()
    {
        Movement2D movement = GameObject.Find("Player").GetComponent<Movement2D>();
        Vector3 oppositeDir = -movement.dashDir.normalized;
        Vector3 effectMove = oppositeDir * moveSpeed * Time.deltaTime;

        transform.Translate(effectMove);
    }

    public void DeactivateDashEffect()
    {
        poolManager.DeactivePoolItem(this.gameObject);
    }
}
