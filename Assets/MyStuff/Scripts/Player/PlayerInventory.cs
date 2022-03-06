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
        GameObject _tempObject = Instantiate(itemPrefab, InventoryHolder);
        ItemEquippable _tempItem = _tempObject.GetComponent<ItemEquippable>();
        _tempItem.OnUnEquip();
    }

    void EquipItemToCharacter() 
    {
    
    }


}
