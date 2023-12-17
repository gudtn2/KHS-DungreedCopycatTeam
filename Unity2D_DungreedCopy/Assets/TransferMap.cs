using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{
    public string               tranferMapName;   // 이동할 맵의 이름

    private PlayerController        player;
    private FadeEffectController    fade;

    private void Awake()
    {
        player  = FindObjectOfType<PlayerController>();
        fade    = FindObjectOfType<FadeEffectController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name =="Player")
        {
            player.curMapName = tranferMapName;

            fade.OnFade(FadeState.FadeOut);

            StartCoroutine(ChangeScene());
        }
    }

    private IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(fade.fadeTime);

        SceneManager.LoadScene(tranferMapName);
    }
}
