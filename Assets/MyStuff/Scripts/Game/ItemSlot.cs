using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Item SlottedItem;
    public Image ItemSprite;
    public Image ItemBackground;
    public TextMeshProUGUI CountTextObject;

    public void SetSlottedItem(Item item) 
    {
        SlottedItem = item;
        ItemSprite.sprite = item.ItemSprite;
        CountTextObject.text = $"x{item.CurrentUseCount}";
    }

}
