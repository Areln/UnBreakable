using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server.Networking
{

    public class ServerCharacterRequestTakeItemFromStorageObject : IServerHandle
    {
        public int GetMessageId()
        {
            return (int)Packets.CharacterRequestTakeItemFromStorageObject;
        }

        public void ReadMessage(int _fromClientId, Packet _packet)
        {
            int storageObjectId = _packet.ReadInt();
            int itemSlotId = _packet.ReadInt();

            ThreadManager.ExecuteOnMainThread(() =>
            {
                ServerStorageObject serverStorageObject = ServerGameManager.Instance.itemStorages[storageObjectId];
                StorageData item = serverStorageObject.ChestContents[itemSlotId];

                // if the slot isnt empty
                if (!string.IsNullOrWhiteSpace(item.GetItemName()))
                {
                    ServerPlayerInventory _serverPlayerInventory = ServerGameManager.Instance.GetPlayer(_fromClientId).GetComponent<ServerPlayerInventory>();
                    // if we can find the characters first open item slot
                    int? x = _serverPlayerInventory.FindFirstOpenItemSlot();
                    if (x != null)
                    {
                        WriteMessage(serverStorageObject.PeekingIds, _fromClientId, itemSlotId, _serverPlayerInventory.AddToServerInventory(x.Value, item.GetItemName(), item.GetAmount()), x.Value);
                        serverStorageObject.ClearSlot(itemSlotId);
                    }
                }
            });
        }
        public void WriteMessage(List<int> peekingIds, int _fromClientId, int takeFromId, StorageData item, int toId)
        {

            //send one packet to fromClientID
            using (Packet _packet = new Packet(GetMessageId()))
            {
                _packet.Write(true);
                _packet.Write(takeFromId);
                _packet.Write(toId);
                _packet.Write(item);

                ServerSend.SendTcpDataAuthenticated(_fromClientId, _packet);
            }

            //send another packet to everyone looking in the chest
            if (peekingIds.Count > 0)
            {
                using (Packet _packet = new Packet(GetMessageId()))
                {
                    _packet.Write(false);
                    _packet.Write(takeFromId);

                    foreach (var peeker in peekingIds)
                    {
                        ServerSend.SendTcpDataAuthenticated(peeker, _packet);
                    }
                }
            }
        }
    }
}