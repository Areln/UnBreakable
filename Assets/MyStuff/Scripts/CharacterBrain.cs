using UnityEngine;
using UnityEngine.AI;

public abstract class CharacterBrain : MonoBehaviour
{
    public string characterName;
    public int maxHealth;
    public int currentHealth;
    public int maxMana;
    public int currentMana;

    internal NavMeshAgent agent;
    internal Stats Stats;

    public abstract void CharacterDie();

    public void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        GetComponent<HPScript>().ChangeHP(-_damage, gameObject.transform.position);

        if (currentHealth <= 0)
        {
            CharacterDie();
        }
    }

    public void StopCharacterFromMoving()
    {
        agent.destination = gameObject.transform.position;
    }
}
