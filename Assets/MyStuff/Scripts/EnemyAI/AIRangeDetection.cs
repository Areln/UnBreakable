using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRangeDetection : MonoBehaviour
{

    public BasicAI aiBrain;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            aiBrain.AddInRage(other.gameObject);
            if (aiBrain.currentAIState != BasicAI.AIState.Attack)
            {
                aiBrain.currentAIState = BasicAI.AIState.Attack;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
