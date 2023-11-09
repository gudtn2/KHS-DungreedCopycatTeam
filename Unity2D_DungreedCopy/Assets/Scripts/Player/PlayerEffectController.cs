using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectController : MonoBehaviour
{

    [Header("∏’¡ˆ ¿Ã∆Â∆Æ")]
    [SerializeField]
    private GameObject      effectDust;
    [SerializeField]
    private Transform       parentofDust;
    [SerializeField]
    private bool            isSpawningDust = false; 

   
    private Movement2D  movement;
    private PoolManager DustPoolManager;

    private void Awake()
    {
        DustPoolManager     = new PoolManager(effectDust);
        movement        = GetComponent<Movement2D>();

    }
    private void OnApplicationQuit()
    {
        DustPoolManager.DestroyObjcts();
    }
    private void Update()
    {
        if (!isSpawningDust)
        {
            StartCoroutine("UpdateDustEffect");
        }
    }
    public IEnumerator UpdateDustEffect()
    {
        isSpawningDust = true;
        while (movement.rigidbody.velocity.x != 0 && movement.rigidbody.velocity.y == 0)
        {
            GameObject dustEffect = DustPoolManager.ActivePoolItem();
            dustEffect.transform.position = parentofDust.position;
            dustEffect.transform.SetParent(parentofDust);
            dustEffect.GetComponent<PlayerDustEffect>().Setup(DustPoolManager);
            yield return new WaitForSeconds(0.3f);
        }
        isSpawningDust = false;
    }
}

