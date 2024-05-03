using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("NPC의 DATA")]
    public string   name;
    public string[] sentences;

    [SerializeField]
    private GameObject  keyObj;             // F키 오브젝트
    private KeyCode     fKey = KeyCode.F;
    private bool        onKey;              // 키가 화면상에 보이는지 확인하는 변수 
    public bool         inputKey;           // 키를 눌렀는지 확인하기 위한 변수

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
