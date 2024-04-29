using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stranger : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        StartCoroutine(CheckTrue());
    }
    private void Update()
    {
        if(spriteRenderer.color.a < 0.1f)
        {
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator CheckTrue()
    {
        while(true)
        {
            yield return new WaitUntil(() => NPCManager.instance.meetKablovinaInDungeon);
            StartCoroutine(UIEffectManager.instance.UIFade(spriteRenderer, 1, 0));

            yield break;
        }
    }
}
