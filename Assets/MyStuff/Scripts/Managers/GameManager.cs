using QFSW.QC;
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
                        GameObject singletonObject = new GameObject();
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

    internal void CreateStorageObject(int objectId, Vector3 vector3, float rotation, string objectName)
    {
        StorageObject lootObject = Instantiate(Resources.Load<StorageObject>($"LootItemObjects/{objectName}StorageObject"), vector3, Quaternion.Euler(0, rotation, 0));
        AddNewItemStorageToList(objectId, lootObject);
    }

    internal PlayerBrain ClientPlayer;

    internal Dictionary<int, CharacterBrain> LoadedCharacters { get; set; } = new Dictionary<int, CharacterBrain>();

    internal Dictionary<int, StorageObject> itemStorages = new Dictionary<int, StorageObject>();

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

    [Command("SpawnAllItems")]
    public void SpawnAllItems()
    {
        foreach (GameObject item in PossibleItems)
        {
            new CharacterSpawnItem().WriteMessage(item.GetComponent<Item>().InternalName);
        }
    }
    public GameObject GetItem(string itemName)
    {
        ItemDirectory.TryGetValue(itemName, out GameObject returnItem);

        return returnItem;
    }

    internal CharacterBrain GetCharacter(int characterIdToMove)
    {
        LoadedCharacters.TryGetValue(characterIdToMove, out CharacterBrain loadedCharacter);
        return loadedCharacter;
    }

    internal void LoadConnectingPlayer(PlayerData playerData)
    {
        GameObject player = Instantiate(BasePlayerPrefab, playerData.Position, Quaternion.Euler(new Vector3(0, playerData.Rotation, 0)));
        PlayerBrain playerBrain = player.GetComponent<PlayerBrain>();
        playerBrain.InitializeData(playerData);

        if (playerData.IsClientPlayer)
        {
            Camera.main.GetComponent<CameraFollow>().target = player.transform;
            ClientPlayer = playerBrain;
        }
        LoadedCharacters.Add(playerData.PlayerId, playerBrain);
    }

    internal void LoadCharacter(CharacterData characterData)
    {
        GameObject character = Instantiate(Resources.Load($"Enemies/{characterData.CharacterPrefabName}") as GameObject, characterData.Position, Quaternion.Euler(new Vector3(0, characterData.Rotation, 0)));
        BasicAI basicAi = character.GetComponent<BasicAI>();
        basicAi.InitializeData(characterData);

        LoadedCharacters.Add(characterData.CharacterId, basicAi);
        basicAi.SetupAbilities();
    }
    public void AddNewItemStorageToList(int objectId, StorageObject worldObjectChest)
    {
        itemStorages.Add(objectId, worldObjectChest);
    }
}
