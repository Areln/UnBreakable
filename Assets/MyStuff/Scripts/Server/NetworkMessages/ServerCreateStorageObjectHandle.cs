using System.Collections.Generic;
using UnityEngine;

namespace Server.Networking
{

    public class ServerCreateStorageObjectHandle : IServerHandle
    {
        public int GetMessageId()
        {
            return (int)Packets.CreateStorageObject;
        }

        public void ReadMessage(int _fromClientId, Packet _packet)
        {
            throw new System.NotImplementedException();
        }

        public void WriteMessage(int objectId, Vector3 position, float rotation, string objectName = "bones")
        {
            using (Packet _packet = new Packet(GetMessageId()))
            {
                _packet.Write(objectId);
                _packet.Write(objectName);
                _packet.Write(position.x);
                _packet.Write(position.y);
                _packet.Write(position.z);
                _packet.Write(rotation);

                ServerSend.SendTcpDataToAllAuthenticated(_packet);
            }

        }
    }
}
