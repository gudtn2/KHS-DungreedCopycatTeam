using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatsUIManager : MonoBehaviour
{
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
    private Image[]     imageDC;

    [SerializeField]
    private PlayerStats playerStats;

    private void Awake()
    {
        playerStats.onHPEvent.AddListener(UpdateImageBloodScreenAndTextHP);
    }
    private void Update()
    {
        UpdateImageDC();
        UpdateImageHP();
    }
    private void UpdateImageHP()
    {
        imageHP.fillAmount = Mathf.Lerp(imageHP.fillAmount, playerStats.HP/playerStats.MaxHP, Time.deltaTime * 5); ;
        
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
}
