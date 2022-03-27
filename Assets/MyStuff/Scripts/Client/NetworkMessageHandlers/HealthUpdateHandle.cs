
public class HealthUpdateHandle : IHandle
{
	public int GetMessageId()
	{
		return (int)Packets.healthUpdate;
	}

	public void ReadMessage(Packet _packet)
	{
		int characterId = _packet.ReadInt();
		int healthChange = _packet.ReadInt();

		ThreadManager.ExecuteOnMainThread(() => {
			var character = GameManager.Instance.GetCharacter(characterId);
			character.ChangeHealth(healthChange);
		});
	}
}
