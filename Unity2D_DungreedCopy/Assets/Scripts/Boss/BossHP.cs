using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI    ;

public class BossHP : MonoBehaviour
{
    public static BossHP instance;

    public float maxHP = 1000; 
    public float curHP;

    [SerializeField]
    private SpriteRenderer      spriteRenderer;

    private BossPattern             bossPattern;
    private BossController          bossController;
    private MainCameraController    mainCam;

    private void Awake()
    {
        instance = this;

        bossPattern     = GetComponent<BossPattern>();
        bossController  = GetComponent<BossController>();

        mainCam         = FindObjectOfType<MainCameraController>();

        curHP = maxHP;
    }
   
    public void BossTakeDamage(float damage)
    {
        curHP -= damage;

        StopCoroutine(HitColorAnimation());
        StartCoroutine(HitColorAnimation());

        if(curHP <= 0)
        {
            bossPattern.ChangeBossState(BossState.Die);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerAttack") 
        {
            Debug.Log("zzzz");
        }
    }


    private IEnumerator HitColorAnimation()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.color = Color.white;
    }

}
