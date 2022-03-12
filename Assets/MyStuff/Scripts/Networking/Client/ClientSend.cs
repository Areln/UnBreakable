using Networking;
using System;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
	internal static void SendTcpData(Packet _packet)
	{
		_packet.WriteLength();
		Client.Instance.TcpData.SendData(_packet);
	}
}