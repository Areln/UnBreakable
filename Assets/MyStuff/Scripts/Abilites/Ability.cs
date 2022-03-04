using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Ability : MonoBehaviour
{

    public enum AbilityType { WeaponAbility, WAbility };

    public CharacterBrain owner;
    public GameObject character;
    public AbilityType abilityType;
    public string abilityName;
    public int manaCost;
    public int damage;
    public float maxCooldown;
    public float currentCooldown;
    public abstract void Activate(Vector3 targetPosition);

    public abstract void SetupAbility(CharacterBrain _owner, NavMeshAgent _agent);

    public abstract void RemoveAbility();

    public virtual float GetCDPercentage() 
    {
        if (currentCooldown > 0)
        {
            return currentCooldown / maxCooldown;
        }
        else
        {
            return 0;
        }
    }
}
