using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    static public MainCameraController instance;

    [SerializeField]
    private Transform   player;
    [SerializeField]
    private float       smooting = 0.2f;

    private void Start()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(player.position.x, player.position.y, this.transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, smooting);
    }
}
