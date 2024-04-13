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
                Debug.Log("Player");
                isExplosion = true;
            }
            else if (hit.collider.CompareTag("Platform"))
            {
                Debug.Log("Platform");
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


    public void DeactivateEffect()
    {
        ani.SetBool("OnEffect",false);
        isExplosion = false;
        pool.DeactivePoolItem(this.gameObject);
    }
}
