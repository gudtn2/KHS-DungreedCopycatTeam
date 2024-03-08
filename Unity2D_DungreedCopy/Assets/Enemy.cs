using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy 정보")]
    [SerializeField]
    private float       curHP;
    [SerializeField]
    private float       maxHP;
    [SerializeField]
    private GameObject  CanvasHP; // 공격을 한 경우만 체력바 보이도록
    [SerializeField]
    private bool        isDie;
    [SerializeField]
    private GameObject  prefabDieEffect;

    [Header("Text Effect 변수")]
    [SerializeField]
    private GameObject  prefabDamageText;
    [SerializeField]
    private float       timeToReturnOriginColor = 0.3f;

    private Color originColor;
    private Color color;

    private PoolManager     textPoolManager;
    private PoolManager     dieEffectPoolManager;

    private SpriteRenderer  spriteRenderer;
    private HPBar           healthBar;

    private void Start()
    {
        spriteRenderer  = GetComponent<SpriteRenderer>();
        healthBar       = GetComponentInChildren<HPBar>();

        textPoolManager         = new PoolManager(prefabDamageText);
        dieEffectPoolManager    = new PoolManager(prefabDieEffect);

        curHP = maxHP;
        healthBar.UpdateHPBar(curHP, maxHP);

        originColor = spriteRenderer.color;
        color = Color.red;

        CanvasHP.SetActive(false);
    }

    private void Update()
    {
        if(curHP <= 0 && !isDie)
        {
            isDie = true;

            if(isDie)
            {
                StartCoroutine("Die");
            }
        }
    }
    private IEnumerator Die()
    {
        ActivateDieEffect();
        Destroy(this.gameObject);
        yield return null;
    }
    private void ActivateDieEffect()
    {
        GameObject effect = dieEffectPoolManager.ActivePoolItem();
        effect.transform.position = transform.position;
        effect.transform.rotation = transform.rotation;
        effect.GetComponent<EffectPool>().Setup(dieEffectPoolManager);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerAttack")
        {
            if(curHP > 0)
            {
                // 공격을 시작한 경우만 체력바 보이도록
                CanvasHP.SetActive(true);

                // 피격시 컬러 변경
                //spriteRenderer.color = color;
                spriteRenderer.color = collision.gameObject.GetComponent<WeponInfo>().textColor;

                // 피격 색깔 원상복구 코루틴 함수 실행
                StartCoroutine(ReturnColor());

                // 적 체력 감소
                TakeDamage(collision.gameObject.GetComponent<WeponInfo>().curATK);

                // 데미지 텍스트 인스턴스 함수
                ActivateText(collision.gameObject.GetComponent<WeponInfo>().curATK,
                            collision.gameObject.GetComponent<WeponInfo>().textColor);

                // 피격시 카메라 흔들림
                MainCameraController.instance.OnShakeCamByPos(0.1f, 0.1f);

                // Enemy 체력바 최신화
                healthBar.UpdateHPBar(curHP, maxHP);
            }
        }
    }

    private IEnumerator ReturnColor()
    {
        yield return new WaitForSeconds(timeToReturnOriginColor);
        spriteRenderer.color = originColor;
    }

    private void TakeDamage(int dam)
    {
        curHP -= dam;
    }

    private void ActivateText(int damage,Color color)
    {
        GameObject dam = textPoolManager.ActivePoolItem();
        dam.transform.position = transform.position;
        dam.transform.rotation = transform.rotation;
        dam.GetComponent<DamageText>().Setup(textPoolManager, damage, color);
    }
}
