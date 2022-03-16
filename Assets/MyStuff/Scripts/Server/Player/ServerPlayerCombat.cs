using UnityEngine;

namespace Server
{
    public class ServerPlayerCombat : MonoBehaviour
    {

        public Animator animator;
        internal Collider WeaponHitBox;

        //Abilites
        public ServerAbility ability1;
        public ServerAbility ability2;
        public ServerAbility ability3;
        public ServerAbility ability4;

        public void Start()
        {
            var player = GetComponent<ServerPlayerBrain>();
            if (ability1 != null)
            {
                ability1.SetupAbility(player);
            }
            if (ability2 != null)
            {
                ability2.SetupAbility(player);
            }
            if (ability3 != null)
            {
                ability3.SetupAbility(player);
            }
            if (ability4 != null)
            {
                ability4.SetupAbility(player);
            }
        }

        public void SetWeaponHitBox(Collider hitbox)
        {
            WeaponHitBox = hitbox;
        }

        // referenced from the animation.
        public void TurnHitBoxOn()
        {
            if (WeaponHitBox == null)
            {
                Debug.Log("No weapon hitbox");
            }
            WeaponHitBox.enabled = true;
        }

        // referenced from the animation.
        public void HitCheck()
        {
            //DoneCasting();
            GetComponent<ServerPlayerBrain>().agent.isStopped = false;
            //disable hitbox
            WeaponHitBox.enabled = false;
        }
    }
}
