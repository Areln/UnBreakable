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

	public void WriteMessage()
	{
		using (Packet _packet = new Packet(GetMessageId()))
		{
			_packet.Write(SignInManager.Instance.LoginUsername.text);
			// TODO: Securely send the password.
			_packet.Write(SignInManager.Instance.LoginPassword.text);

			ClientSend.SendTcpData(_packet);
		}
	}
}
