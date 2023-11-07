using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectController : MonoBehaviour
{
    [Header("먼지 이펙트")]
    [SerializeField]
    private GameObject  effectDust;
    [SerializeField]
    private Transform   parent;
    [SerializeField]
    private float       delayTime;
    [SerializeField]
    private bool        isSpawning = false; // 생성 중인지 여부

    private Movement2D  movement;
    private PoolManager poolManager;

    private void Awake()
    {
        poolManager     = new PoolManager(effectDust);
        movement        = GetComponent<Movement2D>();

    }
    private void OnApplicationQuit()
    {
        poolManager.DestroyObjcts();
    }
    private void Update()
    {
        if (!isSpawning)
        {
            StartCoroutine("UpdateDustEffect");
        }
    }
    public IEnumerator UpdateDustEffect()
    {
        isSpawning = true;
        while (movement.rigidbody.velocity.x != 0 && movement.rigidbody.velocity.y == 0)
        {
            GameObject dustEffect = poolManager.ActivePoolItem();
            dustEffect.transform.position = parent.position;
            dustEffect.transform.SetParent(parent);
            dustEffect.GetComponent<PlayerDustEffect>().Setup(poolManager);
            yield return new WaitForSeconds(delayTime);
        }
        isSpawning = false;
    }
}

   