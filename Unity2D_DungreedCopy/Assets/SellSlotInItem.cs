using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellSlotInItem : MonoBehaviour
{
    [SerializeField]
    private ItemSO item;
    [SerializeField]
    private InventorySO inventory;

    private void Update()
    {
        // 예시 코드임으로 나중에 
        inventory.AddItem(item, 1);
    }
}
