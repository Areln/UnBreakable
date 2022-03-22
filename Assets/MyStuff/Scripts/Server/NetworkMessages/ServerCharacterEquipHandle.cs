using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerCharacterEquipHandle : IServerHandle
{
    public int GetMessageId()
    {
        return (int)Packets.equipItem;
    }

    public void ReadMessage(int _fromClientId, Packet _packet)
    {
        Debug.Log("Reading ServerCharacterEquipHandle");
    }

}
