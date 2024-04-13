using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonster : MonoBehaviour
{
    [SerializeField]
    private GameObject  prefabSpawn;
    [SerializeField]
    private int         monNum; // 0:BigBat

    private PoolManager spawnPool;

    private void Awake()
    {
        spawnPool = new PoolManager(prefabSpawn);
        AvtivateSpawnPrefab(monNum);
    }

    private void OnApplicationQuit()
    {
        spawnPool.DestroyObjcts();
    }

    private void AvtivateSpawnPrefab(int num)
    {
        GameObject spawn = spawnPool.ActivePoolItem();
        spawn.transform.position = transform.position;
        spawn.transform.rotation = transform.rotation;
        spawn.GetComponent<MonsterFactory>().Setup(monNum, spawnPool);
    }
}
