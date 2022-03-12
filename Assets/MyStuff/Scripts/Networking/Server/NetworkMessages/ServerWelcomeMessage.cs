using Networking;

namespace Server.Networking
{
	public static class ServerWelcomeMessage
	{
		public static void WelcomeSend(int _toClient, string _msg)
		{
			using (Packet _packet = new Packet((int)Packets.signIn))
			{
				_packet.Write(_msg);

				ServerSend.SendTcpData(_toClient, _packet);
			}
		}
	}
}
