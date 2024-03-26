using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonName : MonoBehaviour
{
    public string dungeonName;
    public int    dungeonNum;
    public int    curEnemyCount;
    public int    maxEnemyCount;
    public bool   isVisited;
    public bool   canGoToNext;

    private GameObject thisObj; 

    private MapController map;

    private void Awake()
    {
        dungeonName = this.gameObject.name;

        map = FindObjectOfType<MapController>();
    }

    private void Update()
    {
        if(map.dungeonNames.Contains(PlayerController.instance.curDungeonName))
        {
            thisObj = GameObject.Find(PlayerController.instance.curDungeonName);
            DungeonName thisDungeon = thisObj.GetComponent<DungeonName>();
            
            thisDungeon.isVisited = true;
            if (Input.GetKeyDown(KeyCode.K))
            {
                thisDungeon.curEnemyCount++;
                
                if (thisDungeon.curEnemyCount > maxEnemyCount)
                {
                    thisDungeon.canGoToNext = true;
                }
            }
        }
    }

}
