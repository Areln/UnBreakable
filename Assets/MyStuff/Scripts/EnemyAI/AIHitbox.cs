using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHitbox : MonoBehaviour
{

    public bool Detachable = false;

    public BasicAI aiBrain;

    //hitbox
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DamageDealer")
        {
            if (other.GetComponentInParent<BasicAI>())
            {
                return;
            }
            var damager = other.GetComponent<DamageDealer>();
            if(damager == null)
			{
                damager = other.GetComponentInParent<DamageDealer>();
            }
            var character = other.GetComponentInParent<CharacterBrain>();
            if(character == null)
			{
                var ability = other.GetComponent<Ability>();
                if(ability == null)
				{
                    ability = other.GetComponentInParent<Ability>();
                }                    
                character = ability.owner;
            }
            int damage = damager.CalculateDamageDealt(character);
            aiBrain.TakeDamage(damage);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
