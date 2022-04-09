using UnityEngine;

public class CancelActionHandle : IHandle
{
	public int GetMessageId()
	{
		return (int)Packets.cancelAction;
	}

	public void ReadMessage(Packet _packet)
	{
		int characterId = _packet.ReadInt();
		bool wasAbilityCanceled = _packet.ReadBool();

		ThreadManager.ExecuteOnMainThread(() =>
		{
			PlayerBrain player = (PlayerBrain)GameManager.Instance.GetCharacter(characterId);
			player.CancelAll(wasAbilityCanceled);
		});
	}

	public void WriteMessage()
	{
		using (Packet _packet = new Packet(GetMessageId()))
		{
			ClientSend.SendTcpData(_packet);
		}
	}
}
