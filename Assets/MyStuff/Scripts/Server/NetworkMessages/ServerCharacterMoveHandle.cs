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
				Vector3? lastCorner = null;
				foreach (var corner in path.corners)
				{
					var nextCorner = new Vector3(corner.x, movingCharacter.transform.position.y, corner.z);
					_packet.Write(nextCorner.x);
					_packet.Write(nextCorner.y);
					_packet.Write(nextCorner.z);
					var currentPos = (lastCorner.HasValue ? lastCorner.Value : movingCharacter.transform.position);
					var angleRad = Mathf.Atan2(nextCorner.x - currentPos.x, nextCorner.z - currentPos.z); 
					float angle = (180 / Mathf.PI) * angleRad;
					_packet.Write(angle);
					lastCorner = nextCorner;
				}

				ServerSend.SendTcpDataToAllAuthenticated(_packet);
			}			
		}
	}
}