using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemEquipSlot : ItemSlot
{
    public EquipType slotEquipType;
    public ArmorType slotArmorType;
    public WeaponType slotWeaponType;

    public override void OnDrop(PointerEventData eventData)
    {
        if (GameManager.Instance.DraggingObject)
        {
            // can we equip?
            if (EquipCheck(GameManager.Instance.DraggingObject.SlottedItem))
            {
                if (SlottedItem != null)
                {
                    var tempItem = SlottedItem;
                    SetSlottedItem(GameManager.Instance.DraggingObject.SlottedItem);
                    GameManager.Instance.DraggingObject.SetSlottedItem(tempItem);
                }
                else
                {
                    SetSlottedItem(GameManager.Instance.DraggingObject.SlottedItem);
                    GameManager.Instance.DraggingObject.ClearSlot();
                }
            }
        }
    }

    bool EquipCheck(Item item) 
    {
        if (typeof(ItemEquippable).IsAssignableFrom(item.GetType()))
        {
            // do stat check

            if (typeof(ItemArmor).IsAssignableFrom(item.GetType()) && slotEquipType == EquipType.Armor)
            {
                ItemArmor itemArmor = item.GetComponent<ItemArmor>();
                if (itemArmor.ArmorType == slotArmorType)
                {
                    return true;
                }
            }
            else if(typeof(ItemWeapon).IsAssignableFrom(item.GetType()) && slotEquipType == EquipType.Weapon)
            {
                ItemWeapon itemWeapon = item.GetComponent<ItemWeapon>();
                if (itemWeapon.WeaponType == slotWeaponType)
                {
                    return true;
                }
            }
        }
        return false;
    }

}
