using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    [Header("Item 생성 변수")]
    [SerializeField]
    private float       forceY;
    [SerializeField]    
    private int         minItemCount;
    [SerializeField]    
    private int         maxItemCount;
    [SerializeField]
    private GameObject  SpawnItemPrefab;

    private PoolManager thisPool;

    private PoolManager ItemPoolManager;
    public void Setup(PoolManager pool)
    {
        this.thisPool = pool;

        ItemPoolManager = new PoolManager(SpawnItemPrefab);
        StartCoroutine(SpawnCoin());
        DeactivatePoolItem();
    }
    private IEnumerator DeactivatePoolItem()
    {
        yield return new WaitForSeconds(1);
        thisPool.DeactivePoolItem(this.gameObject);
    }

    private void OnApplicationQuit()
    {
        ItemPoolManager.DestroyObjcts();
    }

    
    private IEnumerator SpawnCoin()
    {
        int itemCount = Random.Range(minItemCount, maxItemCount);
        
        for (int i = 0; i < itemCount; i++)
        {
            Vector3 targetPos   = new Vector3(0, Random.Range(0f, forceY),0);
            Vector3 dir         = targetPos - transform.position; 

            GameObject item = ItemPoolManager.ActivePoolItem();
            item.transform.position = transform.position;
            item.transform.rotation = transform.rotation;
            item.GetComponent<GoldItemController>().Setup(ItemPoolManager, dir);
        }
        yield return null;
    }
}
