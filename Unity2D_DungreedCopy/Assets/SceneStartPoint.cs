using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStartPoint : MonoBehaviour
{
    public string                   startPoint;

    [SerializeField]
    private string                  dungeonName;
    [SerializeField]
    private BoxCollider2D           targetBound;

    private PlayerController        player;
    private MainCameraController    mainCam;
    private FadeEffectController    fade;
    private MapController           map;

    private void Awake()
    {
        player  = FindObjectOfType<PlayerController>();
        mainCam = FindObjectOfType<MainCameraController>();
        fade    = FindObjectOfType<FadeEffectController>();
        map     = FindObjectOfType<MapController>();
    }

    private void Start()
    {
        if (startPoint == player.curSceneName)
        {
            fade.OnFade(FadeState.FadeIn);

            mainCam.SetBound(targetBound);

            player.curDungeonName = dungeonName;

            if(player.curDungeonName == dungeonName)
            {
                if(!map.dungeonNames.Contains(dungeonName))
                {
                    map.dungeonNames.Add(dungeonName);
                    Debug.Log(dungeonName + "이 리스트에 추가됐습니다.");
                }
            }

            mainCam.transform.position = new Vector3(transform.position.x,
                                                     transform.position.y,
                                                     mainCam.transform.position.z);

            player.transform.position = this.transform.position;

        }
    }
}
