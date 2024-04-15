using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCollider : MonoBehaviour
{
    private PlayerStats         playerStats;

    private void Awake()
    {
        playerStats         = FindObjectOfType<PlayerStats>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            if (!PlayerController.instance.isDie)
            {
                PlayerController.instance.PlayerDamaged(10);
            }
        }
    }
}
