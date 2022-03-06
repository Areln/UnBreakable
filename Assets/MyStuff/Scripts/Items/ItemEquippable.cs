using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemEquippable : Item
{
    //public ItemType ItemType;
    //public string ItemName;
    //public string Description;
    //public Sprite ItemSprite;
    //public int MonetaryValue;
    public EquipType EquipType;
    public virtual void OnEquip()
    {

    }
    public virtual void OnUnEquip()
    {

    }
}
