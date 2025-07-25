﻿
using UnityEngine;

public class CharacterSpawnItem : IHandle
{
    public int GetMessageId()
    {
        return (int)Packets.CharacterSpawnItem;
    }

    public void ReadMessage(Packet _packet)
    {
        // read data from stream
        int fromId = _packet.ReadInt();
        int slotIndex = _packet.ReadInt();
        StorageData itemStorageData = _packet.ReadStorageData();

        ThreadManager.ExecuteOnMainThread(() =>
        {
            PlayerBrain character = (PlayerBrain)GameManager.Instance.GetCharacter(fromId);
            if (character != null)
            {
                character.playerInventory.AddPrefabItemObjectToPlayerInventory(slotIndex, GameManager.Instance.GetItem(itemStorageData.GetItemName()), itemStorageData.GetAmount());
            }
        });
    }
    public void WriteMessage(string itemInternalName) 
    {

        using (Packet _packet = new Packet(GetMessageId()))
        {
            _packet.Write(itemInternalName);

            ClientSend.SendTcpData(_packet);
        }
    }
}
