using Networking;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Client : MonoBehaviour
{
	public static Client Instance;
	public static int DataBufferSize = 4096;

	public string Ip = "127.0.0.1";
	public ushort Port = 25565;
	//public int ClientId;
	public Tcp TcpData;
	public Udp UdpData;

	private bool isConnected = false;
	public delegate void PacketHandler(Packet _packet);
	public static Dictionary<int, PacketHandler> PacketHandlers;

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
		TcpData = new Tcp();
		UdpData = new Udp();
		ConnectToServer();
	}

	private void OnApplicationQuit()
	{
		Disconnect();
	}

	public void ConnectToServer()
	{
		InitializeClientData();

		TcpData.Connect();

	}

	public class Tcp
	{
		public TcpClient Socket;

		private NetworkStream stream;
		private Packet receivedData;
		private byte[] receiveBuffer;

		public void Connect()
		{
			Socket = new TcpClient()
			{
				ReceiveBufferSize = DataBufferSize,
				SendBufferSize = DataBufferSize
			};

			receiveBuffer = new byte[DataBufferSize];
			Socket.BeginConnect(Instance.Ip, Instance.Port, ConnectCallback, Socket);
		}

		private void ConnectCallback(IAsyncResult _result)
		{
			try
			{
				Socket.EndConnect(_result);

				stream = Socket.GetStream();

				receivedData = new Packet();

				stream.BeginRead(receiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
			}
			catch (Exception)
			{
				throw new Exception("No connection to server.");
			}
			if (!Socket.Connected)
			{
				return;
			}
			Instance.isConnected = true;
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
				Debug.Log($"Error sending data to server via TCP: {_ex}");
			}
		}

		private void ReceiveCallback(IAsyncResult _result)
		{
			try
			{
				int _byteLength = stream.EndRead(_result);
				if (_byteLength <= 0)
				{
					Instance.Disconnect();
					return;
				}
				byte[] _data = new byte[_byteLength];
				Array.Copy(receiveBuffer, _data, _byteLength);

				receivedData.Reset(HandleData(_data));
				stream.BeginRead(receiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
			}
			catch (Exception _ex)
			{
				Console.WriteLine($"Error receiving Tcp data: {_ex}");
				Disconnect();
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
						PacketHandlers[_packetId](_packet);
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

		private void Disconnect()
		{
			Instance.Disconnect();

			stream = null;
			receivedData = null;
			receiveBuffer = null;
			Socket = null;
		}
	}

	public class Udp
	{
		public UdpClient Socket;
		public IPEndPoint EndPoint;

		public Udp()
		{
			EndPoint = new IPEndPoint(IPAddress.Parse(Instance.Ip), Instance.Port);
		}

		public void Connect(int _localPort)
		{
			Socket = new UdpClient(_localPort);

			Socket.Connect(EndPoint);
			Socket.BeginReceive(ReceiveCallback, null);

			using (Packet _packet = new Packet())
			{
				SendData(_packet);
			}
		}

		public void SendData(Packet _packet)
		{
			try
			{
				if (Socket != null)
				{
					Socket.BeginSend(_packet.ToArray(), _packet.Length(), null, null);
				}
			}
			catch (Exception _ex)
			{
				Debug.Log($"Error sending packet to server via Udp: {_ex}");
			}
		}

		public void ReceiveCallback(IAsyncResult _result)
		{
			try
			{
				byte[] _data = Socket.EndReceive(_result, ref EndPoint);
				Socket.BeginReceive(ReceiveCallback, null);

				if (_data.Length < 4)
				{
					Instance.Disconnect();
					return;
				}

				HandleData(_data);
			}
			catch
			{
				Disconnect();
			}
		}

		private void HandleData(byte[] _data)
		{
			using (Packet _packet = new Packet(_data))
			{
				int _packetLength = _packet.ReadInt();
				_data = _packet.ReadBytes(_packetLength);
			}

			ThreadManager.ExecuteOnMainThread(() =>
			{
				using (Packet _packet = new Packet(_data))
				{
					int _packetId = _packet.ReadInt();
					PacketHandlers[_packetId](_packet);
				}
			});
		}

		private void Disconnect()
		{
			Instance.Disconnect();
			EndPoint = null;
			Socket = null;
		}
	}

	private void InitializeClientData()
	{
		PacketHandlers = new Dictionary<int, PacketHandler>();
		var list = InterfaceGetter.GetEnumerableOfType<IHandle>();
		foreach (var item in list)
		{
			IHandle networkAction = (IHandle)Activator.CreateInstance(item);
			PacketHandlers.Add(networkAction.GetMessageId(), networkAction.ReadMessage);
		}
		Debug.Log("Initialized " + PacketHandlers.Count + " packets.");
	}

	private void Disconnect()
	{
		if (isConnected)
		{
			//SteamUser.CancelAuthTicket(PlayerInfoManager.Instance.TicketHandle);
			isConnected = false;
			TcpData.Socket.Close();
			UdpData.Socket.Close();

			Debug.Log("Disconnected from server.");
		}
	}
}
