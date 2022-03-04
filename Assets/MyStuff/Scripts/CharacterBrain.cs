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
}
