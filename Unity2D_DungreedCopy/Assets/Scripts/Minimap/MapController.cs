using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{

    [Header("던전 맵")]
    [SerializeField]
    private GameObject[] dungeonMaps;
    public List<string> dungeonNames;

    [Header("Teleport")]
    public GameObject startTeleport;
    public GameObject targetTeleport;

    public GameObject MapUI;
    public bool MapOn = false;

    void Update()
    {
        DontActivateDungeonMap();
        UpdateDungeonMapUI();
        MapUI.SetActive(MapOn);
    }


    private void DontActivateDungeonMap()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (PlayerDungeonData.instance.isFighting)
            {
                UIManager.instance.StartCoroutine("OnNotificationTxt");
                return;
            }
            MapOn = true;
            PlayerController.instance.dontMovePlayer = true;
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            if (startTeleport == null)
            {
                MapOn = false;
                PlayerController.instance.dontMovePlayer = false;
            }
            else
            {
                if (!startTeleport.GetComponent<TeleportController>().inputKey)
                {
                    MapOn = false;
                    PlayerController.instance.dontMovePlayer = false;
                }
            }
        }
    }

    public void OffDungeonMap()
    {
        MapOn = false;
        PlayerController.instance.dontMovePlayer = false;
        startTeleport.GetComponent<TeleportController>().inputKey = false;
    }

    private void UpdateDungeonMapUI()
    {
        for (int i = 0; i < dungeonMaps.Length; ++i)
        {
            if (dungeonNames.Contains(dungeonMaps[i].name))
            {
                dungeonMaps[i].SetActive(true);
            }
            else
            {
                dungeonMaps[i].SetActive(false);
            }

        }
    }

    public IEnumerator ChangePosPlayer()
    {

        // 타겟이 되는 텔레포트의 위치 받아오기
        Transform targetTelPos = targetTeleport.GetComponent<TeleportDungeon>().transformTeleport;
        // 타겟이 되는 던전의 이름 
        DungeonName targetDungeonName = targetTeleport.GetComponent<DungeonName>();



        yield return new WaitForSeconds(FadeEffectController.instance.fadeTime);

        startTeleport.GetComponent<TeleportController>().inputKey = false;
        startTeleport.GetComponent<Animator>().SetBool("EatPlayer", false);

        // 던전 정보 재설정
        PlayerController.instance.curDungeonName = targetDungeonName.dungeonName;
        PlayerController.instance.curDungeonNum = targetDungeonName.dungeonNum;

        if (PlayerController.instance.curDungeonName == targetDungeonName.dungeonName)
        {
            // 페이드 인 효과 시작
            FadeEffectController.instance.OnFade(FadeState.FadeIn);

            // 플레이어 이동
            PlayerController.instance.transform.position = targetTelPos.position;

            // 플레이어 이동 가능
            PlayerController.instance.dontMovePlayer = false;

            // a 초기화
            PlayerController.instance.spriteRenderer.color = new Color(1, 1, 1, 1);
            PlayerController.instance.weaponRenderer.color = new Color(1, 1, 1, 1);

            MainCameraController.instance.transform.position = new Vector3(targetTelPos.position.x,
                                                                           targetTelPos.position.y,
                                                                           MainCameraController.instance.transform.position.z);
            GameObject targetObject = GameObject.Find(PlayerController.instance.curDungeonName);

            if (targetObject != null)
            {
                BoxCollider2D targetBound = targetObject.GetComponent<BoxCollider2D>();

                // 바운드 재설정
                MainCameraController.instance.SetBound(targetBound);
            }
            else
            {
                Debug.LogWarning("Target object with the specified name not found.");
            }
        }


    }


}
