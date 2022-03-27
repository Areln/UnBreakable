using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateStorageObjectHandle : IHandle
{
    public int GetMessageId()
    {
        return (int)Packets.CreateStorageObject;
    }

    public void ReadMessage(Packet _packet)
    {
        int objectId = _packet.ReadInt();
        string objectName = _packet.ReadString();
        float posX = _packet.ReadFloat();
        float posY = _packet.ReadFloat();
        float posZ = _packet.ReadFloat();
        float rotation = _packet.ReadFloat();

        ThreadManager.ExecuteOnMainThread(() =>
        {
            GameManager.Instance.CreateStorageObject(objectId, new Vector3(posX, posY, posZ), rotation, objectName);
        });
    }
}
