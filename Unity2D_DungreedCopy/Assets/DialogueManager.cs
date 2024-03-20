using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DialogueManager : MonoBehaviour, IPointerDownHandler
{
    public static DialogueManager instance;

    [SerializeField]
    private TextMeshProUGUI     textName;
    [SerializeField]        
    private TextMeshProUGUI     textDialogue;
    [SerializeField]        
    private GameObject          nextText;
    public Queue<string>        sentences;

    private string              curSentence;
    private string              curNPCName;

    [SerializeField]
    private float               typingEffectWaitTime;
    [SerializeField]
    private bool                isTyping;
    public bool                 openDialogue;
    private Animator            ani;
    [SerializeField]
    private Animator[]          buttonsAnimators;

    [Header("Ability UI")]
    [SerializeField]
    private Animator            abillityAnimator;

    [Header("Shop UI")]
    [SerializeField]
    private Animator            shopAnimator;
    [SerializeField]
    private Animator            invenAnimator;

    private NPC npc;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        sentences = new Queue<string>();
        ani = GetComponent<Animator>();
        npc = FindObjectOfType<NPC>();
    }


    public void OnDialogue(string[] lines, string name)
    {
        openDialogue = true;
        sentences.Clear();
        textName.text = name;
        curNPCName = name;

        foreach (string line in lines)
        {
            sentences.Enqueue(line);
        }
        ani.Play("Show");

        NextSentence();
    }

    public void NextSentence()
    {
        if(sentences.Count != 0)
        {
            curSentence = sentences.Dequeue();

            isTyping = true;
            nextText.SetActive(false);
            StartCoroutine(Typing(curSentence)); 
        }
        else
        {
            for (int i = 0; i < buttonsAnimators.Length; ++i)
            {
                buttonsAnimators[i].Play("ShowBottons");
            }
        }
    }

    private IEnumerator Typing(string line)
    {
        textDialogue.text = "";
        foreach(char letter in line.ToCharArray())
        {
            textDialogue.text += letter;
            yield return new WaitForSeconds(typingEffectWaitTime);
        }
    }

    private void Update()
    {
        if(textDialogue.text.Equals(curSentence))
        {
            isTyping = false;
            nextText.SetActive(true);
        }

        if(openDialogue && !isTyping)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                NextSentence();
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(!isTyping)
        {
            NextSentence();
        }
    }

    public void OnEnterButton()
    {
        if(npc.inputKey)
        {
            ani.Play("Hide");
            for (int i = 0; i < buttonsAnimators.Length; ++i)
            {
                buttonsAnimators[i].Play("HideBottons");
            }
            openDialogue = false;

            if(curNPCName == "크록")
            {
                Debug.Log("크록UI");
                shopAnimator.gameObject.SetActive(true);
                invenAnimator.gameObject.SetActive(true);
                shopAnimator.Play("ShopShow");
                invenAnimator.Play("Show");
            }
            else if (curNPCName == "카블로비나")
            {
                Debug.Log("카블로비나UI");
                abillityAnimator.gameObject.SetActive(true);
                abillityAnimator.Play("AbilityShow");
            }
        }
        
    }

    public void OnExitButton()
    {
        ani.Play("Hide");
        for (int i = 0; i < buttonsAnimators.Length; ++i)
        {
            buttonsAnimators[i].Play("HideBottons");
        }
        openDialogue = false;
        npc.inputKey = false;
    }
}
