using UnityEngine;

public class ItemArmor : ItemEquipable
{
    //public ItemType ItemType;
    //public string ItemName;
    //public string Description;
    //public Sprite ItemSprite;
    //public int MonetaryValue;
    public ArmorType ArmorType;

    public GameObject MirrorPrefab;

    public override void OnEquip()
    {
        base.OnEquip();

    }
}
