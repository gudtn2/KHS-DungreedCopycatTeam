using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject MapUI;
    public GameObject MiniMapUI;
    private bool MapOn = true;
    private bool MiniMapOn = false;
    private void Awake()
    {
        
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            MapUI.SetActive(MapOn);
            MiniMapUI.SetActive(MiniMapOn);
            MapOn = !MapOn;
            MiniMapOn = !MiniMapOn;
        }
    }
}
