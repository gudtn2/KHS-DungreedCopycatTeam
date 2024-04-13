using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFactory : MonoBehaviour
{
    #region 생성가능 몬스터 프리팹/ PoolManager
    [SerializeField]
    private GameObject  monsterA;
    [SerializeField]
    private GameObject  monsterB;
    [SerializeField]
    private GameObject  monsterC;

    private PoolManager monsterAPool;
    private PoolManager monsterBPool;
    private PoolManager monsterCPool;
    #endregion
    [SerializeField]
    private int             number;     // 생성할 몬스터의 번혼

    private PoolManager     pool;       // Spawn Effect PoolManager
    public void Setup(int num,PoolManager newPool)
    {
        this.pool = newPool;

        monsterAPool = new PoolManager(monsterA);
        monsterBPool = new PoolManager(monsterB);
        monsterCPool = new PoolManager(monsterC);

        number = num;
    }

    public void ActivateMonster()
    {
        if (number == 0)
        {
            GameObject monA = monsterAPool.ActivePoolItem();
            monA.transform.position = transform.position;
            monA.transform.rotation = transform.rotation;
            monA.GetComponent<MonsterA>().Setup(monsterAPool);
        }
        else if(number == 1)
        {
            GameObject monB = monsterBPool.ActivePoolItem();
            monB.transform.position = transform.position;
            monB.transform.rotation = transform.rotation;
            monB.GetComponent<MonsterB>().Setup(monsterBPool);
        }
        else if(number == 2)
        {
            GameObject monC = monsterCPool.ActivePoolItem();
            monC.transform.position = transform.position;
            monC.transform.rotation = transform.rotation;
            monC.GetComponent<MonsterC>().Setup(monsterCPool);
        }
    }
    public void DeactivateSapwnEffect()
    {
        pool.DeactivePoolItem(this.gameObject);
    }
}
