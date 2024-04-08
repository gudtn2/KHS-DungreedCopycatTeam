using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDungeon : MonoBehaviour
{
    [SerializeField]
    private DoorDungeon door;
    [SerializeField]
    private string      dungeonName;
    [SerializeField]
    private bool        haveDoor;
    public Transform    transformTeleport;


    private void Awake()
    {
        dungeonName = gameObject.name;
    }

    private void Update()
    {
        if (!haveDoor)
        {
            if (PlayerController.instance.curDungeonName == dungeonName)
            {
                transformTeleport.gameObject.SetActive(true);
            }
            else
            {
                transformTeleport.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("zzzzs");
            if (door.curMapEnemies.Count == 0)
            {
                transformTeleport.gameObject.SetActive(true);
            }
            else
            {
                transformTeleport.gameObject.SetActive(false);
            }
        }
    }
}
