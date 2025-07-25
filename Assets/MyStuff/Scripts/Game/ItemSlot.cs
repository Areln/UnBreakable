﻿using System;
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

    public virtual bool IsSlotted()
    {
        return IsSlottedValue;
    }
    public virtual void Setup(int index)
    {
        SlotIndex = index;
        ClearSlot();
    }

    internal virtual void SetSlottedItem(Item item, int itemCount = 1)
    {
        if (item == null)
        {
            return;
        }
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

    public virtual void ClearSlot()
    {
        SlottedItem = null;
        ItemSprite.sprite = null;
        ItemSprite.color = new Color(0, 0, 0, 0);
        CountTextObject.text = "";
        IsSlottedValue = false;
    }

    // This is called when we are already dragging an item and we release it on this slot
    public virtual void OnDrop(PointerEventData eventData)
    {
        if (GameManager.Instance.GetDraggingObject() && GameManager.Instance.GetDraggingObject().IsSlotted())
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
                if (typeof(ItemEquipSlot).IsAssignableFrom(GameManager.Instance.GetDraggingObject().GetType()))
                {
                    GameManager.Instance.GetDraggingObject().GetComponent<ItemEquipSlot>().UnEquipSlottedItem();
                }
                if (typeof(StorageObjectSlot).IsAssignableFrom(GameManager.Instance.GetDraggingObject().GetType()))
                {
                    GameManager.Instance.GetDraggingObject().GetComponent<StorageObjectSlot>().SendRequestForItem();
                }
                else
                {
                    //SetSlottedItem(GameManager.Instance.GetDraggingObject().SlottedItem);
                    //GameManager.Instance.ClientPlayer.playerInventory.SwapInventorySlots(SlotIndex, GameManager.Instance.DraggingObject.SlotIndex);
                    new CharacterInventoryIndexSwap().WriteMessage(SlotIndex, GameManager.Instance.DraggingObject.SlotIndex);
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
                    //new CharacterEquipItem().WriteMessage(SlotIndex);
                    new CharacterDropItemFromInventory().WriteMessage(SlotIndex);
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
