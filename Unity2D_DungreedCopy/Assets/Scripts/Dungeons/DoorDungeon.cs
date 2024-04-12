using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDungeon : MonoBehaviour
{
    [SerializeField]
    private GameObject[]        doors;
    public List<GameObject>     curMapEnemies   = new List<GameObject>();    // 현재하는 dungeon에 있는 enemies List
    [SerializeField]
    private bool                exsistTel;
    [SerializeField]
    private GameObject          curTel;
    [SerializeField]
    private int                 enemiesCount;

    private void Awake()
    {
        foreach(Transform child in transform)
        {
            if(child.CompareTag("Enemy"))
            {
                curMapEnemies.Add(child.gameObject);
            }
        }

        enemiesCount = curMapEnemies.Count;
    }
    private void OnEnable()
    {

        if (enemiesCount > 0)
        {
            PlayerDungeonData.instance.isFighting = true;

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
        
        // dungeon에 Teleport가 존재하면
        if (exsistTel)
        {
            curTel = this.gameObject.GetComponent<TeleportDungeon>().teleport;
            curTel.SetActive(false);
        }
        else
        {
            curTel.SetActive(true);
        }
    }
    private void OnDisable()
    {
    }

    private void Update()
    {
        if(PlayerController.instance.curDungeonName != this.gameObject.name)
        {
            // 이벤트 핸들러 해제
            EnemyEffect.EnemyDieEvent -= this.GetComponent<DoorDungeon>().HandleEnemyDieEvent;
        }
    }

    public void HandleEnemyDieEvent(GameObject enemy)
    {
        // 적이 죽었을 때 리스트에서 제거
        curMapEnemies.Remove(enemy);
        enemiesCount -= 1;

        // 모든 적이 죽었다면 문을 열어줌
        if (enemiesCount == 0)
        {
            OpenTheDoor();

            if (exsistTel)
            {
                curTel.SetActive(true);
                Debug.Log("던전 내 모든 적이 죽었습니다.");
            }
            else return;
        }
    }
    private void CloseTheDoor()
    {
        for (int i = 0; i < doors.Length; ++i)
        {
            doors[i].SetActive(true);

            // 문 닫는 이미지
            doors[i].GetComponent<Animator>().SetTrigger("CloseTheDoor");

            // 콜라이더 활성화
            doors[i].GetComponent<BoxCollider2D>().enabled = true;
        }
        PlayerDungeonData.instance.isFighting = true;
    }

    private void OpenTheDoor()
    {
        for (int i = 0; i < doors.Length; ++i)
        {
            // 문 여는 이미지
            doors[i].GetComponent<Animator>().SetTrigger("OpenTheDoor");
            
            // 콜라이더 비활성화
            doors[i].GetComponent<BoxCollider2D>().enabled = false;
        }
        PlayerDungeonData.instance.isFighting = false;
    }


}
