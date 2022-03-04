using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public bool Detachable = false;

    internal CharacterBrain brain;

	private void Start()
	{
        brain = GetComponent<CharacterBrain>();
        if(brain == null)
		{
            brain = GetComponentInParent<CharacterBrain>();
        }
	}

	//hitbox
	public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DamageDealer")
        {
            var otherBrain = other.GetComponentInParent<CharacterBrain>();
            if (otherBrain == null)
            {
                var ability = other.GetComponent<Ability>();
                if (ability == null)
                {
                    ability = other.GetComponentInParent<Ability>();
                }
                otherBrain = ability.owner;
            }
            if (otherBrain == brain || (otherBrain is BasicAI && brain is BasicAI))
            {
                return;
            }
            var damagers = other.GetComponentsInParent<DamageDealer>().ToList();
            if (damagers == null)
            {
                damagers = other.GetComponentsInParent<DamageDealer>().ToList();
            }
            int damage = 0;
            foreach (var damager in damagers)
            {
                damage += damager.CalculateDamageDealt(otherBrain);
            }
            brain.TakeDamage(damage);
        }
    }



}
