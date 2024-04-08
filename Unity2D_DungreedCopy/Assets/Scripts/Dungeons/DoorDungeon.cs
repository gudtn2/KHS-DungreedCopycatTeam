using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDungeon : MonoBehaviour
{
    [SerializeField]
    private GameObject[]        doors;
    [SerializeField]
    private GameObject[]        enemies;

    private List<GameObject>    curMapEnemies   = new List<GameObject>();    // 현재하는 dungeon에 있는 enemies List
    private void OnEnable()
    {
        if(enemies != null)
        {
            // enemies를 List에 삽입
            curMapEnemies.AddRange(enemies);

            CloseTheDoor();

            // EnemyDieEvent에 이벤트 핸들러 등록
            EnemyEffect.EnemyDieEvent += HandleEnemyDieEvent;
        }
        else
        {
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].SetActive(false);
            }
        }
    }
    private void OnDisable()
    {
        // 이벤트 핸들러 해제
        EnemyEffect.EnemyDieEvent -= HandleEnemyDieEvent;
    }

    private void HandleEnemyDieEvent(GameObject enemy)
    {
        // 적이 죽었을 때 리스트에서 제거
        curMapEnemies.Remove(enemy);

        // 모든 적이 죽었다면 문을 열어줌
        if (curMapEnemies.Count == 0)
        {
            OpenTheDoor();
        }
    }
    private void CloseTheDoor()
    {
        PlayerDungeonData.instance.isFighting = true;
        for (int i = 0; i < doors.Length; ++i)
        {
            // 문 닫는 이미지
            doors[i].GetComponent<Animator>().SetTrigger("CloseTheDoor");

            // 콜라이더 활성화
            doors[i].GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    private void OpenTheDoor()
    {
        PlayerDungeonData.instance.isFighting = false;
        for (int i = 0; i < doors.Length; ++i)
        {
            // 문 여는 이미지
            doors[i].GetComponent<Animator>().SetTrigger("OpenTheDoor");
            
            // 콜라이더 비활성화
            doors[i].GetComponent<BoxCollider2D>().enabled = false;
        }
    }


}
