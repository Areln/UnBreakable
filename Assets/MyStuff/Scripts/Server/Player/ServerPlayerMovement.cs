using Server.Networking;
using UnityEngine;
using UnityEngine.AI;

namespace Server
{
    public class ServerPlayerMovement : MonoBehaviour
    {
        internal NavMeshAgent agent;

        internal void SetPath()
		{

		}

		internal void StopPlayerFromMoving()
		{
			agent.SetDestination(transform.position);
			var path = agent.path;
			agent.CalculatePath(transform.position, path);
			new ServerCharacterMoveHandle().SendCharacterMovement(GetComponent<ServerCharacterBrain>(), path);
		}
	}
}
