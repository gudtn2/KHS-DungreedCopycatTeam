using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsController : Entity
{
    public override float       MaxHP => 100;
    public override float       MaxSTEMINA => 100;
    public override float       RecoverySTEMINA => (float)((int)MaxSTEMINA / MaxDashCount);
    public override float       consumptionSTEMINA => (float)((int)MaxSTEMINA / MaxDashCount);
    public override int         MaxDashCount => 3;

    public float                temp;


    private void Awake()
    {
        base.Setup();
    }

    private void Update()
    {
        Debug.Log(STEMINA);
        Debug.Log("CurDashCount :"+ DashCount);

        if(STEMINA < (float)((int)MaxSTEMINA/MaxDashCount) && STEMINA >0)
        {
            DashCount = 0;
        }
        else if(STEMINA < (float)((int)MaxSTEMINA / MaxDashCount) * 2 && STEMINA > (float)((int)MaxSTEMINA / MaxDashCount))
        {
            DashCount = 1;
        }
        else if(STEMINA < (float)((int)MaxSTEMINA / MaxDashCount) * 3 && STEMINA > (float)((int)MaxSTEMINA / MaxDashCount) * 2)
        {
            DashCount = 2;
        }
        else if(STEMINA == MaxSTEMINA)
        {
            DashCount = 3;
        }
    }


    public override void TakeDamage(float damage)
    {
        HP -= damage;
    }

    public override void ConsumptionSteminaAndCount(float consumptionSTEMINA,int consumptionDashCount)
    {
        STEMINA -= consumptionSTEMINA;
        DashCount -= consumptionDashCount;
    }
}
