using Server.Networking;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class ServerAIInventory : MonoBehaviour
    {
        // 0
        Dictionary<GameObject, int> DropChanceInventory = new Dictionary<GameObject, int>();

        public void DropStorageChest(int characterId)
        {
            new ServerCreateStorageObjectHandle().WriteMessage(GetInstanceID(), gameObject.transform.position, transform.rotation.eulerAngles.y);
        }
    }
}
