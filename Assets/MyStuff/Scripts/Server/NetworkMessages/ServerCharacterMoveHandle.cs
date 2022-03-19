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

			ThreadManager.ExecuteOnMainThread(() =>
			{
				ServerPlayerBrain character = ServerGameManager.Instance.GetPlayer(_fromClientId);
				character.SetCharacterPath(positionToMoveTowards);
			});
		}

		public void SendCharacterMovement(ServerCharacterBrain movingCharacter, NavMeshPath path)
		{
			using (Packet _packet = new Packet(GetMessageId()))
			{
				_packet.Write(movingCharacter.GetInstanceID());
				_packet.Write(path.corners.Length);
				foreach (var corner in path.corners)
				{
					_packet.Write(corner.x);
					_packet.Write(corner.y);
					_packet.Write(corner.z);
				}

				ServerSend.SendTcpDataToAllAuthenticated(_packet);
			}			
		}
	}
}