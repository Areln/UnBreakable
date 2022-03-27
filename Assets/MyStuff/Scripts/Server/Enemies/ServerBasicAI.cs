using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Server
{
	public class ServerBasicAI : ServerCharacterBrain
	{
		public string PrefabName;
		public enum AIState { StandStill, Patrol, Attack }

		public AIState currentAIState;

		public ServerMobRegion Region { get { return _region; } set { _region = value; } }
		private ServerMobRegion _region;
		internal ServerAIInventory aIInventory;

		public GameObject MainTarget;

		public List<GameObject> TargetsInRange = new List<GameObject>();

		public List<Transform> PatrolTargets = new List<Transform>();
		public int currentPatrolTargetIndex;
		public bool isIdle;
		public float maxIdleTime;
		public float currentIdleTime;
		public Collider WeaponHitBox;

		internal HPScript hpScript;

		public GameObject lootChestPrefab;

		private void Awake()
		{
			Stats = GetComponent<Stats>();
			hpScript = GetComponent<HPScript>();
			animator = GetComponent<Animator>();
			agent = GetComponent<NavMeshAgent>();
			aIInventory = GetComponent<ServerAIInventory>();
		}

		// Start is called before the first frame update
		public void Start()
		{
			currentHealth = maxHealth;
			LoadAbilities();
			//sets patrol
			isIdle = true;
			currentIdleTime = maxIdleTime;
		}

		public void SetPatrolTargets(List<Transform> _points)
		{
			PatrolTargets = _points;
		}

		void NextPatrolTarget()
		{
			//currentPatrolTargetIndex++;
			//if (currentPatrolTargetIndex == PatrolTargets.Count)
			//{
			//    currentPatrolTargetIndex = 0;
			//}
			MainTarget = GetRandomPoint().gameObject;
		}

		Transform GetRandomPoint()
		{
			Transform _point = PatrolTargets[Random.Range(0, PatrolTargets.Count)];

			//if (MainTarget == _point)
			//{
			//    _point = GetRandomPoint();
			//}

			return _point;
		}

		// Update is called once per frame
		void Update()
		{
			//Animations
			if (agent.velocity != Vector3.zero)
			{
				animator.SetBool("IsWalking", true);
			}
			else
			{
				animator.SetBool("IsWalking", false);
			}

			// AI States
			switch (currentAIState)
			{
				case AIState.StandStill:
					AutoChangeState();
					break;

				case AIState.Patrol:
					if (MainTarget == null)
					{
						NextPatrolTarget();
					}
					// Debug.Log(Vector3.Distance(MainTarget.transform.position, gameObject.transform.position));
					if (Vector3.Distance(MainTarget.transform.position, gameObject.transform.position) <= 5)
					{
						//Debug.Log("idling");
						isIdle = true;
					}
					else
					{
						// Debug.Log("next target");
						isIdle = false;

						ChaseTarget();
					}
					if (isIdle)
					{
						currentIdleTime -= Time.deltaTime;
						if (currentIdleTime <= 0)
						{
							currentIdleTime = maxIdleTime;
							NextPatrolTarget();
						}
					}

					break;
				case AIState.Attack:
					GetAttackTarget();

					// moves toward target
					ChaseTarget();

					// checks cooldown of abilities then range of target and activates if in range
					CheckAttackRange();

					break;
				default:
					break;
			}

		}

		void GetAttackTarget()
		{
			MainTarget = TargetsInRange[0];
		}

		public override void CharacterDie()
		{
			//drop loot
			Vector3 dropPos = gameObject.transform.position;
			dropPos.y = 0.5f;
			GameObject lootChest = Instantiate(lootChestPrefab, dropPos, gameObject.transform.rotation);
			//add to servergamemanager
			//send message to createstorage
			if (Region)
			{
				Region.RemoveEnemy(this);
			}

			//destroys character model
			Destroy(gameObject);
		}

		public void AddInRage(GameObject target)
		{
			TargetsInRange.Add(target);
		}
		void AutoChangeState()
		{
			if (TargetsInRange.Count >= 1)
			{
				currentAIState = AIState.Patrol;
			}
		}
		void ChaseTarget()
		{
			if (MainTarget != null)
			{
				agent.SetDestination(MainTarget.transform.position);
				SetCharacterPath(MainTarget.transform.position);
			}
		}

		// referenced from the animation.
		public void TurnHitBoxOn()
		{
			WeaponHitBox.enabled = true;
		}

		// referenced from the animation.
		public void HitCheck()
		{
			//DoneCasting();
			agent.isStopped = false;
			//disable hitbox
			WeaponHitBox.enabled = false;
		}

		void CheckAttackRange()
		{
			if (abilities[0] != null && abilities[0].GetComponent<ServerAbility>().currentCooldown <= 0)
			{
				//check range
				if (Vector3.Distance(transform.position, MainTarget.transform.position) <= 1.5)
				{
					Vector3 targetPostition = new Vector3(agent.destination.x, this.transform.position.y, agent.destination.z);
					CastAbility(0, targetPostition);
				}
			}
		}
	}
}
