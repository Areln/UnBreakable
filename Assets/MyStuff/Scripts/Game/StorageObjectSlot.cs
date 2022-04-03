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
                    new CharacterRequestTakeItemFromStorage().WriteMessage(StorageObjectId, SlotIndex);
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
