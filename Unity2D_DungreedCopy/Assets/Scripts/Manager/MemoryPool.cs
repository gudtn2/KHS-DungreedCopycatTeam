using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 작성자: YS

public class MemoryPool : MonoBehaviour
{
    //메모리 풀로 관리되는 오브젝트 정보
    public class PoolItem
    {
        public bool isActive;               // "gameObject"의 활성 비활성화 정보
        public GameObject gameObject;       // 화면에 보이는 실제 오브젝트
    }

    private int increaseCount = 5;          // 오브젝트 부족시 Instantiate()로 추가되 생성되는 오브젝트 개순
    private int maxCount;                   // 현재 리스트에 등록된 오브젝트 개수
    private int activeCount;                // 현재 게임에 사용되고있는(활성화) 오브젝트 개수

    private GameObject      poolObject;     // 오브젝트풀링에서 관리하는 게임오브젝트 프리팹
    private List<PoolItem>  poolItemList;   // 관리하는 모든 오브젝트를 저장하는 프리팹

    // 설명: 클래스 이름과 같은 이름의 함수는 "생성자"로 해당클래스의 변수를 선언하고
    //       메모리가 할당될 때 자동으로 호출
    public MemoryPool(GameObject poolObject)
    {
        maxCount = 0;
        activeCount = 0;
        this.poolObject = poolObject;

        poolItemList = new List<PoolItem>();

        InstantiateObjects();
    }

    // 설명: increaseCount단위로 오브젝트 생성 
    public void InstantiateObjects()
    {
        maxCount += increaseCount;

        for (int i = 0; i < increaseCount; ++i)
        {
            PoolItem poolItem = new PoolItem();

            poolItem.isActive = false;
            poolItem.gameObject = GameObject.Instantiate(poolObject);

            poolItemList.Add(poolItem);
        }
    }

    //설명: 현재 관리중인(활성/비활성) 모든 오브젝트 삭제
    public void DestroyObjects()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            GameObject.Destroy(poolItemList[i].gameObject);
        }
        poolItemList.Clear();
    }

    // 설명: poolItemList에 저장되어있는 모근 오브젝트를 활성화해서 사용
    //       만약, 모든 오브젝트가 활성화중이면 InstantiateObjects()로 추가 생성
    public GameObject ActivePoolItem()
    {
        if (poolItemList == null) return null;

        // 현재 생성해서 관리하는 모든 오브젝트 개수와 현재 활성화된 상태인 오브젝트 개수 비교
        // 모두 활성화 상태이면 새로운 오브젝트 필요
        if(maxCount == activeCount)
        {
            InstantiateObjects();
        }

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if(poolItem.isActive == false)
            {
                activeCount++;

                poolItem.isActive = true;
                poolItem.gameObject.SetActive(true);

                return poolItem.gameObject;
            }
        }

        return null;
    }

    // 설명: 현재 사용이 완료된 오브젝트를 비활성화 살태로 설정
    public void DeactivatePoolItem(GameObject removeItem)
    {
        if (poolItemList == null || removeItem == null) return;
        
        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.gameObject == removeItem)
            {
                activeCount--;

                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);

                return;
            }
        }
    }

    // 설명: 게임에 사용중인 모든 오브젝트를 비활성화 상태로 전환
    public void DeactivateAllPoolItems()
    {
        if (poolItemList == null) return;
        
        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.gameObject != null && poolItem.isActive == true)
            {
                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);
            }
        }

        activeCount = 0;

    }
}