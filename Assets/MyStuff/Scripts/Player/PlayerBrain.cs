using UnityEngine;
using UnityEngine.AI;

public class PlayerBrain : CharacterBrain
{
    internal PlayerLook playerLook;
    internal PlayerMovement playerMovement;
    internal HPScript hpScript;
    internal PlayerInventory playerInventory;

    public float manaRegenTime;
    internal float manaRegenCurrentTime;
    public int manaRegenAmount;

    //
    public int interactableRange = 3;


    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        agent = GetComponent<NavMeshAgent>();
        Stats = GetComponent<Stats>();
        hpScript = GetComponent<HPScript>();
        playerLook = GetComponent<PlayerLook>();
        playerMovement = GetComponent<PlayerMovement>();
        playerInventory = GetComponent<PlayerInventory>();
        animator = GetComponent<Animator>();
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
                if (currentMana > maxMana)
                {
                    currentMana = maxMana;
                }
            }
        }

        if (GetComponent<PlayerBrain>() == GameManager.Instance.ClientPlayer)
        {
            if (Input.GetMouseButtonDown(0) && !GameManager.Instance.UsingUI && !GameManager.Instance.DraggingObject)
            {
                RaycastHit hit;

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, LayerMask.GetMask("Ground")))
                {
                    new CharacterMoveHandle().WriteMessage(hit.point);
                }
            }
            ////////////////////////////////
            // Inputs
            ////////////////////////////////
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 100, LayerMask.GetMask("Ground")))
                {
                    Vector3 targetPostition = new Vector3(hit.point.x, this.transform.position.y, hit.point.z);
                    new AbilityHandle().SendAbilityCast(0, targetPostition);
                }
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 100, LayerMask.GetMask("Ground")))
                {
                    Vector3 targetPostition = new Vector3(hit.point.x, this.transform.position.y, hit.point.z);
                    new AbilityHandle().SendAbilityCast(1, targetPostition);
                }
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 100, LayerMask.GetMask("Ground")))
                {
                    Vector3 targetPostition = new Vector3(hit.point.x, this.transform.position.y, hit.point.z);
                    new AbilityHandle().SendAbilityCast(2, targetPostition);
                }
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 100, LayerMask.GetMask("Ground")))
                {
                    Vector3 targetPostition = new Vector3(hit.point.x, this.transform.position.y, hit.point.z);
                    new AbilityHandle().SendAbilityCast(3, targetPostition);
                }
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                // stop char movement
                if (CurrentlyCastingAbility)
                {
                    CurrentlyCastingAbility.IsCanceled = true;
                    CurrentlyCastingAbility = null;
                }
                playerMovement.StopPlayerFromMoving();
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                //toggle inv/char
                HudManager.Instance.ToggleInventory();
            }

            //Right click
            //gets object info
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
        }
    }

    internal void InitializeData(PlayerData playerData)
    {
        characterName = playerData.CharacterName;

        abilities = new Ability[playerData.Abilities.Length];
        for (int i = 0; i < playerData.Abilities.Length; i++)
        {
            var ability = Instantiate(Resources.Load("Abilities/" + playerData.Abilities[i]) as GameObject, AbilityHolder.transform).GetComponent<Ability>();
            ability.SetupAbility(this);
            abilities[i] = ability;
        }

        ItemSlot tempSlot;

        if (!string.IsNullOrWhiteSpace(playerData.EquippedHelmetPiece))
        {
            tempSlot = playerInventory.AddPrefabItemObjectToPlayerInventory(GameManager.Instance.GetItem(playerData.EquippedHelmetPiece));
            playerInventory.EquipItemToCharacter((ItemEquipable)tempSlot.SlottedItem, tempSlot);
        }
        if (!string.IsNullOrWhiteSpace(playerData.EquippedChestPiece))
        {
            tempSlot = playerInventory.AddPrefabItemObjectToPlayerInventory(GameManager.Instance.GetItem(playerData.EquippedChestPiece));
            playerInventory.EquipItemToCharacter((ItemEquipable)tempSlot.SlottedItem, tempSlot);
        }
        if (!string.IsNullOrWhiteSpace(playerData.EquippedGlovePiece))
        {
            tempSlot = playerInventory.AddPrefabItemObjectToPlayerInventory(GameManager.Instance.GetItem(playerData.EquippedGlovePiece));
            playerInventory.EquipItemToCharacter((ItemEquipable)tempSlot.SlottedItem, tempSlot);
        }
        if (!string.IsNullOrWhiteSpace(playerData.EquippedLegPiece))
        {
            tempSlot = playerInventory.AddPrefabItemObjectToPlayerInventory(GameManager.Instance.GetItem(playerData.EquippedLegPiece));
            playerInventory.EquipItemToCharacter((ItemEquipable)tempSlot.SlottedItem, tempSlot);
        }
        if (!string.IsNullOrWhiteSpace(playerData.EquippedNecklacePiece))
        {
            tempSlot = playerInventory.AddPrefabItemObjectToPlayerInventory(GameManager.Instance.GetItem(playerData.EquippedNecklacePiece));
            playerInventory.EquipItemToCharacter((ItemEquipable)tempSlot.SlottedItem, tempSlot);
        }
        if (!string.IsNullOrWhiteSpace(playerData.EquippedRing1Piece))
        {
            tempSlot = playerInventory.AddPrefabItemObjectToPlayerInventory(GameManager.Instance.GetItem(playerData.EquippedRing1Piece));
            playerInventory.EquipItemToCharacter((ItemEquipable)tempSlot.SlottedItem, tempSlot);
        }
        if (!string.IsNullOrWhiteSpace(playerData.EquippedRing2Piece))
        {
            tempSlot = playerInventory.AddPrefabItemObjectToPlayerInventory(GameManager.Instance.GetItem(playerData.EquippedRing2Piece));
            playerInventory.EquipItemToCharacter((ItemEquipable)tempSlot.SlottedItem, tempSlot);
        }
        if (!string.IsNullOrWhiteSpace(playerData.EquippedMainHandWeapon))
        {
            tempSlot = playerInventory.AddPrefabItemObjectToPlayerInventory(GameManager.Instance.GetItem(playerData.EquippedMainHandWeapon));
            playerInventory.EquipItemToCharacter((ItemEquipable)tempSlot.SlottedItem, tempSlot);
        }
        if (!string.IsNullOrWhiteSpace(playerData.EquippedOffHandWeapon))
        {
            tempSlot = playerInventory.AddPrefabItemObjectToPlayerInventory(GameManager.Instance.GetItem(playerData.EquippedOffHandWeapon));
            playerInventory.EquipItemToCharacter((ItemEquipable)tempSlot.SlottedItem, tempSlot);
        }

        foreach (var item in playerData.Items)
        {
            new CharacterSpawnItem().WriteMessage(item);
            //playerInventory.AddPrefabItemObjectToPlayerInventory(GameManager.Instance.GetItem(item));
        }
    }

    public override void CharacterDie()
    {
        Debug.Log("Die");
    }
}
