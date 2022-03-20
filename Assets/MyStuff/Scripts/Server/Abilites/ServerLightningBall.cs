using UnityEngine;

namespace Server
{
	public class ServerLightningBall : ServerAbility
	{
		public float MoveSpeed;
		public GameObject LightingPrefab;
		public GameObject LightningParticlesPrefab;
		private Vector3 targetPosition;

		// Update is called once per frame
		private void FixedUpdate()
		{
			transform.position = Vector3.MoveTowards(transform.position, this.targetPosition, Time.fixedDeltaTime * MoveSpeed);
			if (Vector3.Distance(transform.position, targetPosition) < .1)
			{
				FinishSpell();
			}
		}

		private void FinishSpell()
		{
			var script = Instantiate(LightingPrefab, transform.position, Quaternion.identity).GetComponent<ServerLightningStrike>();
			script.SetupAbility(owner);

			//Instantiate(LightningParticlesPrefab, transform.position, Quaternion.identity);

			Destroy(gameObject);
		}

		public void InitializeSpell(Vector3 targetPosition, ServerCharacterBrain _owner)
		{
			this.targetPosition = targetPosition;
			owner = _owner;
		}

		public override void Activate(Vector3 targetPosition)
		{
			throw new System.NotImplementedException();
		}

		public override void RemoveAbility()
		{
			throw new System.NotImplementedException();
		}

		public override void SetupAbility(ServerCharacterBrain _owner)
		{
			throw new System.NotImplementedException();
		}
	}
}
