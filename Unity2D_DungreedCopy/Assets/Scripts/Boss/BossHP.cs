using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI    ;

public class BossHP : MonoBehaviour
{
    public float maxHP = 1000; 
    public float curHP;

    [SerializeField]
    private Image               imageBossDieEffect;
    [SerializeField]
    private SpriteRenderer      spriteRenderer;

    private BossPattern             bossPattern;
    private BossController          bossController;
    private MainCameraController    mainCam;

    private void Awake()
    {
        bossPattern     = GetComponent<BossPattern>();
        bossController  = GetComponent<BossController>();

        mainCam         = FindObjectOfType<MainCameraController>();

        curHP = maxHP;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q) && bossController.isAbleToAttack == true)
        {
            BossTakeDamage(100);
            mainCam.OnShakeCam();
        }
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


    private IEnumerator HitColorAnimation()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.color = Color.white;
    }

}
