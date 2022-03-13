using UnityEngine;
using UnityEngine.AI;

public class PlayerCombat : MonoBehaviour
{

    public Animator animator;
    public Collider WeaponHitBox;

    //Abilites
    public Ability ability1;
    public Ability ability2;
    public Ability ability3;
    public Ability ability4;

    public void Start()
    {
        ability1.SetupAbility(GameManager.Instance.clientPlayer);
        ability4.SetupAbility(GameManager.Instance.clientPlayer);
        ability3.SetupAbility(GameManager.Instance.clientPlayer);
        ability2.SetupAbility(GameManager.Instance.clientPlayer);
    }

    public void SetWeaponHitBox(Collider hitbox) 
    {
        WeaponHitBox = hitbox;
    }

    // referenced from the animation.
    public void TurnHitBoxOn()
    {
        if (WeaponHitBox == null)
        {
            Debug.Log("No weapon hitbox");
        }
        WeaponHitBox.enabled = true;
    }

    // referenced from the animation.
    public void HitCheck()
    {
        //DoneCasting();
        GameManager.Instance.clientPlayer.agent.isStopped = false;
        //disable hitbox
        WeaponHitBox.enabled = false;
    }
}
