using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDustEffect : MonoBehaviour
{
    private MemoryPool      memoryPool;

    private float           time;
    private float           fadeTime = 1.0f;

    public void Setup(MemoryPool memoryPool)
    {
        this.memoryPool = memoryPool;
    }

    private void Update()
    {
        //  FadeObject();
    }

    public void AnimationEnd()
    {
        Destroy(this.gameObject);
    }

    //private void FadeObject()
    //{
    //    time = Time.deltaTime;
    //
    //    if (time < fadeTime)
    //        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1 - time / fadeTime);
    //    else
    //        memoryPool.DeactivatePoolItem(this.gameObject);
    //}
}
