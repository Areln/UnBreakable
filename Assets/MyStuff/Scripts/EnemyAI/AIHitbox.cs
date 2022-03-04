using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHitbox : MonoBehaviour
{

    public bool Detachable = false;

    public BasicAI aiBrain;

    //hitbox
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon")
        {
            if (other.GetComponentInParent<BasicAI>())
            {
                return;
            }

            aiBrain.TakeDamage(other.GetComponentInParent<Ability>().damage);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
