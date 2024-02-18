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
    [SerializeField]
    private int         ID;             // Talk의 Dictionary에 쓰일 key값
    [SerializeField]
    private string[]    talkSentences;
    [SerializeField]
    private string      name;


    private KeyCode     activateChatKey = KeyCode.F;    // 대화창을 활성화 시킬 Key
    private bool        isActivateKey;                  // KeyUI를 활성/비활성화 
    private bool        inputKey;                       // key를 눌렸는지 여부
    
    //private TalkManager talkManager;

    private void Awake()
    {
        //talkManager = FindObjectOfType<TalkManager>();
        
        //talkManager.AddTalkData(ID, talkSentences);
        
    }
    private void Update()
    {
        keyObj.SetActive(isActivateKey);    

        // Key를 누를 수 있는 범위 내에 있으면서 Key를 누르면
        if(Input.GetKeyDown(activateChatKey) && isActivateKey)
        {
            // Talk의 Data를 TalkManager의 Dictionary에 저장 
            TalkManager.Instance.AddTalkData(ID, name, talkSentences);
            // Key를 누르면 KeyUI가 활성화되지 않도록
            isActivateKey = false;
            // Key활성화 로직이 활성화 되지 않도록
            inputKey      = true;
            
            // UIManager호출해 문자열 실제로 띄움
            UIManager.instance.OnTalk(name,this.gameObject);
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
