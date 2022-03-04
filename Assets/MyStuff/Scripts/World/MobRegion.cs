using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobRegion : MonoBehaviour
{
    public List<Transform> points = new List<Transform>();
    public List<BasicAI> npcList = new List<BasicAI>();

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
            BasicAI _basicAi = _newEnemy.GetComponent<BasicAI>();
            
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

    public void RemoveEnemy(BasicAI _npc) 
    {
        if (npcList.Count == MaxNPCCount)
        {
            currentMobRespawnTimer = maxMobRespawnTimer;
        }

        npcList.Remove(_npc);
    }

    GameObject SpawnEnemy() 
    {
        Transform _point = GetRandomPoint();
        return Instantiate(EnemyPrefab, _point.transform.position, _point.transform.rotation);
    }

    Transform GetRandomPoint()
    {
        return points[Random.Range(0, points.Count)];
    }

}
