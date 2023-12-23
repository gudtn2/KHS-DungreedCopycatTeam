using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalStartPoint : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D   targetBound;            // YS: 이동한 방의 카메라 바운드
    public string           startPointDungeonName;  // YS: 스타트 지점의 맵 이름

    private PlayerController        player;
    private FadeEffectController    fade;
    private MainCameraController    mainCam;

    private void Awake()
    {
        player  = FindObjectOfType<PlayerController>();
        fade    = FindObjectOfType<FadeEffectController>();
        mainCam = FindObjectOfType<MainCameraController>();

    }
    public IEnumerator ChangePlayerPosition()
    {
        yield return new WaitForSeconds(fade.fadeTime);
            
        if (startPointDungeonName == player.curDungeonName)
        {
            // 플레이어 위치 이동
            player.transform.position = this.transform.position;

            // 카메라 위치 이동
            mainCam.transform.position = new Vector3(this.transform.position.x,
                                                     this.transform.position.y,
                                                     mainCam.transform.position.z);

            // 페이드 효과
            fade.OnFade(FadeState.FadeIn);

            // 바운드 재설정
            mainCam.SetBound(targetBound);
        }
    }
}
