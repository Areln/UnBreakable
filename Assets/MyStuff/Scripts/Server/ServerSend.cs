namespace Server.Networking
{
	internal class ServerSend
	{
		#region Send Methods...
		internal static void SendTcpData(int _toClient, Packet _packet)
		{
			_packet.WriteLength();
			Server.Instance.Clients[_toClient].TcpData.SendData(_packet);
		}
		internal static void SendTcpDataAuthenticated(int _toClient, Packet _packet)
		{
			_packet.WriteLength();

			var client = Server.Instance.Clients[_toClient];
			if (client.isAuthenticated)
			{

				client.TcpData.SendData(_packet);
			}
			else
			{
				_packet.Dispose();
			}
		}
		internal static void SendUdpData(int _toClient, Packet _packet)
		{
			_packet.WriteLength();
			Server.Instance.Clients[_toClient].UdpData.SendData(_packet);
		}

		internal static void SendTcpDataToAll(Packet _packet)
		{
			_packet.WriteLength();
			for (int i = 1; i <= Server.Instance.MaxPlayers; i++)
			{
				var client = Server.Instance.Clients[i];
				client.TcpData.SendData(_packet);				
			}
		}

		internal static void SendTcpDataToAllAuthenticated(Packet _packet)
		{
			_packet.WriteLength();
			for (int i = 1; i <= Server.Instance.MaxPlayers; i++)
			{
				var client = Server.Instance.Clients[i];
				if (client.isAuthenticated)
				{
					client.TcpData.SendData(_packet);
				}
			}
		}
		internal static void SendTcpDataToAllAuthenticated(int _exceptClient, Packet _packet)
		{
			_packet.WriteLength();
			for (int i = 1; i <= Server.Instance.MaxPlayers; i++)
			{
				if (i != _exceptClient)
				{
					var client = Server.Instance.Clients[i];
					if (client.isAuthenticated)
					{
						client.TcpData.SendData(_packet);
					}
				}
			}
		}

		internal static void SendTcpDataToAll(int _exceptClient, Packet _packet)
		{
			_packet.WriteLength();
			for (int i = 1; i <= Server.Instance.MaxPlayers; i++)
			{
				if (i != _exceptClient)
				{
					Server.Instance.Clients[i].TcpData.SendData(_packet);
				}
			}
		}
		internal static void SendUdpDataToAll(Packet _packet)
		{
			_packet.WriteLength();
			for (int i = 1; i <= Server.Instance.MaxPlayers; i++)
			{
				Server.Instance.Clients[i].UdpData.SendData(_packet);
			}
		}
		internal static void SendUdpDataToAll(int _exceptClient, Packet _packet)
		{
			_packet.WriteLength();
			for (int i = 1; i <= Server.Instance.MaxPlayers; i++)
			{
				if (i != _exceptClient)
				{
					Server.Instance.Clients[i].UdpData.SendData(_packet);
				}
			}
		}
		#endregion...
	}
}

