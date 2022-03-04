using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    public bool Detachable = false;

    public PlayerBrain playerBrain;

    //hitbox
    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Weapon")
        {
            if (other.GetComponentInParent<Ability>().owner == playerBrain)
            {
                return;
            }
            if (other.GetComponentInParent<PlayerBrain>())
            {
                return;
            }
            playerBrain.TakeDamage(other.GetComponentInParent<Ability>().damage);
        }
    }

}
