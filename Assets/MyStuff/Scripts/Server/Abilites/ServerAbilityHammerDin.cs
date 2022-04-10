using Server.Networking;
using System.Collections;
using UnityEngine;

namespace Server
{
    public class ServerAbilityHammerDin : ServerAbility
    {
        public GameObject hammerDinPrefab;
        public float CastTime;
        private bool finishedCasting;

        public override bool Activate(Vector3 targetPosition)
        {
            //checks if ability is on cooldown or if the player is casting an ability already.
            if (currentCooldown > 0 || owner.CurrentlyCastingAbility != null)
            {
                return false;
            }

            //uses mana
            if (owner.currentMana >= manaCost)
            {
                owner.UpdateMana(-manaCost);
                currentCooldown = maxCooldown;
            }
            else
            {
                return false;
            }

            // Pause moving

            // Player hammerdin cast anim
            //animator.SetTrigger("HDinCast");
            StartCoroutine(CastSpell(CastTime));

            //instantiate din

            //resume moving
            return true;
        }
        private IEnumerator CastSpell(float sec)
        {
            IsCanceled = false;
            finishedCasting = false;
            owner.agent.isStopped = true;
            owner.CurrentlyCastingAbility = this;
            yield return new WaitForSeconds(sec);
            if (!IsCanceled)
            {
                owner.CurrentlyCastingAbility = null;
                finishedCasting = true;
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

        public override void SetupAbility(ServerCharacterBrain _owner)
        {
            owner = _owner;
        }

        public void Update()
        {
            if (IsCanceled && !finishedCasting)
            {
                owner.agent.isStopped = false;
                owner.CurrentlyCastingAbility = null;
            }
            if (currentCooldown > 0)
            {
                currentCooldown -= Time.deltaTime;
            }
        }
    }
}
