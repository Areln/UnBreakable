using UnityEngine;

namespace Server
{
    public abstract class ServerAbility : HealthChanger
    {
        internal ServerCharacterBrain owner;
        public string abilityName;
        public string PrefabName;
        public int manaCost;
        public float maxCooldown;
        public float currentCooldown;

        internal bool IsCanceled { get; set; }

        public abstract bool Activate(Vector3 targetPosition);

        public abstract void SetupAbility(ServerCharacterBrain _owner);

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

        private void OnTriggerEnter(Collider other)
        {
            var characterBrainTakingDamage = other.GetComponent<ServerCharacterBrain>();
            if (characterBrainTakingDamage != null && characterBrainTakingDamage != owner)
            {
                var damage = CalculateDamageDealt(owner);
                characterBrainTakingDamage.UpdateHealth(damage);
            }
        }
    }
}