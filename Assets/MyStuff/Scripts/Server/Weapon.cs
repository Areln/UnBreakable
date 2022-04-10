using System.Collections.Generic;
using UnityEngine;

namespace Server
{
	public class Weapon : HealthChanger
	{
		public List<GameObject> PossibleAbilities;
		public Collider WeaponHitBox;



        private void OnTriggerEnter(Collider other)
        {
            var characterBrainTakingDamage = other.GetComponent<ServerCharacterBrain>();
            var weaponOwner = GetComponentInParent<ServerCharacterBrain>();
            if (characterBrainTakingDamage != null && characterBrainTakingDamage != weaponOwner)
            {
                var damage = CalculateDamageDealt(weaponOwner);
                characterBrainTakingDamage.UpdateHealth(damage);
            }
        }
    }
}