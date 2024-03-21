using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUIManager: MonoBehaviour
{
    public static ShopUIManager     instance;

    [SerializeField]
    private GameObject              descriptionUI;
    private RectTransform           rectDiscriptionUI;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        rectDiscriptionUI = descriptionUI.GetComponent<RectTransform>();
    }

    public void DiscriptionPosToSlot(Vector2 slopPos)
    {
        DebugManager.Instance.DebugTargetQuadrant(slopPos);

        // 설명 UI 활성환
        descriptionUI.SetActive(true);
        
        if (DebugManager.Instance.targetPosInQuadrant == 2)
        {
            // 위치를 UI의 위치의 아래로
            rectDiscriptionUI.position = slopPos;
        }
        else if(DebugManager.Instance.targetPosInQuadrant == 3)
        {
            rectDiscriptionUI.position = new Vector2(slopPos.x,slopPos.y + rectDiscriptionUI.rect.height);
        }
        

    }

    public void DeactiateDiscriptionUI()
    {
        descriptionUI.SetActive(false);
    }

}
