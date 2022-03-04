using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCombat : MonoBehaviour
{

    public Animator animator;
    public PlayerBrain playerBrain;
    public NavMeshAgent agent;
    public Collider WeaponHitBox;

    //Abilites
    public Ability ability1;
    public Ability ability2;
    public Ability ability3;
    public Ability ability4;

    public void Start()
    {
        ability1.SetupAbility(playerBrain, agent);
        ability4.SetupAbility(playerBrain, agent);
        ability3.SetupAbility(playerBrain, agent);
        ability2.SetupAbility(playerBrain, agent);
    }


    // referenced from the animation.
    public void TurnHitBoxOn()
    {
        WeaponHitBox.enabled = true;
    }

    // referenced from the animation.
    public void HitCheck()
    {
        //DoneCasting();
        agent.isStopped = false;
        //disable hitbox
        WeaponHitBox.enabled = false;
    }
}
