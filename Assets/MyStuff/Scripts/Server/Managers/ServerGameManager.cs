using System.Collections.Generic;
using UnityEngine;

namespace Server
{
	public class ServerGameManager : MonoBehaviour
    {
        private static ServerGameManager m_Instance;

        private static object m_Lock = new object();
        public static ServerGameManager Instance
        {
            get
            {
                lock (m_Lock)
                {
                    if (m_Instance == null)
                    {
                        // Search for existing instance.
                        m_Instance = (ServerGameManager)FindObjectOfType(typeof(ServerGameManager));

                        // Create new instance if one doesn't already exist.
                        if (m_Instance == null)
                        {
                            // Need to create a new GameObject to attach the singleton to.
                            var singletonObject = new GameObject();
                            m_Instance = singletonObject.AddComponent<ServerGameManager>();
                            singletonObject.name = typeof(ServerGameManager).ToString() + " (Singleton)";

                            // Make instance persistent.
                            //DontDestroyOnLoad(singletonObject);
                        }
                    }

                    return m_Instance;
                };
            }
        }

		internal Dictionary<int, ServerPlayerBrain> ClientPlayers = new Dictionary<int, ServerPlayerBrain>();

        public List<GameObject> PossibleItems = new List<GameObject>();

        public GameObject BasePlayerPrefab;
        public Vector3 SpawnPoint;

        static readonly Dictionary<string, GameObject> ItemDirectory = new Dictionary<string, GameObject>();

        private void Awake()
        {
            foreach (GameObject item in PossibleItems)
            {
                ItemDirectory.Add(item.GetComponent<Item>().InternalName, item);
            }
        }

        public GameObject SearchItems(string itemName)
        {
            ItemDirectory.TryGetValue(itemName, out GameObject returnItem);
            return returnItem;
        }

        internal ServerPlayerBrain LoadPlayer(int _clientId, string username)
        {
            // TODO: LoadFrom File or database if exists.
            var connectionPlayerGameObject = Instantiate(BasePlayerPrefab, SpawnPoint, Quaternion.identity);
            var playerBrain = connectionPlayerGameObject.GetComponent<ServerPlayerBrain>();
            playerBrain.InitializeData(username);
            ClientPlayers.Add(_clientId, playerBrain);
            return playerBrain;
        }
    }
}
