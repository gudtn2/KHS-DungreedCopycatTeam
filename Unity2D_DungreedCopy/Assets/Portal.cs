using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField]
    private string              transferDungeonName;    // YS: 이동할 맵의 이름
    [SerializeField]
    private PortalStartPoint    portalStartPoint;

    private PlayerController        player;
    private FadeEffectController    fade;


    private void Awake()
    {
        player  = FindObjectOfType<PlayerController>();
        fade    = FindObjectOfType<FadeEffectController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            player.curDungeonName = transferDungeonName;
            fade.OnFade(FadeState.FadeOut);
            StartCoroutine(portalStartPoint.ChangePlayerPosition());
        }
    }
}
