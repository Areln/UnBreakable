using UnityEngine;

public class WelcomeHandle : IHandle // : MonoBehaviour
{
	public int GetMessageId()
	{
		return (int)Packets.welcome;
	}

	public void ReadMessage(Packet _packet)
	{
		string _msg = _packet.ReadString();

		Debug.Log($"Message from server: {_msg}");
	}
}
