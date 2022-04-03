using Server.Networking;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class ServerStorageObject : MonoBehaviour
    {
        //public List<StorageData> ChestContents = new List<StorageData>();
        public Dictionary<int, StorageData> ChestContents = new Dictionary<int, StorageData>();
        public List<int> PeekingIds = new List<int>();

        public void AddPeeker(int Id)
        {
            PeekingIds.Add(Id);
        }

        public void RemovePeeker(int Id)
        {
            PeekingIds.Remove(Id);
        }

        public void SetContents(List<StorageData> itemsList)
        {
            int x = 0;
            foreach (StorageData item in itemsList)
            {
                ChestContents.Add(x, item);
                x++;
            }
        }

        public StorageData GetItemFromSlotId(int id) 
        {
            if (ChestContents.TryGetValue(id, out StorageData temp))
            {
                if (!string.IsNullOrWhiteSpace(temp.GetItemName()))
                {
                    return temp;
                }
            }
            return null;
        }

        public void ClearSlot(int slotId)
        {
            ChestContents.TryGetValue(slotId, out StorageData storageData);
            storageData.Set("", 0);
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
        Set(itemName, amount);
    }

    public void Set(string itemName, int amount) 
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
