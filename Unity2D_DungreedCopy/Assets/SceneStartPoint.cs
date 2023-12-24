using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStartPoint : MonoBehaviour
{
    public string                   startPoint;
    [SerializeField]
    private BoxCollider2D           targetBound;

    private PlayerController        player;
    private MainCameraController    mainCam;
    private FadeEffectController    fade;

    private void Awake()
    {
        player  = FindObjectOfType<PlayerController>();
        mainCam = FindObjectOfType<MainCameraController>();
        fade    = FindObjectOfType<FadeEffectController>();

       
    }

    private void Start()
    {
        if (startPoint == player.curSceneName)
        {
            fade.OnFade(FadeState.FadeIn);

            mainCam.SetBound(targetBound);

            player.curDungeonName = "dungeon00";

            mainCam.transform.position = new Vector3(transform.position.x,
                                                     transform.position.y,
                                                     mainCam.transform.position.z);

            player.transform.position = this.transform.position;

        }
    }
}
