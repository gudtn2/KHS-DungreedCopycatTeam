using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack: MonoBehaviour
{
    [Header("ȭ�� ������ ���� ������")]
    [SerializeField]
    private GameObject  arrowPrefab;        // ������ ȭ�� ������
    private float       arrowSpeed = 50f;  // ȭ���� �ӵ�
    private Transform   arrowSpawn;

    private PoolManager arrowpoolManager;        

    private void Awake()
    {
        arrowpoolManager = new PoolManager(arrowPrefab);
        arrowSpawn = transform.GetChild(0).GetComponent<Transform>();
    }

    private void OnApplicationQuit()
    {
        arrowpoolManager.DestroyObjcts();
    }
    void Update()
    {

        PlayerController player = PlayerController.instance;

        if (Input.GetKeyDown(KeyCode.Mouse0)&& player.canAttack && !player.dontMovePlayer)
        {
            Fire();
            StartCoroutine(PlayerController.instance.AbleToAttack());
        }
    }
    void Fire()
    {
        GameObject arrow = arrowpoolManager.ActivePoolItem();
        arrow.transform.position = arrowSpawn.position;
        arrow.transform.rotation = transform.rotation;
        Rigidbody2D rigidbody = arrow.GetComponent<Rigidbody2D>();
        rigidbody.velocity = transform.right * arrowSpeed;
        arrow.GetComponent<Arrow>().Setup(arrowpoolManager);
    }
}
