using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonName : MonoBehaviour
{
    [SerializeField]
    private string  dungeonName;
    [SerializeField]
    private Image   blinkedImage;
    [SerializeField]
    private float   blinkTime;

    private PlayerController player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        Color color = blinkedImage.color;
        
        if (dungeonName == player.curDungeonName)
        {
            StartCoroutine(ActivateCurPlayerPosition());
        }
        else
        {
            color.a = 0;
            blinkedImage.color = color;
        }
    }
    private IEnumerator ActivateCurPlayerPosition()
    {
        Color color = blinkedImage.color;

        while (true)
        {
            yield return new WaitForSeconds(blinkTime);
            color.a = 1;
            blinkedImage.color = color;
            yield return new WaitForSeconds(blinkTime);
            color.a = 0;
            blinkedImage.color = color;

        }
    }
}
