using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Basic, Equippable, Consumable}

public enum EquipType { Weapon, Armor}

public enum WeaponType { MainHand, OffHand}

public enum ArmorType { HeadPiece, ChestPiece, GlovePiece, LegPiece, Ring, Knecklace}

public abstract class Item : MonoBehaviour
{
    public ItemType ItemType;
    public string ItemName;
    public string InternalName;
    public string Description;
    public Sprite ItemSprite;
    public int MonetaryValue;

    public virtual void OnItemPickup() 
    {
        
    }
    public virtual void OnItemDrop()
    {

    }
}
