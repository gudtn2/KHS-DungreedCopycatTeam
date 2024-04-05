using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    static public UIManager instance;

    [Header("LV")]
    [SerializeField]
    private TextMeshProUGUI textLV;
    
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
    [SerializeField]
    private GameObject      textNoGold;

    [Header("Acquired Item")]
    [SerializeField]
    private GameObject      AcquiredItemUI;         // 아이템을 먹을때 활성/비활성화
    [SerializeField]
    private Image           imageAcquiredItem;      // 얻은 아이템 이미지
    [SerializeField]
    private TextMeshProUGUI textAcquiredItemName;   // 얻은 아이템 이름

    [SerializeField]
    private PlayerStats         playerStats;

    [SerializeField]
    private GameObject menuUI;   // 메뉴창
    [SerializeField]
    private bool menuUIon;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);

            playerStats.onHPEvent.AddListener(UpdateImageBloodScreenAndTextHP);


            textNoGold.SetActive(false);
            AcquiredItemUI.SetActive(false);
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
        UpdateTextLV();
        OnMenuUI();

        textHP.text = (int)playerStats.HP + "/" + (int)playerStats.MaxHP;
    }
    private void UpdateImageHP()
    {
        imageHP.fillAmount = Mathf.Lerp(imageHP.fillAmount, playerStats.HP/playerStats.MaxHP, Time.deltaTime * 5);
        
    }

    public void UpdateTextNoGold(bool onText)
    {
        textNoGold.SetActive(onText);
    }

    private void UpdateTextGold()
    {
        textGOLD.text = playerStats.GOLD.ToString();
    }
    private void UpdateTextLV()
    {
        textLV.text = playerStats.LV.ToString();
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
    public void OnAcquiredItem(string itemName, Sprite itemImage)
    {
        // UI 활성화
        AcquiredItemUI.SetActive(true);

        // 이미지 적용
        imageAcquiredItem.sprite = itemImage;

        // 이미지 사이즈 원본 사이즈에 맞게
        imageAcquiredItem.SetNativeSize();

        // 이름 적용
        textAcquiredItemName.text = itemName;

        // 비활성화 코루틴
        StartCoroutine(OffAcquiredItem());
    }

    private IEnumerator OffAcquiredItem()
    {
        yield return new WaitForSeconds(3f);
        // UI 비활성화
        AcquiredItemUI.SetActive(false);
    }

    public void OnMenuUI()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && DialogueManager.instance.onShop == false)
        {
            menuUI.SetActive(menuUIon);
            menuUIon = !menuUIon;
        }
    }

    public void OnMenuUIButton()
    {
        menuUI.SetActive(menuUIon);
        menuUIon = !menuUIon;
    }
}
