using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonPortal : MonoBehaviour
{
    public bool                     eatPlayer = false;
    public string                   tranferMapName;   // 이동할 맵의 이름

    private PoolManager             poolManager;
    private PlayerController        player;
    private FadeEffectController    fade;
    private DungeonPortalController dungeonPortalController;

    private void Awake()
    {
        player                  = FindObjectOfType<PlayerController>();
        fade                    = FindObjectOfType<FadeEffectController>();
        dungeonPortalController = FindObjectOfType<DungeonPortalController>();
    }
    public void Setup(PoolManager poolManager)
    {
        this.poolManager = poolManager;
    }

    public void ThePortalEatPlayer()
    {
        eatPlayer = true;
    }
    public void FalseToEatPlayer()
    {
        eatPlayer = false;

        player.curSceneName = tranferMapName;

        fade.OnFade(FadeState.FadeOut);

        StartCoroutine(ChangeScene());
    }

    private IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(fade.fadeTime);
        dungeonPortalController.isCollideToPlayer = false;
        poolManager.DeactivePoolItem(gameObject);
        SceneManager.LoadScene(tranferMapName);
    }




}
