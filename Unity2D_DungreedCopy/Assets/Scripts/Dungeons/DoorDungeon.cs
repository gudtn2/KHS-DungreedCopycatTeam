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
    [HideInInspector]
    public List<GameObject>     curMapEnemies   = new List<GameObject>();    // �����ϴ� dungeon�� �ִ� enemies List
    [SerializeField]
    private bool                exsistTel;
    [SerializeField]
    private GameObject          curTel;

    private void OnEnable()
    {
        //if (exsistTel)
        //{
        //    curTel = this.gameObject.GetComponent<TeleportDungeon>().teleport;
        //    curTel.SetActive(false);
        //}
        //else return;

        if (enemies != null)
        {
            // enemies�� List�� ����
            curMapEnemies.AddRange(enemies);

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
    }
    private void OnDisable()
    {
        // �̺�Ʈ �ڵ鷯 ����
        EnemyEffect.EnemyDieEvent -= HandleEnemyDieEvent;
    }

    private void HandleEnemyDieEvent(GameObject enemy)
    {
        // ���� �׾��� �� ����Ʈ���� ����
        curMapEnemies.Remove(enemy);

        // ��� ���� �׾��ٸ� ���� ������
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
            doors[i].SetActive(true);

            // �� �ݴ� �̹���
            doors[i].GetComponent<Animator>().SetTrigger("CloseTheDoor");

            // �ݶ��̴� Ȱ��ȭ
            doors[i].GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    private void OpenTheDoor()
    {
        PlayerDungeonData.instance.isFighting = false;
        for (int i = 0; i < doors.Length; ++i)
        {
            // �� ���� �̹���
            doors[i].GetComponent<Animator>().SetTrigger("OpenTheDoor");
            
            // �ݶ��̴� ��Ȱ��ȭ
            doors[i].GetComponent<BoxCollider2D>().enabled = false;
        }

        //curTel.SetActive(true);
    }


}
