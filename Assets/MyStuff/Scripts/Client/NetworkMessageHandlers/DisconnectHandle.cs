public class DisconnectHandle : IHandle
{
	public int GetMessageId()
	{
		return (int)Packets.disconnect;
	}

	public void ReadMessage(Packet _packet)
	{

	}
}
