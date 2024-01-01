using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passing : MonoBehaviour
{
    private CompositeCollider2D collider;

    private void Awake()
    {
        collider = GetComponent<CompositeCollider2D>();
    }
    public void OnPassing()
    {
        collider.isTrigger = true;
    }
    public void OffPassing()
    {
        collider.isTrigger = false;
    }

    public void PassingRoutain(float time)
    {
        OnPassing();
        Invoke("OffPassing", time);
    }
}
