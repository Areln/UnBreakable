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

	Transform ActiveWordObjectTransform;

	//
	public int interactableRange = 3;


	// Start is called before the first frame update
	void Awake()
	{
		currentHealth = maxHealth;
		currentMana = maxMana;
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
		if (GetComponent<PlayerBrain>() == GameManager.Instance.ClientPlayer)
		{
			if (Input.GetMouseButtonDown(0) && !GameManager.Instance.UsingUI && !GameManager.Instance.GetDraggingObject())
			{
				var mask = (1 << LayerMask.NameToLayer("Ground"));
				if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, mask))
				{
					Debug.Log(transform.position.y);
					new CharacterMoveHandle().WriteMessage(new Vector3(hit.point.x, transform.position.y, hit.point.z));
				}
			}
			////////////////////////////////
			// Inputs
			////////////////////////////////
			if (Input.GetKeyDown(KeyCode.Q))
			{
				if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, LayerMask.GetMask("Ground")))
				{
					Vector3 targetPostition = new Vector3(hit.point.x, this.transform.position.y, hit.point.z);
					new AbilityHandle().SendAbilityCast(0, targetPostition);
				}
			}
			if (Input.GetKeyDown(KeyCode.W))
			{
				if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, LayerMask.GetMask("Ground")))
				{
					Vector3 targetPostition = new Vector3(hit.point.x, this.transform.position.y, hit.point.z);
					new AbilityHandle().SendAbilityCast(1, targetPostition);
				}
			}
			if (Input.GetKeyDown(KeyCode.E))
			{
				if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, LayerMask.GetMask("Ground")))
				{
					Vector3 targetPostition = new Vector3(hit.point.x, this.transform.position.y, hit.point.z);
					new AbilityHandle().SendAbilityCast(2, targetPostition);
				}
			}
			if (Input.GetKeyDown(KeyCode.R))
			{
				if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, LayerMask.GetMask("Ground")))
				{
					Vector3 targetPostition = new Vector3(hit.point.x, this.transform.position.y, hit.point.z);
					new AbilityHandle().SendAbilityCast(3, targetPostition);
				}
			}
			if (Input.GetKeyDown(KeyCode.S))
			{
				new CancelActionHandle().WriteMessage();
			}
			if (Input.GetKeyDown(KeyCode.B))
			{
				//toggle inv/char
				HudManager.Instance.ToggleInventory();
				HudManager.Instance.DisableContainerDisplay();
			}

			//Right click
			//gets object info
			if (Input.GetMouseButtonDown(1))
			{
				var mask = ( 1 << LayerMask.NameToLayer("WorldObject"));
				if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, mask))
				{
					WorldObject temp = hit.collider.GetComponentInParent<WorldObject>();
					if (temp != null && Vector3.Distance(gameObject.transform.position, temp.transform.position) <= interactableRange)
					{
						Debug.Log("Right Click " + temp.objectName);
						SetActiveWorldObject(temp.transform);
						temp.Activate(this);
					}
				}
			}
		}
	}

	public void CancelAll(bool isAbilityCanceled)
	{
		// stop char ability
		if (CurrentlyCastingAbility && isAbilityCanceled)
		{
			CurrentlyCastingAbility.IsCanceled = true;
			CurrentlyCastingAbility = null;
		}
		// stop char movement
		playerMovement.DestroyDestinationMarker();
		StopCharacterFromMoving();
	}

	public void SetActiveWorldObject(Transform transform) 
	{
		ActiveWordObjectTransform = transform;
	}
	public Transform GetActiveWorldObjectTransform()
	{
		return ActiveWordObjectTransform;
	}

	internal void InitializeData(PlayerData playerData)
	{
		characterName = playerData.CharacterName;

		abilities = new Ability[playerData.Abilities.Length];
		for (int i = 0; i < playerData.Abilities.Length; i++)
		{
			Ability ability = Instantiate(Resources.Load("Abilities/" + playerData.Abilities[i]) as GameObject, AbilityHolder.transform).GetComponent<Ability>();
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
			if (item.Value.GetItemName() != string.Empty)
			{
				playerInventory.AddPrefabItemObjectToPlayerInventory(item.Key, GameManager.Instance.GetItem(item.Value.GetItemName()), item.Value.GetAmount());
			}
		}
	}

	public override void CharacterDie()
	{
		Debug.Log("Die");
	}
}
