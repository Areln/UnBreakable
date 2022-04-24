using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDropItemFromInventory : IHandle
{
    public int GetMessageId()
    {
        return (int)Packets.CharacterDropItemFromInventory;
    }

    public void ReadMessage(Packet _packet)
    {
        Debug.Log("drop");
        var slotIndex = _packet.ReadInt();

        ThreadManager.ExecuteOnMainThread(() =>
        {
            GameManager.Instance.ClientPlayer.playerInventory.ClearInventorySlot(slotIndex);
        });
    }
    public void WriteMessage(int slotIndex)
    {
        using (Packet _packet = new Packet(GetMessageId()))
        {
            _packet.Write(slotIndex);

            ClientSend.SendTcpData(_packet);
        }
    }
}
