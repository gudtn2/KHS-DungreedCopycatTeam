using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeponInfo : MonoBehaviour
{
    [Header("무기 정보")]
    [SerializeField]
    private int     minATK,maxATK;
    
    [HideInInspector]
    public int      curATK;
    [HideInInspector]
    public Color    textColor;

    private PlayerStats stats;
    private System.Random random = new System.Random();

    private void Awake()
    {
        stats = FindObjectOfType<PlayerStats>();

        CalculateDamage();
    }
    private void CalculateDamage()
    {
        // 랜덤 공격력 계산
        int randomATK = Random.Range(minATK, maxATK + 1);

        //크리티컬 발동시
        if (IsCritical())
        {
            // 크리티컬시 공격력 = 최대 무기피해 + (최대무기피해 * 0.5) + 플레이어 공격력
            curATK = maxATK + (int)(maxATK * 0.5f) + stats.ATK;
            Debug.Log("크리티컬 발동!. 공격력: " + curATK);
            textColor = Color.yellow;
        }
        else
        {
            // 일반공격시 공격력 = 최대 무기피해 + 플레이어 공격력
            curATK = randomATK + stats.ATK;
            Debug.Log("공격력: " + curATK);
            textColor = Color.red;
        }
    }

    public bool IsCritical()
    {
        return (random.NextDouble() < stats.CRI);
    }
}
