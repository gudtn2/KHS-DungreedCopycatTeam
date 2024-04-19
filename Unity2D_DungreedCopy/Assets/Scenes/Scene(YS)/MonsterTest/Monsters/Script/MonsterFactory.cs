using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFactory : MonoBehaviour
{
    [SerializeField]
    private GameObject[]    monPrefabs;
    private PoolManager[]   monPools;

    [SerializeField]
    private int             number;     // 생성할 몬스터의 번혼

    private PoolManager     pool;       // Spawn Effect PoolManager
    public void Setup(int num,PoolManager newPool)
    {
        this.pool = newPool;
        number = num;

        monPools = new PoolManager[monPrefabs.Length];
        for (int i = 0; i < monPrefabs.Length; ++i)
        {
            monPools[i] = new PoolManager(monPrefabs[i]);
        }
    }

    public void ActivateMonster()
    {
        if(number >= 0 && number < monPrefabs.Length)
        {
            GameObject mon = monPools[number].ActivePoolItem();
            mon.transform.position = transform.position;
            mon.transform.rotation = transform.rotation;

            switch (number)
            {
                case 0:
                    mon.GetComponent<MonsterA>().Setup(monPools[number]);
                    break;
                case 1:
                    mon.GetComponent<MonsterB>().Setup(monPools[number]);
                    break;
                case 2:
                    mon.GetComponent<MonsterC>().Setup(monPools[number]);
                    break;
                case 3:
                    mon.GetComponent<MonsterD>().Setup(monPools[number]);
                    break;
                case 4:
                    mon.GetComponent<MonsterE>().Setup(monPools[number]);
                    break;
                case 5:
                    mon.GetComponent<MonsterG1>().Setup(monPools[number]);
                    break;
            }

        }
        else
        {
            Debug.LogError("없는 몬스터 넘버를 기입하셨습니다.");
        }
    }
    public void DeactivateSapwnEffect()
    {
        pool.DeactivePoolItem(this.gameObject);
    }
}
