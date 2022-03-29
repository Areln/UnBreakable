using Server.Networking;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class ServerStorageObject : MonoBehaviour
    {
        public List<StorageData> ChestContents = new List<StorageData>();

        public void SetContents(List<StorageData> itemsList)
        {
            ChestContents = itemsList;
        }
        public void DropStorageChest(ServerAIInventory serverAIInventory)
        {
            SetContents(serverAIInventory.GenerateDrops());
            ServerGameManager.Instance.AddNewItemStorageToList(this);
            if (ChestContents.Count > 0)
            {
                new ServerCreateStorageObjectHandle().WriteMessage(GetInstanceID(), gameObject.transform.position, transform.rotation.eulerAngles.y);
            }
            else
            {
                new ServerCreateStorageObjectHandle().WriteMessage(GetInstanceID(), gameObject.transform.position, transform.rotation.eulerAngles.y);
            }

        }
    }
}

[Serializable]
public class StorageData
{
    string ItemName;
    int Amount;

    public StorageData(string itemName, int amount)
    {
        ItemName = itemName;
        Amount = amount;
    }

    public string GetItemName()
    {
        return ItemName;
    }

    public int GetAmount()
    {
        return Amount;
    }
}
