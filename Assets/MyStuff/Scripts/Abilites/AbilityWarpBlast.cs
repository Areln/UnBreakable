using System.Collections;
using UnityEngine;

public class AbilityWarpBlast : Ability
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

	public override void Activate(Vector3 targetPosition)
	{
		//checks if ability is on cooldown or if the player is casting an ability already.
		if (currentCooldown > 0 || owner.CurrentlyCastingAbility != null)
		{
			return;
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
			owner.agent.GetComponent<PlayerMovement>().StopPlayerFromMoving();

			//instantiates particle object
			Instantiate(particlePrefab, transform.position, transform.rotation);

			//hitbox
			HitCheck();
		}
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

	public override void SetupAbility(CharacterBrain _owner)
	{
		owner = _owner;
	}

}
