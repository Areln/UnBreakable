using UnityEngine;

public class AIRangeDetection : MonoBehaviour
{

    internal BasicAI aiBrain;

    public void Start()
	{
        aiBrain = GetComponentInParent<BasicAI>();
	}

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
    // Update is called once per frame
    void Update()
    {
        
    }
}
