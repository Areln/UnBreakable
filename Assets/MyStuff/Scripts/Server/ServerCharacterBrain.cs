using Server.Networking;
using System.Collections.Generic;
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

		public Transform AbilityHolder;
		public GameObject[] abilities;

		internal ServerAbility CurrentlyCastingAbility { get; set; }

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
			var path = agent.path;
			agent.SetDestination(newPosition);
			agent.CalculatePath(newPosition, path);
			new ServerCharacterMoveHandle().SendCharacterMovement(this, path);
		}

		public virtual void CastAbility(int abilityCastId, Vector3 targetPosition)
		{
			// TODO: update target position based to be within max range of ability.
			var ability = abilities[abilityCastId].GetComponent<ServerAbility>();
			if (ability.owner == null)
			{
				ability.SetupAbility(this);
			}
			if (ability.Activate(targetPosition))
			{
				new ServerAbilityHandle().SendAbilityCastToAll(GetInstanceID(), abilityCastId, transform.position, targetPosition);
			}
		}

		internal virtual void LoadAbilities()
		{
			for (int i = 0; i < abilities.Length; i++)
			{
				var abilityGo = Instantiate(abilities[i], AbilityHolder);
				abilities[i] = abilityGo;
				abilityGo.SetActive(true);
				abilityGo.GetComponent<ServerAbility>().SetupAbility(this);
			}
		}

	}
}
