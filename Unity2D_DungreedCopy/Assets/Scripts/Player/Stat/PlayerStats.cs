using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HPEvent : UnityEngine.Events.UnityEvent<float, float> { }
public class DCEvect : UnityEngine.Events.UnityEvent<int, int> { }
public class PlayerStats : StatManager
{
    [HideInInspector]
    public HPEvent onHPEvent = new HPEvent();
    [HideInInspector]
    public DCEvect onDCEvent = new DCEvect();

    private PlayerController playerController;

    private void Awake()
    {
        base.Setup();
        playerController = GetComponent<PlayerController>();
    }
    public float            RecoverTimeDC = 3.0f;
    public float            timer; 

    public override float   MaxHP => 100;
    public override int     MaxDC => 3;
    public override int     MaxGOLD => 999999;

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
    }

    private void RecoveryDC()
    {
        if(DC < MaxDC && DashRecoveryTimerExpired())
        {
            DC = Mathf.Min(DC + 1, MaxDC);
            DashRecoveryTimerExpired();
            if(timer >= RecoverTimeDC)
            {
                timer = 0;
            }
        }
    }

    public void TakeGold(int value)
    {
        GOLD += value;
    }

    public bool DecreaseHP(float monAtt)
    {
        float preHP = HP;
        HP = HP - monAtt > 0 ? HP - monAtt : 0;

        onHPEvent.Invoke(preHP, HP);

        if(HP == 0)
        {
            //IsDie
            playerController.isDie = true;
            return true;
        }
        return false;
    }

    public void IncreaseHP(float heal)
    {
        float preHP = HP;

        HP = HP + heal > MaxHP ? MaxHP : HP + heal;

        onHPEvent.Invoke(preHP, HP);
    }
    private bool DashRecoveryTimerExpired()
    {
        timer += Time.deltaTime;
        return timer >= RecoverTimeDC;
    }
}
