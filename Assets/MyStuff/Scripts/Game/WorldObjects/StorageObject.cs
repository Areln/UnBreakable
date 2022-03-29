using System;
using System.Collections.Generic;
using UnityEngine;

public class StorageObject : WorldObject
{
    public int StorageId { get; set; }
    // If this is greater than the amount of chest contents, then we add empty slots to the inventory UI when we display the contents after we draw slots for the items. ex. 3 items, 5 max slots = 5 slots total, 2 empty slots.
    public int MaxSlots;
    //contents of chest
    List<GameObject> ChestContents = new List<GameObject>();

    public override void Activate(PlayerBrain pb)
    {
        HudManager.Instance.EnableContainerDisplay();
        new CharacterRequestStorageObjectContents().WriteMessage(StorageId);
    }
}
