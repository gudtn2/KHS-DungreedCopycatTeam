using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Stats
{
    [HideInInspector]
    public float    HP;     // 플레이어 체력
    [HideInInspector]       
    public int      DC;     // 플레이어 대시 카운트
}

public abstract class StatManager : MonoBehaviour
{
    private Stats       stats;                // 캐릭터 정보

    public float HP
    {
        set => stats.HP = Mathf.Clamp(value, 0, MaxHP);
        get => stats.HP;
    }
    public int DC
    {
        set => stats.DC = Mathf.Clamp(value, 0, MaxDC);
        get => stats.DC;
    }

    public abstract float       MaxHP { get; }              // 최대 체력
    public abstract int         MaxDC { get; }              // 최대 대시 카운트
    
    public void Setup()
    {
        HP = MaxHP;
        DC = MaxDC;
    }
}
