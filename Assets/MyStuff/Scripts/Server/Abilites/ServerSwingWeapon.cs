﻿using UnityEngine;

namespace Server
{
    public class ServerSwingWeapon : ServerAbility
    {
        internal Animator animator;

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

        public override bool Activate(Vector3 targetPosition)
        {
            //checks if ability is on cooldown or if the player is casting an ability already.
            if (currentCooldown > 0 || owner.CurrentlyCastingAbility != null)
            {
                return false;
            }

            if (owner.currentMana >= manaCost)
            {
                currentCooldown = maxCooldown;
                owner.UpdateMana(-manaCost);
                owner.gameObject.transform.LookAt(targetPosition);
                owner.agent.isStopped = true;
                animator.SetTrigger("Attack");
            }
			else
			{
                return false;
			}

            return true;
        }

        public void DoneCasting()
        {
            owner.agent.isStopped = false;
        }

        public override void SetupAbility(ServerCharacterBrain _owner)
        {
            owner = _owner;
        }

        public override void RemoveAbility()
        {
            throw new System.NotImplementedException();
        }
    }
}
