using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipWeapon : MonoBehaviour
{
    //zzz
    [Header("무기 종류")]
    private KeyCode weapon1 = KeyCode.Alpha1;
    public GameObject[] Weapons;
    public bool hasWeapons1;
    public bool hasWeapons2;
    public int equipWeapon = 0;
    
    public Weapon weapon;
    public Sprite[] sprites;

    public GameObject MeleePos1;
    public GameObject MeleePos2;
    public bool SwitchPos = true;

    void Awake()
    {
        weapon = Weapons[0].GetComponent<Weapon>();
    }

    void Update()
    {
        if(Weapons[0] != null)
        {
            hasWeapons1 = true;
        }
        else if(Weapons[0] == null)
        {
            hasWeapons1 = false;
        }

        if(Input.GetKeyDown(weapon1) && hasWeapons1 == true && weapon.type == Weapon.Type.Melee)
        {
            MeleePos1.SetActive(true);
            equipWeapon = 1;

        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && equipWeapon != 0)
        {
            if (SwitchPos == true)
            {
                //MeleeSpr1.sprite = sprites[weapon.value];
                MeleePos1.SetActive(false);
                MeleePos2.SetActive(true);
                SwitchPos = false;
            }
            else if (SwitchPos == false)
            {
                MeleePos1.SetActive(true);
                MeleePos2.SetActive(false);
                SwitchPos = true;
            }
        }
    }
}
