using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRequestTakeItemFromStorage : IHandle
{
    public int GetMessageId()
    {
        return (int)Packets.CharacterRequestTakeItemFromStorageObject;
    }

    public void ReadMessage(Packet _packet)
    {
        bool isFromClient = _packet.ReadBool();
        int fromId = _packet.ReadInt();
        int toId = 0;
        StorageData item = new StorageData("", 0);

        if (isFromClient)
        {
            toId = _packet.ReadInt();
            item = _packet.ReadStorageData();
        }

        ThreadManager.ExecuteOnMainThread(() =>
        {
            // clear the ui itemslot
            HudManager.Instance.ContainerItemSlots[fromId].ClearSlot();

            if (isFromClient)
            {
                // Add to our inventory
                GameManager.Instance.ClientPlayer.playerInventory.AddPrefabItemObjectToPlayerInventory(toId, GameManager.Instance.GetItem(item.GetItemName()));
            }
        });
    }

    public void WriteMessage(int storageObjectId, int itemSlotIndex)
    {
        using (Packet _packet = new Packet(GetMessageId()))
        {
            _packet.Write(storageObjectId);
            _packet.Write(itemSlotIndex);

            ClientSend.SendTcpData(_packet);
        }
    }
}
