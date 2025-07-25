﻿using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    internal CharacterBrain owner;
    public string abilityName;
    public int manaCost;
    public float maxCooldown;
    public float currentCooldown;

    internal bool IsCanceled { get; set; }

    public abstract void Activate(Vector3 startPosition, Vector3 targetPosition);

    public abstract void SetupAbility(CharacterBrain _owner);

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
