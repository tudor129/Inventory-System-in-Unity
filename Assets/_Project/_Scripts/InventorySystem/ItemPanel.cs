using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemPanel : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler, IDropHandler
{
    public Inventory inventory;
    public ItemSlotInfo itemSlot;
    public Image itemImage;
    public TextMeshProUGUI stacksText;

    Mouse _mouse;

    bool _click;
    public void OnClick()
    {
        if (inventory != null)
        {
            _mouse = inventory.mouse;
            
            // Grab item if mouse slot is empty
            if (_mouse.itemSlot.item == null)
            {
                if (itemSlot.item != null)
                {
                    PickUpItem();
                    FadeOut();
                }
            }
            else
            {
                // Clicked on original slot
                if (itemSlot == _mouse.itemSlot)
                {
                    inventory.RefreshInventory();
                }
                // Clicked on empty slot
                else if (itemSlot.item == null)
                {
                    DropItem();
                    inventory.RefreshInventory();
                }
                // Clicked on occupied slot
                else if (itemSlot.item.GiveName() != _mouse.itemSlot.item.GiveName())
                {
                   SwapItem(itemSlot, _mouse.itemSlot);
                   inventory.RefreshInventory();
                }
            }
        }
    }

    public void PickUpItem()
    {
        _mouse.itemSlot = itemSlot;
        _mouse.SetUI();
    }
    public void FadeOut()
    {
        itemImage.CrossFadeAlpha(0.3f, 0.05f, true);
    }
    public void DropItem()
    {
        itemSlot.item = _mouse.itemSlot.item;
        itemSlot.stacks = _mouse.itemSlot.stacks;
        inventory.ClearSlot(_mouse.itemSlot);
    }
    public void SwapItem(ItemSlotInfo slotA, ItemSlotInfo slotB)
    {
        ItemSlotInfo tempItem = new ItemSlotInfo(slotA.item, slotA.stacks);

        slotA.item = slotB.item;
        slotA.stacks = slotB.stacks;

        slotB.item = tempItem.item;
        slotB.stacks = tempItem.stacks;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.pointerPress = gameObject;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _click = true;
        Debug.Log("PointerDown: " + _click);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (_click)
        {
            Debug.Log("Pointer Up: " + _click);
            OnClick();
            _click = false;
        }
        
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (_click)
        {
            Debug.Log("OnDrag: " + _click);
            OnClick();
            _click = false;
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        OnClick();
        _click = false;
    }
}
