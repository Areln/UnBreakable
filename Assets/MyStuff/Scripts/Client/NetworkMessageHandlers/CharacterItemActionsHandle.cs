using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterItemActionsHandle : IHandle
{
    public int GetMessageId()
    {
        return (int)Packets.itemAction;
    }

    public void ReadMessage(Packet _packet)
    {
        throw new System.NotImplementedException();
    }

}
