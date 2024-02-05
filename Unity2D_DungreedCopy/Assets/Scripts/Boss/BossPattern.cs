using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BossState 
{
    None = -1,
    Idle = 0,
    HeadAttack,    
    HandsAttack,   
    SwordAttack,
    Die
}
public class BossPattern : MonoBehaviour
{
    public BossState   bossState;

    [Header("Die")]
    private PoolManager     explosionEffectPoolManager;
    [SerializeField]
    private float           slowFactor;
    [SerializeField]
    private float           fasterRate;
    [SerializeField]
    private Image           imageBossDieEffect;
    [SerializeField]
    private GameObject      explosionEffectPrefab;
    [SerializeField]
    private int             explosionEffectCount;       // 생성할 폭발이펙트 수
    [SerializeField]
    private GameObject      diePiecePrefab;
    [SerializeField]
    private Transform       camViewPos;
    public bool             isDie = false;

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
    [SerializeField]
    private float           headAttackMinTime = 3.0f;
    [SerializeField]
    private float           headAttackMaxTime = 5.0f;
    [SerializeField]
    private float           headAttackTime = 0;
    [SerializeField]
    private bool            isHeadAttack;
    [SerializeField]
    private Transform       headAttackTransform;


    [Header("SwordAttack")]
    [SerializeField]
    private GameObject          bossSwordSpawnPrefab;
    [HideInInspector]
    public  PoolManager         bossSwordSpawnPoolManager;
    [SerializeField]
    private float               bossSwordSpawnDelayTime;
    [SerializeField]
    private Transform[]         spawnTransforms;
    public int                  DeactivateSwordCount;

    [Header("HandsAttack")]
    [SerializeField]
    private GameObject          selectedHand;
    [SerializeField]
    private GameObject          leftHand;
    [SerializeField]
    private GameObject          rightHand;
    [SerializeField]
    private float               waitHandAttackTime;
    [SerializeField]
    private float               handsMoveTime;
    [SerializeField]
    private int                 count;
    [SerializeField]
    private int                 maxCount;
    [SerializeField]
    private int                 minCount;
    [SerializeField]
    private bool                isHandsAttack = false;

    private GameObject              player;
    private BossController          boss;
    private UIEffectManager         uiEffectManager;
    private MainCameraController    mainCam;
    private PlayerController        playerController;

    private void OnEnable()
    {
        ChangeBossState(BossState.Idle);    
    }
    private void OnDisable()
    {
        StopCoroutine(bossState.ToString());
        bossState = BossState.None;
    }
    private void Awake()
    {
        headAttackPoolManager       = new PoolManager(headBulletPrefab);
        bossSwordSpawnPoolManager   = new PoolManager(bossSwordSpawnPrefab);
        explosionEffectPoolManager  = new PoolManager(explosionEffectPrefab);

        player = GameObject.FindGameObjectWithTag("Player");

        boss = GetComponent<BossController>();

        uiEffectManager     = FindObjectOfType<UIEffectManager>();
        mainCam             = FindObjectOfType<MainCameraController>();
        playerController    = FindObjectOfType<PlayerController>();
    }
    private void OnApplicationQuit()
    {
        headAttackPoolManager.DestroyObjcts();
        bossSwordSpawnPoolManager.DestroyObjcts();
        explosionEffectPoolManager.DestroyObjcts();
    }
    private void Update()
    {
        // HeadAttack을 랜덤 시간으로 돌리기 위한 조건
        if(isHeadAttack)
        {
            headAttackTime += Time.deltaTime;

            if(headAttackTime > Random.Range(headAttackMinTime,headAttackMaxTime))
            {
                isHeadAttack = false;

                if(!isHeadAttack)
                {
                    StartCoroutine(HeadAttackTimeReturnZero());
                }
            }
        }

        // SwordAttack을 끝내기 위한 초기화
        if(DeactivateSwordCount >= 5)
        {
            DeactivateSwordCount = 0;
            ChangeBossState(BossState.Idle);
        }

    }
    private IEnumerator Idle()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine("AutoChangeBossAttack");

