using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventoryIndexSwap : IHandle
{
    public int GetMessageId()
    {
        return (int)Packets.CharacterInventoryIndexSwap;
    }

    public void ReadMessage(Packet _packet)
    {
        int index1 = _packet.ReadInt();
        int index2 = _packet.ReadInt();

        ThreadManager.ExecuteOnMainThread(() =>
        {
            GameManager.Instance.ClientPlayer.playerInventory.SwapInventorySlots(index1, index2);
        });
    }

    public void WriteMessage(int index1, int index2)
    {
        using (Packet _packet = new Packet(GetMessageId()))
        {
            _packet.Write(index1);
            _packet.Write(index2);

            ClientSend.SendTcpData(_packet);
        }
    }

}
