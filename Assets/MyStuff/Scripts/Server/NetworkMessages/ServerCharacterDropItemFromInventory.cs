using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server.Networking
{
    public class ServerCharacterDropItemFromInventory : IServerHandle
    {
        public int GetMessageId()
        {
            return (int)Packets.CharacterDropItemFromInventory;
        }

        public void ReadMessage(int _fromClientId, Packet _packet)
        {
            var slotIndex = _packet.ReadInt();

            ThreadManager.ExecuteOnMainThread(() =>
            {
                ServerPlayerBrain character = ServerGameManager.Instance.GetPlayer(_fromClientId);

                if (character.playerInventory.ServerGetInventoryItemFromIndex(slotIndex) != null)
                {
                    character.playerInventory.serverInventory.TryGetValue(slotIndex, out var value);

                    value.Clear();

                    WriteMessage(_fromClientId, slotIndex);
                }
            });
        }

        public void WriteMessage(int _fromClientId, int slotIndex)
        {
            using (Packet _packet = new Packet(GetMessageId()))
            {
                _packet.Write(slotIndex);

                ServerSend.SendTcpDataAuthenticated(_fromClientId, _packet);
            }
        }
    }
}
