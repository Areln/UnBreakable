namespace Server.Networking
{
	public static class ServerWelcomeHandle
	{
		public static void WelcomeSend(int _toClient, string _msg)
		{
			using (Packet _packet = new Packet((int)Packets.welcome))
			{
				_packet.Write(_msg);

				ServerSend.SendTcpData(_toClient, _packet);
			}
		}
	}
}
