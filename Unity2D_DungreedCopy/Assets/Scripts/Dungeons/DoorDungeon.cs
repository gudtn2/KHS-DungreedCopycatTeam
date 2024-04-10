using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDungeon : MonoBehaviour
{
    [SerializeField]
    private GameObject[]        doors;
    public List<GameObject>     curMapEnemies   = new List<GameObject>();    // �����ϴ� dungeon�� �ִ� enemies List
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

            // EnemyDieEvent�� �̺�Ʈ �ڵ鷯 ���
            EnemyEffect.EnemyDieEvent += HandleEnemyDieEvent;
        }
        else
        {
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].SetActive(false);
            }
        }
        
        // dungeon�� Teleport�� �����ϸ�
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
            // �̺�Ʈ �ڵ鷯 ����
            EnemyEffect.EnemyDieEvent -= this.GetComponent<DoorDungeon>().HandleEnemyDieEvent;
        }
    }

    public void HandleEnemyDieEvent(GameObject enemy)
    {
        // ���� �׾��� �� ����Ʈ���� ����
        curMapEnemies.Remove(enemy);
        enemiesCount -= 1;

        // ��� ���� �׾��ٸ� ���� ������
        if (enemiesCount == 0)
        {
            OpenTheDoor();

            if (exsistTel)
            {
                curTel.SetActive(true);
                Debug.Log("���� �� ��� ���� �׾����ϴ�.");
            }
            else return;
        }
    }
    private void CloseTheDoor()
    {
        for (int i = 0; i < doors.Length; ++i)
        {
            doors[i].SetActive(true);

            // �� �ݴ� �̹���
            doors[i].GetComponent<Animator>().SetTrigger("CloseTheDoor");

            // �ݶ��̴� Ȱ��ȭ
            doors[i].GetComponent<BoxCollider2D>().enabled = true;
        }
        PlayerDungeonData.instance.isFighting = true;
    }

    private void OpenTheDoor()
    {
        for (int i = 0; i < doors.Length; ++i)
        {
            // �� ���� �̹���
            doors[i].GetComponent<Animator>().SetTrigger("OpenTheDoor");
            
            // �ݶ��̴� ��Ȱ��ȭ
            doors[i].GetComponent<BoxCollider2D>().enabled = false;
        }
        PlayerDungeonData.instance.isFighting = false;
    }


}
