using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 차후 생성을 몬스터 파괴나 이벤트에 따라 PoolManager로 관리
public class GoldController : MonoBehaviour
{
    public float        magnetDis;

    [SerializeField]
    private GameObject          magneticCoinPrefab;
    private PoolManager         magneticCoinPoolManager;

    private Transform           playerTransform;
    private Rigidbody2D         rigidbody2D;
    private PoolManager         poolManager;

    public void Setup(PoolManager newpool,Vector3 dir)
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rigidbody2D = GetComponent<Rigidbody2D>();

        poolManager = newpool;

        rigidbody2D.velocity = new Vector3(dir.x, dir.y, 0);
    }
    private void Awake()
    {
        magneticCoinPoolManager = new PoolManager(magneticCoinPrefab);
    }
    private void OnApplicationQuit()
    {
        magneticCoinPoolManager.DestroyObjcts();
    }

    private void Update()
    {
        if(Vector2.Distance(transform.position,playerTransform.position) <= magnetDis)
        {
            ChangeMagneticCoin();
        }

    }
    private void ChangeMagneticCoin()
    {
        GameObject maneticCoin = magneticCoinPoolManager.ActivePoolItem();
        maneticCoin.transform.position = transform.position;
        maneticCoin.transform.rotation = transform.rotation;
        maneticCoin.GetComponent<MagneticCoin>().Setup(magneticCoinPoolManager);
        poolManager.DeactivePoolItem(gameObject);
    }

    
}
