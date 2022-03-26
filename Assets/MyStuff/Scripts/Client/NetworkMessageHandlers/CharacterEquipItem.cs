using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipItem : IHandle
{
    public int GetMessageId()
    {
        return (int)Packets.CharacterEquipItem;
    }

    public void ReadMessage(Packet _packet)
    {
        bool isFromClient= _packet.ReadBool();


        if (isFromClient)
        {
            int slotIndex = _packet.ReadInt();

            ThreadManager.ExecuteOnMainThread(() =>
            {
                GameManager.Instance.ClientPlayer.playerInventory.EquipItemToCharacter(slotIndex);
            });
        }
        else
        {
            int characterId = _packet.ReadInt();
            string itemInternalName = _packet.ReadString();

            ThreadManager.ExecuteOnMainThread(() =>
            {
                PlayerBrain character = (PlayerBrain)GameManager.Instance.GetCharacter(characterId);

                character.playerInventory.EquipItemToCharacter(itemInternalName);
            });
        }
    }

    public void WriteMessage(int itemSlotIndex)
    {
        using (Packet _packet = new Packet(GetMessageId()))
        {
            _packet.Write(itemSlotIndex);

            ClientSend.SendTcpData(_packet);
        }
    }
}
