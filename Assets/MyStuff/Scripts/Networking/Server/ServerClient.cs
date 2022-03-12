using Networking;
using System;
using System.Net;
using System.Net.Sockets;

namespace Server.Networking
{
	class ServerClient
	{
		public static int DataBufferSize = 4096;
		public int Id;
		public Tcp TcpData;
		public Udp UdpData;

		public ServerClient(int _clientId)
		{
			Id = _clientId;
			TcpData = new Tcp(Id);
			UdpData = new Udp(Id);
		}

		public class Tcp
		{
			public TcpClient Socket;

			private readonly int id;
			private NetworkStream stream;
			private Packet receivedData;
			private byte[] receiveBuffer;

			public Tcp(int _id)
			{
				id = _id;
			}

			public void Connect(TcpClient _socket)
			{
				Socket = _socket;
				Socket.ReceiveBufferSize = DataBufferSize;
				Socket.SendBufferSize = DataBufferSize;

				stream = Socket.GetStream();

				receivedData = new Packet();
				receiveBuffer = new byte[DataBufferSize];

				stream.BeginRead(receiveBuffer, 0, DataBufferSize, new AsyncCallback(ReceiveCallback), null);

				ServerWelcomeMessage.WelcomeSend(id, "Welcome to the server!");
			}

			public void SendData(Packet _packet)
			{
				try
				{
					if (Socket != null)
					{
						stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
					}
				}
				catch (Exception _ex)
				{
					Console.WriteLine($"Error sending data to player {id} via TCP: {_ex}");
				}
			}

			private void ReceiveCallback(IAsyncResult _result)
			{
				try
				{
					int _byteLength = stream.EndRead(_result);
					if (_byteLength <= 0)
					{
						Server.Instance.Clients[id].Disconnect();
						return;
					}

					byte[] _data = new byte[_byteLength];
					Array.Copy(receiveBuffer, _data, _byteLength);

					receivedData.Reset(HandleData(_data));
					stream.BeginRead(receiveBuffer, 0, DataBufferSize, new AsyncCallback(ReceiveCallback), null);
				}
				catch (Exception _ex)
				{
					Console.WriteLine($"Error receiving Tcp data: {_ex}");
					Server.Instance.Clients[id].Disconnect();
				}
			}

			private bool HandleData(byte[] _data)
			{
				int _packetLength = 0;

				receivedData.SetBytes(_data);

				if (receivedData.UnreadLength() >= 4)
				{
					_packetLength = receivedData.ReadInt();
					if (_packetLength <= 0)
					{
						return true;
					}
				}

				while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
				{
					byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
					ThreadManager.ExecuteOnMainThread(() =>
					{
						using (Packet _packet = new Packet(_packetBytes))
						{
							int _packetId = _packet.ReadInt();
							Server.PacketHandlers[_packetId](id, _packet);
						}
					});

					_packetLength = 0;
					if (receivedData.UnreadLength() >= 4)
					{
						_packetLength = receivedData.ReadInt();
						if (_packetLength <= 0)
						{
							return true;
						}
					}
				}

				if (_packetLength <= 1)
				{
					return true;
				}

				return false;
			}

			public void Disconnect()
			{
				Socket.Close();
				stream = null;
				receivedData = null;
				receiveBuffer = null;
				Socket = null;
			}
		}

		public class Udp
		{
			public IPEndPoint EndPoint;

			private int id;

			public Udp(int _id)
			{
				id = _id;
			}

			public void Connect(IPEndPoint _endPoint)
			{
				EndPoint = _endPoint;
			}

			public void SendData(Packet _packet)
			{
				Server.Instance.SendUdpData(EndPoint, _packet);
			}

			public void HandleData(Packet _packetData)
			{
				int _packetLength = _packetData.ReadInt();
				byte[] _packetBytes = _packetData.ReadBytes(_packetLength);

				ThreadManager.ExecuteOnMainThread(() =>
				{
					using (Packet _packet = new Packet(_packetBytes))
					{
						int _packetId = _packet.ReadInt();
						Server.PacketHandlers[_packetId](id, _packet);
					}
				});
			}

			public void Disconnect()
			{
				EndPoint = null;
			}
		}

		private void Disconnect()
		{
			Console.WriteLine($"{TcpData.Socket.Client.RemoteEndPoint} has disconnected.");
			TcpData.Disconnect();
			UdpData.Disconnect();

			// TODO: Send Out message that this player disconnected so we can unload them from the clients.
		}
	}
}
