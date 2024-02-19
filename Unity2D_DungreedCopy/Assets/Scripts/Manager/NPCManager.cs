using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// == NPC ID목록==
// 크록(던전 상점) => 10

public class NPCManager : MonoBehaviour
{
    [Header("NPC UI")]
    [SerializeField]
    private GameObject  keyObj;
    
    [Header("NPC Data")]
    public int          ID;             // Talk의 Dictionary에 쓰일 key값
    public string[]     talkSentences;
    public string       npcName;


    private KeyCode     activateChatKey = KeyCode.F;    // 대화창을 활성화 시킬 Key
    private bool        isActivateKey;                  // KeyUI를 활성/비활성화 
    private bool        inputKey;                       // key를 눌렸는지 여부
    private void Update()
    {
        keyObj.SetActive(isActivateKey);    

        // Key를 누를 수 있는 범위 내에 있으면서 Key를 누르면
        if(Input.GetKeyDown(activateChatKey) && isActivateKey)
        {
            // Talk의 Data를 TalkManager의 Dictionary에 저장 
            TalkManager.Instance.AddTalkData(ID, talkSentences);
            // Key를 누르면 KeyUI가 활성화되지 않도록
            isActivateKey = false;
            // Key활성화 로직이 활성화 되지 않도록
            inputKey      = true;

            UIManager.instance.OnTalkPanel();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player"&& !inputKey)
        {
            isActivateKey = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player"&& !inputKey)
        {
            isActivateKey = false;
        }
    }
}
