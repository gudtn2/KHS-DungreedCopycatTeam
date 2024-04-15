using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBullet : MonoBehaviour
{
    [SerializeField]
    private float       inputSpeed;
    private float       speed;
    [SerializeField]
    private float       radius;
    private PoolManager pool;
    private Animator    ani;
    private Vector3     dir;

    private bool        isExplosion = false;

    public void Setup(PoolManager newPool,Vector3 newDir)
    {
        pool = newPool;
        ani = GetComponent<Animator>();

        dir = newDir;

        Invoke("DeactivateEffect", 5);
    }

    private void Update()
    {
        transform.position += dir * speed * Time.deltaTime;

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

        if (isExplosion)
        {
            speed = 0;
            ani.SetBool("OnEffect",true);
        }
        else
        {
            speed = inputSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ★ 차후 몬스터 완성되면 주석 지우고 코드 활성화

        if(this.gameObject.name == "BatBullet(Clone)")
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                PlayerController.instance.PlayerDamaged(10);
            }
        }
        else if(this.gameObject.name == "BansheeBullet(Clone)")
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerController.instance.PlayerDamaged(5);
            }
        }
    }


    public void DeactivateEffect()
    {
        ani.SetBool("OnEffect",false);
        isExplosion = false;
        pool.DeactivePoolItem(this.gameObject);
    }
}
