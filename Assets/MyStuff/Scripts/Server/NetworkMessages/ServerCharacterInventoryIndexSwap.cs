using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server.Networking
{
    public class ServerCharacterInventoryIndexSwap : IServerHandle
    {
        public int GetMessageId()
        {
            return (int)Packets.CharacterInventoryIndexSwap;
        }

        public void ReadMessage(int _fromClientId, Packet _packet)
        {
            int index1 = _packet.ReadInt();
            int index2 = _packet.ReadInt();

            ThreadManager.ExecuteOnMainThread(() =>
            {
                ServerPlayerBrain character = ServerGameManager.Instance.GetPlayer(_fromClientId);
                character.playerInventory.SwapSlotIndex(index1, index2);
                WriteMessage(_fromClientId, index1, index2);
            });
        }

        public void WriteMessage(int _fromClientId, int index1, int index2)
        {
            using (Packet _packet = new Packet(GetMessageId()))
            {
                _packet.Write(index1);
                _packet.Write(index2);
                
                ServerSend.SendTcpDataAuthenticated(_fromClientId, _packet);
            }
        }
    }
}