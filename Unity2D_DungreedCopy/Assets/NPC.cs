using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("NPC�� DATA")]
    public string   name;
    public string[] sentences;

    [SerializeField]
    private GameObject  keyObj;             // FŰ ������Ʈ
    private KeyCode     fKey = KeyCode.F;
    private bool        onKey;              // Ű�� ȭ��� ���̴��� Ȯ���ϴ� ���� 
    public bool         inputKey;           // Ű�� �������� Ȯ���ϱ� ���� ����

    private void Update()
    {
        if (Input.GetKeyDown(fKey) && onKey && !inputKey)
        {
            DialogueManager dialogue = DialogueManager.instance;
            
            inputKey = true;
            onKey = false;

            PlayerController.instance.dontMovePlayer = true;

            if (dialogue != null)
            {
                dialogue.gameObject.SetActive(true);
                dialogue.OnDialogue(sentences, name);
            }
            else
            {
                dialogue = GameObject.Find("MainCanvas").transform.GetChild(5).GetComponent<DialogueManager>();
                dialogue.gameObject.SetActive(true);
                dialogue.OnDialogue(sentences, name);
            }
        }

        keyObj.SetActive(onKey);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && !inputKey)
        {
            onKey = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && !inputKey)
        {
            onKey = false;
            inputKey = false;
        }
    }


}
