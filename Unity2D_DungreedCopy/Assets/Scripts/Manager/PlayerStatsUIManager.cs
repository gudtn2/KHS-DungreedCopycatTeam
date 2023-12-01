using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUIManager : MonoBehaviour
{
    [Header("HP")]
    [SerializeField]
    private Image       imageHP;
    
    [Header("DC")]
    [SerializeField]
    private Image[]     imageDC;

    [SerializeField]
    private PlayerStats playerStats;

    private void Update()
    {
        UpdateImageDC();
    }
    private void UpdateImageDC()
    {
        for (int i = 0; i < imageDC.Length; ++i)
        {
            float fillAmount = i >= playerStats.DC ? 0f : 1f;
            imageDC[i].fillAmount = Mathf.Lerp(imageDC[i].fillAmount, fillAmount, Time.deltaTime * 5f); ;
        }
    }
}
