using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AbilityWarpBlast : Ability
{
    internal Collider wepHitBox;
    public GameObject particlePrefab;
    Vector3 portLocation;
    bool portLoactionSet = false;
    public float maxCastTime;
    float currentCastTime;

    public void Start()
	{
        wepHitBox = GetComponentInChildren<Collider>();
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
        //checks if ability is on cooldown
        if (currentCooldown > 0)
        {
            return;
        }

        owner.gameObject.transform.LookAt(targetPosition);
        portLocation = targetPosition;
        portLoactionSet = true;

        //checks to see if we can tele
        if (portLoactionSet == false)
        {
            return;
        }
        else
        {
            //uses mana
            if (owner.currentMana >= manaCost)
            {
                owner.currentMana -= manaCost;
                //cast setup successfull so set ability CD
                currentCooldown = maxCooldown;
            }
            else
            {
                return;
            }
        }

        //teleport player
        owner.agent.gameObject.transform.position = portLocation;
        owner.agent.GetComponent<PlayerMovement>().StopPlayerFromMoving();

		//instantiates particle object
		Instantiate(particlePrefab, transform.position, transform.rotation);

        //hitbox
        HitCheck();

        //resets port locations
        portLoactionSet = false;
    }

    private IEnumerator waitForSec(float sec)
    {
        wepHitBox.enabled = true;
        yield return new WaitForSeconds(sec);
        wepHitBox.enabled = false;
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
