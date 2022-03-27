using UnityEngine;

namespace Server
{
    public abstract class HealthChanger : MonoBehaviour
    {
        public int HealthChange;

        public StatTypes StatModifier;

        // TODO: Create better formula for adding damage to abilities based on stats;
        internal int CalculateDamageDealt(ServerCharacterBrain brain)
        {
            int healthChangeMultiplier = HealthChange >= 0 ? 1 : -1;

            int healthChange = HealthChange;
            switch (StatModifier)
            {
                case StatTypes.Strength:
                    healthChange += brain.Stats.Strength * healthChangeMultiplier;
                    break;
                case StatTypes.Dexterity:
                    healthChange += brain.Stats.Dexterity * healthChangeMultiplier;
                    break;
                case StatTypes.Intelligence:
                    healthChange += brain.Stats.Intelligence * healthChangeMultiplier;
                    break;
                case StatTypes.Vitality:
                    healthChange += brain.Stats.Vitality * healthChangeMultiplier;
                    break;
                default:
                    break;
            }
            return healthChange;
        }
    }
}
