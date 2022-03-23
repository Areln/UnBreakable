using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipHandle : IHandle
{
    public int GetMessageId()
    {
        return (int)Packets.equipItem;
    }

    public void ReadMessage(Packet _packet)
    {
        Debug.Log("Reading CharacterEquipHandle");
    }

}
