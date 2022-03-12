using Networking;

namespace Server.Networking
{
	public static class ServerLoginMessage
	{
		public static void SignInSend(int _toClientId, string username)
		{
			// TODO: Send Player Data to all connected clients.
			using (Packet _packet = new Packet((int)Packets.signIn))
			{
				_packet.Write($"You have signed in as { username }");
				ServerSend.SendTcpDataToAll(_packet);
			}

			// TODO: Send connected other players Data to the newly connectd client.
			//foreach player in players
			//using (Packet _packet = new Packet((int)Packets.signIn))
			//{
			//	_packet.Write($"You have signed in as { username }");
			//	ServerSend.SendTcpData(_toClientId, _packet);
			//}
		}

		public static void SignInRecieved(int _fromClientId, Packet _packet)
		{
			string _username = _packet.ReadString();
			string _password = _packet.ReadString();

			//TODO: Load the player who logged in and store player in dictionary with the fromClientId as the key.

			ServerLoginMessage.SignInSend(_fromClientId, _username);
		}
	}
}
