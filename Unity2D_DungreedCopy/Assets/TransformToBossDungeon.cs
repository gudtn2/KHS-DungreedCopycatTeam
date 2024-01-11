using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TransformToBossDungeon : MonoBehaviour
{
    private KeyCode         transferBossKey = KeyCode.F;
    [SerializeField]
    private SpriteRenderer  spriteKey_F;
    [SerializeField]
    private string          transferBossStageName;

    private PlayerController        player;
    private FadeEffectController    fade;
    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        fade   = FindObjectOfType<FadeEffectController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            spriteKey_F.gameObject.SetActive(true);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if(Input.GetKeyDown(transferBossKey))
            {
                StartCoroutine("TranferBossStage");
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            spriteKey_F.gameObject.SetActive(false);
        }
    }
    private IEnumerator TranferBossStage()
    {
        player.curSceneName = transferBossStageName;

        fade.OnFade(FadeState.FadeInOut);

        yield return new WaitForSeconds(fade.fadeTime);
        SceneManager.LoadScene(transferBossStageName);
    }
}
