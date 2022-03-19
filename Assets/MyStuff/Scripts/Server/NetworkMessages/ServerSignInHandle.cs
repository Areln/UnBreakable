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

				ServerSend.SendTcpDataToAll(_toClientId, _packet);
			}

			// Send all players to the newly connected client
			foreach (var player in ServerGameManager.Instance.ClientPlayers)
			{
				using (Packet _packet = new Packet((int)Packets.playerData))
				{
					WritePlayerPacket(_packet, player.Value, player.Key == _toClientId);
					ServerSend.SendTcpData(_toClientId, _packet);
				}
			}
			// Send all Characters to newly connected client
			foreach (var character in ServerGameManager.Instance.Characters.Values)
			{
				using (Packet _packet = new Packet((int)Packets.characterData))
				{
					new ServerCharacterDataHandle().WriteCharacterData(character);
					ServerSend.SendTcpData(_toClientId, _packet);
				}
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

			// Equipment Data
			_packet.Write(playerData.playerInventory.EquippedHelmetPiece?.InternalName ?? DefaultEmptySlot);
			_packet.Write(playerData.playerInventory.EquippedChestPiece?.InternalName ?? DefaultEmptySlot);
			_packet.Write(playerData.playerInventory.EquippedGlovePiece?.InternalName ?? DefaultEmptySlot);
			_packet.Write(playerData.playerInventory.EquippedLegPiece?.InternalName ?? DefaultEmptySlot);
			_packet.Write(playerData.playerInventory.EquippedNecklacePiece?.InternalName ?? DefaultEmptySlot);
			_packet.Write(playerData.playerInventory.EquippedRing1Piece?.InternalName ?? DefaultEmptySlot);
			_packet.Write(playerData.playerInventory.EquippedRing2Piece?.InternalName ?? DefaultEmptySlot);
			_packet.Write(playerData.playerInventory.EquippedMainHandWeapon?.InternalName ?? DefaultEmptySlot);
			_packet.Write(playerData.playerInventory.EquippedOffHandWeapon?.InternalName ?? DefaultEmptySlot);

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