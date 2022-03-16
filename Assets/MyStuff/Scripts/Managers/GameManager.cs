using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static GameManager m_Instance;

	private static object m_Lock = new object();
	public static GameManager Instance
	{
		get
		{
			lock (m_Lock)
			{
				if (m_Instance == null)
				{
					// Search for existing instance.
					m_Instance = (GameManager)FindObjectOfType(typeof(GameManager));

					// Create new instance if one doesn't already exist.
					if (m_Instance == null)
					{
						// Need to create a new GameObject to attach the singleton to.
						var singletonObject = new GameObject();
						m_Instance = singletonObject.AddComponent<GameManager>();
						singletonObject.name = typeof(GameManager).ToString() + " (Singleton)";

						// Make instance persistent.
						//DontDestroyOnLoad(singletonObject);
					}
				}

				return m_Instance;
			};
		}
	}

	internal PlayerBrain ClientPlayer;

	internal Dictionary<int, CharacterBrain> LoadedCharacters { get; set; } = new Dictionary<int, CharacterBrain>();

	public List<GameObject> PossibleItems;
	public GameObject BasePlayerPrefab;

	static readonly Dictionary<string, GameObject> ItemDirectory = new Dictionary<string, GameObject>();

	internal bool UsingUI;
	internal ItemSlot DraggingObject;

	private void Awake()
	{
		foreach (GameObject item in PossibleItems)
		{
			ItemDirectory.Add(item.GetComponent<Item>().InternalName, item);
		}
	}

	private void Start()
	{
		var player = transform.Find("Player");
		if (player != null && player.gameObject.activeSelf)
		{
			ClientPlayer = player.GetComponent<PlayerBrain>();
			LoadOfflinePlayer(ClientPlayer);
		}
	}

	public GameObject GetItem(string itemName)
	{
		GameObject returnItem;

		ItemDirectory.TryGetValue(itemName, out returnItem);

		return returnItem;
	}

	internal CharacterBrain GetCharacter(int characterIdToMove)
	{
		LoadedCharacters.TryGetValue(characterIdToMove, out var loadedCharacter);
		return loadedCharacter;
	}

	internal void LoadConnectingPlayer(PlayerData playerData)
	{
		var player = Instantiate(BasePlayerPrefab, playerData.Position, Quaternion.Euler(new Vector3(0, playerData.Rotation, 0)));
		var playerBrain = player.GetComponent<PlayerBrain>();
		playerBrain.InitializeData(playerData);

		if (playerData.IsClientPlayer)
		{
			Camera.main.GetComponent<CameraFollow>().target = player.transform;
			ClientPlayer = playerBrain;
		}
		LoadedCharacters.Add(playerData.PlayerId, playerBrain);
		playerBrain.playerCombat.SetupAbilities();
	}

	internal void LoadOfflinePlayer(PlayerBrain playerBrain)
	{
		foreach (GameObject item in Instance.PossibleItems)
		{
			ClientPlayer.playerInventory.AddPrefabItemObjectToPlayerInventory(item);
		}
		Camera.main.GetComponent<CameraFollow>().target = playerBrain.transform;
		ClientPlayer = playerBrain;
		LoadedCharacters.Add(0, playerBrain);
		playerBrain.playerCombat.SetupAbilities();
	}
}
