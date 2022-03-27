using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicAI : CharacterBrain
{

	public enum AIState { StandStill, Patrol, Attack }

	public AIState currentAIState;

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
		foreach (var ability in abilities)
		{
			if (ability != null)
			{
				ability.SetupAbility(this);
			}
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
		StorageObject woChest = lootChest.GetComponent<StorageObject>();
		//woChest.SetContents(AIInventory);

		//destroys character model
		Destroy(gameObject);
	}

	// referenced from the animation.
	public void TurnHitBoxOn()
	{
	}

	// referenced from the animation.
	public void HitCheck()
	{
	}
}
