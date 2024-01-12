using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoss : MonoBehaviour
{
    [SerializeField]
    private Transform           bossCamViewPos;
    [SerializeField]
    private SpriteRenderer[]    spritesBoss;
    
    private CircleCollider2D        circleCollider2D;        
    private SpriteEffectManager     spriteEffectManager;
    private MainCameraController    mainCam;
    private PlayerController        player;

    private void Awake()
    {
        spriteEffectManager = GetComponent<SpriteEffectManager>();
        circleCollider2D    = GetComponent<CircleCollider2D>();
        mainCam             = FindObjectOfType<MainCameraController>();
        player              = FindObjectOfType<PlayerController>();
    }
    private void Start()
    {
        for (int i = 0; i < spritesBoss.Length; ++i)
        {
            spriteEffectManager.StartSpriteSetting(spritesBoss[i]);
        }
    }

    private void Update()
    {
        ChangeCamViewToBoss();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            player.playerMeetsBoss = true;
            for (int i = 0; i < spritesBoss.Length; ++i)
            {
                StartCoroutine(spriteEffectManager.ActiveSprite(spritesBoss[i]));
            }
            circleCollider2D.enabled = false;
            
        }
    }

    private void ChangeCamViewToBoss()
    {
        if (player.playerMeetsBoss)
        {
            mainCam.ChangeView(bossCamViewPos);
        }
    }

    

    
}
