
using UnityEngine;

public class AbilityHandle : IHandle
{
	public int GetMessageId()
	{
		return (int)Packets.castAbility;
	}

	public void ReadMessage(Packet _packet)
	{
		var ownerId = _packet.ReadInt();
		var abilityIndex = _packet.ReadInt();
		var startPosition = new Vector3(_packet.ReadFloat(), _packet.ReadFloat(), _packet.ReadFloat());
		var targetPosition = new Vector3(_packet.ReadFloat(), _packet.ReadFloat(), _packet.ReadFloat());
		ThreadManager.ExecuteOnMainThread(() =>
		{
			Debug.Log($"Character {ownerId} is casting abiliy {abilityIndex}");
			var character = GameManager.Instance.GetCharacter(ownerId);
			character.CastAbility(abilityIndex, startPosition, targetPosition);
		});
	}

	public void SendAbilityCast(int abilityIndex, Vector3 targetPosition)
	{
		using (Packet _packet = new Packet(GetMessageId()))
		{
			_packet.Write(abilityIndex);
			_packet.Write(targetPosition.x);
			_packet.Write(targetPosition.y);
			_packet.Write(targetPosition.z);

			ClientSend.SendTcpData(_packet);
		}
	}
}
