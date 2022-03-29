using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IDropHandler
{
    public int SlotIndex;
    public Item SlottedItem;
    public Image ItemSprite;
    public Image ItemBackground;
    public TextMeshProUGUI CountTextObject;
    bool IsSlottedValue = false;

    public bool IsSlotted()
    {
        return IsSlottedValue;
    }
    public void Setup(int index)
    {
        SlotIndex = index;
        ClearSlot();
    }

    internal void SetSlottedItem(Item item, int itemCount = 1)
    {
        SlottedItem = item;
        SlottedItem.CurrentUseCount = itemCount;
        ItemSprite.sprite = item.ItemSprite;
        ItemSprite.color = new Color(1, 1, 1, 1);

        if (SlottedItem.CurrentUseCount != 1 && SlottedItem.CurrentUseCount > 0)
        {
            CountTextObject.text = $"x{SlottedItem.CurrentUseCount}";
        }
        else
        {
            CountTextObject.text = "";
        }

        IsSlottedValue = true;
    }

    public void ClearSlot()
    {
        SlottedItem = null;
        ItemSprite.sprite = null;
        ItemSprite.color = new Color(0, 0, 0, 0);
        CountTextObject.text = "";
        IsSlottedValue = false;
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        if (GameManager.Instance.DraggingObject)
        {
            if (SlottedItem != null)
            {
                //var tempItem = SlottedItem;
                //SetSlottedItem(GameManager.Instance.DraggingObject.SlottedItem);
                //GameManager.Instance.DraggingObject.SetSlottedItem(tempItem);
            }
			else
            {

                //if we are draggin from equip slot, then un equip the item
                if (typeof(ItemEquipSlot).IsAssignableFrom(GameManager.Instance.DraggingObject.GetType()))
                {
                    GameManager.Instance.DraggingObject.GetComponent<ItemEquipSlot>().UnEquipSlottedItem();
                }
                else
                {
                    SetSlottedItem(GameManager.Instance.DraggingObject.SlottedItem);
                    GameManager.Instance.DraggingObject.ClearSlot();
                }
            }
        }
	}

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (IsSlotted())
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                // Get type of item then decide what to do
                if (typeof(ItemEquipable).IsAssignableFrom(SlottedItem.GetType()))
                {
                    // Check stats if we can equip then handle inventory then equip item
                   //GameManager.Instance.ClientPlayer.playerInventory.EquipItemToCharacter(SlottedItem.GetComponent<ItemEquipable>(), this);
                    new CharacterEquipItem().WriteMessage(SlotIndex);
                }
                else if (typeof(ItemBasic).IsAssignableFrom(SlottedItem.GetType()))
                {
                    Debug.Log("Left Clicked BasicItem");
                }
                else if (typeof(ItemConsumable).IsAssignableFrom(SlottedItem.GetType()))
                {
                    Debug.Log("Left Clicked Consumable");
                }

            }
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (typeof(ItemEquipable).IsAssignableFrom(SlottedItem.GetType()))
                {
                    Debug.Log("Right Clicked Equippable");
                }
                else if (typeof(ItemBasic).IsAssignableFrom(SlottedItem.GetType()))
                {
                    Debug.Log("Right Clicked BasicItem");
                }
                else if (typeof(ItemConsumable).IsAssignableFrom(SlottedItem.GetType()))
                {
                    Debug.Log("Right Clicked Consumable");
                }
            }

        }
    }
}
