using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDungeon : MonoBehaviour
{
    [SerializeField]
    private GameObject[] doors;                                     // ������ �ִ� ��
    public List<GameObject> curMapEnemies = new List<GameObject>();    // �����ϴ� dungeon�� �ִ� enemies List
    [SerializeField]
    private bool exsistTel;      // Tel�� ���� dungeon�� �����ϴ��� ����                     
    [SerializeField]
    private GameObject curTel;         // ����  dungeon�� Tel
    public int enemiesCount;   // ���� ���� ��

    private void Awake()
    {
        StartCoroutine(OpenTheDoor());
    }
    private void OnEnable()
    {
        // UI���Ѱ�
        PlayerDungeonData.instance.isFighting = true;

        // Ȱ��ȭ ���ڸ��� �� ��ױ�
        CloseTheDoor();

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

    private IEnumerator OpenTheDoor()
    {
        while (true)
        {
            if(enemiesCount == 0)
            {
                for (int i = 0; i < doors.Length; ++i)
                {
                    // �� ���� �̹���
                    doors[i].GetComponent<Animator>().SetTrigger("OpenTheDoor");

                    // �ݶ��̴� ��Ȱ��ȭ
                    doors[i].GetComponent<BoxCollider2D>().enabled = false;
                    PlayerDungeonData.instance.isFighting = false;
                }

            }
            yield return null;
        }
    }


}
