using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPickUpItem : IHandle
{
    public int GetMessageId()
    {
        return (int)Packets.CharacterPickUpItem;
    }

    public void ReadMessage(Packet _packet)
    {
        throw new System.NotImplementedException();
    }


}
