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
    public int equipWeapon = -1;

    public Weapon weapon;
    public Sprite[] sprites;

    public GameObject MeleePos;
    public GameObject RangePos;
    public bool SwitchPos = true;
    [SerializeField]
    private InventorySO inventory;
    [SerializeField]
    private GameObject Swing;

    public bool EquipMelee;
    void Awake()
    {
        weapon = Weapons[0].GetComponent<Weapon>();
    }

    void Update()
    {
        ItemSO EquipItem1 = inventory.inventoryItems[15].item;
        // 근접무기 
        if (inventory.inventoryItems[15].item != null)
        {
            hasWeapons1 = true;
        }
        else if(Weapons[0] == null)
        {
            hasWeapons1 = false;
        }

        if (Input.GetKeyDown(weapon1) && hasWeapons1 == true && weapon.type == Weapon.Type.Melee)
        {
            MeleePos.SetActive(true);
            equipWeapon = 1;

        }
        else if (hasWeapons1 && Input.GetKeyDown(weapon1)) return;


        //if (Input.GetKeyDown(KeyCode.Mouse0) && equipWeapon != -1  && PlayerController.instance.canAttack)
        //{
        //    if (SwitchPos == true)
        //    {
        //        //MeleeSpr1.sprite = sprites[weapon.value];
        //        MeleePos1.SetActive(false);
        //        MeleePos2.SetActive(true);
        //        SwitchPos = false;
        //    }
        //    else if (SwitchPos == false)
        //    {
        //        MeleePos1.SetActive(true);
        //        MeleePos2.SetActive(false);
        //        SwitchPos = true;
        //    }
        //}
    }

    private void SwitchingWeapon(ItemSO EquipItem)
    {
        if (Input.GetKeyDown(weapon1) && EquipItem != null && equipWeapon != 0)
        {
            Weapons[EquipItem.Code].SetActive(true);
            MeleePos.SetActive(EquipItem.Melee);
            RangePos.SetActive(!EquipItem.Melee);
            if(EquipItem.Melee == true)
            {
                EquipMelee = true;
            }
            else
            {
                EquipMelee = false;
            }
            equipWeapon = 0;
        }
        if (Input.GetKeyDown(weapon2) && EquipItem != null && equipWeapon != 1)
        {
            Weapons[EquipItem.Code].SetActive(true);
            MeleePos.SetActive(EquipItem.Melee);
            RangePos.SetActive(!EquipItem.Melee);
            if (EquipItem.Melee == true)
            {
                EquipMelee = true;
            }
            else
            {
                EquipMelee = false;
            }
            equipWeapon = 1;
        }
    }

    private void AttackPos(ItemSO EquipItem)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && equipWeapon != -1 && PlayerController.instance.canAttack)
        {
            if (EquipItem.Melee == true) { }
        }
    }
}
