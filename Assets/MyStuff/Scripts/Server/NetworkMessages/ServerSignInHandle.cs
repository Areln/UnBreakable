using System;
using UnityEngine;

namespace Server.Networking
{
    public class ServerSignInHandle : IServerHandle
    {
        private const string DefaultEmptySlot = " ";

        public int GetMessageId()
        {
            return (int)Packets.signIn;
        }

        public void ReadMessage(int _fromClientId, Packet _packet)
        {
            string _username = _packet.ReadString();
            string _password = _packet.ReadString();

            ThreadManager.ExecuteOnMainThread(() =>
            {
                var playerLogginIn = ServerGameManager.Instance.LoadPlayer(_fromClientId, _username);
                Server.Instance.Clients[_fromClientId].isAuthenticated = true;
                SendPlayerData(_fromClientId, playerLogginIn);
            });
        }

        public void SendPlayerData(int _toClientId, ServerPlayerBrain connectingPlayerData)
        {
            // Send newly connected player data to all other connected clients.
            using (Packet _packet = new Packet((int)Packets.playerData))
            {
                WritePlayerPacket(_packet, connectingPlayerData, false);

                ServerSend.SendTcpDataToAllAuthenticated(_toClientId, _packet);
            }

            // Send all players to the newly connected client
            foreach (var player in ServerGameManager.Instance.ClientPlayers)
            {
                using (Packet _packet = new Packet((int)Packets.playerData))
                {
                    WritePlayerPacket(_packet, player.Value, player.Key == _toClientId);
                    ServerSend.SendTcpDataAuthenticated(_toClientId, _packet);
                }
            }
            // Send all Characters to newly connected client
            foreach (var character in ServerGameManager.Instance.Characters.Values)
            {
                new ServerCharacterDataHandle().WriteCharacterData(_toClientId, character);
            }

        }

        private void WritePlayerPacket(Packet _packet, ServerPlayerBrain playerData, bool isClientPlayer)
        {
            _packet.Write(isClientPlayer);
            _packet.Write(playerData.characterName);
            _packet.Write(playerData.GetInstanceID());

            // Position
            _packet.Write(playerData.transform.position.x);
            _packet.Write(playerData.transform.position.y);
            _packet.Write(playerData.transform.position.z);

            // Rotation 
            _packet.Write(playerData.transform.rotation.eulerAngles.y);

            //Abilities
            _packet.Write(playerData.abilities.Length);
            for (int i = 0; i < playerData.abilities.Length; i++)
            {
                _packet.Write(playerData.abilities[i].GetComponent<ServerAbility>().PrefabName);
            }

            // Equipment Data
            _packet.Write(playerData.playerInventory.serverArmorEquipSlots.TryGetValue(ArmorType.HeadPiece, out var armorItem) ? armorItem.InternalName : DefaultEmptySlot);
            _packet.Write(playerData.playerInventory.serverArmorEquipSlots.TryGetValue(ArmorType.ChestPiece, out armorItem) ? armorItem.InternalName : DefaultEmptySlot);
            _packet.Write(playerData.playerInventory.serverArmorEquipSlots.TryGetValue(ArmorType.GlovePiece, out armorItem) ? armorItem.InternalName : DefaultEmptySlot);
            _packet.Write(playerData.playerInventory.serverArmorEquipSlots.TryGetValue(ArmorType.LegPiece, out armorItem) ? armorItem.InternalName : DefaultEmptySlot);
            _packet.Write(playerData.playerInventory.serverArmorEquipSlots.TryGetValue(ArmorType.Necklace, out armorItem) ? armorItem.InternalName : DefaultEmptySlot);
            _packet.Write(playerData.playerInventory.serverArmorEquipSlots.TryGetValue(ArmorType.RingR, out armorItem) ? armorItem.InternalName : DefaultEmptySlot);
            _packet.Write(playerData.playerInventory.serverArmorEquipSlots.TryGetValue(ArmorType.RingL, out armorItem) ? armorItem.InternalName : DefaultEmptySlot);
            _packet.Write(playerData.playerInventory.serverWeaponEquipSlots.TryGetValue(WeaponType.MainHand, out var weaponItem) ? weaponItem.InternalName : DefaultEmptySlot);
            _packet.Write(playerData.playerInventory.serverWeaponEquipSlots.TryGetValue(WeaponType.OffHand, out weaponItem) ? weaponItem.InternalName : DefaultEmptySlot);

            if (isClientPlayer)
            {
                // Inventory Data
                _packet.Write(playerData.playerInventory.InventoryItems.Count);
                foreach (var item in playerData.playerInventory.InventoryItems)
                {
                    _packet.Write(item.InternalName);
                }
            }
        }
    }
}