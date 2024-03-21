using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EquipWeapon : MonoBehaviour
{
    [Header("무기 종류")]
    private KeyCode weapon1 = KeyCode.Alpha1;
    private KeyCode weapon2 = KeyCode.Alpha2;
    public GameObject[] Weapons;
    public bool hasWeapons1;
    public bool hasWeapons2;
    private int equipWeapon = 0;

    public Sprite[] sprites;

    public GameObject MeleePos;
    public GameObject RangePos;
    public bool SwitchPos = true;
    [SerializeField]
    private InventorySO inventory;
    [SerializeField]
    private GameObject Swing;

    private int currentWeapon1code;
    private int currentWeapon2code;

    public bool EquipMelee;
    void Awake()
    {
    }

    void Update()
    {
        ItemSO EquipItem1 = inventory.inventoryItems[15].item;
        ItemSO EquipItem2 = inventory.inventoryItems[16].item;

        if (Input.GetKeyDown(weapon1))
        {
            PlayerController.instance.canAttack = true;
            SwitchingWeapon1(EquipItem1, EquipItem2);
        }
        if (Input.GetKeyDown(weapon2))
        {
            PlayerController.instance.canAttack = true;
            SwitchingWeapon2(EquipItem1, EquipItem2);
        }

        CheckWeapon(EquipItem1, EquipItem2);
    }

    private void CheckWeapon(ItemSO EquipItem1, ItemSO EquipItem2)
    {
        if (equipWeapon == 0 && EquipItem1 == null)
        {
            Weapons[currentWeapon1code].SetActive(false);
            equipWeapon = -1;
        }
        if (equipWeapon == 1 && EquipItem2 == null)
        {
            Weapons[currentWeapon2code].SetActive(false);
            equipWeapon = -1;
        }
    }

    private void SwitchingWeapon1(ItemSO EquipItem1, ItemSO EquipItem2)
    {
        if (EquipItem1 != null && equipWeapon != 0)
        {
            if (equipWeapon == 1)
            {
                Weapons[EquipItem2.Code].SetActive(false);
            }
            Weapons[EquipItem1.Code].SetActive(true);
            currentWeapon1code = EquipItem1.Code;
            equipWeapon = 0;
        }
    }
    private void SwitchingWeapon2(ItemSO EquipItem1, ItemSO EquipItem2)
    {
        if (EquipItem2 != null && equipWeapon != 1)
        {
            if (equipWeapon == 0)
            {
                Weapons[EquipItem1.Code].SetActive(false);
            }
            Weapons[EquipItem2.Code].SetActive(true);
            currentWeapon2code = EquipItem2.Code;
            equipWeapon = 1;
        }
    }
}