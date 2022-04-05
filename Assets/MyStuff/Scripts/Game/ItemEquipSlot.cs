using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemEquipSlot : ItemSlot
{
    public EquipType slotEquipType;
    public ArmorType slotArmorType;
    public WeaponType slotWeaponType;

    // This is called when we are already dragging an item and we release it on this slot
    public override void OnDrop(PointerEventData eventData)
    {
        if (GameManager.Instance.DraggingObject)
        {
            // can we equip?
            if (EquipCheck(GameManager.Instance.DraggingObject.SlottedItem))
            {
                GameManager.Instance.ClientPlayer.playerInventory.EquipItemToCharacter(GameManager.Instance.DraggingObject.SlottedItem.GetComponent<ItemEquipable>(), GameManager.Instance.DraggingObject);
            }
        }
    }
    bool EquipCheck(Item item)
    {
        if (typeof(ItemEquipable).IsAssignableFrom(item.GetType()))
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
            else if (typeof(ItemWeapon).IsAssignableFrom(item.GetType()) && slotEquipType == EquipType.Weapon)
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
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (IsSlotted())
        {
            // Un-equip item
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                //UnEquipSlottedItem();
                if (slotEquipType == EquipType.Armor)
                {
                    new CharacterUnEquipItem().WriteMessage(true, (int)slotArmorType);
                }
                else
                {
                    new CharacterUnEquipItem().WriteMessage(false, (int)slotWeaponType);
                }

            }

            if (eventData.button == PointerEventData.InputButton.Right)
            {

            }

        }
    }
    public void UnEquipSlottedItem(ItemSlot _itemSlot = null)
    {

        if (_itemSlot == null)
        {
            _itemSlot = GameManager.Instance.ClientPlayer.playerInventory.FindFirstOpenItemSlot();
        }

        switch (slotEquipType)
        {
            case EquipType.Weapon:
                GameManager.Instance.ClientPlayer.playerInventory.UnEquipWeapon(slotWeaponType, _itemSlot);
                break;
            case EquipType.Armor:
                GameManager.Instance.ClientPlayer.playerInventory.UnEquipArmor(slotArmorType, _itemSlot);
                break;
            default:
                break;
        }

    }
}
