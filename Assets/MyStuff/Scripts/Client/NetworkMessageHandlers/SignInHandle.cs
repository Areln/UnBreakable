using System;
using UnityEngine;

public class SignInHandle : IHandle
{
	public int GetMessageId()
	{
		return (int)Packets.signIn;
	}

	public void ReadMessage(Packet _packet)
	{
		throw new NotImplementedException();
	}

	public void WriteMessage(string username, string password)
	{
		using (Packet _packet = new Packet(GetMessageId()))
		{
			_packet.Write(username);
			// TODO: Securely send the password.
			_packet.Write(password);

			ClientSend.SendTcpData(_packet);
		}
	}
}
