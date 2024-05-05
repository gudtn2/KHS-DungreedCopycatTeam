using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VillageStartPoint : MonoBehaviour
{
    [SerializeField]
    private string          startPoint;       // 시작 지점
    [SerializeField]
    private InventorySO     inventory;
    public GameObject       targetObj;        // bound오브젝트
    [SerializeField]
    private UIInventoryPage inventoryUI;
    private void OnEnable()
    {
        PlayerController player = PlayerController.instance;
        PlayerDungeonData data = PlayerDungeonData.instance;
        FadeEffectController fade = FadeEffectController.instance;
        MainCameraController cam = MainCameraController.instance;
        PlayerStats stats = PlayerStats.instance;

        if(stats != null)
            stats.ResetAllStat();

        inventoryUI.ResetAllItems();
        inventory.Initialize();

        if (data != null)
            data.ResetDungeonData();
        else return;

        player.transform.position = transform.position;

        player.isDie = false;
        player.ani.SetBool("IsDie", false);

        //PlayerController.instance.spriteRenderer.color = new Color(1, 1, 1, 1);
        //PlayerController.instance.weaponRenderer.color = new Color(1, 1, 1, 1);
        if(player!= null)
        {
            player.spriteRenderer.color = new Color(1, 1, 1, 1);
            player.weaponRenderer.color = new Color(1, 1, 1, 1);
        }

        FadeEffectController.instance.OnFade(FadeState.FadeIn);

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
