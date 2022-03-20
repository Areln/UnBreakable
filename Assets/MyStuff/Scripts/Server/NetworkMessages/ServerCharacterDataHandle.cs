using Server.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
	public class ServerCharacterDataHandle : IServerHandle
	{
		public int GetMessageId()
		{
			return (int)Packets.characterData;
		}

		public void ReadMessage(int _fromClientId, Packet _packet)
		{
			throw new System.NotImplementedException();
		}

		public void WriteCharacterData(ServerBasicAI character)
		{
			using (var _packet = new Packet(GetMessageId()))
			{
				WriteCharacterPacket(_packet, character);

				ServerSend.SendTcpDataToAllAuthenticated(_packet);
			}
		}
		public void WriteCharacterData(int _toClientId, ServerBasicAI character)
		{
			using (var _packet = new Packet(GetMessageId()))
			{
				WriteCharacterPacket(_packet, character);

				ServerSend.SendTcpDataAuthenticated(_toClientId,_packet);
			}
		}

		private void WriteCharacterPacket(Packet _packet, ServerBasicAI characterData)
		{
			if (string.IsNullOrWhiteSpace(characterData.characterName))
			{
				characterData.characterName = characterData.PrefabName;
			}
			Debug.Log(characterData.characterName);
			_packet.Write(characterData.characterName);
			_packet.Write(characterData.GetInstanceID());
			_packet.Write(characterData.PrefabName);

			// Position
			_packet.Write(characterData.transform.position.x);
			_packet.Write(characterData.transform.position.y);
			_packet.Write(characterData.transform.position.z);

			// Rotation 
			_packet.Write(characterData.transform.rotation.eulerAngles.y);
		}
	}
}
