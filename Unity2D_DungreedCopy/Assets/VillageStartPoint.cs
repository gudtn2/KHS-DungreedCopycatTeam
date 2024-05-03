using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageStartPoint : MonoBehaviour
{
    [SerializeField]
    private string startPoint;     // ���� ����
    public GameObject targetObj;      // bound������Ʈ

    private void OnEnable()
    {
        PlayerController player = PlayerController.instance;
        PlayerDungeonData data = PlayerDungeonData.instance;
        FadeEffectController fade = FadeEffectController.instance;
        MainCameraController cam = MainCameraController.instance;

        //PlayerStats.instance.ResetAllStat();

        if (data != null)
            data.ResetDungeonData();
        else return;

        player.transform.position = transform.position;

        player.isDie = false;
        player.ani.SetBool("IsDie", false);

        PlayerController.instance.spriteRenderer.color = new Color(1, 1, 1, 1);
        PlayerController.instance.weaponRenderer.color = new Color(1, 1, 1, 1);

        FadeEffectController.instance.OnFade(FadeState.FadeIn);

        if (targetObj != null)
        {
            BoxCollider2D targetBound = targetObj.GetComponent<BoxCollider2D>();

            // �ٿ�� �缳��
            cam.SetBound(targetBound);
        }
        else
        {
            Debug.LogWarning("Target object with the specified name not found.");
        }
    }
}
