﻿using System.Collections;
using UnityEngine;

public class AbilityLightningBlast : Ability
{
	public GameObject BlastPrefab;
	public GameObject CastParticles;
	public float CastTime;

	private GameObject lightingBallObject;
	private Vector3 targetPosition;
	private GameObject castParticles;
	private bool finishedCasting;

	public void Awake()
	{
		SetupAbility(GetComponentInParent<CharacterBrain>());
	}

	public void Update()
	{
		if (IsCanceled && !finishedCasting)
		{
			finishedCasting = true;
			owner.animator.SetBool("Casting", false);
			owner.IsMovementPaused = false;
			owner.CurrentlyCastingAbility = null;
			Destroy(lightingBallObject);
			Destroy(castParticles);
		}
		if (currentCooldown > 0)
		{
			currentCooldown -= Time.deltaTime;
		}
	}

	public override void Activate(Vector3 startPosition, Vector3 targetPosition)
	{
		owner.CurrentlyCastingAbility = this;
		var newPos = targetPosition + -transform.up * 1;
		this.targetPosition = newPos;
		lightingBallObject = Instantiate(BlastPrefab, owner.transform.position, Quaternion.identity);
		lightingBallObject.GetComponent<LightningBall>().InitializeSpell(targetPosition, owner);
		StartCoroutine(CastSpell(CastTime));
	}

	public override void RemoveAbility()
	{
		throw new System.NotImplementedException();
	}

	private IEnumerator CastSpell(float sec)
	{
		IsCanceled = false;
		finishedCasting = false;
		currentCooldown = maxCooldown; // TODO: server side this.
		owner.gameObject.transform.LookAt(targetPosition);
		var newPosition = owner.transform.position + owner.transform.forward * 1f;
		castParticles = Instantiate(CastParticles, newPosition, owner.transform.rotation);
		owner.animator.SetBool("Casting", true);
		owner.IsMovementPaused = true;
		yield return new WaitForSeconds(sec);
		if (!IsCanceled)
		{
			owner.CurrentlyCastingAbility = null;
			finishedCasting = true;
			owner.animator.SetBool("Casting", false);
			lightingBallObject.SetActive(true);
			owner.IsMovementPaused = false;
		}
		else
		{
			IsCanceled = false;
		}
	}

	public override void SetupAbility(CharacterBrain _owner)
	{
		owner = _owner;
	}

}
