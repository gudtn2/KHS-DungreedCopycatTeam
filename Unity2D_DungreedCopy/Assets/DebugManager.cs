using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    private static DebugManager instance;

    // 싱글톤 인스턴스에 접근할 수 있는 프로퍼티
    public static DebugManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우 생성
            if (instance == null)
            {
                GameObject debugManager = new GameObject("DebugManager");
                instance = debugManager.AddComponent<DebugManager>();
            }
            return instance;
        }
    }

    public int targetPosInQuadrant;

    public void DebugTargetQuadrant(Vector2 targetPos)
    {
        // 스크린의 중심 좌표
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        // 마우스의 스크린 좌표를 중심을 기준으로 상하좌우 어느 쪽인지 확인합니다.
        bool isTargetRight = targetPos.x > screenCenter.x;
        bool isTargetLeft = !isTargetRight;
        bool isTargetUp = targetPos.y > screenCenter.y;
        bool isTargetDown = !isTargetUp;

        // 마우스의 위치가 어느 사사분면에 있는지 디버그 로그로 출력합니다.
        if (isTargetRight && isTargetUp)
        {
            Debug.Log("Target이 우상단에 위치합니다.");
            targetPosInQuadrant = 1;
        }
        else if (isTargetLeft && isTargetUp)
        {
            Debug.Log("Target이 좌상단에 위치합니다.");
            targetPosInQuadrant = 2;
        }
        else if (isTargetLeft && isTargetDown)
        {
            Debug.Log("Target이 좌하단에 위치합니다.");
            targetPosInQuadrant = 3;
        }
        else if (isTargetRight && isTargetDown)
        {
            Debug.Log("Target이 우하단에 위치합니다.");
            targetPosInQuadrant = 4;
        }
    }
}
