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

                if (isArmor)
                {
                    charInv.ServerUnEquipItem((ArmorType)equipableType);
                }
                else 
                {
                    charInv.ServerUnEquipItem((WeaponType)equipableType);
                }


                WriteMessage(isArmor, character.GetInstanceID(), equipableType);
            });
        }
        void WriteMessage(bool isArmor, int characterId, int equipableType) 
        {
            using (Packet _packet = new Packet(GetMessageId()))
            {
                _packet.Write(isArmor);
                _packet.Write(characterId);
                _packet.Write(equipableType);

                ServerSend.SendTcpDataToAllAuthenticated(_packet);
            }
        }
    }
}