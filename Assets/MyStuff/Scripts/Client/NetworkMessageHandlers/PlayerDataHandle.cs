using System.Collections.Generic;
using UnityEngine;

public class PlayerDataHandle : IHandle
{
	public int GetMessageId()
	{
		return (int)Packets.playerData;
	}

	public void ReadMessage(Packet _packet)
	{
		var playerData = ReadPlayerPacket(_packet);
		ThreadManager.ExecuteOnMainThread(() =>
		{
			GameManager.Instance.LoadConnectingPlayer(playerData);
		});
	}

	private PlayerData ReadPlayerPacket(Packet _packet)
	{
		var isClientPlayer = _packet.ReadBool();
		var characterName = _packet.ReadString();
		var playerId = _packet.ReadInt();

		// Position
		var positionX = _packet.ReadFloat();
		var positionY = _packet.ReadFloat();
		var positionZ = _packet.ReadFloat();

		// Rotation 
		var rotation = _packet.ReadFloat();

		//Abilities
		var abilityCount = _packet.ReadInt();
		var abilities = new string[abilityCount];
		for (int i = 0; i < abilityCount; i++)
		{
			abilities[i] =_packet.ReadString();
		}

		// Equipment Data
		var helm = _packet.ReadString();
		var chest = _packet.ReadString();
		var gloves = _packet.ReadString();
		var legs = _packet.ReadString();
		var neck = _packet.ReadString();
		var ring1 = _packet.ReadString();
		var ring2 = _packet.ReadString();
		var mainHand = _packet.ReadString();
		var offHand = _packet.ReadString();

		// Inventory Data
		var items = new Dictionary<int, StorageData>();
		if (isClientPlayer)
		{
			var itemCount = _packet.ReadInt();
			for (int i = 0; i < itemCount; i++)
			{
				items.Add(_packet.ReadInt(), _packet.ReadStorageData());
			}
		}

		return new PlayerData()
		{
			IsClientPlayer = isClientPlayer,
			CharacterName = characterName,
			PlayerId = playerId,
			Position = new Vector3(positionX, positionY, positionZ),
			Rotation = rotation,
			Abilities = abilities,
			Items = items,
			EquippedHelmetPiece = helm,
			EquippedChestPiece = chest,
			EquippedGlovePiece = gloves,
			EquippedLegPiece = legs,
			EquippedNecklacePiece = neck,
			EquippedRing1Piece = ring1,
			EquippedRing2Piece = ring2,
			EquippedMainHandWeapon = mainHand,
			EquippedOffHandWeapon = offHand
		};
	}
}

public class PlayerData
{
	public bool IsClientPlayer { get; set; }

	public string CharacterName { get; set; }

	public int PlayerId { get; set; }

	public Vector3 Position { get; set; }

	public float Rotation { get; set; }

	public string[] Abilities { get; set; }

	public Dictionary<int, StorageData> Items { get; set; }

	public string EquippedHelmetPiece { get; set; }

	public string EquippedChestPiece { get; set; }

	public string EquippedGlovePiece { get; set; }

	public string EquippedLegPiece { get; set; }

	public string EquippedNecklacePiece { get; set; }

	public string EquippedRing1Piece { get; set; }

	public string EquippedRing2Piece { get; set; }

	public string EquippedMainHandWeapon { get; set; }

	public string EquippedOffHandWeapon { get; set; }
}
