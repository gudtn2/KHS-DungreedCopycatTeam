using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPool : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabItem;
    private PoolManager itemPool;

    private Transform itemSpqwnPos;
    private GameObject keyPrefab;

    [SerializeField]
    private bool onKey;
    [SerializeField]
    public bool inputKey;

    private PoolManager poolManager;
    private Animator ani;
    private Rigidbody2D rigidBody;
    public void Setup(PoolManager poolManager)
    {
        this.poolManager = poolManager;

        ani = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();

        itemSpqwnPos = transform.GetChild(0).gameObject.GetComponent<Transform>();
        keyPrefab = transform.GetChild(1).gameObject;

        itemPool = new PoolManager(prefabItem);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            rigidBody.bodyType = RigidbodyType2D.Static;
        }

        if (collision.gameObject.tag == "Player" && !inputKey && rigidBody.bodyType == RigidbodyType2D.Static)
        {
            onKey = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !inputKey)
        {
            onKey = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && onKey)
        {
            inputKey = true;
            onKey = false;

            ani.SetTrigger("IsOpen");
            SpawnRandomItem();
        }

        keyPrefab.SetActive(onKey);
    }

    private void SpawnRandomItem()
    {
        GameObject item = itemPool.ActivePoolItem();
        item.transform.position = itemSpqwnPos.position;
        item.transform.rotation = transform.rotation;
        item.GetComponent<RandomItemCreator>().Setup(itemPool);
    }
}
