using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform bulletSpawn;

    private float bulletSpeed = 50f;


    // Update is called once per frame
    void Update()
    {
        Fire();
    }

    void Fire()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, transform.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);
            StartCoroutine(PlayerController.instance.AbleToAttack());
        }
    }
}
