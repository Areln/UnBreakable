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
                Dictionary<int, StorageData> contents = ServerGameManager.Instance.itemStorages[storageObjectId].ChestContents;
                WriteMessage(_fromClientId, storageObjectId, contents);
            });
        }
        public void WriteMessage(int _fromClientId, int storageObjectId, Dictionary<int, StorageData> contents)
        {
            using (Packet _packet = new Packet(GetMessageId()))
            {
                _packet.Write(storageObjectId);
                _packet.Write(contents.Count);

                foreach (var item in contents)
                {
                    //_packet.Write(item.GetItemName());
                    //_packet.Write(item.GetAmount());
                    _packet.Write(item.Key);
                    _packet.Write(item.Value);
                }

                ServerSend.SendTcpDataAuthenticated(_fromClientId, _packet);
            }
        }
    }
}
