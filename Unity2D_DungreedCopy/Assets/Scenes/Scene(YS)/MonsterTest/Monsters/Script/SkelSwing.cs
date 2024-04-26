using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkelSwing : MonoBehaviour
{
    public BoxCollider2D attackBoxCollider;
    public void EnableAttackCollider()
    {
        attackBoxCollider.enabled = true;
    }
    public void DisableAttackCollider()
    {
        attackBoxCollider.enabled = false;
    }

}