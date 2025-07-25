﻿using UnityEngine;

public class SwingWeapon : Ability
{
	internal Animator animator;

	public void Awake()
	{
		if (animator == null)
		{
			animator = gameObject.GetComponentInParent<Animator>();
		}
	}
	public void Update()
	{
		if (currentCooldown > 0)
		{
			currentCooldown -= Time.deltaTime;
		}
	}

	public override void Activate(Vector3 startPosition, Vector3 targetPosition)
	{
		//checks if ability is on cooldown or if the player is casting an ability already.
		if (currentCooldown > 0 || owner.CurrentlyCastingAbility != null)
		{
			return;
		}
		currentCooldown = maxCooldown;
		owner.gameObject.transform.LookAt(targetPosition);
		owner.IsMovementPaused = true;
		if (animator == null)
		{
			animator = gameObject.GetComponentInParent<Animator>();
		}
		animator.SetTrigger("Attack");

	}

	public void DoneCasting()
	{
		owner.IsMovementPaused = false;
	}

	public override void SetupAbility(CharacterBrain _owner)
	{
		owner = _owner;
	}

	public override void RemoveAbility()
	{
		throw new System.NotImplementedException();
	}
}
