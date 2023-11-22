using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("HP")]
    [SerializeField]
    private Image                   hpBar;

    [SerializeField]
    private PlayerStatsController   playerStats;

    private void Update()
    {
        hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount,playerStats.HP/playerStats.MaxHP,Time.deltaTime);
    }
}
