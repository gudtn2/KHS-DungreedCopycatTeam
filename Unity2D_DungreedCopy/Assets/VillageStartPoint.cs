using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageStartPoint : MonoBehaviour
{
    public string startPoint;
    public GameObject targetObj;

    private void OnEnable()
    {
        PlayerController player = PlayerController.instance;
        PlayerDungeonData data = PlayerDungeonData.instance;
        FadeEffectController fade = FadeEffectController.instance;
        MainCameraController cam = MainCameraController.instance;

        if (startPoint == player.curSceneName)
        {
            data.ResetDungeonData();

            player.transform.position = transform.position;

            player.isDie = false;
            player.ani.SetBool("IsDie", false);

            if (player.spriteRenderer.color == Color.white) return;
            else
            {
                player.spriteRenderer.color = new Color(1, 1, 1, 1);
                player.weaponRenderer.color = new Color(1, 1, 1, 1);
            }

            PlayerStats.instance.ResetAllStat();

            fade.OnFade(FadeState.FadeIn);

            if (targetObj != null)
            {
                BoxCollider2D targetBound = targetObj.GetComponent<BoxCollider2D>();

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
