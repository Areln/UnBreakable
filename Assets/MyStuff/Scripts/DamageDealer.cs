using UnityEngine;

public abstract class DamageDealer : MonoBehaviour
{
    public int Damage;

    public StatTypes StatModifier;

    // TODO: Create better formula for adding damage to abilities based on stats;
    internal int CalculateDamageDealt(CharacterBrain brain)
    {
        int damage = Damage;
        switch (StatModifier)
        {
            case StatTypes.Strength:
                damage += brain.Stats.Strength;
                break;
            case StatTypes.Dexterity:
                damage += brain.Stats.Dexterity;
                break;
            case StatTypes.Intelligence:
                damage += brain.Stats.Intelligence;
                break;
            case StatTypes.Vitality:
                damage += brain.Stats.Vitality;
                break;
            default:
                break;
        }
        return damage;
    }
}
