using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
    public ItemSO 아이템;

    [SerializeField]
    private InventorySO 인벤토리;

    void Update()
    {
        인벤토리.AddItem(아이템, 1);
    }
}
