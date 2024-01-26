using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer[]    bossSprites;
    [SerializeField]
    private float               camSmoothMoveTime;
    [SerializeField]
    private Transform           camViewPos;
    public bool                 isAbleToAttack = false;

    private UIEffectManager         uiEffectManager;
    private MainCameraController    mainCam;
    private PlayerController        player;
    private void Awake()
    {
        mainCam             = FindObjectOfType<MainCameraController>();
        uiEffectManager     = FindObjectOfType<UIEffectManager>();
        player              = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        player.playerMeetsBoss = true;

        StartCoroutine(FadeInBossSprite());
    }

    private IEnumerator FadeInBossSprite()
    {
        StartCoroutine(mainCam.ChangeView(camViewPos, camSmoothMoveTime));
        StartCoroutine(GameObject.FindObjectOfType<UIBossIntroduce>().OnIntroduceBoss(0, 1));

        for (int i = 0; i < bossSprites.Length; ++i)
        {
            StartCoroutine(uiEffectManager.UIFade(bossSprites[i], 0, 1));
            yield return new WaitForSeconds(1);

            if(i == bossSprites.Length-1)
            {
                yield return new WaitForSeconds(2);
                player.playerMeetsBoss = false;
                isAbleToAttack = true;
                StartCoroutine(GameObject.FindObjectOfType<UIBossIntroduce>().OffIntroduceBoss(1, 0));
            }
        }
    }
}
