
namespace Server.Networking
{
	public class ServerHealthUpdateHandle : IServerHandle
	{
		public int GetMessageId()
		{
			return (int)Packets.healthUpdate;
		}

		public void ReadMessage(int _fromClientId, Packet _packet)
		{
			throw new System.NotImplementedException();
		}

		public void WriteMessage(int characterId, int healthChange)
		{
			using (var packet = new Packet(GetMessageId()))
			{
				packet.Write(characterId);
				packet.Write(healthChange);

				ServerSend.SendTcpDataToAllAuthenticated(packet);
			}
		}
	}
}
