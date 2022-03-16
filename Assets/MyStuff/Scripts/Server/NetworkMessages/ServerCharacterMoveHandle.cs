using System;
using UnityEngine;
using UnityEngine.AI;

namespace Server.Networking
{
	public class ServerCharacterMoveHandle : IServerHandle
	{
		public int GetMessageId()
		{
			return (int)Packets.moveCharacter;
		}

		public void ReadMessage(int _fromClientId, Packet _packet)
		{
			var positionToMoveTowards = new Vector3(_packet.ReadFloat(), _packet.ReadFloat(), _packet.ReadFloat());

			//TODO: Load the player who logged in and store player in dictionary with the fromClientId as the key.
			ThreadManager.ExecuteOnMainThread(() =>
			{
				ServerPlayerBrain character = ServerGameManager.Instance.GetPlayer(_fromClientId);
				character.SetCharacterPath(positionToMoveTowards);
			});
		}

		public void SendCharacterMovement(ServerCharacterBrain movingCharacter)
		{
			using (Packet _packet = new Packet((int)Packets.playerData))
			{
				_packet.Write(movingCharacter.GetInstanceID());
				_packet.Write(movingCharacter.agent.path.corners.Length);
				foreach (var corner in movingCharacter.agent.path.corners)
				{
					_packet.Write(corner.x);
					_packet.Write(corner.y);
					_packet.Write(corner.z);
				}

				ServerSend.SendTcpDataToAll(_packet);
			}			
		}
	}
}