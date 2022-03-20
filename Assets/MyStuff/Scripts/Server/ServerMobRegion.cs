using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class ServerMobRegion : MonoBehaviour
    {
        public List<Transform> points = new List<Transform>();
        public List<ServerBasicAI> npcList = new List<ServerBasicAI>();

        public float MaxNPCCount = 4;
        public float maxMobRespawnTimer = 1;
        public float currentMobRespawnTimer;

        public GameObject EnemyPrefab;

        // Start is called before the first frame update
        void Start()
        {
            currentMobRespawnTimer = maxMobRespawnTimer;
        }

        // Update is called once per frame
        void Update()
        {
            if (currentMobRespawnTimer < 0 && npcList.Count < MaxNPCCount)
            {
                GameObject _newEnemy = SpawnEnemy();
                ServerBasicAI _basicAi = _newEnemy.GetComponent<ServerBasicAI>();

                npcList.Add(_basicAi);

                _basicAi.SetPatrolTargets(points);
                _basicAi.Region = this;

                currentMobRespawnTimer = maxMobRespawnTimer;
            }
            else if (currentMobRespawnTimer > 0)
            {
                currentMobRespawnTimer -= Time.deltaTime;
            }
        }

        public void RemoveEnemy(ServerBasicAI _npc)
        {
            if (npcList.Count == MaxNPCCount)
            {
                currentMobRespawnTimer = maxMobRespawnTimer;
            }

            ServerGameManager.Instance.Characters.Remove(_npc.GetInstanceID());
            npcList.Remove(_npc);
            // TODO: Send remove message to clients
        }

        GameObject SpawnEnemy()
        {
            Transform _point = GetRandomPoint();
            var character = Instantiate(EnemyPrefab, _point.transform.position, _point.transform.rotation).GetComponent<ServerBasicAI>();
            ServerGameManager.Instance.Characters.Add(character.GetInstanceID(), character);
            // Send spawn message to clients
            new ServerCharacterDataHandle().WriteCharacterData(character);
            return character.gameObject;
        }

        Transform GetRandomPoint()
        {
            return points[Random.Range(0, points.Count)];
        }

    }
}
