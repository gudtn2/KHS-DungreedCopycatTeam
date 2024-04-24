using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWhiteSkelAttackCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string parentObjectName = transform.parent.name;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.instance.TakeDamage(15);
        }
    }
}
