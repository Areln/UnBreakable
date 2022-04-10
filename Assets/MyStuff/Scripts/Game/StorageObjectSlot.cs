using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StorageObjectSlot : ItemSlot
{
    int StorageObjectId;

    public virtual void Setup(int storageObjectId, int index)
    {
        StorageObjectId = storageObjectId;
        SlotIndex = index;
        ClearSlot();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (IsSlotted())
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                // Get type of item then decide what to do
                if (typeof(ItemEquipable).IsAssignableFrom(SlottedItem.GetType()))
                {
                    Debug.Log("Left Clicked Equipable");
                }
                else if (typeof(ItemBasic).IsAssignableFrom(SlottedItem.GetType()))
                {
                    Debug.Log("Left Clicked BasicItem");
                }
                else if (typeof(ItemConsumable).IsAssignableFrom(SlottedItem.GetType()))
                {
                    Debug.Log("Left Clicked Consumable");
                }

                SendRequestForItem();

            }
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (typeof(ItemEquipable).IsAssignableFrom(SlottedItem.GetType()))
                {
                    Debug.Log("Right Clicked Equipable");
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

    public void SendRequestForItem()
    {
        new CharacterRequestTakeItemFromStorage().WriteMessage(StorageObjectId, SlotIndex);
    }

    // This is called when we are already dragging an item and we release it on this slot
    public override void OnDrop(PointerEventData eventData) 
    {
        //GameManager.Instance.ClientPlayer.playerInventory.EquipItemToCharacter(GameManager.Instance.DraggingObject.SlottedItem.GetComponent<ItemEquipable>(), GameManager.Instance.DraggingObject);
    }
}
