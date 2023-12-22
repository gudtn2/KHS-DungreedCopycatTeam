using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPortal : MonoBehaviour
{
    [SerializeField]
    private float               playerColorBackTime;

    private PoolManager         poolManager;
    private PlayerController    player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }
    public void Setup(PoolManager poolManager)
    {
        this.poolManager = poolManager;
    }

    public void ThePortalEatPlayer()
    {
        Color color = player.spriteRenderer.color;
        color.a = 0;
        player.spriteRenderer.color = color;
        StartCoroutine("PlayerColorBack");
    }

    private IEnumerator PlayerColorBack()
    {
        yield return new WaitForSeconds(playerColorBackTime);
        Color color = player.spriteRenderer.color;
        color.a = 1;
        player.spriteRenderer.color = color;
    }
}
