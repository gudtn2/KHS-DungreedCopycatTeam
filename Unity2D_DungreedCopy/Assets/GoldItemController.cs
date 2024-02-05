using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldItemController : MonoBehaviour
{
    public int             GoldValue;
    [SerializeField]
    private GameObject      textGoldPrefab;

    private PoolManager     textGoldpoolManager;
    private Rigidbody2D     rigid;
    private PoolManager     poolManager;
    private PlayerStats     playerStats;

    public void Setup(PoolManager newPool,Vector3 dir)
    {
        this.poolManager = newPool;

        playerStats = FindObjectOfType<PlayerStats>();
        
        rigid = GetComponent<Rigidbody2D>();

        rigid.velocity = new Vector3(dir.x, dir.y, 0);
    }

    private void Awake()
    {
        textGoldpoolManager = new PoolManager(textGoldPrefab);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform") ||
            collision.gameObject.layer == LayerMask.NameToLayer("PassingPlatform"))
        {
            rigid.bodyType = RigidbodyType2D.Static;
        }
        
        if (collision.gameObject.name == "Player")
        {
            // 플레이어에게 Bullion의 가치만큼 GOLD지급
            playerStats.TakeGold(GoldValue);

            // Bullion Prefab 비활성화
            poolManager.DeactivePoolItem(gameObject);

            ActivateGoldText();
        }
    }
    private void ActivateGoldText()
    {
        GameObject goldText = textGoldpoolManager.ActivePoolItem();
        goldText.transform.position = transform.position;
        goldText.transform.rotation = transform.rotation;
        goldText.GetComponent<TextGoldController>().Setup(textGoldpoolManager);
    }
}
