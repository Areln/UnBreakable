using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRequestStorageObjectContents : IHandle    
{
    public int GetMessageId()
    {
        return (int)Packets.CharacterRequestStorageObjectContents;
    }

    public void ReadMessage(Packet _packet)
    {
        int length = _packet.ReadInt();
        
        List<StorageData> contents = new List<StorageData>();

        for (int i = 0; i < length; i++)
        {
            contents.Add(new StorageData(_packet.ReadString(), _packet.ReadInt()));
        }


        ThreadManager.ExecuteOnMainThread(() =>
        {
            HudManager.Instance.PopulateStorageContainer(contents);
        });
    }

    public void WriteMessage(int storageObjectId) 
    {
        using (Packet _packet = new Packet(GetMessageId()))
        {
            _packet.Write(storageObjectId);

            ClientSend.SendTcpData(_packet);
        }
    }
}
