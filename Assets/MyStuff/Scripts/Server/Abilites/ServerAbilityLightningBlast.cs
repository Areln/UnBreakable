using System.Collections;
using UnityEngine;

namespace Server
{
	public class ServerAbilityLightningBlast : ServerAbility
	{
		public GameObject BlastPrefab;
		public GameObject CastParticles;
		public float CastTime;

		private GameObject lightingBallObject;
		private Animator animator;
		private Vector3 targetPosition;
		private GameObject castParticles;
		private bool finishedCasting;

		public void Awake()
		{
			SetupAbility(GetComponentInParent<ServerCharacterBrain>());
			animator = gameObject.GetComponent<Animator>();
			if (animator == null)
			{
				animator = gameObject.GetComponentInParent<Animator>();
			}
		}

		public void Update()
		{
			if (IsCanceled && !finishedCasting)
			{
				animator.SetBool("Casting", false);
				owner.agent.isStopped = false;
				owner.CurrentlyCastingAbility = null;
				Destroy(lightingBallObject);
				Destroy(castParticles);
			}
			if (currentCooldown > 0)
			{
				currentCooldown -= Time.deltaTime;
			}
		}

		public override void Activate(Vector3 targetPosition)
		{
			if (currentCooldown <= 0 && manaCost <= owner.currentMana)
			{
				owner.CurrentlyCastingAbility = this;
				var newPos = targetPosition + -transform.up * 1;
				this.targetPosition = newPos;
				lightingBallObject = Instantiate(BlastPrefab, owner.transform.position, Quaternion.identity);
				lightingBallObject.GetComponent<ServerLightningBall>().InitializeSpell(targetPosition, owner);
				StartCoroutine(CastSpell(CastTime));
			}
		}

		public override void RemoveAbility()
		{
			throw new System.NotImplementedException();
		}

		private IEnumerator CastSpell(float sec)
		{
			owner.currentMana -= manaCost;
			IsCanceled = false;
			finishedCasting = false;
			currentCooldown = maxCooldown;
			owner.gameObject.transform.LookAt(targetPosition);
			var newPosition = owner.transform.position + owner.transform.forward * 1f;
			castParticles = Instantiate(CastParticles, newPosition, owner.transform.rotation);
			animator.SetBool("Casting", true);
			owner.agent.isStopped = true;
			yield return new WaitForSeconds(sec);
			if (!IsCanceled)
			{
				owner.CurrentlyCastingAbility = null;
				finishedCasting = true;
				animator.SetBool("Casting", false);
				lightingBallObject.SetActive(true);
				owner.agent.isStopped = false;
			}
			else
			{
				IsCanceled = false;
			}
		}

		public override void SetupAbility(ServerCharacterBrain _owner)
		{
			owner = _owner;
		}

	}
}
