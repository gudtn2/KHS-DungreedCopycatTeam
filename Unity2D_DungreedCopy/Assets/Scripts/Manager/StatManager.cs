using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static StatManager Instace { get; private set; }

    // 기본 stat
    public int      maxHP       { get; set; } = 100;
    public float    maxStemina  { get; set; } = 100;
    public int      maxDash     { get; set; } = 2;
    public int      baseAtt     { get; set; } = 10;

    // 현재 stat
    public int      curHP        { get; private set; }
    public float    curStemina   { get; private set; }
    public int      curDash      { get; private set; } 
    public int      curAtt       { get; private set; }

    private void Awake()
    {
        if(Instace == null)
        {
            Instace = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        curHP       = maxHP;
        curStemina  = maxStemina;
        curAtt      = baseAtt;
        curDash     = maxDash;
    }

    public void SteminaSetting()
    {
        if(curStemina < maxStemina)
        {

        }
    }



}
