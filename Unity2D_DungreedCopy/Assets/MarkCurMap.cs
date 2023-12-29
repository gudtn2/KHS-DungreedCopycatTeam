using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkCurMap : MonoBehaviour
{
    [SerializeField]
    private string  curmapName;
    [SerializeField]
    private Image   imageBlinkedEffect;
    [SerializeField]
    private float   blinkedTime = 0.5f;
    [SerializeField]
    private float   time = 0;


    private Color   color;
    [Header("던전맵 방향 오브젝트")]
    [SerializeField]
    private GameObject  Right;
    [SerializeField]
    private GameObject  Left;
    [SerializeField]
    private GameObject  Up;
    [SerializeField]
    private GameObject  Down;
    
    [Header("던전맵에 해당하는 미니맵 오브젝트 삽입")]
    [SerializeField]
    private Portal  thisPortal;
    public string   dungeonMapDir;

    private PlayerController player;
    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }
    private void Start()
    {
        curmapName  = this.gameObject.name;
        color       = imageBlinkedEffect.color;
    }

    private void Update()
    {
        if(curmapName == player.curDungeonName)
        {
            UpdateMarkCurMap();
        }
        else
        {
            color.a = 0;
            imageBlinkedEffect.color = color;
        }

        UpdateMarkMapDir();
    }

    private void UpdateMarkCurMap()
    {
        time += Time.deltaTime;
        
        if(time < blinkedTime)
        {
            color.a = 0;
            imageBlinkedEffect.color = color;
        }
        else
        {
            color.a = 1;
            imageBlinkedEffect.color = color;

            if (time > (2 * blinkedTime))
            {
                time = 0;
            }
        }
    }

    private void UpdateMarkMapDir()
    {
        switch (dungeonMapDir)
        {
            case "R":
                Right.SetActive(true);
                break;
            case "L":
                Left.SetActive(true);
                break;
            case "U":
                Up.SetActive(true);
                break;
            case "D":
                Down.SetActive(true);
                break;
        }
    }
}
