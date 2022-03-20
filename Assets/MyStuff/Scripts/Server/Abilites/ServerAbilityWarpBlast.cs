using System.Collections;
using UnityEngine;

namespace Server
{
	public class ServerAbilityWarpBlast : ServerAbility
	{
		internal Collider abilityHitBox;
		public GameObject particlePrefab;
		public float maxCastTime;

		public void Start()
		{
			abilityHitBox = GetComponentInChildren<Collider>();
		}

		public void Update()
		{
			if (currentCooldown > 0)
			{
				currentCooldown -= Time.deltaTime;
			}
		}

		public override bool Activate(Vector3 targetPosition)
		{
			//checks if ability is on cooldown or if the player is casting an ability already.
			if (currentCooldown > 0 || owner.CurrentlyCastingAbility != null)
			{
				return false;
			}
			//uses mana
			if (owner.currentMana >= manaCost)
			{
				owner.currentMana -= manaCost;
				//cast setup successfull so set ability CD
				currentCooldown = maxCooldown;
				owner.gameObject.transform.LookAt(targetPosition);
				//teleport player
				owner.gameObject.transform.position = targetPosition;
				owner.agent.Warp(targetPosition);
				var playerMovement = owner.agent.GetComponent<ServerPlayerMovement>();
				if (playerMovement != null)
				{
					playerMovement.StopPlayerFromMoving();
				}

				//instantiates particle object
				//Instantiate(particlePrefab, transform.position, transform.rotation);

				//hitbox
				HitCheck();
			}
			else
			{
				return false;
			}

			return true;
		}

		private IEnumerator waitForSec(float sec)
		{
			abilityHitBox.enabled = true;
			yield return new WaitForSeconds(sec);
			abilityHitBox.enabled = false;
		}

		public void HitCheck()
		{
			StartCoroutine(waitForSec(.5f));
		}

		public override void RemoveAbility()
		{
			throw new System.NotImplementedException();
		}

		public override void SetupAbility(ServerCharacterBrain _owner)
		{
			owner = _owner;
		}
	}
}
