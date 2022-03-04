using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicAI : CharacterBrain
{

    public enum AIState { StandStill, Patrol, Attack }

    public AIState currentAIState;

    public MobRegion Region { get {  return _region; } set { _region = value; } }
    private MobRegion _region;

    public NavMeshAgent agent;

    public GameObject MainTarget;

    public AIRangeDetection rangeDetection;

    public List<GameObject> TargetsInRange = new List<GameObject>();

    public List<Transform> PatrolTargets = new List<Transform>();
    public int currentPatrolTargetIndex;
    public bool isIdle;
    public float maxIdleTime;
    public float currentIdleTime;
    public Collider WeaponHitBox;

    public Animator animator;

    public HPScript hpScript;

    public GameObject lootChestPrefab;

    public Ability ability1;
    public Ability ability2;
    public Ability ability3;
    public Ability ability4;

    // Start is called before the first frame update
    public void Start()
    {
        currentHealth = maxHealth;
        ability1.SetupAbility(this, agent);
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
                if (Vector3.Distance(MainTarget.transform.position, gameObject.transform.position) <= 2)
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

                // gets first target in range
                if (TargetsInRange.Count == 1)
                {
                    MainTarget = TargetsInRange[0];
                }

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
    public void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        hpScript.ChangeHP(-_damage, gameObject.transform.position);

        if (currentHealth <= 0)
        {
            CharacterDie();
        }
    }

    void CharacterDie() 
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
            agent.destination = MainTarget.transform.position;
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
        if (ability1.currentCooldown <= 0)
        {
            //check range
            if (agent.remainingDistance <= 1.5)
            {
                Vector3 targetPostition = new Vector3(agent.destination.x, this.transform.position.y, agent.destination.z);
                ability1.Activate(targetPostition);
            }
        }

        //if (ability2.currentCooldown <= 0)
        //{
        //    //check range
        //    if (agent.remainingDistance <= 1)
        //    {

        //    }
        //}

        //if (ability3.currentCooldown <= 0)
        //{
        //    //check range
        //    if (agent.remainingDistance <= 1)
        //    {

        //    }
        //}

        //if (ability4.currentCooldown <= 0)
        //{
        //    //check range
        //    if (agent.remainingDistance <= 1)
        //    {

        //    }
        //}

    }
}
