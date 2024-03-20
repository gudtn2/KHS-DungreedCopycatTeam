using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Example : MonoBehaviour
{
    [SerializeField]
    private InventorySO inventory;

    [SerializeField]
    private ItemSO testitem;

    private void Awake()
    {
        
    }

    private void Update()
    {
        
        if (Input.GetKeyUp(KeyCode.Q))
        {
            Call();

        }
    }

    private void Call()
    {
        testitem = inventory.inventoryItems[15].item;
        Debug.Log(testitem.Damage);
        Debug.Log(testitem.name);
        Debug.Log(testitem.AttckSpeed);
    }
}
