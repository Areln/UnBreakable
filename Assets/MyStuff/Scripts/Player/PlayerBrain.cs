using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBrain : CharacterBrain
{
    public PlayerCombat playerCombat;
    public PlayerLook playerLook;
    public PlayerMovement playerMovement;

    public HPScript hpScript;

    public HudManager hudManager;

    public float manaRegenTime;
    public float manaRegenCurrentTime;
    public int manaRegenAmount;

    public bool isMoving = false;

    //
    public int interactableRange = 3;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        playerCombat = GetComponent<PlayerCombat>();
        playerLook = GetComponent<PlayerLook>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //Mana Regen
        if (currentMana < maxMana)
        {
            if (manaRegenCurrentTime > 0)
            {
                manaRegenCurrentTime -= Time.deltaTime;
            }
            else
            {
                currentMana += manaRegenAmount;
                manaRegenCurrentTime = manaRegenTime;
            }
        }

        //Inputs
        if (Input.GetKeyDown(KeyCode.Q))
        {
            playerCombat.ability1.Activate();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            playerCombat.ability2.Activate();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            playerCombat.ability3.Activate();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            playerCombat.ability4.Activate();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            //toggle inv/char
            HudManager.Instance.ToggleInventory();
        }

        //Right click
        //sets destination for navmesh and creates marker
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                Debug.Log("Right Click " + hit.collider.tag);
                if (hit.collider.tag == "WorldObject")
                {
                    WorldObject temp = hit.collider.GetComponentInParent<WorldObject>();

                    if (Vector3.Distance(gameObject.transform.position, temp.transform.position) <= interactableRange)
                    {
                        temp.Activate(this);
                    }
                }
            }
        }

        //Animations
        if (playerMovement.agent.velocity != Vector3.zero)
        {
            isMoving = true;
            playerCombat.animator.SetBool("IsWalking", isMoving);
        }
        else
        {
            isMoving = false;
            playerCombat.animator.SetBool("IsWalking", isMoving);
        }
    }
    public void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        hpScript.ChangeHP(-_damage, gameObject.transform.position);

        if (currentHealth <= 0)
        {
            CharacterDie();
        }
    }

    void CharacterDie()
    {
        Debug.Log("Die");
    }
}
