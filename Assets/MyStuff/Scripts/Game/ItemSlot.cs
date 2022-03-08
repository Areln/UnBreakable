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
        ItemSprite.color = new Color(1, 1, 1, 1);
        if (item.CurrentUseCount != 1)
        {
            CountTextObject.text = $"x{item.CurrentUseCount}";
        }
        else
        {
            CountTextObject.text = "";
        }
        
    }

    public void ClearSlot() 
    {
        SlottedItem = null;
        ItemSprite.sprite = null;
        ItemSprite.color = new Color(0, 0, 0, 0);
        CountTextObject.text = "";
    }

}
