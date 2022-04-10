using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Server.Networking
{
    public class ServerCharacterUnEquipItem : IServerHandle
    {
        public int GetMessageId()
        {
            return (int)Packets.CharacterUnEquipItem;
        }

        public void ReadMessage(int _fromClientId, Packet _packet)
        {
            bool isArmor = _packet.ReadBool();
            int equipableType = _packet.ReadInt();

            ThreadManager.ExecuteOnMainThread(() =>
            {
                ServerPlayerBrain character = ServerGameManager.Instance.GetPlayer(_fromClientId);
                ServerPlayerInventory charInv = character.GetComponent<ServerPlayerInventory>();
                int? slotIndex = null;
                if (isArmor)
                {
                    slotIndex = charInv.ServerUnEquipItem((ArmorType)equipableType);
                }
                else 
                {
                    slotIndex = charInv.ServerUnEquipItem((WeaponType)equipableType);
                }

                WriteMessage(isArmor, character.GetInstanceID(), equipableType, slotIndex);
            });
        }

        void WriteMessage(bool isArmor, int characterId, int equipableType, int? slotIndex) 
        {
            using (Packet _packet = new Packet(GetMessageId()))
            {
                _packet.Write(isArmor);
                _packet.Write(characterId);
                _packet.Write(equipableType);
                _packet.Write(slotIndex.HasValue ? slotIndex.Value : -1);

                ServerSend.SendTcpDataToAllAuthenticated(_packet);
            }
        }
    }
}