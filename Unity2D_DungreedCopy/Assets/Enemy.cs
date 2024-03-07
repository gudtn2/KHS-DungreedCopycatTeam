using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy 정보")]
    private float curHP;
    [SerializeField]
    private float maxHP;


    [SerializeField]
    private float   timeToReturnOriginColor = 0.3f;

    private Color originColor;
    private Color color;

    private SpriteRenderer  spriteRenderer;
    private HPBar           healthBar;

    private void Start()
    {
        spriteRenderer  = GetComponent<SpriteRenderer>();
        healthBar       = GetComponentInChildren<HPBar>();

        curHP = maxHP;
        healthBar.UpdateHPBar(curHP, maxHP);

        originColor = spriteRenderer.color;
        color = Color.red;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerAttack")
        {
            // 피격시 컬러 변경
            //spriteRenderer.color = color;
            spriteRenderer.color = collision.gameObject.GetComponent<WeponInfo>().textColor;

            // 피격 색깔 원상복구 코루틴 함수 실행
            StartCoroutine(ReturnColor());

            // 적 체력 감소
            TakeDamage(collision.gameObject.GetComponent<WeponInfo>().curATK,   
                       collision.gameObject.GetComponent<WeponInfo>().textColor);
            
            // Enemy 체력바 최신화
            healthBar.UpdateHPBar(curHP, maxHP);
        }
    }

    private IEnumerator ReturnColor()
    {
        yield return new WaitForSeconds(timeToReturnOriginColor);
        spriteRenderer.color = originColor;
    }

    private void TakeDamage(int dam,Color color)
    {
        Color textColor = Color.white;

        if(curHP >0)
        {
            curHP -= dam;

            textColor = color;
            Debug.Log(textColor);
        }
    }

}
