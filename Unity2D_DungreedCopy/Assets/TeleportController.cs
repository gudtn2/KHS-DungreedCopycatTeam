using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    [SerializeField]
    private GameObject  prefabKey;
    private KeyCode     fKey = KeyCode.F;
    private Vector3     keyPos;
    private bool        onKey;
    public bool        inputKey;

    private GameObject      instantiatedKey;
    private PoolManager     keyPoolManager;
    private MapController   mapController;

    private void Awake()
    {
        keyPoolManager = new PoolManager(prefabKey);

        mapController = FindObjectOfType<MapController>();

        keyPos = new Vector3(-0.2f, 1.5f);
        CreateKey();
    }

    private void Update()
    {
        if(onKey && Input.GetKeyDown(fKey))
        {
            mapController.MapOn = true;
            inputKey = true;
            onKey = false;

            mapController.startTeleport = this.gameObject;
        }
        instantiatedKey.SetActive(onKey);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player" && !inputKey)
        {
            onKey = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player" && !inputKey)
        {
            onKey = false;
        }
    }
    private void CreateKey()
    {
        Vector3 keyPosition = new Vector3(transform.position.x + keyPos.x, transform.position.y + keyPos.y);
        instantiatedKey = keyPoolManager.ActivePoolItem();
        instantiatedKey.transform.position = keyPosition;
        instantiatedKey.transform.rotation = transform.rotation;
        instantiatedKey.GetComponent<EffectPool>().Setup(keyPoolManager);
    }

    // 먹을때 발동되는 애니메이션 이벤트
    public void HidePlayer()
    {
        PlayerController.instance.spriteRenderer.color = new Color(1, 1, 1, 0);
        PlayerController.instance.weaponRenderer.color = new Color(1, 1, 1, 0);

        // 페이드 아웃 효과 시작
        FadeEffectController.instance.OnFade(FadeState.FadeOut);

        // ＠
        StartCoroutine(mapController.ChangePosPlayer());
    }

    
}
