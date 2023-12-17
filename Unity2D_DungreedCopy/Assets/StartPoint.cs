using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public string startPoint;

    private PlayerController        player;
    private MainCameraController    mainCam;
    private FadeEffectController    fade;

    private void Awake()
    {
        player  = FindObjectOfType<PlayerController>();
        mainCam = FindObjectOfType<MainCameraController>();
        fade    = FindObjectOfType<FadeEffectController>();

        if (startPoint == player.curMapName)
        {
            fade.OnFade(FadeState.FadeIn);

            mainCam.transform.position = new Vector3(transform.position.x,
                                                     transform.position.y,
                                                     mainCam.transform.position.z);
            
            player.transform.position = this.transform.position;
        }
    }

    void Update()
    {
        
    }
}
