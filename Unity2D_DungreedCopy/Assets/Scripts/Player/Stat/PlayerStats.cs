using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : StatManager
{
    private Movement2D  playerMove;
    
    private void Awake()
    {
        playerMove = GetComponent<Movement2D>();

        base.Setup();
    }
    public float            RecoverTimeDC = 3.0f;
    public float            timer; 

    public override float   MaxHP => 100;
    public override int     MaxDC => 3;

    private void Start()
    {
        StartCoroutine("RecoveryDC");
        timer = 0;
    }

    private void Update()
    {
        RecoveryDC();
    }
    public void UseDC()
    {
        DC = Mathf.Max(DC - 1, 0);
        Debug.Log("대시 횟수 감소: " + DC);
    }

    private void RecoveryDC()
    {
        if(DC < MaxDC && DashRecoveryTimerExpired())
        {
            DC = Mathf.Min(DC + 1, MaxDC);
            Debug.Log("대시 횟수 증가: " + DC);
            DashRecoveryTimerExpired();
            if(timer >= RecoverTimeDC)
            {
                timer = 0;
            }
        }
    }

    private bool DashRecoveryTimerExpired()
    {
        timer += Time.deltaTime;
        return timer >= RecoverTimeDC;
    }

}
