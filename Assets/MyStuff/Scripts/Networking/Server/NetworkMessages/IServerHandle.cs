
using Networking;

public interface IServerHandle
{
	int GetMessageId();

	void ReadMessage(int _fromClientId, Packet _packet);

}
