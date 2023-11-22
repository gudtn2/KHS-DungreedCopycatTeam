using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Stats
{
    [HideInInspector]
    public float HP;
    [HideInInspector]
    public float STEMINA;
    [HideInInspector]
    public int   DashCount;
}
public abstract class Entity : MonoBehaviour
{
    private Stats       stats;      // 캐릭터 정보
    public  Entity      target;     // 공격 대상

    public float HP
    {
        set => stats.HP = Mathf.Clamp(value, 0, MaxHP);
        get => stats.HP;
    }
    public float STEMINA
    {
        set => stats.STEMINA = Mathf.Clamp(value, 0, MaxSTEMINA);
        get => stats.STEMINA;
    }

    public int DashCount
    {
        set => stats.DashCount = Mathf.Clamp(value, 0, MaxDashCount);
        get => stats.DashCount;
    }

    public abstract float MaxHP { get; }               // 최대 체력
    public abstract float MaxSTEMINA { get; }          // 최대 스테미나
    public abstract float RecoverySTEMINA { get; }     // 스테미나 초당 회복량
    public abstract float consumptionSTEMINA { get; }  // 스테미나 초당 회복량
    public abstract int   MaxDashCount{ get; }         // 최대 대시 카운트

    protected void Setup()
    {
        HP = MaxHP;
        STEMINA = MaxSTEMINA;
        DashCount = MaxDashCount;

        StartCoroutine("Recovery");
    }

    // YS: 초당 체력 회복
    protected IEnumerator Recovery()
    {
        while(true)
        {
            if (STEMINA < MaxSTEMINA) STEMINA += RecoverySTEMINA;

            yield return new WaitForSeconds(3);
        }
    }

    public abstract void ConsumptionSteminaAndCount(float consumptionSTEMINA, int consumptionDashCount);

    public abstract void TakeDamage(float damage);

}
