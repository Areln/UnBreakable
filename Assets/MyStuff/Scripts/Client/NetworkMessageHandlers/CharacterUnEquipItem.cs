using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUnEquipItem : IHandle
{
    public int GetMessageId()
    {
        return (int)Packets.CharacterUnEquipItem;
    }

    public void ReadMessage(Packet _packet)
    {
        bool isArmor = _packet.ReadBool();
        int characterId = _packet.ReadInt();
        int equipableType = _packet.ReadInt();
        int slotIndex = _packet.ReadInt();

        ThreadManager.ExecuteOnMainThread(() =>
        {
            PlayerBrain character = (PlayerBrain)GameManager.Instance.GetCharacter(characterId);
            PlayerInventory charInv = character.GetComponent<PlayerInventory>();

            if (isArmor)
            {
                charInv.UnEquipArmor((ArmorType)equipableType, HudManager.Instance.InventoryItemSlots[slotIndex]);
            }
            else
            {
                charInv.UnEquipWeapon((WeaponType)equipableType, HudManager.Instance.InventoryItemSlots[slotIndex]);
            }
        });
    }

    public void WriteMessage(bool isArmor, int equipableType) 
    {
        using (Packet _packet = new Packet(GetMessageId()))
        {
            _packet.Write(isArmor);
            _packet.Write(equipableType);

            ClientSend.SendTcpData(_packet);
        }
    }
}
