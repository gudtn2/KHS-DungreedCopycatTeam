using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [Header("´øÀü ¸Ê")]
    [SerializeField]
    private GameObject[]        dungeonMaps;
    public List<string>         dungeonNames;

    public GameObject MapUI;

    private bool MapOn = false;
    void Update()
    {
        DontActivateDungeonMap();
        UpdateDungeonMapUI();

        if(MapOn)   PlayerController.instance.onUI = true;
        else        PlayerController.instance.onUI = false;
    }
    private void DontActivateDungeonMap()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            MapOn       = !MapOn;

            MapUI.SetActive(MapOn);
        }
    }

    private void UpdateDungeonMapUI()
    {
        for (int i = 0; i < dungeonMaps.Length; ++i)
        {
            if(dungeonNames.Contains(dungeonMaps[i].name))
            {
                dungeonMaps[i].SetActive(true);
            }
            else
            {
                dungeonMaps[i].SetActive(false);
            }

        }
    }
}
