using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    public bool Detachable = false;

    public PlayerBrain playerBrain;

    //hitbox
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DamageDealer")
        {
            var enemy = other.GetComponentInParent<CharacterBrain>();
            if (enemy == null)
            {
                var ability = other.GetComponent<Ability>();
                if (ability == null)
                {
                    ability = other.GetComponentInParent<Ability>();
                }
                enemy = ability.owner;
            }
            if (enemy == playerBrain)
            {
                return;
            }
            if (other.GetComponentInParent<PlayerBrain>())
            {
                return;
            }

            var damager = other.GetComponent<DamageDealer>();
            if (damager == null)
            {
                damager = other.GetComponentInParent<DamageDealer>();
            }
            
            int damage = damager.CalculateDamageDealt(enemy);
            playerBrain.TakeDamage(damage);
        }
    }



}
