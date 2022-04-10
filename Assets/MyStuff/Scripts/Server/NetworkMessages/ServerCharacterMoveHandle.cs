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

				if(movingCharacter is ServerPlayerBrain)
				{
					Debug.Log(path.corners.Length);
				}

				foreach (var corner in path.corners)
				{
					var nextCorner = new Vector3(corner.x, movingCharacter.transform.position.y, corner.z);
					_packet.Write(nextCorner.x);
					_packet.Write(movingCharacter.transform.position.y);
					_packet.Write(nextCorner.z);
					var currentPos = (lastCorner.HasValue ? new Vector3(lastCorner.Value.x, movingCharacter.transform.position.y, lastCorner.Value.z) : movingCharacter.transform.position);

					lastCorner = nextCorner;
					float angleRad = 0f;

					if (path.corners[0] == corner && path.corners.Length > 1)
					{
						nextCorner = new Vector3(path.corners[1].x, movingCharacter.transform.position.y, path.corners[1].z);
						angleRad = Mathf.Atan2(nextCorner.x - currentPos.x, nextCorner.z - currentPos.z);
					}
					else if (path.corners.Length > 1)
					{
						angleRad = Mathf.Atan2(nextCorner.x - currentPos.x, nextCorner.z - currentPos.z);
					}
					else
					{
						nextCorner = movingCharacter.transform.position + movingCharacter.transform.forward;
						angleRad = Mathf.Atan2(nextCorner.x - currentPos.x, nextCorner.z - currentPos.z);
					}

					float angle = (180 / Mathf.PI) * angleRad;
					_packet.Write(angle);
				}

				ServerSend.SendTcpDataToAllAuthenticated(_packet);
			}			
		}
	}
}