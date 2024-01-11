using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// YS
public class DungeonPortalController : MonoBehaviour
{
    public bool         isCollideToPlayer = false;
    
    [SerializeField]
    private GameObject              dungeonPortalPrefab;

    private PoolManager             dungeonPortalPoolMnager;
    private PlayerController        player;

    private void Awake()
    {
        dungeonPortalPoolMnager = new PoolManager(dungeonPortalPrefab);
        
        player = FindObjectOfType<PlayerController>();
    }
    private void OnApplicationQuit()
    {
        dungeonPortalPoolMnager.DestroyObjcts();
    }

    private void ActiveDungeonPortal()
    {
        GameObject dungeonPortal = dungeonPortalPoolMnager.ActivePoolItem();
        dungeonPortal.transform.position = new Vector3(player.transform.position.x, -1.5f,transform.position.z);
        dungeonPortal.transform.rotation = transform.rotation;
        dungeonPortal.GetComponent<DungeonPortal>().Setup(dungeonPortalPoolMnager);
    }

    // 플레이어와 충돌시 MemoryPool로 DungeonPortal 불러오기
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            ActiveDungeonPortal();
            isCollideToPlayer = true;
        }
    }
}
