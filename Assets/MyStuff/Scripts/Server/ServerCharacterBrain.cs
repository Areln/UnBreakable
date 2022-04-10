using Server.Networking;
using System;
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
		public string lootObjectName;

		internal NavMeshAgent agent;
		internal Stats Stats;
		internal Animator animator;

		public Transform AbilityHolder;
		public GameObject[] abilities;

		internal ServerAbility CurrentlyCastingAbility { get; set; }

		public abstract void CharacterDie();

		private void Start()
		{
			animator = GetComponent<Animator>();
		}

		public void ChangeHealth(int healthChange)
		{
			currentHealth += healthChange;
			GetComponent<HPScript>().ChangeHP(healthChange, gameObject.transform.position);

			new ServerHealthUpdateHandle().WriteMessage(GetInstanceID(), healthChange);

			if (currentHealth <= 0)
			{
				CharacterDie();
			}
		}

		public void StopCharacterFromMoving()
		{
			SetCharacterPath(gameObject.transform.position);
		}

		public void SetCharacterPath(Vector3 newPosition)
		{
			var path = agent.path;
			agent.SetDestination(newPosition);
			agent.CalculatePath(newPosition, path);
			new ServerCharacterMoveHandle().SendCharacterMovement(this, path);
		}

		internal void UpdateMana(int manaCost)
		{
			currentMana += manaCost;
			new ServerManaUpdateHandle().WriteMessage(GetInstanceID(), manaCost);
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
