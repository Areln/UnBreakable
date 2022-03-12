using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWeapon : ItemEquippable
{
    //public ItemType ItemType;
    //public string ItemName;
    //public string Description;
    //public Sprite ItemSprite;
    //public int MonetaryValue;
    public WeaponType WeaponType;
    public GameObject WeaponObjectPrefab;
    public override void OnEquip()
    {
        base.OnEquip();

    }
}
