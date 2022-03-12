using Networking;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Server.Networking
{
	class Server : MonoBehaviour
	{
		public static Server Instance;
		public int MaxPlayers = 1000;
		public int Port = 25565;

		public Dictionary<int, ServerClient> Clients = new Dictionary<int, ServerClient>();
		public delegate void PacketHandler(int _fromClient, Packet _packet);
		public static Dictionary<int, PacketHandler> PacketHandlers;

		private TcpListener tcpListener;
		private UdpClient udpListener;

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
				DontDestroyOnLoad(this);
			}
			else if (Instance != this)
			{
				Debug.Log("Instance already exists, destroying object!");
				Destroy(this);
			}
		}

		private void Start()
		{
			Console.WriteLine("Starting server...");
			InitializeServerData();

			tcpListener = new TcpListener(IPAddress.Any, Port);
			tcpListener.Start();
			tcpListener.BeginAcceptTcpClient(new AsyncCallback(TcpConnectCallback), null);

			udpListener = new UdpClient(Port);
			udpListener.BeginReceive(UdpReceiveCallback, null);

			Console.WriteLine($"Server started on port {Port}.");
		}

		private void TcpConnectCallback(IAsyncResult _result)
		{
			TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
			tcpListener.BeginAcceptTcpClient(new AsyncCallback(TcpConnectCallback), null);
			Console.WriteLine($"Incomming connection from {_client.Client.RemoteEndPoint}...");

			for (int i = 1; i <= MaxPlayers; i++)
			{
				if (Clients[i].TcpData.Socket == null)
				{
					Clients[i].TcpData.Connect(_client);
					return;
				}
			}

			Console.WriteLine($"{_client.Client.RemoteEndPoint} failed to connect. Server full!");
		}

		private void UdpReceiveCallback(IAsyncResult _result)
		{
			try
			{
				IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
				byte[] _data = udpListener.EndReceive(_result, ref _clientEndPoint);
				udpListener.BeginReceive(UdpReceiveCallback, null);

				if (_data.Length < 4)
				{
					return;
				}
				using (Packet _packet = new Packet(_data))
				{
					int _clientId = _packet.ReadInt();

					if (_clientId == 0)
					{
						return;
					}

					if (Clients[_clientId].UdpData.EndPoint == null)
					{
						Clients[_clientId].UdpData.Connect(_clientEndPoint);
						return;
					}

					if (Clients[_clientId].UdpData.EndPoint.ToString() == _clientEndPoint.ToString())
					{
						Clients[_clientId].UdpData.HandleData(_packet);
					}
				}
			}
			catch (Exception _ex)
			{
				Console.WriteLine($"Error receiving Udp data: {_ex}");
			}
		}

		public void SendUdpData(IPEndPoint _clientEndPoint, Packet _packet)
		{
			try
			{
				if (_clientEndPoint != null)
				{
					udpListener.BeginSend(_packet.ToArray(), _packet.Length(), _clientEndPoint, null, null);
				}
			}
			catch (Exception _ex)
			{
				Console.WriteLine($"Error sending data to {_clientEndPoint} via Udp: {_ex}");
			}

		}

		private void InitializeServerData()
		{
			for (int i = 1; i <= MaxPlayers; i++)
			{
				Clients.Add(i, new ServerClient(i));
			}

			PacketHandlers = new Dictionary<int, PacketHandler>();
			var list = InterfaceGetter.GetEnumerableOfType<IServerHandle>();
			foreach (var item in list)
			{
				IServerHandle networkAction = (IServerHandle)Activator.CreateInstance(item);
				PacketHandlers.Add(networkAction.GetMessageId(), networkAction.ReadMessage);
			}

			Console.WriteLine("Initialized packets.");
		}
	}
}

