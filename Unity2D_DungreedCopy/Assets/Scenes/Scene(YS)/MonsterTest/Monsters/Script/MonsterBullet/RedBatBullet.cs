using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBatBullet : MonoBehaviour
{
    [SerializeField]
    private float inputSpeed;
    [SerializeField]
    private float radius;
    private PoolManager pool;
    private Animator ani;
    private Rigidbody2D rigid;

    private bool isExplosion = false;

    public GameObject bulletPos;
    public static bool fullCharge;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        fullCharge = false;
    }

    public void Setup(PoolManager newPool, GameObject pos)
    {
        pool = newPool;
        ani = GetComponent<Animator>();
        bulletPos = pos;

        Invoke("DeactivateEffect", 5);
    }

    private void Update()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, radius, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Player"))
            {
                isExplosion = true;
            }
            else if (hit.collider.CompareTag("Platform"))
            {
                isExplosion = true;
            }
        }

        if (fullCharge)
        {
            if (isExplosion)
            {
                rigid.velocity = Vector2.zero;
                ani.SetBool("OnEffect", true);
            }
            else
            rigid.velocity = bulletPos.transform.right * 5;
        }
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.gameObject.name == "BatBullet(Clone)")
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerController.instance.TakeDamage(10);
            }
        }
        else if (this.gameObject.name == "BansheeBullet(Clone)")
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerController.instance.TakeDamage(5);
            }
        }
        else if (this.gameObject.name == "RedBatBullet(Clone)")
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerController.instance.TakeDamage(10);
            }
        }
    }


    public void DeactivateEffect()
    {
        ani.SetBool("OnEffect", false);
        isExplosion = false;
        pool.DeactivePoolItem(this.gameObject);
        fullCharge = false;
    }
}