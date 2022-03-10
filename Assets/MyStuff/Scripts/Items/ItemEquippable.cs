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

    // list of components to enable/disable when equipped
    public List<Behaviour> EquipComponents = new List<Behaviour>();
    public List<MeshRenderer> EquipRenderers = new List<MeshRenderer>();

    public override void OnItemPickup()
    {
        base.OnItemPickup();
        ToggleEquipComponents(false);
    }

    public virtual void OnEquip()
    {
        ToggleEquipComponents(true);
    }

    public virtual void OnUnEquip()
    {
        ToggleEquipComponents(false);
    }

    internal void ToggleEquipComponents(bool value) 
    {
        foreach (Behaviour item in EquipComponents)
        {
            item.enabled = value;
        }
        foreach (MeshRenderer item in EquipRenderers)
        {
            item.enabled = value;
        }
    }
}
