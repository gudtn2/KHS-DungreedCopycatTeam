using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    [Header("Item 생성 변수")]
    [SerializeField]
    private float       forceX;
    [SerializeField]
    private float       forceY;
    [SerializeField]    
    private int         minItemCount;
    [SerializeField]    
    private int         maxItemCount;
    [SerializeField]
    private GameObject  SpawnItemPrefab;

    private PoolManager ItemPoolManager;
    
    private void Awake()
    {
        ItemPoolManager = new PoolManager(SpawnItemPrefab);

    }

    private void OnApplicationQuit()
    {
        ItemPoolManager.DestroyObjcts();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(SpawnCoin());
        }
    }
    private IEnumerator SpawnCoin()
    {
        int itemCount = Random.Range(minItemCount, maxItemCount);
        
        for (int i = 0; i < itemCount; i++)
        {
            Vector3 targetPos   = new Vector3(Random.Range(-forceX, forceX), Random.Range(0f, forceY),0);
            Vector3 dir         = targetPos - transform.position; 

            GameObject item = ItemPoolManager.ActivePoolItem();
            item.transform.position = transform.position;
            item.transform.rotation = transform.rotation;
            item.GetComponent<GoldItemController>().Setup(ItemPoolManager, dir);
        }
        yield return null;
    }
}
