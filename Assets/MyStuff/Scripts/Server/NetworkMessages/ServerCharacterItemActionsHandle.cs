using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerCharacterItemActionsHandle : IServerHandle
{
    public int GetMessageId()
    {
        return (int)Packets.itemAction;
    }

    public void ReadMessage(int _fromClientId, Packet _packet)
    {
        throw new System.NotImplementedException();
    }
}
