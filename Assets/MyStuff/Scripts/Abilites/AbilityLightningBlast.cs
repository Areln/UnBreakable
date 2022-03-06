using System.Collections;
using UnityEngine;

public class AbilityLightningBlast : Ability
{
	public GameObject BlastPrefab;
	public GameObject CastParticles;
	public float CastTime;

	private GameObject lightingBallObject;
	private Animator animator;
	private Vector3 targetPosition;

	public void Awake()
	{
		SetupAbility(GetComponentInParent<CharacterBrain>());
		animator = gameObject.GetComponent<Animator>();
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

	public override void Activate(Vector3 targetPosition)
	{
		if (currentCooldown <= 0)
		{
			var newPos = targetPosition + -transform.up * 1;
			this.targetPosition = newPos;
			lightingBallObject = Instantiate(BlastPrefab, owner.transform.position, Quaternion.identity);
			lightingBallObject.GetComponent<LightningBall>().InitializeSpell(targetPosition, owner);
			CastSpell();
		}
	}

	public void CastSpell()
	{
		StartCoroutine(waitForSec(CastTime));
	}

	public override void RemoveAbility()
	{
		throw new System.NotImplementedException();
	}

	private IEnumerator waitForSec(float sec)
	{
		currentCooldown = maxCooldown;
		owner.gameObject.transform.LookAt(targetPosition);
		var newPosition = owner.transform.position + owner.transform.forward * 1f;
		Instantiate(CastParticles, newPosition, owner.transform.rotation);
		animator.SetBool("Casting", true);
		owner.agent.isStopped = true;
		yield return new WaitForSeconds(sec);
		animator.SetBool("Casting", false);
		lightingBallObject.SetActive(true);
		owner.agent.isStopped = false;
	}

	public override void SetupAbility(CharacterBrain _owner)
	{
		owner = _owner;
	}

}
