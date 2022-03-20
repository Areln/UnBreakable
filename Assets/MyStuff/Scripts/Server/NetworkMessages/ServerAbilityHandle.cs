using System;
using UnityEngine;
using UnityEngine.AI;

namespace Server.Networking
{
	public class ServerAbilityHandle : IServerHandle
	{
		public int GetMessageId()
		{
			return (int)Packets.castAbility;
		}

		public void ReadMessage(int _fromClientId, Packet _packet)
		{
			var abilityCastId = _packet.ReadInt();
			var targetPosition = new Vector3(_packet.ReadFloat(), _packet.ReadFloat(), _packet.ReadFloat());

			ThreadManager.ExecuteOnMainThread(() =>
			{
				ServerPlayerBrain character = ServerGameManager.Instance.GetPlayer(_fromClientId);
				character.CastAbility(abilityCastId, targetPosition);
			});
		}

		public void SendAbilityCastToAll(int ownerId, int abilityCasting, Vector3 startPosition, Vector3 targetPosition)
		{
			using (Packet _packet = new Packet(GetMessageId()))
			{
				_packet.Write(ownerId);
				_packet.Write(abilityCasting);
				_packet.Write(startPosition.x);
				_packet.Write(startPosition.y);
				_packet.Write(startPosition.z);
				_packet.Write(targetPosition.x);
				_packet.Write(targetPosition.y);
				_packet.Write(targetPosition.z);

				ServerSend.SendTcpDataToAllAuthenticated(_packet);
			}
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