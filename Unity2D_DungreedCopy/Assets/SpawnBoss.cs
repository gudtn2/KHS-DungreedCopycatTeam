using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoss : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer[]    spritesBoss;

    private SpriteEffectManager spriteEffectManager;

    private void Awake()
    {
        spriteEffectManager = GetComponent<SpriteEffectManager>();
    }
    private void Start()
    {
        for (int i = 0; i < spritesBoss.Length; ++i)
        {
            spriteEffectManager.StartSpriteSetting(spritesBoss[i]);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            StartCoroutine(spriteEffectManager.RoutainActiveSprite(spritesBoss));
            //StartCoroutine(spriteEffectManager.ActiveSprite(spritesBoss[0]));
        }
    }

    

    
}
