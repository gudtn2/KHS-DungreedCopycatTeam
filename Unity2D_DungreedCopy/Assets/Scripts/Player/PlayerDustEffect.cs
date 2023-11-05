using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDustEffect : MonoBehaviour
{
    [SerializeField]
    private float               moveSpeed;

    private PoolManager         poolManager;
    private Animator            ani;

    private void Awake()
    {
        ani     = GetComponent<Animator>();
    }
    public void Setup(PoolManager poolManager)
    {
        this.poolManager = poolManager;
    }
    private void Update()
    {
        PlayerController player = GameObject.Find("Player").GetComponent<PlayerController>();
        Debug.Log(player.lastMoveDir);

        if (player.lastMoveDir != 0)
        {
            MovePrefab(player.lastMoveDir * (-1));

            if(player.transform.rotation.y == 180)
            {
                MovePrefab(player.lastMoveDir);
            }

        }
    }
    private void MovePrefab(float direction)
    {
        transform.Translate(Vector3.right * direction * moveSpeed * Time.deltaTime);
    }
    public void DeactivateEffect()
    {
        poolManager.DeactivePoolItem(this.gameObject);
    }
}
