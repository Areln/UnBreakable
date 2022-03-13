public interface IHandle
{
	int GetMessageId();

	void ReadMessage(Packet _packet);

}
