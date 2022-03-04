using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AbilitySmash : Ability
{
    public NavMeshAgent agent;
    public BoxCollider wepHitBox;
    public LayerMask hitMask;
    public GameObject weaponPrefab;
    public Animator animator;

    public void Update()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
    }
    public override void Activate()
    {
        if (currentCooldown > 0)
        {
            return;
        }
        currentCooldown = maxCooldown;
        CastSetup();
        animator.SetTrigger("Attack");

        if (owner.currentMana >= manaCost)
        {
            owner.currentMana -= manaCost;
        }
    }

    public void CastSetup()
    {
        if (GetComponentInParent<BasicAI>())
        {
            Vector3 targetPostition = new Vector3(agent.destination.x, this.transform.position.y, agent.destination.z);
            character.gameObject.transform.LookAt(targetPostition);
        }
        else
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, hitMask))
            {
                Vector3 targetPostition = new Vector3(hit.point.x, this.transform.position.y, hit.point.z);
                character.gameObject.transform.LookAt(targetPostition);
            }
        }

        agent.isStopped = true;
    }

    public void DoneCasting()
    {
        agent.isStopped = false;
    }
    public void TurnHitBoxOn()
    {
        wepHitBox.enabled = true;
    }
    public void HitCheck()
    {
        DoneCasting();

        //disable hitbox
        wepHitBox.enabled = false;
    }


    public override void SetupAbility(CharacterBrain _owner, NavMeshAgent _agent)
    {
        agent = _agent;
        owner = _owner;
        character = owner.gameObject;
    }

    public override void RemoveAbility()
    {
        throw new System.NotImplementedException();
    }
}
