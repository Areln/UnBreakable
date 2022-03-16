using Server.Networking;
using UnityEngine;
using UnityEngine.AI;

namespace Server
{
	public abstract class ServerCharacterBrain : MonoBehaviour
	{
		public string characterName;
		public int maxHealth;
		public int currentHealth;
		public int maxMana;
		public int currentMana;

		internal NavMeshAgent agent;
		internal Stats Stats;

		internal Ability CurrentlyCastingAbility { get; set; }

		public abstract void CharacterDie();

		public void TakeDamage(int _damage)
		{
			currentHealth -= _damage;
			GetComponent<HPScript>().ChangeHP(-_damage, gameObject.transform.position);

			if (currentHealth <= 0)
			{
				CharacterDie();
			}
		}

		public void StopCharacterFromMoving()
		{
			agent.destination = gameObject.transform.position;
		}

		public void SetCharacterPath(Vector3 newPosition)
		{
			agent.CalculatePath(newPosition, agent.path);
			new ServerCharacterMoveHandle().SendCharacterMovement(this);
			//agent.SetDestination(newPosition);
		}
	}
}
