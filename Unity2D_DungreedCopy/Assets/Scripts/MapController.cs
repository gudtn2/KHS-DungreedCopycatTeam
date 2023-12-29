using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [Header("던전 맵")]
    [SerializeField]
    private GameObject[]        dungeonMaps;
    public List<string>         dungeonNames;

    public GameObject MapUI;
    public GameObject MiniMapUI;

    private bool MapOn = false;
    private bool MiniMapOn = true;

    // YS: 플레이어 컨트롤러 스크립트에서 curScenename을 받아와 Village에서는 던전맵이 켜지지 않게 하기 위함
    private PlayerController player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }
    void Update()
    {
        DontActivateDungeonMap();
        UpdateDungeonMapUI();
    }

    private void DontActivateDungeonMap()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            MapOn       = !MapOn;
            MiniMapOn   = !MiniMapOn;

            MapUI.SetActive(MapOn);
            MiniMapUI.SetActive(MiniMapOn);
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
