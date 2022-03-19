using System.Collections.Generic;
using UnityEngine;

public class CharacterDataHandle : IHandle
{
	public int GetMessageId()
	{
		return (int)Packets.characterData;
	}

	public void ReadMessage(Packet _packet)
	{
		var characterData = ReadCharacterPacket(_packet);
		ThreadManager.ExecuteOnMainThread(() =>
		{
			GameManager.Instance.LoadCharacter(characterData);
		});
	}

	private CharacterData ReadCharacterPacket(Packet _packet)
	{
		var characterName = _packet.ReadString();
		var characterId = _packet.ReadInt();
		var characterPrefabName = _packet.ReadString();

		// Position
		var positionX = _packet.ReadFloat();
		var positionY = _packet.ReadFloat();
		var positionZ = _packet.ReadFloat();

		// Rotation 
		var rotation = _packet.ReadFloat();

		return new CharacterData()
		{
			CharacterName = characterName,
			CharacterId = characterId,
			CharacterPrefabName = characterPrefabName,
			Position = new Vector3(positionX, positionY, positionZ),
			Rotation = rotation,
		};
	}
}

public class CharacterData
{
	public string CharacterName { get; set; }

	public int CharacterId { get; set; }

	public string CharacterPrefabName { get; set; }

	public Vector3 Position { get; set; }

	public float Rotation { get; set; }
}
