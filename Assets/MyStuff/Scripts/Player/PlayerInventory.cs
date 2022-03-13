using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Transform InventoryHolder;

    public ItemArmor EquippedHelmetPiece;
    public Transform HeadTransform;

    public ItemArmor EquippedChestPiece;
    public Transform ChestPieceTransform;

    public ItemArmor EquippedGlovePiece;
    public Transform GlovePieceTransform;

    public ItemArmor EquippedLegPiece;
    public Transform LegPieceTransform;

    public ItemArmor EquippedKnecklacePiece;
    public Transform KnecklacePieceTransform;

    public ItemArmor EquippedRing1Piece;
    public Transform Ring1PieceTransform;

    public ItemArmor EquippedRing2Piece;
    public Transform Ring2PieceTransform;

    public ItemWeapon EquippedMainHandWeapon;
    public Transform MainHandWeaponTransform;

    public ItemWeapon EquippedOffHandWeapon;
    public Transform OffHandWeaponTransform;

    //array 
    public List<Item> InventoryItems = new List<Item>();

    public void AddPrefabItemObjectToPlayerInventory(GameObject itemPrefab)
    {
        // Instantiates Item's gameobject and sets the parent as the InventoryHolder
        GameObject _tempObject = Instantiate(itemPrefab, InventoryHolder);

        // Grab reference to the item script
        Item _tempItem = _tempObject.GetComponent<Item>();

        _tempItem.OnItemPickup();

        // Find the first open inventory slot and set the slotted item. we should have already checked if player's inventory is full
        FindFirstOpenItemSlot().SetSlottedItem(_tempItem);
    }

    ItemSlot FindFirstOpenItemSlot()
    {
        foreach (ItemSlot itemSlot in HudManager.Instance.InventoryItemSlots)
        {
            if (itemSlot.SlottedItem == null)
            {
                return itemSlot;
            }
        }
        return null;
    }

    public void EquipItemToCharacter(ItemEquippable item, ItemSlot itemSlot)
    {

        bool successfullEquip = false;

        if (typeof(ItemArmor).IsAssignableFrom(item.GetType()))
        {
            successfullEquip = EquipArmor(item.GetComponent<ItemArmor>(), itemSlot);
        }
        else if (typeof(ItemWeapon).IsAssignableFrom(item.GetType()))
        {
            successfullEquip = EquipWeapon(item.GetComponent<ItemWeapon>(), itemSlot);
        }

        if (successfullEquip)
        {
            item.OnEquip();
            HudManager.Instance.EquippedItem(item);
        }
    }
    bool EquipWeapon(ItemWeapon itemWeapon, ItemSlot itemSlot)
    {
        ItemWeapon _tempWeapon = GetEquippedWeapon(itemWeapon.WeaponType);

        if (_tempWeapon)
        {
            UnEquipWeapon(itemWeapon.WeaponType, itemSlot);
        }
        else
        {
            itemSlot.ClearSlot();
        }

        switch (itemWeapon.WeaponType)
        {
            case WeaponType.MainHand:
                itemWeapon.gameObject.transform.SetParent(MainHandWeaponTransform, false);
                EquippedMainHandWeapon = itemWeapon;
                return true;
            case WeaponType.OffHand:
                itemWeapon.gameObject.transform.SetParent(OffHandWeaponTransform, false);
                EquippedOffHandWeapon = itemWeapon;
                return true;
            default:
                break;
        }

        return false;
    }
    void UnEquipWeapon(WeaponType weaponType, ItemSlot itemSlot = null)
    {
        ItemWeapon _tempSel = EquippedMainHandWeapon;

        switch (weaponType)
        {
            case WeaponType.MainHand:
                _tempSel = EquippedMainHandWeapon;
                EquippedMainHandWeapon = null;
                break;
            case WeaponType.OffHand:
                _tempSel = EquippedOffHandWeapon;
                EquippedOffHandWeapon = null;
                break;
            default:
                break;
        }

        // 
        HudManager.Instance.UnEquippedItem(_tempSel);
        _tempSel.OnUnEquip();
        if (itemSlot)
        {
            _tempSel.gameObject.transform.SetParent(InventoryHolder, false);
            itemSlot.SetSlottedItem(_tempSel);
        }
        else
        {
            _tempSel.gameObject.transform.SetParent(null, false);
        }
    }
    bool EquipArmor(ItemArmor itemArmor, ItemSlot itemSlot)
    {
        ItemArmor _tempArmor = GetEquippedArmor(itemArmor.ArmorType);
        
        if (_tempArmor)
        {
            UnEquipArmor(itemArmor.ArmorType, itemSlot);
        }
        else
        {
            itemSlot.ClearSlot();
        }

        switch (itemArmor.ArmorType)
        {
            case ArmorType.HeadPiece:
                itemArmor.gameObject.transform.SetParent(HeadTransform, false);
                EquippedHelmetPiece = itemArmor;
                return true;
            case ArmorType.ChestPiece:
                itemArmor.gameObject.transform.SetParent(ChestPieceTransform, false);
                EquippedChestPiece = itemArmor;
                return true;
            case ArmorType.GlovePiece:
                itemArmor.gameObject.transform.SetParent(GlovePieceTransform, false);
                EquippedGlovePiece = itemArmor;
                return true;
            case ArmorType.LegPiece:
                itemArmor.gameObject.transform.SetParent(LegPieceTransform, false);
                EquippedLegPiece = itemArmor;
                return true;
            case ArmorType.Ring:
                itemArmor.gameObject.transform.SetParent(Ring1PieceTransform, false);
                EquippedRing1Piece = itemArmor;
                return true;
            case ArmorType.Knecklace:
                itemArmor.gameObject.transform.SetParent(Ring2PieceTransform, false);
                EquippedRing2Piece = itemArmor;
                return true;
            default:
                break;
        }

        return false;
    }
    void UnEquipArmor(ArmorType armorType, ItemSlot itemSlot = null)
    {
        ItemArmor _tempSel = EquippedChestPiece;

        switch (armorType)
        {
            case ArmorType.HeadPiece:
                _tempSel = EquippedHelmetPiece;
                EquippedHelmetPiece = null;
                break;
            case ArmorType.ChestPiece:
                _tempSel = EquippedChestPiece;
                EquippedChestPiece = null;
                break;
            case ArmorType.GlovePiece:
                _tempSel = EquippedGlovePiece;
                EquippedGlovePiece = null;
                break;
            case ArmorType.LegPiece:
                _tempSel = EquippedLegPiece;
                EquippedLegPiece = null;
                break;
            case ArmorType.Ring:
                _tempSel = EquippedRing1Piece;
                EquippedRing1Piece = null;
                break;
            case ArmorType.Knecklace:
                _tempSel = EquippedKnecklacePiece;
                EquippedKnecklacePiece = null;
                break;
            default:
                break;
        }

        // 
        HudManager.Instance.UnEquippedItem(_tempSel);
        _tempSel.OnUnEquip();
        if (itemSlot)
        {
            _tempSel.gameObject.transform.SetParent(InventoryHolder, false);
            itemSlot.SetSlottedItem(_tempSel);
        }
        else
        {
            _tempSel.gameObject.transform.SetParent(null, false);
        }
    }
    ItemArmor GetEquippedArmor(ArmorType armorType)
    {
        switch (armorType)
        {
            case ArmorType.HeadPiece:
                return EquippedHelmetPiece;
            case ArmorType.ChestPiece:
                return EquippedChestPiece;
            case ArmorType.GlovePiece:
                return EquippedGlovePiece;
            case ArmorType.LegPiece:
                return EquippedLegPiece;
            case ArmorType.Ring:
                return EquippedRing1Piece;
            case ArmorType.Knecklace:
                return EquippedKnecklacePiece;
            default:
                break;
        }
        return null;
    }
    ItemWeapon GetEquippedWeapon(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.MainHand:
                return EquippedMainHandWeapon;
            case WeaponType.OffHand:
                return EquippedOffHandWeapon;
            default:
                break;
        }
        return null;
    }

}
