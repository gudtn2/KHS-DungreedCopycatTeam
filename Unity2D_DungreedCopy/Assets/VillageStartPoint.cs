using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageStartPoint : MonoBehaviour
{
    public string startPoint;

    private void Awake()
    {
        PlayerController player     = PlayerController.instance;
        FadeEffectController fade   = FadeEffectController.instance;
        MainCameraController cam    = MainCameraController.instance;
        
        if (startPoint == player.curSceneName)
        {
            player.transform.position = transform.position;

            PlayerController.instance.isDie = false;
            PlayerController.instance.ani.SetBool("IsDie",false);

            PlayerController.instance.spriteRenderer.color = new Color(1, 1, 1, 1);
            PlayerController.instance.weaponRenderer.color = new Color(1, 1, 1, 1);

            PlayerStats.instance.ResetAllStat();

            fade.OnFade(FadeState.FadeIn);

            GameObject targetObject = GameObject.Find("Bound");
            if (targetObject != null)
            {
                BoxCollider2D targetBound = targetObject.GetComponent<BoxCollider2D>();

                // 바운드 재설정
                cam.SetBound(targetBound);
            }
            else
            {
                Debug.LogWarning("Target object with the specified name not found.");
            }
        }
    }
}
