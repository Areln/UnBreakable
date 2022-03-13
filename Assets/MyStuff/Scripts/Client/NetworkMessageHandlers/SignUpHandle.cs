using UnityEngine;

public class SignUpHandle : IHandle
{
	public int GetMessageId()
	{
		return (int)Packets.signUp;
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
			_packet.Write(SignUpManager.Instance.SignUpUsername.text);
			_packet.Write(SignUpManager.Instance.SignUpEmail.text);
			// TODO: Securely send the password.
			_packet.Write(SignUpManager.Instance.SignUpPassword.text);

			ClientSend.SendTcpData(_packet);
		}
	}
}
