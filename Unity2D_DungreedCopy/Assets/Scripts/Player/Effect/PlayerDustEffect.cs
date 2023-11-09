using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDustEffect : MonoBehaviour
{
    [SerializeField]
    private float               moveSpeed;

    private PoolManager         poolManager;
    public void Setup(PoolManager poolManager)
    {
        this.poolManager = poolManager;
    }
    private void Update()
    {
        PlayerController player     = GameObject.Find("Player").GetComponent<PlayerController>();

        if (player.lastMoveDirX != 0)
        {
            float a = 1;
            if(player.transform.localRotation == Quaternion.Euler(0, 180, 0))
            {
                a = -1;
            }
            MovePrefab(player.lastMoveDirX * (-1) * a);
        }
    }
    private void MovePrefab(float direction)
    {
        transform.Translate(Vector3.right * direction * moveSpeed * Time.deltaTime);
    }
    public void DeactivateDustEffect()
    {
        poolManager.DeactivePoolItem(this.gameObject);
    }
}
