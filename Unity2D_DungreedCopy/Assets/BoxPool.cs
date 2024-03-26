using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPool : MonoBehaviour
{
    [SerializeField]
    private GameObject      prefabItem;
    private PoolManager     itemPool;

    private Transform       itemSpqwnPos;
    private PoolManager     poolManager;
    private Animator        ani;
    private Rigidbody2D     rigidBody;
    public void Setup(PoolManager poolManager)
    {
        this.poolManager = poolManager;

        ani         = GetComponent<Animator>();
        rigidBody   = GetComponent<Rigidbody2D>();

        itemSpqwnPos = transform.GetChild(0).gameObject.GetComponent<Transform>();
        itemPool = new PoolManager(prefabItem);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Platform")
        {
            rigidBody.bodyType = RigidbodyType2D.Static;
            ani.SetTrigger("IsOpen");
            SpawnRandomItem();
        }
    }

    private void SpawnRandomItem()
    {
        GameObject item = itemPool.ActivePoolItem();
        item.transform.position = itemSpqwnPos.position;
        item.transform.rotation = transform.rotation;
        item.GetComponent<RandomItemCreator>().Setup(itemPool);
    }
}
