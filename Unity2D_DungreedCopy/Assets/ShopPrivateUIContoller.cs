using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ShopPrivateUIContoller : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    private Image   baseImage;
    [SerializeField]
    private Sprite  originSprite;
    [SerializeField]
    private Sprite  SelectedSprite;

    [Header("아이템 정보")]
    [SerializeField]
    private ItemSO      item;
    [SerializeField]
    private InventorySO inven;
    [SerializeField]
    private Image           imageItem;
    [SerializeField]
    private TextMeshProUGUI textItemName;
    [SerializeField]        
    private TextMeshProUGUI textItemGold;
    private int gold;


    private void Awake()
    {
        baseImage = GetComponent<Image>();
        baseImage.sprite = originSprite;

        imageItem = transform.GetChild(0).GetComponent<Image>();
        textItemName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        textItemGold = transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        SetupItemInfo();
    }

    private void SetupItemInfo()
    {
        imageItem.sprite  = item.ItemImage;
        imageItem.SetNativeSize();
        textItemName.text = item.Name;
        // 골드 추가
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        baseImage.sprite = SelectedSprite;
        ShopUIManager.instance.DiscriptionPosToSlot(eventData.position);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        baseImage.sprite = originSprite;
        ShopUIManager.instance.DeactiateDiscriptionUI();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            inven.AddItem(item, 1);
            Destroy(this.gameObject);

            // 골드만큼 플레이어 골드에서 차감
            PlayerStats.instance.GOLD -= gold;
        }
    }
}
