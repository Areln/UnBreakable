using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AbilityHammerDin : Ability
{
    public GameObject hammerDinPrefab;

    public override void Activate(Vector3 targetPosition)
    {
        if (currentCooldown > 0)
        {
            return;
        }
        currentCooldown = maxCooldown;

        //uses mana
        if (owner.currentMana >= manaCost)
        {
            owner.currentMana -= manaCost;
        }
        else
        {
            return;
        }

        HammerDinSpin tempSpin = Instantiate(hammerDinPrefab, transform.position, transform.rotation).GetComponentInChildren<HammerDinSpin>();
        tempSpin.SetupAbility(GetComponentInParent<CharacterBrain>());
    }

    public override void RemoveAbility()
    {
        throw new System.NotImplementedException();
    }

    public override void SetupAbility(CharacterBrain _owner)
    {
        owner = _owner;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

}
