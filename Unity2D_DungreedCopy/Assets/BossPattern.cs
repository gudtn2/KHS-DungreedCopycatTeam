using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState 
{
    Idle = 0,
    HeadAttack,    
    ArmsAttack,   
    SwordAttack    
}
public class BossPattern : MonoBehaviour
{
    public BossState   bossState;

    [Header("HeadAttack")]
    [SerializeField]
    private GameObject      headBulletPrefab;
    [SerializeField]
    private int             angleInterval = -10;    // 양수 = 반시계 방향, 음수 = 시계 방향
    [SerializeField]
    private int             fireDirCount = 4;       // bullet이 나가는 방향의 갯수
    [SerializeField]
    private float           fireRateTime = 0.2f;    // bullet의 생성 시간 제어
    [HideInInspector]
    public  PoolManager     headAttackPoolManager;

    [Header("SwordAttack")]
    [SerializeField]
    private GameObject          bossSwordSpawnPrefab;
    [HideInInspector]
    public  PoolManager         bossSwordSpawnPoolManager;
    [SerializeField]
    private float               bossSwordSpawnDelayTime;
    [SerializeField]
    private Transform[]         spawnTransforms;
    public bool                 isSpawnAllSword = false;
    public List<GameObject>     swordList = new List<GameObject>();

    private void Awake()
    {
        headAttackPoolManager       = new PoolManager(headBulletPrefab);
        bossSwordSpawnPoolManager   = new PoolManager(bossSwordSpawnPrefab);

    }
    private void Start()
    {
        ChangeBossState(BossState.SwordAttack);
    }
    private IEnumerator SwordAttack()
    {
        for (int i = 0; i < spawnTransforms.Length; ++i)
        {
            yield return new WaitForSeconds(bossSwordSpawnDelayTime);
            GameObject bossSwordSpawn = bossSwordSpawnPoolManager.ActivePoolItem();
            bossSwordSpawn.transform.position = spawnTransforms[i].position;
            bossSwordSpawn.transform.rotation = transform.rotation;
            bossSwordSpawn.GetComponent<BossSwordSpawnEffect>().Setup(bossSwordSpawnPoolManager);
        }
    }

    private IEnumerator HeadAttack()
    {
        int fireAngle = 0;  // 초기값은 0도

        while(true)
        {

            for (int i = 0; i < fireDirCount; ++i)
            {
                fireAngle += i + 90;

                GameObject tempObj = headAttackPoolManager.ActivePoolItem();

                Vector2 dir = new Vector2(Mathf.Cos(fireAngle * Mathf.Deg2Rad), Mathf.Sin(fireAngle * Mathf.Deg2Rad));

                tempObj.transform.right = dir;
                tempObj.transform.position = transform.position;
                tempObj.GetComponent<BossHeadBullet>().Setup(headAttackPoolManager);
            }


            yield return new WaitForSeconds(fireRateTime);

            fireAngle += angleInterval;

            if (fireAngle > 360) fireAngle -= 360;
        }
    }

    private void ChangeBossState(BossState newState)
    {
        // 이전에 재생하던 상태 종료 
        StopCoroutine(bossState.ToString());

        // 상태 변경
        bossState = newState;
        
        // 새로운 상태 재생
        StartCoroutine(bossState.ToString());
    }
}
