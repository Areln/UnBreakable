using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Transform InventoryHolder;
    public ItemArmor EquippedHelmetPiece;
    public ItemArmor EquippedChestPiece;
    public ItemArmor EquippedGlovePiece;
    public ItemArmor EquippedLegPiece;
    public ItemArmor EquippedKnecklacePiece;
    public ItemArmor EquippedRing1Piece;
    public ItemArmor EquippedRing2Piece;
    public ItemWeapon EquippedMainHandWeapon;
    public ItemWeapon EquippedOffHandWeapon;

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

    void EquipItemToCharacter() 
    {
    
    }


}
