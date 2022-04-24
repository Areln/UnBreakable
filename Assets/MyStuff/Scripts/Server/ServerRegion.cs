
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
	public class ServerRegion : MonoBehaviour
	{
		public Coordinates Location;

		internal Dictionary<int, ServerBasicAI> Characters { get; set; } = new Dictionary<int, ServerBasicAI>();


		internal Dictionary<int, ServerPlayerBrain> ClientPlayers = new Dictionary<int, ServerPlayerBrain>();
	}
}
