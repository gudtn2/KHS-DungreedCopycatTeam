using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    private float       deactivateTime;

    private PoolManager poolManager;
    public void Setup(PoolManager newPool)
    {
        this.poolManager = newPool;
        StartCoroutine(DeactivateArrow());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            poolManager.DeactivePoolItem(this.gameObject);
        }
    }
    private IEnumerator DeactivateArrow()
    {
        yield return new WaitForSeconds(deactivateTime);
        poolManager.DeactivePoolItem(this.gameObject);
    }
}
