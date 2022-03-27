using Server.Networking;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class ServerAIInventory : MonoBehaviour
    {
        public List<DropItemRecord> PossibleDrops = new List<DropItemRecord>();

        public List<ServerStorageItem> GenerateDrops() 
        {
            List<ServerStorageItem> returnList = new List<ServerStorageItem>();

            foreach (DropItemRecord record in PossibleDrops)
            {
                float tmp = UnityEngine.Random.Range(0f, 1f);
                
                if (tmp <= record.DropChance)
                {
                    returnList.Add(new ServerStorageItem(record.Item.InternalName, UnityEngine.Random.Range(record.MinStackAmount, record.MaxStackAmount+1)));
                }
            }

            return returnList;
        }
    }

    [Serializable]
    public class DropItemRecord 
    {
        public Item Item;
        public int MinStackAmount;
        public int MaxStackAmount;
        public float DropChance;
    }
}
