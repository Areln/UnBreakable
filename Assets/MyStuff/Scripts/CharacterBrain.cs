using System.Collections.Generic;
using System.Linq;
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
    internal bool updateMove;

    internal Ability CurrentlyCastingAbility { get; set; }

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

    public void SetCharacterPath(Vector3[] pathCorners)
    {
        Debug.Log("move message recieved" + pathCorners.Length);
        updateMove = true;
        var path = new NavMeshPath();
		agent.CalculatePath(pathCorners.Last(), path);
        for (var i = 0; i < path.corners.Length; i++)
        {
            var point = path.corners[i];
            point.x = pathCorners[i].x;
            point.y = pathCorners[i].y;
            point.z = pathCorners[i].z;
        }
        agent.SetPath(path);
	}
}
