
namespace Server.Networking
{
	public class ServerCharacterDieHandle : IServerHandle
	{
		public int GetMessageId()
		{
			return (int)Packets.characterDie;
		}

		public void ReadMessage(int _fromClientId, Packet _packet)
		{
			throw new System.NotImplementedException();
		}

		public void WriteMessage(int characterId)
		{
			using(var packet = new Packet(GetMessageId()))
			{
				packet.Write(characterId);

				ServerSend.SendTcpDataToAllAuthenticated(packet);
			}
		}
	}
}
