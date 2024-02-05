using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticCoin : MonoBehaviour
{
    [Header("자석 효과")]
    private float       magnetDis;
    [SerializeField]
    private float       magnetStrngth;
    [SerializeField]
    private int         magnetDirection = 1; // 인력은 1, 척력은 -1

    [Header("골드 정보")]
    public int         goldValue;

    [SerializeField]
    private GameObject          textGoldPrefab;
    private PoolManager         TextGoldpoolManager;
    
    private Transform           playerTransform;
    private PoolManager         poolManager;
    private GoldController      goldController;
    private PlayerStats         playerStats;
    private Rigidbody2D         rigidbody2D;


    public void Setup(PoolManager newPool)
    {
        poolManager = newPool;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        goldController = FindObjectOfType<GoldController>();
        playerStats = FindObjectOfType<PlayerStats>();

        rigidbody2D = GetComponent<Rigidbody2D>();

        magnetDis = goldController.magnetDis;

    }

    private void Awake()
    {
        TextGoldpoolManager = new PoolManager(textGoldPrefab);
    }

    private void Update()
    {
        CheckDisToPlayer();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            // 비활성화
            poolManager.DeactivePoolItem(gameObject);

            // 플레이어 총 골드에 추가하는 스크립트
            playerStats.TakeGold(goldValue);

            // 텍스트 활성화
            ActivateGoldText();
        }
    }

    private void ActivateGoldText()
    {
        GameObject goldText = TextGoldpoolManager.ActivePoolItem();
        goldText.transform.position = transform.position;
        goldText.transform.rotation = transform.rotation;
        goldText.GetComponent<TextGoldController>().Setup(TextGoldpoolManager);
    }
    private void CheckDisToPlayer()
    {
        Vector2 dirToTarget= playerTransform.position - transform.position;
        float dis = Vector2.Distance(playerTransform.position, transform.position);
        float magnetDisStr = (magnetDis / dis) * magnetStrngth;
        transform.Translate(magnetDisStr * (dirToTarget * magnetDirection) * Time.deltaTime);
    }
}
