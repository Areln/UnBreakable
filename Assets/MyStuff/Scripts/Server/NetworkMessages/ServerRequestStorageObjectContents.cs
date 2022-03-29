using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server.Networking
{
    public class ServerRequestStorageObjectContents : IServerHandle
    {
        public int GetMessageId()
        {
            return (int)Packets.CharacterRequestStorageObjectContents;
        }

        public void ReadMessage(int _fromClientId, Packet _packet)
        {
            int storageObjectId = _packet.ReadInt();

            ThreadManager.ExecuteOnMainThread(() =>
            {
                List<StorageData> contents = ServerGameManager.Instance.itemStorages[storageObjectId].ChestContents;
                WriteMessage(_fromClientId, contents);
            });
        }
        public void WriteMessage(int _fromClientId, List<StorageData> contents)
        {
            using (Packet _packet = new Packet(GetMessageId()))
            {
                _packet.Write(contents.Count);

                foreach (StorageData item in contents)
                {
                    _packet.Write(item.GetItemName());
                    _packet.Write(item.GetAmount());
                }

                ServerSend.SendTcpDataAuthenticated(_fromClientId, _packet);
            }
        }
    }
}
