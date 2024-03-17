using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    private Animator    ani;
    private NPC         npc;

    private void Awake()
    {
        ani = GetComponent<Animator>();

        npc = FindObjectOfType<NPC>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ani.Play("ShopHide");
            npc.inputKey = false;
            PlayerController.instance.onUI = false;
        }
    }
}
