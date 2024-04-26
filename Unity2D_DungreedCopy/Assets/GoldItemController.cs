using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldItemController : MonoBehaviour
{
    public int             GoldValue;
    [SerializeField]
    private GameObject      textGoldPrefab;
    private Color           rayColor;
    private bool            isGround;

    private Rigidbody2D         rigid;
    private PoolManager         poolManager;
    private PoolManager         textGoldpoolManager;
    private CircleCollider2D    circleCollider2D;

    public void Setup(PoolManager newPool,Vector3 dir)
    {
        this.poolManager = newPool;

        rigid               = GetComponent<Rigidbody2D>();
        circleCollider2D    = GetComponent<CircleCollider2D>();

        rigid.velocity = new Vector3(dir.x, dir.y, 0);
    }

    private void Awake()
    {
        textGoldpoolManager = new PoolManager(textGoldPrefab);
    }
    private void FixedUpdate()
    {
        AddGravity();
    }

    private void AddGravity()
    {
        // ������ �Ʒ� �������� Ray�� ��
        // collider�� �ϸ� collider�� ��� ��� ������ �˻��ϱ� ������ ���Ͻ�
        // ���� ���� ��츸 ���ߵ��� �����ϱ� ���� 
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, circleCollider2D.radius, LayerMask.GetMask("Platform"));
        Debug.DrawRay(transform.position, Vector2.down * circleCollider2D.radius, rayColor);

        if (hit.collider != null)
        {
            isGround = true;
            rayColor = Color.green;
        }
        else
        {
            isGround = false;
            rayColor = Color.red;
        }

        if (isGround)
        {
            rigid.velocity = Vector2.zero;
            rigid.gravityScale = 0;
        }
        else
        {
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y);
            rigid.gravityScale = 1;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            // �÷��̾�� Bullion�� ��ġ��ŭ GOLD����
            PlayerStats.instance.TakeGold(GoldValue);

            // Bullion Prefab ��Ȱ��ȭ
            poolManager.DeactivePoolItem(gameObject);

            ActivateGoldText();
        }
    }
    private void ActivateGoldText()
    {
        GameObject goldText = textGoldpoolManager.ActivePoolItem();
        goldText.transform.position = transform.position;
        goldText.transform.rotation = transform.rotation;
        goldText.GetComponent<TextGoldController>().Setup(textGoldpoolManager, GoldValue);
    }
}
