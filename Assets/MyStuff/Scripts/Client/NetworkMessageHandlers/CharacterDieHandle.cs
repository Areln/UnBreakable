using UnityEngine;

public class CharacterDieHandle : IHandle
{
	public int GetMessageId()
	{
		return (int)Packets.characterDie;
	}

	public void ReadMessage(Packet _packet)
	{
		int characterId = _packet.ReadInt();

		ThreadManager.ExecuteOnMainThread(() => {
			var character = GameManager.Instance.GetCharacter(characterId);
			character.CharacterDie();
		});
	}
}
