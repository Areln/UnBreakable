using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerBrain : CharacterBrain
{
    internal PlayerCombat playerCombat;
    internal PlayerLook playerLook;
    internal PlayerMovement playerMovement;
    //internal PlayerInventory playerInventory;
    internal HPScript hpScript;

    public float manaRegenTime;
    internal float manaRegenCurrentTime;
    public int manaRegenAmount;

    internal bool isMoving = false;

    //
    public int interactableRange = 3;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        agent = GetComponent<NavMeshAgent>();
        Stats = GetComponent<Stats>();
        hpScript = GetComponent<HPScript>();
        playerCombat = GetComponent<PlayerCombat>();
        playerLook = GetComponent<PlayerLook>();
        playerMovement = GetComponent<PlayerMovement>();
        //playerInventory = GetComponent<PlayerInventory>();
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
                if(currentMana > maxMana)
				{
                    currentMana = maxMana;
				}
            }
        }

        //Inputs
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 100, LayerMask.GetMask("Ground")))
            {
                Vector3 targetPostition = new Vector3(hit.point.x, this.transform.position.y, hit.point.z);
                playerCombat.ability1.Activate(targetPostition);
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 100, LayerMask.GetMask("Ground")))
            {
                Vector3 targetPostition = new Vector3(hit.point.x, this.transform.position.y, hit.point.z);
                playerCombat.ability2.Activate(targetPostition);
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 100, LayerMask.GetMask("Ground")))
            {
                Vector3 targetPostition = new Vector3(hit.point.x, this.transform.position.y, hit.point.z);
                playerCombat.ability3.Activate(targetPostition);
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 100, LayerMask.GetMask("Ground")))
            {
                Vector3 targetPostition = new Vector3(hit.point.x, this.transform.position.y, hit.point.z);
                playerCombat.ability4.Activate(targetPostition);
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            // stop char movement
            CurrentlyCastingAbility.IsCanceled = true;
            CurrentlyCastingAbility = null;
            playerMovement.StopPlayerFromMoving();
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

	public override void CharacterDie()
    {
        Debug.Log("Die");
    }
}
