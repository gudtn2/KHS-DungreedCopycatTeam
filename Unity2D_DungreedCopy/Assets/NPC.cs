using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("NPC�� DATA")]
    public string   name;
    public string[] sentences;

    [Header("FŰ ���� ����")]
    [SerializeField]
    private KeyCode     fKey = KeyCode.F;
    [SerializeField]
    private GameObject  keyObj; // FŰ ������Ʈ
    [SerializeField]
    private bool        onKey;
    [SerializeField]
    public bool         inputKey;

    private void Update()
    {
        if(Input.GetKeyDown(fKey) && onKey)
        {
            inputKey = true;
            onKey = false;

            PlayerController.instance.dontMovePlayer = true;

            DialogueManager.instance.OnDialogue(sentences, name);
        }
        
        keyObj.SetActive(onKey);
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
        if (collision.gameObject.name == "Player"&& !inputKey)
        {
            onKey = false;
        }
    }


}
