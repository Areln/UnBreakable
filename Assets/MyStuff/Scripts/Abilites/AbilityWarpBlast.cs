using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AbilityWarpBlast : Ability
{
    public NavMeshAgent agent;
    public SphereCollider wepHitBox;
    public LayerMask hitMask;
    public GameObject particlePrefab;
    Vector3 portLocation;
    bool portLoactionSet = false;
    public float maxCastTime;
    float currentCastTime;

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

        character.gameObject.transform.LookAt(targetPosition);
        portLocation = targetPosition;
        portLoactionSet = true;
        //CastSetup();

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
        agent.gameObject.transform.position = portLocation;
        agent.GetComponent<PlayerMovement>().StopPlayerFromMoving();
		//if (!agent.GetComponent<PlayerBrain>().isMoving)
		//{
		//	agent.destination = portLocation;
		//}

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
    //public void CastSetup()
    //{
    //    if (GetComponentInParent<BasicAI>())
    //    {
    //        Vector3 targetPostition = new Vector3(agent.destination.x, this.transform.position.y, agent.destination.z);
    //        character.gameObject.transform.LookAt(targetPostition);
    //    }
    //    else
    //    {
    //        RaycastHit hit;

    //        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, hitMask))
    //        {
    //            Vector3 targetPostition = new Vector3(hit.point.x, this.transform.position.y, hit.point.z);

    //            //set port location
    //            portLoactionSet = true;
    //            portLocation = hit.point;
    //        }
    //    }
        
    //}

    public void HitCheck()
    {
        StartCoroutine(waitForSec(.5f));
    }

    public override void RemoveAbility()
    {
        throw new System.NotImplementedException();
    }

    public override void SetupAbility(CharacterBrain _owner, NavMeshAgent _agent)
    {
        agent = _agent;
        owner = _owner;
        character = owner.gameObject;
    }

}
