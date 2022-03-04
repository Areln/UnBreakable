using UnityEngine;

public class SwingWeapon : Ability
{
    internal Animator animator;

    public void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        if (animator == null)
        {
            animator = gameObject.GetComponentInParent<Animator>();
        }
	}

    public override void Activate(Vector3 targetPosition)
    {
        if (currentCooldown > 0)
        {
            return;
        }
        currentCooldown = maxCooldown;
        owner.gameObject.transform.LookAt(targetPosition);
        owner.agent.isStopped = true;
        animator.SetTrigger("Attack");

        if (owner.currentMana >= manaCost)
        {
            owner.currentMana -= manaCost;
        }
    }

    public void DoneCasting()
    {
        owner.agent.isStopped = false;
    }

    public override void SetupAbility(CharacterBrain _owner)
    {
        owner = _owner;
    }

    public override void RemoveAbility()
    {
        throw new System.NotImplementedException();
    }
}
