using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffect : MonoBehaviour
{
    public static event Action<GameObject> EnemyDieEvent; // 적이 죽을 때 발생하는 이벤트

    [Header("Enemy Info")]
    [SerializeField]
    private float       curHP;
    [SerializeField]
    private float       maxHP;
    [SerializeField]
    private bool        isDie;
    [SerializeField]
    private GameObject  prefabDieEffect;
    [SerializeField]
    private GameObject  CanvasHP;           // CanvasEnemy GameObject

    [Header("Text Effect")]
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

    private void Awake()
    {
        spriteRenderer  = GetComponent<SpriteRenderer>();
        healthBar       = GetComponentInChildren<HPBar>();

        textPoolManager = new PoolManager(prefabDamageText);
        dieEffectPoolManager = new PoolManager(prefabDieEffect);
    }
    private void Start()
    {
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

        // 적이 죽었음을 이벤트로 발생시킴
        if (EnemyDieEvent != null)
        {
            EnemyDieEvent(gameObject);
        }

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
                // ������ ������ ��츸 ü�¹� ���̵���
                CanvasHP.SetActive(true);

                // �ǰݽ� �÷� ����
                spriteRenderer.color = color;

                // �ǰ� ���� ���󺹱� �ڷ�ƾ �Լ� ����
                StartCoroutine(ReturnColor());

                // �� ü�� ����
                TakeDamage(collision.gameObject.GetComponent<WeponInfo>().curATK);

                // ������ �ؽ�Ʈ �ν��Ͻ� �Լ�
                ActivateText(collision.gameObject.GetComponent<WeponInfo>().curATK,
                            collision.gameObject.GetComponent<WeponInfo>().textColor);

                // �ǰݽ� ī�޶� ��鸲
                MainCameraController.instance.OnShakeCamByPos(0.1f, 0.1f);

                // Enemy ü�¹� �ֽ�ȭ
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
