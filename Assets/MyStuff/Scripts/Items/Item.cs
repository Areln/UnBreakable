using UnityEngine;

public enum ItemType { Basic, Equippable, Consumable}

public enum EquipType { Weapon, Armor}

public enum WeaponType { MainHand, OffHand}

public enum ArmorType { HeadPiece, ChestPiece, GlovePiece, LegPiece, RingR, RingL, Necklace}

public abstract class Item : MonoBehaviour
{
    public ItemType ItemType;
    public string ItemName;
    public string InternalName;
    public string Description;
    public Sprite ItemSprite;
    public int MaxUseCount = 1;
    public int CurrentUseCount;
    public int MonetaryValue;

    public virtual void OnItemPickup() 
    {
        
    }
    public virtual void OnItemDrop()
    {

    }
}
