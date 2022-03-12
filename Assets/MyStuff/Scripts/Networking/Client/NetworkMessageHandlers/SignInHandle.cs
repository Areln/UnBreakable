using Networking;
using UnityEngine;

public class SignInHandle : IHandle // : MonoBehaviour
{
	public int GetMessageId()
	{
		return (int)Packets.signIn;
	}

	public void ReadMessage(Packet _packet)
	{
		string _msg = _packet.ReadString();

		Debug.Log($"Message from server: {_msg}");
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
