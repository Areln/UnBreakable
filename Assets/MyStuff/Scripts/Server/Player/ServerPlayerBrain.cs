using System;
using UnityEngine;
using UnityEngine.AI;

namespace Server
{
    public class ServerPlayerBrain : ServerCharacterBrain
    {
        internal HPScript hpScript;
        internal ServerPlayerInventory playerInventory;

        public float manaRegenTime;
        internal float manaRegenCurrentTime;
        public int manaRegenAmount;

        internal bool isMoving = false;

		internal bool CancelAll()
        {
            bool isAbilityCanceled = false;
            if (CurrentlyCastingAbility)
            {
                CurrentlyCastingAbility.IsCanceled = true;
                CurrentlyCastingAbility = null;
                isAbilityCanceled = true;
            }
            StopCharacterFromMoving();

            return isAbilityCanceled;
        }

		public int interactableRange = 3;


        // Start is called before the first frame update
        void Awake()
        {
            currentHealth = maxHealth;
            currentMana = maxMana;
            agent = GetComponent<NavMeshAgent>();
            Stats = GetComponent<Stats>();
            hpScript = GetComponent<HPScript>();
            playerInventory = GetComponent<ServerPlayerInventory>();
        }

		// Update is called once per frame
		void Update()
        {
            //Mana Regen
            if (currentMana < maxMana)
            {
                if (manaRegenCurrentTime > 0)
                {
                    manaRegenCurrentTime -= Time.deltaTime;
                }
                else
                {
                    currentMana += manaRegenAmount;
                    manaRegenCurrentTime = manaRegenTime;
                    if (currentMana > maxMana)
                    {
                        currentMana = maxMana;
                    }
                }
            }

            //Animations
            //if (playerMovement.agent.velocity != Vector3.zero)
            //{
            //    isMoving = true;
            //    playerCombat.animator.SetBool("IsWalking", isMoving);
            //}
            //else
            //{
            //    isMoving = false;
            //    playerCombat.animator.SetBool("IsWalking", isMoving);
            //}
        }

        internal void InitializeData(string username)
        {
            characterName = username;
            LoadAbilities();

            //foreach (GameObject item in ServerGameManager.Instance.PossibleItems)
            //{
            //    playerInventory.AddPrefabItemObjectToPlayerInventory(item);
            //}
        }

        public override void CharacterDie()
        {
            Debug.Log("Die");
        }
	}
}