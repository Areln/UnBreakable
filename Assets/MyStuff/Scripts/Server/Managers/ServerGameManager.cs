using Server.Networking;
using System.Collections.Generic;
using System.Linq;
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

        //internal Dictionary<int, ServerPlayerBrain> ClientPlayers = new Dictionary<int, ServerPlayerBrain>();

        //internal Dictionary<int, ServerBasicAI> Characters { get; set; } = new Dictionary<int, ServerBasicAI>();

        internal Dictionary<int, ServerStorageObject> itemStorages = new Dictionary<int, ServerStorageObject>();

        internal Dictionary<Coordinates, ServerRegion> LoadedRegions = new Dictionary<Coordinates, ServerRegion>(new Coordinates.EqualityComparer());

        public List<GameObject> PossibleItems = new List<GameObject>();

        public GameObject BasePlayerPrefab;
        public Vector3 SpawnPoint;

        static readonly Dictionary<string, GameObject> ItemDirectory = new Dictionary<string, GameObject>();

        public ServerRegion defaultRegion;

        private void Awake()
        {
            LoadedRegions.Add(new Coordinates() { X = 0, Y = 0 }, defaultRegion);

            foreach (GameObject item in PossibleItems)
            {
                ItemDirectory.Add(item.GetComponent<Item>().InternalName, item);
            }
        }

        public void AddNewItemStorageToList(ServerStorageObject worldObjectChest)
        {
            itemStorages.Add(worldObjectChest.GetInstanceID(), worldObjectChest);
        }

        internal ServerPlayerBrain GetPlayer(int playerId)
        {
            ServerPlayerBrain player = null;
            foreach (var region in LoadedRegions.Values)
            {
                if (region.ClientPlayers.TryGetValue(playerId, out player))
                {
                    return player;
                }
            }
            return player;
        }

        public GameObject SearchItems(string itemName)
        {
            ItemDirectory.TryGetValue(itemName, out GameObject returnItem);
            return returnItem;
        }

        internal ServerPlayerBrain LoadPlayer(int _clientId, string username)
        {
            // TODO: LoadFrom File or database if exists.
            Coordinates defaultCoordinates = new Coordinates { X = 0, Y = 0 };
            var connectionPlayerGameObject = Instantiate(BasePlayerPrefab, SpawnPoint, Quaternion.identity);
            var playerBrain = connectionPlayerGameObject.GetComponent<ServerPlayerBrain>();

            if (LoadedRegions.TryGetValue(defaultCoordinates, out var region))
            {
                region.ClientPlayers.Add(_clientId, playerBrain);
            }
            else
            {
                LoadRegion(defaultCoordinates);
                region.ClientPlayers.Add(_clientId, playerBrain);
            }
            var regionsToSendToPlayer = LoadNearbyRegions(defaultCoordinates);
            regionsToSendToPlayer.Add(region);

            new ServerRegionLoadHandle().WriteMessage(_clientId, regionsToSendToPlayer.Select(x => x.Location).ToList());

            playerBrain.InitializeData(username, region);
            return playerBrain;
        }

        internal ServerRegion LoadRegion(Coordinates coords)
        {
            try
            {
                var gameObject = Instantiate(Resources.Load($"ServerRegions/ServerRegion{coords.X},{coords.Y}") as GameObject);
                var region = gameObject.GetComponent<ServerRegion>();
                LoadedRegions.Add(coords, region);
                return region;
            }
            catch
            {
                // handle error when an area has not been made or doesnt exist.
            }
            return null;
        }

        internal List<ServerRegion> LoadNearbyRegions(Coordinates startCoords)
        {
            var loadedRegions = new List<ServerRegion>();
            // Up
            var nextCoords = new Coordinates() { X = startCoords.X, Y = startCoords.Y + 1 };
            if (!LoadedRegions.ContainsKey(nextCoords))
            {
                var region = LoadRegion(nextCoords);
                if (region != null)
                {
                    loadedRegions.Add(region);
                }
            }

            // Right
            nextCoords = new Coordinates() { X = startCoords.X + 1, Y = startCoords.Y };
            if (!LoadedRegions.ContainsKey(nextCoords))
            {
                var region = LoadRegion(nextCoords);
                if (region != null)
                {
                    loadedRegions.Add(region);
                }
            }

            // Down
            nextCoords = new Coordinates() { X = startCoords.X, Y = startCoords.Y - 1 };
            if (!LoadedRegions.ContainsKey(nextCoords))
            {
                var region = LoadRegion(nextCoords);
                if (region != null)
                {
                    loadedRegions.Add(region);
                }
            }

            // Left
            nextCoords = new Coordinates() { X = startCoords.X - 1, Y = startCoords.Y };
            if (!LoadedRegions.ContainsKey(nextCoords))
            {
                var region = LoadRegion(nextCoords);
                if (region != null)
                {
                    loadedRegions.Add(region);
                }
            }

            return loadedRegions;
        }
    }
}
