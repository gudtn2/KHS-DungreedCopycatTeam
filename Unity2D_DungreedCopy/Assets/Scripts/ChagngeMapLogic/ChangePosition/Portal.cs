using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    [SerializeField]
    private PortalStartPoint    portalStartPoint;

    private PlayerController        player;
    private FadeEffectController    fade;
    
    [Header("해당 MarkCurMap 삽입")]
    [SerializeField]
    private MarkCurMap              markCurMap;
    public string                   dungeonMapMoveDir;      // 오른쪽: R, 왼쪽: L, 아래:D, 위:U

    [Header("다음 이동할 dungeon오브젝트 삽입")]
    [SerializeField]
    private GameObject              nextDungeon;
    private string                  transferDungeonName;    // YS: 이동할 맵의 이름


    private void Awake()
    {
        player  = FindObjectOfType<PlayerController>();
        fade    = FindObjectOfType<FadeEffectController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            transferDungeonName     = nextDungeon.name;
            player.curDungeonName   = transferDungeonName;
            fade.OnFade(FadeState.FadeOut);
            StartCoroutine(portalStartPoint.ChangePlayerPosition());
            markCurMap.dungeonMapDir = dungeonMapMoveDir;
        }
    }
}
