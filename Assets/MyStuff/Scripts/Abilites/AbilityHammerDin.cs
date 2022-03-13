using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AbilityHammerDin : Ability
{
    internal Animator animator;
    public GameObject hammerDinPrefab;
    public float CastTime;
    private bool finishedCasting;

    public override void Activate(Vector3 targetPosition)
    {
        //checks if ability is on cooldown or if the player is casting an ability already.
        if (currentCooldown > 0 || owner.CurrentlyCastingAbility != null)
        {
            return;
        }

        //uses mana
        if (owner.currentMana >= manaCost)
        {
            owner.currentMana -= manaCost;
            currentCooldown = maxCooldown;
        }
        else
        {
            return;
        }

        // Pause moving

        // Player hammerdin cast anim
        //animator.SetTrigger("HDinCast");
        StartCoroutine(CastSpell(CastTime));

        //instantiate din

        //resume moving
    }
    private IEnumerator CastSpell(float sec)
    {
        IsCanceled = false;
        finishedCasting = false;
        animator.SetBool("Casting", true);
        owner.agent.isStopped = true;
        owner.CurrentlyCastingAbility = this;
        yield return new WaitForSeconds(sec);
        if (!IsCanceled)
        {
            owner.CurrentlyCastingAbility = null;
            finishedCasting = true;
            animator.SetBool("Casting", false);
            HammerDinSpin tempSpin = Instantiate(hammerDinPrefab, owner.transform.position, Quaternion.identity).GetComponentInChildren<HammerDinSpin>();
            tempSpin.SetupAbility(GetComponentInParent<CharacterBrain>());
            owner.agent.isStopped = false;
        }
        else
        {
            IsCanceled = false;
        }
    }
    public override void RemoveAbility()
    {
        throw new System.NotImplementedException();
    }

    public override void SetupAbility(CharacterBrain _owner)
    {
        owner = _owner;
    }

    public void Awake()
    {
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

}
