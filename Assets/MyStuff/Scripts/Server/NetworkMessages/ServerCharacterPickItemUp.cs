using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerCharacterPickUpItem : IServerHandle
{
    public int GetMessageId()
    {
        return (int)Packets.CharacterPickUpItem;
    }

    public void ReadMessage(int _fromClientId, Packet _packet)
    {
        throw new System.NotImplementedException();
    }
}
