using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGhostEffect : MonoBehaviour
{
    private PoolManager poolManager;
    public void Setup(PoolManager poolManager)
    {
        this.poolManager = poolManager;
    }
    public void DeactivateGhostEffect()
    {
        poolManager.DeactivePoolItem(this.gameObject);
    }
}
