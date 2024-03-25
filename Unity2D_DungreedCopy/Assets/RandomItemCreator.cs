using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemCreator : MonoBehaviour
{
    private float           speedY = 5.0f;
    private PoolManager     poolManager;
    private Rigidbody2D     rigidBody;

    public void Setup(PoolManager poolManager)
    {
        this.poolManager = poolManager;

        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = new Vector3(0, speedY);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            rigidBody.bodyType = RigidbodyType2D.Static;
        }
    }
    public void DeactivateEffect()
    {
        poolManager.DeactivePoolItem(gameObject);
    }
}
