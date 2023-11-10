using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectPool : MonoBehaviour
{
    private PoolManager poolManager;
    public void Setup(PoolManager poolManager)
    {
        this.poolManager = poolManager;
    }
    public void DeactivateEffect()
    {
        poolManager.DeactivePoolItem(gameObject);
    }
}
