using Server.Networking;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class ServerStorageObject : MonoBehaviour
    {
        public List<ServerStorageItem> ChestContents = new List<ServerStorageItem>();

        public void SetContents(List<ServerStorageItem> itemsList)
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
    public class ServerStorageItem
    {
        string ItemName;
        int Amount;

        public ServerStorageItem(string itemName, int amount)
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
}
