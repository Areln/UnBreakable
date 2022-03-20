using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicAI : CharacterBrain
{

	public enum AIState { StandStill, Patrol, Attack }

	public AIState currentAIState;

	public MobRegion Region { get { return _region; } set { _region = value; } }
	private MobRegion _region;

	public GameObject MainTarget;

	public List<GameObject> TargetsInRange = new List<GameObject>();

	public List<Transform> PatrolTargets = new List<Transform>();
	public int currentPatrolTargetIndex;
	public bool isIdle;
	public float maxIdleTime;
	public float currentIdleTime;
	public Collider WeaponHitBox;

	internal Animator animator;

	internal HPScript hpScript;

	public GameObject lootChestPrefab;

	public Ability ability1;
	public Ability ability2;
	public Ability ability3;
	public Ability ability4;

	private void Awake()
	{
		Stats = GetComponent<Stats>();
		hpScript = GetComponent<HPScript>();
		animator = GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent>();
	}

	// Start is called before the first frame update
	public void Start()
	{
		currentHealth = maxHealth;
		SetupAbilities();
		//sets patrol
		isIdle = true;
		currentIdleTime = maxIdleTime;
	}

	internal void SetupAbilities()
	{
		if (ability1 != null)
		{
			ability1.SetupAbility(this);
		}
		if (ability2 != null)
		{
			ability2.SetupAbility(this);
		}
		if (ability3 != null)
		{
			ability3.SetupAbility(this);
		}
		if (ability4 != null)
		{
			ability4.SetupAbility(this);
		}
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
	}
	
	public override void CharacterDie()
	{
		//drop loot
		Vector3 dropPos = gameObject.transform.position;
		dropPos.y = 0.5f;
		GameObject lootChest = Instantiate(lootChestPrefab, dropPos, gameObject.transform.rotation);

		if (Region)
		{
			Region.RemoveEnemy(this);
		}

		//destroys character model
		Destroy(gameObject);
	}

	// referenced from the animation.
	public void TurnHitBoxOn()
	{
		//WeaponHitBox.enabled = true;
	}

	// referenced from the animation.
	public void HitCheck()
	{
		//DoneCasting();
		//agent.isStopped = false;
		//disable hitbox
		//WeaponHitBox.enabled = false;
	}
}