        while (true)
        {
            // "Idle"일때 하는 행동

            yield return null;
        }
    }

    private IEnumerator Die()
    {
        imageBossDieEffect.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.01f);

        StartCoroutine(uiEffectManager.UIFade(imageBossDieEffect, 1, 0));

        yield return new WaitForSeconds(1);
        GameObject explosionEffect = explosionEffectPoolManager.ActivePoolItem();
        explosionEffect.transform.position      = new Vector2(transform.position.x + 0.5f, transform.position.y - 1);
        explosionEffect.transform.rotation      = transform.rotation;
        explosionEffect.transform.localScale    = new Vector2(2,2);
        explosionEffect.GetComponent<EffectPool>().Setup(explosionEffectPoolManager);

        yield return new WaitForSeconds(1);
        playerController.isBossDie = true;
        Time.timeScale = slowFactor;
        //mainCam.ChangeView(camViewPos, 0.5f);

        for (int i = 0; i <= explosionEffectCount; i++)
        {
            yield return new WaitForSeconds(0.05f);
            Vector2 randomPos = new Vector2(Random.Range(transform.position.x - 4, transform.position.x + 4), Random.Range(transform.position.y - 4, transform.position.y + 4));
            explosionEffect = explosionEffectPoolManager.ActivePoolItem();
            explosionEffect.transform.position = randomPos;
            explosionEffect.transform.rotation = transform.rotation;
            explosionEffect.GetComponent<EffectPool>().Setup(explosionEffectPoolManager);
            mainCam.OnShakeCamByPos(0.05f,0.1f);
            Time.timeScale += fasterRate;

            if(i >= 100)
            {
                GameObject diePiece = Instantiate(diePiecePrefab);
                diePiece.transform.position = new Vector2(transform.position.x +1.45f , transform.position.y -1);
                diePiece.transform.rotation= transform.rotation;
                Destroy(this.gameObject);
            }
        }

    }

    private IEnumerator AutoChangeBossAttack()
    {
        int changeWaitTime = Random.Range(1, 4);

        yield return new WaitForSeconds(changeWaitTime);

        int count = Random.Range(0, 3);
        switch (count)
        {
            case 0:
                ChangeBossState(BossState.HandsAttack);
                break;
            case 1:
                ChangeBossState(BossState.HeadAttack);
                break;
            case 2:
                ChangeBossState(BossState.SwordAttack);
                break;
        }
    }

    private IEnumerator HandsAttack()
    {
        count = Random.Range(minCount, maxCount);

        while (count >= 0)
        {
            count--;

            int randomIndex = Random.Range(0, 2);

            if(randomIndex == 0)
            {
                selectedHand = leftHand;
            }
            else
            {
                selectedHand = rightHand;
            }

            Vector2 startPos = selectedHand.transform.position;
            Vector2 targetPos = new Vector2(selectedHand.transform.position.x, player.transform.position.y);

            float elapsedTime = 0f;

            while(elapsedTime < handsMoveTime)
            {
                selectedHand.transform.position = Vector2.Lerp(startPos, targetPos, elapsedTime / handsMoveTime);
                elapsedTime += Time.deltaTime;
             
                if(elapsedTime >= handsMoveTime)
                {
                    selectedHand.GetComponent<BossHands>().StartAttackAni();
                }
                yield return null;
            }

            yield return new WaitForSeconds(waitHandAttackTime);
        }

        ChangeBossState(BossState.Idle);
    }
    private IEnumerator HeadAttackTimeReturnZero()
    {
        yield return new WaitForSeconds(3f);
        headAttackTime = 0;

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
        
        GameObject.Find("BossHead").GetComponent<Animator>().SetBool("IsHeadAttack", true);

        yield return new WaitForSeconds(0.03f);
        isHeadAttack = true;

        while (isHeadAttack == true)
        {

            for (int i = 0; i < fireDirCount; ++i)
            {
                fireAngle += i + 90;

                GameObject tempObj = headAttackPoolManager.ActivePoolItem();

                Vector2 dir = new Vector2(Mathf.Cos(fireAngle * Mathf.Deg2Rad), Mathf.Sin(fireAngle * Mathf.Deg2Rad));

                tempObj.transform.right = dir;
                tempObj.transform.position = headAttackTransform.position;
                tempObj.GetComponent<BossHeadBullet>().Setup(headAttackPoolManager);
            }


            yield return new WaitForSeconds(fireRateTime);

            fireAngle += angleInterval;

            if (fireAngle > 360) fireAngle -= 360;
        }
        GameObject.Find("BossHead").GetComponent<Animator>().SetBool("IsHeadAttack", false);

        ChangeBossState(BossState.Idle);
    }

    public void ChangeBossState(BossState newState)
    {

        if (bossState == newState) return;

        // 이전에 재생하던 상태 종료 
        StopCoroutine(bossState.ToString());

        // 상태 변경
        bossState = newState;
        
        // 새로운 상태 재생
        StartCoroutine(bossState.ToString());
    }
}
