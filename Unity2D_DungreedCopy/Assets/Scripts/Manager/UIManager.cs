using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    static public UIManager instance;

    [Header("HP")]
    [SerializeField]
    private Image           imageHP;
    [SerializeField]
    private Image           imageBloodScreen;
    [SerializeField]
    private AnimationCurve  curveBloodScreen;
    [SerializeField]
    private TextMeshProUGUI textHP;
    
    [Header("DC")]
    [SerializeField]
    private Image[]         imageDC;

    [Header("GOLD")]
    [SerializeField]
    private TextMeshProUGUI textGOLD;

    [Header("Talk")]
    [SerializeField]
    private GameObject      panelTalk;
    [SerializeField]
    private TextMeshProUGUI textName;
    [SerializeField]
    private TypingEffect    talk;
    private GameObject      scannedObj;
    private int             talkIndex = 0;
    [HideInInspector]
    public bool             onTalk;

    [SerializeField]
    private PlayerStats         playerStats;

    private PlayerController    player;

    private void Awake()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(gameObject);

            playerStats.onHPEvent.AddListener(UpdateImageBloodScreenAndTextHP);

            player = FindObjectOfType<PlayerController>();
            talk = FindObjectOfType<TypingEffect>();

            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Update()
    {
        UpdateImageDC();
        UpdateImageHP();
        UpdateTextGold();

        panelTalk.SetActive(onTalk);

        if(onTalk && Input.GetKeyDown(KeyCode.Space))
        {
            talkIndex++;
        }
    }
    private void UpdateImageHP()
    {
        imageHP.fillAmount = Mathf.Lerp(imageHP.fillAmount, playerStats.HP/playerStats.MaxHP, Time.deltaTime * 5);
        
    }

    private void UpdateTextGold()
    {
        textGOLD.text = playerStats.GOLD.ToString();
    }

    private void UpdateImageDC()
    {
        for (int i = 0; i < imageDC.Length; ++i)
        {
            float fillAmount = i >= playerStats.DC ? 0f : 1f;
            imageDC[i].fillAmount = Mathf.Lerp(imageDC[i].fillAmount, fillAmount, Time.deltaTime * 5f); ;
        }
    }

    public IEnumerator OnBloodScreen()
    {
        float percent = 0;

        while(percent < 1)
        {
            percent += Time.deltaTime;

            Color color = imageBloodScreen.color;
            color.a = Mathf.Lerp(1, 0, curveBloodScreen.Evaluate(percent));
            imageBloodScreen.color = color;

            yield return null;
        }
    }

   public void UpdateImageBloodScreenAndTextHP(float pre, float cur)
    {
        textHP.text = (int)playerStats.HP + "/"  + (int)playerStats.MaxHP;

        if (pre <= cur) return;
        
        if (pre - cur > 0)
        {
            StopCoroutine(OnBloodScreen());
            StartCoroutine(OnBloodScreen());
        }
    }

    
    public void OnTalkPanel()
    {
        onTalk = true;
    }
    public void OffTalkPanel()
    {
        onTalk = false;
    }

    public void OnTalk(GameObject newScannedObj)
    {
        // 스캔할 오브젝트를 해당 오브젝트로 선정
        scannedObj = newScannedObj;

        // npcData에 스캔한 오브젝트의 데이터 집어넣음
        NPCManager npcData = scannedObj.GetComponent<NPCManager>();

        // 이름text에 스캔한 오브젝트의 이름 기입
        textName.text = npcData.npcName;

        // 플레이어를 움직이지 못하게 
        player.onUI = true;

        Talk(npcData.ID);
    }

    private void Talk(int id)
    {
        string  talkData    = TalkManager.Instance.GetTalk(id, talkIndex);

        if (talkData == null) return;

        talk.SetMSG(talkData);
        talkIndex++;
    }
}
