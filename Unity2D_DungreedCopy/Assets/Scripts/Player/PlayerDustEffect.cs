using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDustEffect : MonoBehaviour
{
    private MemoryPool      memoryPool;

    public void Setup(MemoryPool memoryPool)
    {
        this.memoryPool = memoryPool;
    }
    public void AnimationEnd()
    {
        memoryPool.DeactivatePoolItem(this.gameObject);
    }
}
