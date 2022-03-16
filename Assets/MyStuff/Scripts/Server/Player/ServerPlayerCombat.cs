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
            ability1.SetupAbility(player);
            ability4.SetupAbility(player);
            ability3.SetupAbility(player);
            ability2.SetupAbility(player);
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
